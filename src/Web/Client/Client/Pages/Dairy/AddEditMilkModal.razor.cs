using Famtela.Application.Features.Milks.Commands.AddEdit;
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
using Famtela.Client.Infrastructure.Managers.Dairy.Cow;
using Famtela.Client.Infrastructure.Managers.Dairy.Milk;
using Famtela.Application.Features.Cows.Queries.GetAll;

namespace Famtela.Client.Pages.Dairy
{
    public partial class AddEditMilkModal
    {
        [Inject] private IMilkManager MilkManager { get; set; }
        [Inject] private ICowManager CowManager { get; set; }

        [Parameter] public AddEditMilkCommand AddEditMilkModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllCowsResponse> _cows = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await MilkManager.SaveAsync(AddEditMilkModel);
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
            await LoadCowsAsync();
        }

        private async Task LoadCowsAsync()
        {
            var data = await CowManager.GetAllAsync();
            if (data.Succeeded)
            {
                _cows = data.Data;
            }
        }

        private async Task<IEnumerable<int>> SearchCows(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _cows.Select(x => x.Id);

            return _cows.Where(x => x.EarTagNumber.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
    }
}