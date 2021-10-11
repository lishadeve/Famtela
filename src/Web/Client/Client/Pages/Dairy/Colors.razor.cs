using Famtela.Application.Features.Colors.Queries.GetAll;
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
using Famtela.Application.Features.Colors.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Dairy.Color;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Famtela.Application.Features.Colors.Commands.Import;
using Famtela.Shared.Wrapper;
using Famtela.Application.Requests;
using Famtela.Client.Shared.Components;

namespace Famtela.Client.Pages.Dairy
{
    public partial class Colors
    {
        [Inject] private IColorManager ColorManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllColorsResponse> _colorList = new();
        private GetAllColorsResponse _color = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateColors;
        private bool _canEditColors;
        private bool _canDeleteColors;
        private bool _canExportColors;
        private bool _canSearchColors;
        private bool _canImportColors;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateColors = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Colors.Create)).Succeeded;
            _canEditColors = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Colors.Edit)).Succeeded;
            _canDeleteColors = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Colors.Delete)).Succeeded;
            _canExportColors = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Colors.Export)).Succeeded;
            _canSearchColors = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Colors.Search)).Succeeded;
            _canImportColors = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Colors.Import)).Succeeded;

            await GetColorsAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetColorsAsync()
        {
            var response = await ColorManager.GetAllAsync();
            if (response.Succeeded)
            {
                _colorList = response.Data.ToList();
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
                var response = await ColorManager.DeleteAsync(id);
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
            var response = await ColorManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Colors).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Colors exported"]
                    : _localizer["Filtered Colors exported"], Severity.Success);
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
                _color = _colorList.FirstOrDefault(c => c.Id == id);
                if (_color != null)
                {
                    parameters.Add(nameof(AddEditColorModal.AddEditColorModel), new AddEditColorCommand
                    {
                        Id = _color.Id,
                        Name = _color.Name
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditColorModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportColorsCommand { UploadRequest = uploadFile };
            var result = await ColorManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), _localizer["Colors"].ToString() }
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
            _color = new GetAllColorsResponse();
            await GetColorsAsync();
        }

        private bool Search(GetAllColorsResponse color)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return color.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}