using Famtela.Application.Features.Chicks.Queries.GetAll;
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
using Famtela.Application.Features.Chicks.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Chicken.Chicks;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Famtela.Application.Features.Chicks.Commands.Import;
using Famtela.Shared.Wrapper;
using Famtela.Application.Requests;
using Famtela.Client.Shared.Components;

namespace Famtela.Client.Pages.Chicken
{
    public partial class Chicks
    {
        [Inject] private IChicksManager ChicksManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllChicksResponse> _chicksList = new();
        private GetAllChicksResponse _chicks = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateChicks;
        private bool _canEditChicks;
        private bool _canDeleteChicks;
        private bool _canExportChicks;
        private bool _canSearchChicks;
        private bool _canImportChicks;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateChicks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Chicks.Create)).Succeeded;
            _canEditChicks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Chicks.Edit)).Succeeded;
            _canDeleteChicks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Chicks.Delete)).Succeeded;
            _canExportChicks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Chicks.Export)).Succeeded;
            _canSearchChicks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Chicks.Search)).Succeeded;
            _canImportChicks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Chicks.Import)).Succeeded;

            await GetChicksAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetChicksAsync()
        {
            var response = await ChicksManager.GetAllAsync();
            if (response.Succeeded)
            {
                _chicksList = response.Data.ToList();
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
                var response = await ChicksManager.DeleteAsync(id);
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
            var response = await ChicksManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Chicks).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Chicks exported"]
                    : _localizer["Filtered Chicks exported"], Severity.Success);
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
                _chicks = _chicksList.FirstOrDefault(c => c.Id == id);
                if (_chicks != null)
                {
                    parameters.Add(nameof(AddEditChicksModal.AddEditChickModel), new AddEditChickCommand
                    {
                        Id = _chicks.Id,
                        NumberofBirds = _chicks.NumberofBirds,
                        Disease = _chicks.Disease,
                        Mortality = _chicks.Mortality,
                        Vaccination = _chicks.Vaccination,
                        Medication = _chicks.Medication,
                        Feed = _chicks.Feed,
                        TypeofFeed = _chicks.TypeofFeed,
                        Remarks = _chicks.Remarks
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditChicksModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportChicksCommand { UploadRequest = uploadFile };
            var result = await ChicksManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), _localizer["Chicks"].ToString() }
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
            _chicks = new GetAllChicksResponse();
            await GetChicksAsync();
        }

        private bool Search(GetAllChicksResponse chicks)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (chicks.Medication?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (chicks.Disease?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (chicks.Vaccination?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (chicks.TypeofFeed?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return chicks.Remarks?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}