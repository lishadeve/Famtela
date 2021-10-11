using Famtela.Application.Features.Layers.Queries.GetAll;
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
using Famtela.Application.Features.Layers.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Chicken.Layers;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Famtela.Application.Features.Layers.Commands.Import;
using Famtela.Shared.Wrapper;
using Famtela.Application.Requests;
using Famtela.Client.Shared.Components;

namespace Famtela.Client.Pages.Chicken
{
    public partial class Layers
    {
        [Inject] private ILayersManager LayersManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllLayersResponse> _layersList = new();
        private GetAllLayersResponse _layers = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateLayers;
        private bool _canEditLayers;
        private bool _canDeleteLayers;
        private bool _canExportLayers;
        private bool _canSearchLayers;
        private bool _canImportLayers;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateLayers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Layers.Create)).Succeeded;
            _canEditLayers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Layers.Edit)).Succeeded;
            _canDeleteLayers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Layers.Delete)).Succeeded;
            _canExportLayers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Layers.Export)).Succeeded;
            _canSearchLayers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Layers.Search)).Succeeded;
            _canImportLayers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Layers.Import)).Succeeded;

            await GetLayersAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetLayersAsync()
        {
            var response = await LayersManager.GetAllAsync();
            if (response.Succeeded)
            {
                _layersList = response.Data.ToList();
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
                var response = await LayersManager.DeleteAsync(id);
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
            var response = await LayersManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Layers).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Layers exported"]
                    : _localizer["Filtered Layers exported"], Severity.Success);
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
                _layers = _layersList.FirstOrDefault(c => c.Id == id);
                if (_layers != null)
                {
                    parameters.Add(nameof(AddEditLayersModal.AddEditLayerModel), new AddEditLayerCommand
                    {
                        Id = _layers.Id,
                        NumberofBirds = _layers.NumberofBirds,
                        Disease = _layers.Disease,
                        Mortality = _layers.Mortality,
                        Vaccination = _layers.Vaccination,
                        Medication = _layers.Medication,
                        Feed = _layers.Feed,
                        TypeofFeed = _layers.TypeofFeed,
                        Eggs = _layers.Eggs,
                        Remarks = _layers.Remarks
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditLayersModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportLayersCommand { UploadRequest = uploadFile };
            var result = await LayersManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), _localizer["Layers"].ToString() }
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
            _layers = new GetAllLayersResponse();
            await GetLayersAsync();
        }

        private bool Search(GetAllLayersResponse layers)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (layers.Medication?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (layers.Disease?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (layers.Vaccination?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (layers.TypeofFeed?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return layers.Remarks?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}