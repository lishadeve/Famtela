using Famtela.Application.Features.ChickenExpenses.Queries.GetAll;
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
using Famtela.Application.Features.ChickenExpenses.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Chicken.ChickenExpense;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Famtela.Application.Features.ChickenExpenses.Commands.Import;
using Famtela.Shared.Wrapper;
using Famtela.Application.Requests;
using Famtela.Client.Shared.Components;

namespace Famtela.Client.Pages.Chicken
{
    public partial class ChickenExpenses
    {
        [Inject] private IChickenExpenseManager ChickenExpenseManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllChickenExpensesResponse> _chickenexpenseList = new();
        private GetAllChickenExpensesResponse _chickenexpense = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateChickenExpenses;
        private bool _canEditChickenExpenses;
        private bool _canDeleteChickenExpenses;
        private bool _canExportChickenExpenses;
        private bool _canSearchChickenExpenses;
        private bool _canImportChickenExpenses;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateChickenExpenses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ChickenExpenses.Create)).Succeeded;
            _canEditChickenExpenses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ChickenExpenses.Edit)).Succeeded;
            _canDeleteChickenExpenses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ChickenExpenses.Delete)).Succeeded;
            _canExportChickenExpenses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ChickenExpenses.Export)).Succeeded;
            _canSearchChickenExpenses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ChickenExpenses.Search)).Succeeded;
            _canImportChickenExpenses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ChickenExpenses.Import)).Succeeded;

            await GetChickenExpensesAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetChickenExpensesAsync()
        {
            var response = await ChickenExpenseManager.GetAllAsync();
            if (response.Succeeded)
            {
                _chickenexpenseList = response.Data.ToList();
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
                var response = await ChickenExpenseManager.DeleteAsync(id);
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
            var response = await ChickenExpenseManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(ChickenExpenses).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
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
                _chickenexpense = _chickenexpenseList.FirstOrDefault(c => c.Id == id);
                if (_chickenexpense != null)
                {
                    parameters.Add(nameof(AddEditChickenExpenseModal.AddEditChickenExpenseModel), new AddEditChickenExpenseCommand
                    {
                        Id = _chickenexpense.Id,
                        Description = _chickenexpense.Description,
                        Quantity = _chickenexpense.Quantity,
                        UnitCost = _chickenexpense.UnitCost,
                        Transport = _chickenexpense.Transport
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditChickenExpenseModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportChickenExpensesCommand { UploadRequest = uploadFile };
            var result = await ChickenExpenseManager.ImportAsync(request);
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
            _chickenexpense = new GetAllChickenExpensesResponse();
            await GetChickenExpensesAsync();
        }

        private bool Search(GetAllChickenExpensesResponse chickenexpense)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return chickenexpense.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}