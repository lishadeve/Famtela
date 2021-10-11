using Famtela.Application.Features.TypesofFeed.Queries.GetAll;
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
using Famtela.Application.Features.TypesofFeed.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Chicken.TypeofFeed;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Famtela.Application.Features.TypesofFeed.Commands.Import;
using Famtela.Shared.Wrapper;
using Famtela.Application.Requests;
using Famtela.Client.Shared.Components;

namespace Famtela.Client.Pages.Chicken
{
    public partial class TypesofFeed
    {
        [Inject] private ITypeofFeedManager TypeofFeedManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllTypesofFeedResponse> _typeoffeedList = new();
        private GetAllTypesofFeedResponse _typeoffeed = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateTypesofFeed;
        private bool _canEditTypesofFeed;
        private bool _canDeleteTypesofFeed;
        private bool _canExportTypesofFeed;
        private bool _canSearchTypesofFeed;
        private bool _canImportTypesofFeed;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateTypesofFeed = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TypesofFeed.Create)).Succeeded;
            _canEditTypesofFeed = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TypesofFeed.Edit)).Succeeded;
            _canDeleteTypesofFeed = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TypesofFeed.Delete)).Succeeded;
            _canExportTypesofFeed = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TypesofFeed.Export)).Succeeded;
            _canSearchTypesofFeed = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TypesofFeed.Search)).Succeeded;
            _canImportTypesofFeed = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.TypesofFeed.Import)).Succeeded;

            await GetTypesofFeedAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetTypesofFeedAsync()
        {
            var response = await TypeofFeedManager.GetAllAsync();
            if (response.Succeeded)
            {
                _typeoffeedList = response.Data.ToList();
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
                var response = await TypeofFeedManager.DeleteAsync(id);
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
            var response = await TypeofFeedManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(TypesofFeed).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Chicken Expenses exported"]
                    : _localizer["Filtered Chicken Expenses exported"], Severity.Success);
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
                _typeoffeed = _typeoffeedList.FirstOrDefault(c => c.Id == id);
                if (_typeoffeed != null)
                {
                    parameters.Add(nameof(AddEditTypeofFeedModal.AddEditTypeofFeedModel), new AddEditTypeofFeedCommand
                    {
                        Id = _typeoffeed.Id,
                        Name = _typeoffeed.Name
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditTypeofFeedModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportTypesofFeedCommand { UploadRequest = uploadFile };
            var result = await TypeofFeedManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), _localizer["Chicken Expenses"].ToString() }
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
            _typeoffeed = new GetAllTypesofFeedResponse();
            await GetTypesofFeedAsync();
        }

        private bool Search(GetAllTypesofFeedResponse typeoffeed)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return typeoffeed.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}