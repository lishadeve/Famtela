using Famtela.Application.Features.Cows.Commands.AddEdit;
using Famtela.Client.Extensions;
using Famtela.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using Famtela.Client.Infrastructure.Managers.Dairy.Breed;
using Famtela.Client.Infrastructure.Managers.Dairy.Color;
using Famtela.Client.Infrastructure.Managers.Dairy.Status;
using Famtela.Client.Infrastructure.Managers.Dairy.Tag;
using Famtela.Client.Infrastructure.Managers.Dairy.Cow;
using Famtela.Application.Features.Breeds.Queries.GetAll;
using Famtela.Application.Features.Colors.Queries.GetAll;
using Famtela.Application.Features.Statuses.Queries.GetAll;
using Famtela.Application.Features.Tags.Queries.GetAll;

namespace Famtela.Client.Pages.Dairy
{
    public partial class AddEditCowModal
    {
        [Inject] private ICowManager CowManager { get; set; }
        [Inject] private IBreedManager BreedManager { get; set; }
        [Inject] private IColorManager ColorManager { get; set; }
        [Inject] private IStatusManager StatusManager { get; set; }
        [Inject] private ITagManager TagManager { get; set; }

        [Parameter] public AddEditCowCommand AddEditCowModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllBreedsResponse> _breeds = new();
        private List<GetAllColorsResponse> _colors = new();
        private List<GetAllStatusesResponse> _statuses = new();
        private List<GetAllTagsResponse> _tags = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await CowManager.SaveAsync(AddEditCowModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            await LoadBreedsAsync();
            await LoadColorsAsync();
            await LoadStatusesAsync();
            await LoadTagsAsync();
        }

        private async Task LoadBreedsAsync()
        {
            var data = await BreedManager.GetAllAsync();
            if (data.Succeeded)
            {
                _breeds = data.Data;
            }
        }

        private async Task LoadColorsAsync()
        {
            var data = await ColorManager.GetAllAsync();
            if (data.Succeeded)
            {
                _colors = data.Data;
            }
        }

        private async Task LoadStatusesAsync()
        {
            var data = await StatusManager.GetAllAsync();
            if (data.Succeeded)
            {
                _statuses = data.Data;
            }
        }

        private async Task LoadTagsAsync()
        {
            var data = await TagManager.GetAllAsync();
            if (data.Succeeded)
            {
                _tags = data.Data;
            }
        }

        private async Task<IEnumerable<int>> SearchBreeds(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _breeds.Select(x => x.Id);

            return _breeds.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        private async Task<IEnumerable<int>> SearchColors(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _colors.Select(x => x.Id);

            return _colors.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        private async Task<IEnumerable<int>> SearchStatuses(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _statuses.Select(x => x.Id);

            return _statuses.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        private async Task<IEnumerable<int>> SearchTags(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _tags.Select(x => x.Id);

            return _tags.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
    }
}