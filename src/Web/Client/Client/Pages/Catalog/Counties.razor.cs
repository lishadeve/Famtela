using Famtela.Application.Features.Counties.Queries.GetAll;
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
using Famtela.Application.Features.Counties.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Catalog.County;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Famtela.Application.Features.Counties.Commands.Import;
using Famtela.Shared.Wrapper;
using Famtela.Application.Requests;
using Famtela.Client.Shared.Components;

namespace Famtela.Client.Pages.Catalog
{
    public partial class Counties
    {
        [Inject] private ICountyManager CountyManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllCountiesResponse> _countyList = new();
        private GetAllCountiesResponse _county = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateCounties;
        private bool _canEditCounties;
        private bool _canDeleteCounties;
        private bool _canExportCounties;
        private bool _canSearchCounties;
        private bool _canImportCounties;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateCounties = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Counties.Create)).Succeeded;
            _canEditCounties = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Counties.Edit)).Succeeded;
            _canDeleteCounties = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Counties.Delete)).Succeeded;
            _canExportCounties = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Counties.Export)).Succeeded;
            _canSearchCounties = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Counties.Search)).Succeeded;
            _canImportCounties = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Counties.Import)).Succeeded;

            await GetCountiesAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetCountiesAsync()
        {
            var response = await CountyManager.GetAllAsync();
            if (response.Succeeded)
            {
                _countyList = response.Data.ToList();
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
                var response = await CountyManager.DeleteAsync(id);
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
            var response = await CountyManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Counties).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Counties exported"]
                    : _localizer["Filtered Counties exported"], Severity.Success);
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
                _county = _countyList.FirstOrDefault(c => c.Id == id);
                if (_county != null)
                {
                    parameters.Add(nameof(AddEditCountyModal.AddEditCountyModel), new AddEditCountyCommand
                    {
                        Id = _county.Id,
                        Name = _county.Name
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditCountyModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportCountiesCommand { UploadRequest = uploadFile };
            var result = await CountyManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), _localizer["Counties"].ToString() }
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
            _county = new GetAllCountiesResponse();
            await GetCountiesAsync();
        }

        private bool Search(GetAllCountiesResponse county)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return county.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}