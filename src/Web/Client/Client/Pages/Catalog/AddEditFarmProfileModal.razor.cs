using Famtela.Application.Features.Counties.Queries.GetAll;
using Famtela.Application.Features.FarmProfiles.Commands.AddEdit;
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
using Famtela.Client.Infrastructure.Managers.Catalog.County;
using Famtela.Client.Infrastructure.Managers.Catalog.FarmProfile;
using Famtela.Client.Infrastructure.Managers.Catalog.TypeofFarming;
using Famtela.Application.Features.TypesofFarming.Queries.GetAll;

namespace Famtela.Client.Pages.Catalog
{
    public partial class AddEditFarmProfileModal
    {
        [Inject] private IFarmProfileManager FarmProfileManager { get; set; }
        [Inject] private ICountyManager CountyManager { get; set; }
        [Inject] private ITypeofFarmingManager TypeofFarmingManager { get; set; }

        [Parameter] public AddEditFarmProfileCommand AddEditFarmProfileModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllCountiesResponse> _counties = new();
        private List<GetAllTypesofFarmingResponse> _typesoffarming = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await FarmProfileManager.SaveAsync(AddEditFarmProfileModel);
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
            await LoadCountiesAsync();
            await LoadTypesofFarmingAsync();
        }

        private async Task LoadCountiesAsync()
        {
            var data = await CountyManager.GetAllAsync();
            if (data.Succeeded)
            {
                _counties = data.Data;
            }
        }

        private async Task LoadTypesofFarmingAsync()
        {
            var data = await TypeofFarmingManager.GetAllAsync();
            if (data.Succeeded)
            {
                _typesoffarming = data.Data;
            }
        }

        private async Task<IEnumerable<int>> SearchCounties(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _counties.Select(x => x.Id);

            return _counties.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        private async Task<IEnumerable<int>> SearchTypesofFarming(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _typesoffarming.Select(x => x.Id);

            return _typesoffarming.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
    }
}