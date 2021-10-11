using Famtela.Application.Features.TypesofFarming.Queries.GetAll;
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
using Famtela.Application.Features.TypesofFarming.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Catalog.TypeofFarming;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Famtela.Application.Features.TypesofFarming.Commands.Import;
using Famtela.Shared.Wrapper;
using Famtela.Application.Requests;
using Famtela.Client.Shared.Components;

namespace Famtela.Client.Pages.Catalog
{
    public partial class TypesofFarming
    {
        [Inject] private ITypeofFarmingManager TypeofFarmingManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllTypesofFarmingResponse> _typeoffarmingList = new();
        private GetAllTypesofFarmingResponse _typeoffarming = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateTypesofFarming;
        private bool _canEditTypesofFarming;
        private bool _canDeleteTypesofFarming;
        private bool _canExportTypesofFarming;
        private bool _canSearchTypesofFarming;
        private bool _canImportTypesofFarming;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateTypesofFarming = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TypesofFarming.Create)).Succeeded;
            _canEditTypesofFarming = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TypesofFarming.Edit)).Succeeded;
            _canDeleteTypesofFarming = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TypesofFarming.Delete)).Succeeded;
            _canExportTypesofFarming = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TypesofFarming.Export)).Succeeded;
            _canSearchTypesofFarming = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TypesofFarming.Search)).Succeeded;
            _canImportTypesofFarming = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TypesofFarming.Import)).Succeeded;

            await GetTypesofFarmingAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetTypesofFarmingAsync()
        {
            var response = await TypeofFarmingManager.GetAllAsync();
            if (response.Succeeded)
            {
                _typeoffarmingList = response.Data.ToList();
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
                var response = await TypeofFarmingManager.DeleteAsync(id);
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
            var response = await TypeofFarmingManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(TypesofFarming).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Types of farming exported"]
                    : _localizer["Filtered Types of Farming exported"], Severity.Success);
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
                _typeoffarming = _typeoffarmingList.FirstOrDefault(c => c.Id == id);
                if (_typeoffarming != null)
                {
                    parameters.Add(nameof(AddEditTypeofFarmingModal.AddEditTypeofFarmingModel), new AddEditTypeofFarmingCommand
                    {
                        Id = _typeoffarming.Id,
                        Name = _typeoffarming.Name
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditTypeofFarmingModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportTypesofFarmingCommand { UploadRequest = uploadFile };
            var result = await TypeofFarmingManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), _localizer["TypesofFarming"].ToString() }
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
            _typeoffarming = new GetAllTypesofFarmingResponse();
            await GetTypesofFarmingAsync();
        }

        private bool Search(GetAllTypesofFarmingResponse typeoffarming)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return typeoffarming.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}