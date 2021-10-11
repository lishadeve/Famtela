using Famtela.Application.Features.Ages.Queries.GetAll;
using Famtela.Application.Features.Vaccinations.Commands.AddEdit;
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
using Famtela.Client.Infrastructure.Managers.Chicken.Vaccination;
using Famtela.Client.Infrastructure.Managers.Chicken.Disease;
using Famtela.Application.Features.Diseases.Queries.GetAll;

namespace Famtela.Client.Pages.Chicken
{
    public partial class AddEditVaccinationModal
    {
        [Inject] private IVaccinationManager VaccinationManager { get; set; }
        [Inject] private IAgeManager AgeManager { get; set; }
        [Inject] private IDiseaseManager DiseaseManager { get; set; }

        [Parameter] public AddEditVaccinationCommand AddEditVaccinationModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllAgesResponse> _ages = new();
        private List<GetAllDiseasesResponse> _diseases = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await VaccinationManager.SaveAsync(AddEditVaccinationModel);
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
            await LoadDiseasesAsync();
        }

        private async Task LoadAgesAsync()
        {
            var data = await AgeManager.GetAllAsync();
            if (data.Succeeded)
            {
                _ages = data.Data;
            }
        }

        private async Task LoadDiseasesAsync()
        {
            var data = await DiseaseManager.GetAllAsync();
            if (data.Succeeded)
            {
                _diseases = data.Data;
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

        private async Task<IEnumerable<int>> SearchDiseases(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _diseases.Select(x => x.Id);

            return _diseases.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
    }
}