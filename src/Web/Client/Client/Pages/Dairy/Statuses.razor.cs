using Famtela.Application.Features.Statuses.Queries.GetAll;
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
using Famtela.Application.Features.Statuses.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Dairy.Status;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Famtela.Application.Features.Statuses.Commands.Import;
using Famtela.Shared.Wrapper;
using Famtela.Application.Requests;
using Famtela.Client.Shared.Components;

namespace Famtela.Client.Pages.Dairy
{
    public partial class Statuses
    {
        [Inject] private IStatusManager StatusManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllStatusesResponse> _statusList = new();
        private GetAllStatusesResponse _status = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateStatuses;
        private bool _canEditStatuses;
        private bool _canDeleteStatuses;
        private bool _canExportStatuses;
        private bool _canSearchStatuses;
        private bool _canImportStatuses;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateStatuses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Statuses.Create)).Succeeded;
            _canEditStatuses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Statuses.Edit)).Succeeded;
            _canDeleteStatuses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Statuses.Delete)).Succeeded;
            _canExportStatuses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Statuses.Export)).Succeeded;
            _canSearchStatuses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Statuses.Search)).Succeeded;
            _canImportStatuses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Statuses.Import)).Succeeded;

            await GetStatusesAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetStatusesAsync()
        {
            var response = await StatusManager.GetAllAsync();
            if (response.Succeeded)
            {
                _statusList = response.Data.ToList();
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
                var response = await StatusManager.DeleteAsync(id);
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
            var response = await StatusManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Statuses).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Statuses exported"]
                    : _localizer["Filtered Statuses exported"], Severity.Success);
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
                _status = _statusList.FirstOrDefault(c => c.Id == id);
                if (_status != null)
                {
                    parameters.Add(nameof(AddEditStatusModal.AddEditStatusModel), new AddEditStatusCommand
                    {
                        Id = _status.Id,
                        Name = _status.Name
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditStatusModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportStatusesCommand { UploadRequest = uploadFile };
            var result = await StatusManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), _localizer["Statuses"].ToString() }
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
            _status = new GetAllStatusesResponse();
            await GetStatusesAsync();
        }

        private bool Search(GetAllStatusesResponse status)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return status.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}