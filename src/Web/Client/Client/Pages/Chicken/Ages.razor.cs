using Famtela.Application.Features.Ages.Queries.GetAll;
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
using Famtela.Application.Features.Ages.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Chicken.Age;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Famtela.Application.Features.Ages.Commands.Import;
using Famtela.Shared.Wrapper;
using Famtela.Application.Requests;
using Famtela.Client.Shared.Components;

namespace Famtela.Client.Pages.Chicken
{
    public partial class Ages
    {
        [Inject] private IAgeManager AgeManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllAgesResponse> _ageList = new();
        private GetAllAgesResponse _age = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateAges;
        private bool _canEditAges;
        private bool _canDeleteAges;
        private bool _canExportAges;
        private bool _canSearchAges;
        private bool _canImportAges;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateAges = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Ages.Create)).Succeeded;
            _canEditAges = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Ages.Edit)).Succeeded;
            _canDeleteAges = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Ages.Delete)).Succeeded;
            _canExportAges = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Ages.Export)).Succeeded;
            _canSearchAges = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Ages.Search)).Succeeded;
            _canImportAges = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Ages.Import)).Succeeded;

            await GetAgesAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetAgesAsync()
        {
            var response = await AgeManager.GetAllAsync();
            if (response.Succeeded)
            {
                _ageList = response.Data.ToList();
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
                var response = await AgeManager.DeleteAsync(id);
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
            var response = await AgeManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Ages).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Ages exported"]
                    : _localizer["Filtered Ages exported"], Severity.Success);
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
                _age = _ageList.FirstOrDefault(c => c.Id == id);
                if (_age != null)
                {
                    parameters.Add(nameof(AddEditAgeModal.AddEditAgeModel), new AddEditAgeCommand
                    {
                        Id = _age.Id,
                        Name = _age.Name
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditAgeModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportAgesCommand { UploadRequest = uploadFile };
            var result = await AgeManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), _localizer["Ages"].ToString() }
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
            _age = new GetAllAgesResponse();
            await GetAgesAsync();
        }

        private bool Search(GetAllAgesResponse age)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return age.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}