using Famtela.Application.Features.Diseases.Queries.GetAll;
using Famtela.Client.Extensions;
using Famtela.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Famtela.Application.Features.Diseases.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Chicken.Disease;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Famtela.Application.Features.Diseases.Commands.Import;
using Famtela.Shared.Wrapper;
using Famtela.Application.Requests;
using Famtela.Client.Shared.Components;

namespace Famtela.Client.Pages.Chicken
{
    public partial class Diseases
    {
        [Inject] private IDiseaseManager DiseaseManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllDiseasesResponse> _diseaseList = new();
        private GetAllDiseasesResponse _disease = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateDiseases;
        private bool _canEditDiseases;
        private bool _canDeleteDiseases;
        private bool _canExportDiseases;
        private bool _canSearchDiseases;
        private bool _canImportDiseases;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateDiseases = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Diseases.Create)).Succeeded;
            _canEditDiseases = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Diseases.Edit)).Succeeded;
            _canDeleteDiseases = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Diseases.Delete)).Succeeded;
            _canExportDiseases = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Diseases.Export)).Succeeded;
            _canSearchDiseases = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Diseases.Search)).Succeeded;
            _canImportDiseases = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Diseases.Import)).Succeeded;

            await GetDiseasesAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetDiseasesAsync()
        {
            var response = await DiseaseManager.GetAllAsync();
            if (response.Succeeded)
            {
                _diseaseList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                { nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id) }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await DiseaseManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task ExportToExcel()
        {
            var response = await DiseaseManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Diseases).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Chicken Expenses exported"]
                    : _localizer["Filtered Chicken Expenses exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _disease = _diseaseList.FirstOrDefault(c => c.Id == id);
                if (_disease != null)
                {
                    parameters.Add(nameof(AddEditDiseaseModal.AddEditDiseaseModel), new AddEditDiseaseCommand
                    {
                        Id = _disease.Id,
                        Name = _disease.Name
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditDiseaseModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportDiseasesCommand { UploadRequest = uploadFile };
            var result = await DiseaseManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), _localizer["Chicken Expenses"].ToString() }
            };
            Func<UploadRequest, Task<IResult<int>>> importExcel = ImportExcel;
            parameters.Add(nameof(ImportExcelModal.OnSaved), importExcel);
            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                DisableBackdropClick = true
            };
            var dialog = _dialogService.Show<ImportExcelModal>(_localizer["Import"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _disease = new GetAllDiseasesResponse();
            await GetDiseasesAsync();
        }

        private bool Search(GetAllDiseasesResponse disease)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return disease.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}