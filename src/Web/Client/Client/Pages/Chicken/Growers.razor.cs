using Famtela.Application.Features.Growers.Queries.GetAll;
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
using Famtela.Application.Features.Growers.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Chicken.Growers;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Famtela.Application.Features.Growers.Commands.Import;
using Famtela.Shared.Wrapper;
using Famtela.Application.Requests;
using Famtela.Client.Shared.Components;

namespace Famtela.Client.Pages.Chicken
{
    public partial class Growers
    {
        [Inject] private IGrowersManager GrowersManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllGrowersResponse> _growersList = new();
        private GetAllGrowersResponse _growers = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateGrowers;
        private bool _canEditGrowers;
        private bool _canDeleteGrowers;
        private bool _canExportGrowers;
        private bool _canSearchGrowers;
        private bool _canImportGrowers;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateGrowers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Growers.Create)).Succeeded;
            _canEditGrowers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Growers.Edit)).Succeeded;
            _canDeleteGrowers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Growers.Delete)).Succeeded;
            _canExportGrowers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Growers.Export)).Succeeded;
            _canSearchGrowers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Growers.Search)).Succeeded;
            _canImportGrowers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Growers.Import)).Succeeded;

            await GetGrowersAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetGrowersAsync()
        {
            var response = await GrowersManager.GetAllAsync();
            if (response.Succeeded)
            {
                _growersList = response.Data.ToList();
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
                var response = await GrowersManager.DeleteAsync(id);
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
            var response = await GrowersManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Growers).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Growers exported"]
                    : _localizer["Filtered Growers exported"], Severity.Success);
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
                _growers = _growersList.FirstOrDefault(c => c.Id == id);
                if (_growers != null)
                {
                    parameters.Add(nameof(AddEditGrowersModal.AddEditGrowerModel), new AddEditGrowerCommand
                    {
                        Id = _growers.Id,
                        NumberofBirds = _growers.NumberofBirds,
                        Disease = _growers.Disease,
                        Mortality = _growers.Mortality,
                        Vaccination = _growers.Vaccination,
                        Medication = _growers.Medication,
                        Feed = _growers.Feed,
                        TypeofFeed = _growers.TypeofFeed,
                        Remarks = _growers.Remarks
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditGrowersModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportGrowersCommand { UploadRequest = uploadFile };
            var result = await GrowersManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), _localizer["Growers"].ToString() }
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
            _growers = new GetAllGrowersResponse();
            await GetGrowersAsync();
        }

        private bool Search(GetAllGrowersResponse growers)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (growers.Medication?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (growers.Disease?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (growers.Vaccination?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (growers.TypeofFeed?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return growers.Remarks?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}