using Famtela.Application.Features.Ages.Queries.GetAll;
using Famtela.Application.Features.Consumptions.Commands.AddEdit;
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
using Famtela.Client.Infrastructure.Managers.Chicken.Age;
using Famtela.Client.Infrastructure.Managers.Chicken.Consumption;
using Famtela.Client.Infrastructure.Managers.Chicken.TypeofFeed;
using Famtela.Application.Features.TypesofFeed.Queries.GetAll;

namespace Famtela.Client.Pages.Chicken
{
    public partial class AddEditConsumptionModal
    {
        [Inject] private IConsumptionManager ConsumptionManager { get; set; }
        [Inject] private IAgeManager AgeManager { get; set; }
        [Inject] private ITypeofFeedManager TypeofFeedManager { get; set; }

        [Parameter] public AddEditConsumptionCommand AddEditConsumptionModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllAgesResponse> _ages = new();
        private List<GetAllTypesofFeedResponse> _typesofFeed = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await ConsumptionManager.SaveAsync(AddEditConsumptionModel);
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
            await LoadAgesAsync();
            await LoadTypesofFeedAsync();
        }

        private async Task LoadAgesAsync()
        {
            var data = await AgeManager.GetAllAsync();
            if (data.Succeeded)
            {
                _ages = data.Data;
            }
        }

        private async Task LoadTypesofFeedAsync()
        {
            var data = await TypeofFeedManager.GetAllAsync();
            if (data.Succeeded)
            {
                _typesofFeed = data.Data;
            }
        }

        private async Task<IEnumerable<int>> SearchAges(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _ages.Select(x => x.Id);

            return _ages.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        private async Task<IEnumerable<int>> SearchTypesofFeed(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _typesofFeed.Select(x => x.Id);

            return _typesofFeed.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
    }
}