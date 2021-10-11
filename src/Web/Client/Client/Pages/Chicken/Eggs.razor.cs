using Famtela.Application.Features.Eggs.Queries.GetAll;
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
using Famtela.Application.Features.Eggs.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Chicken.Egg;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Famtela.Application.Features.Eggs.Commands.Import;
using Famtela.Shared.Wrapper;
using Famtela.Application.Requests;
using Famtela.Client.Shared.Components;

namespace Famtela.Client.Pages.Chicken
{
    public partial class Eggs
    {
        [Inject] private IEggManager EggManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllEggsResponse> _eggList = new();
        private GetAllEggsResponse _egg = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateEggs;
        private bool _canEditEggs;
        private bool _canDeleteEggs;
        private bool _canExportEggs;
        private bool _canSearchEggs;
        private bool _canImportEggs;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateEggs = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Eggs.Create)).Succeeded;
            _canEditEggs = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Eggs.Edit)).Succeeded;
            _canDeleteEggs = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Eggs.Delete)).Succeeded;
            _canExportEggs = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Eggs.Export)).Succeeded;
            _canSearchEggs = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Eggs.Search)).Succeeded;
            _canImportEggs = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Eggs.Import)).Succeeded;

            await GetEggsAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetEggsAsync()
        {
            var response = await EggManager.GetAllAsync();
            if (response.Succeeded)
            {
                _eggList = response.Data.ToList();
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
                var response = await EggManager.DeleteAsync(id);
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
            var response = await EggManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Eggs).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Egg records exported"]
                    : _localizer["Filtered Egg records exported"], Severity.Success);
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
                _egg = _eggList.FirstOrDefault(c => c.Id == id);
                if (_egg != null)
                {
                    parameters.Add(nameof(AddEditEggModal.AddEditEggModel), new AddEditEggCommand
                    {
                        Id = _egg.Id,
                        Sold = _egg.Sold,
                        UnitPrice = _egg.UnitPrice,
                        Retained = _egg.Retained,
                        Rejected = _egg.Rejected,
                        Home = _egg.Home,
                        Transport = _egg.Transport,
                        Remarks = _egg.Remarks
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditEggModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportEggsCommand { UploadRequest = uploadFile };
            var result = await EggManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), _localizer["Egg records"].ToString() }
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
            _egg = new GetAllEggsResponse();
            await GetEggsAsync();
        }

        private bool Search(GetAllEggsResponse egg)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return egg.Remarks?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}