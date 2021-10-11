using Famtela.Application.Features.Breeds.Queries.GetAll;
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
using Famtela.Application.Features.Breeds.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Dairy.Breed;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Famtela.Application.Features.Breeds.Commands.Import;
using Famtela.Shared.Wrapper;
using Famtela.Application.Requests;
using Famtela.Client.Shared.Components;

namespace Famtela.Client.Pages.Dairy
{
    public partial class Breeds
    {
        [Inject] private IBreedManager BreedManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllBreedsResponse> _breedList = new();
        private GetAllBreedsResponse _breed = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateBreeds;
        private bool _canEditBreeds;
        private bool _canDeleteBreeds;
        private bool _canExportBreeds;
        private bool _canSearchBreeds;
        private bool _canImportBreeds;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateBreeds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Breeds.Create)).Succeeded;
            _canEditBreeds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Breeds.Edit)).Succeeded;
            _canDeleteBreeds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Breeds.Delete)).Succeeded;
            _canExportBreeds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Breeds.Export)).Succeeded;
            _canSearchBreeds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Breeds.Search)).Succeeded;
            _canImportBreeds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Breeds.Import)).Succeeded;

            await GetBreedsAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetBreedsAsync()
        {
            var response = await BreedManager.GetAllAsync();
            if (response.Succeeded)
            {
                _breedList = response.Data.ToList();
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
                var response = await BreedManager.DeleteAsync(id);
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
            var response = await BreedManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Breeds).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Breeds exported"]
                    : _localizer["Filtered Breeds exported"], Severity.Success);
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
                _breed = _breedList.FirstOrDefault(c => c.Id == id);
                if (_breed != null)
                {
                    parameters.Add(nameof(AddEditBreedModal.AddEditBreedModel), new AddEditBreedCommand
                    {
                        Id = _breed.Id,
                        Name = _breed.Name
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditBreedModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportBreedsCommand { UploadRequest = uploadFile };
            var result = await BreedManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), _localizer["Breeds"].ToString() }
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
            _breed = new GetAllBreedsResponse();
            await GetBreedsAsync();
        }

        private bool Search(GetAllBreedsResponse breed)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return breed.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}