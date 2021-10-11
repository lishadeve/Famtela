using Famtela.Application.Features.WeightEstimates.Queries.GetAll;
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
using Famtela.Application.Features.WeightEstimates.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Dairy.WeightEstimate;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Famtela.Application.Features.WeightEstimates.Commands.Import;
using Famtela.Shared.Wrapper;
using Famtela.Application.Requests;
using Famtela.Client.Shared.Components;

namespace Famtela.Client.Pages.Dairy
{
    public partial class WeightEstimates
    {
        [Inject] private IWeightEstimateManager WeightEstimateManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllWeightEstimatesResponse> _weightestimateList = new();
        private GetAllWeightEstimatesResponse _weightestimate = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateWeightEstimates;
        private bool _canEditWeightEstimates;
        private bool _canDeleteWeightEstimates;
        private bool _canExportWeightEstimates;
        private bool _canSearchWeightEstimates;
        private bool _canImportWeightEstimates;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateWeightEstimates = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WeightEstimates.Create)).Succeeded;
            _canEditWeightEstimates = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WeightEstimates.Edit)).Succeeded;
            _canDeleteWeightEstimates = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WeightEstimates.Delete)).Succeeded;
            _canExportWeightEstimates = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WeightEstimates.Export)).Succeeded;
            _canSearchWeightEstimates = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WeightEstimates.Search)).Succeeded;
            _canImportWeightEstimates = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.WeightEstimates.Import)).Succeeded;

            await GetWeightEstimatesAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetWeightEstimatesAsync()
        {
            var response = await WeightEstimateManager.GetAllAsync();
            if (response.Succeeded)
            {
                _weightestimateList = response.Data.ToList();
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
                var response = await WeightEstimateManager.DeleteAsync(id);
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
            var response = await WeightEstimateManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(WeightEstimates).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Weight Estimates exported"]
                    : _localizer["Filtered Weight Estimates exported"], Severity.Success);
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
                _weightestimate = _weightestimateList.FirstOrDefault(c => c.Id == id);
                if (_weightestimate != null)
                {
                    parameters.Add(nameof(AddEditWeightEstimateModal.AddEditWeightEstimateModel), new AddEditWeightEstimateCommand
                    {
                        Id = _weightestimate.Id,
                        CM = _weightestimate.CM,
                        KG = _weightestimate.KG,
                        Remarks = _weightestimate.Remarks
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditWeightEstimateModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportWeightEstimatesCommand { UploadRequest = uploadFile };
            var result = await WeightEstimateManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), _localizer["Weight Estimates"].ToString() }
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
            _weightestimate = new GetAllWeightEstimatesResponse();
            await GetWeightEstimatesAsync();
        }

        private bool Search(GetAllWeightEstimatesResponse weightestimate)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return weightestimate.Remarks?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}