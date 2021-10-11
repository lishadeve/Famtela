using Famtela.Application.Features.DairyExpenses.Queries.GetAll;
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
using Famtela.Application.Features.DairyExpenses.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Dairy.DairyExpense;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Famtela.Application.Features.DairyExpenses.Commands.Import;
using Famtela.Shared.Wrapper;
using Famtela.Application.Requests;
using Famtela.Client.Shared.Components;

namespace Famtela.Client.Pages.Dairy
{
    public partial class DairyExpenses
    {
        [Inject] private IDairyExpenseManager DairyExpenseManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllDairyExpensesResponse> _dairyexpenseList = new();
        private GetAllDairyExpensesResponse _dairyexpense = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateDairyExpenses;
        private bool _canEditDairyExpenses;
        private bool _canDeleteDairyExpenses;
        private bool _canExportDairyExpenses;
        private bool _canSearchDairyExpenses;
        private bool _canImportDairyExpenses;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateDairyExpenses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DairyExpenses.Create)).Succeeded;
            _canEditDairyExpenses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DairyExpenses.Edit)).Succeeded;
            _canDeleteDairyExpenses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DairyExpenses.Delete)).Succeeded;
            _canExportDairyExpenses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DairyExpenses.Export)).Succeeded;
            _canSearchDairyExpenses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DairyExpenses.Search)).Succeeded;
            _canImportDairyExpenses = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DairyExpenses.Import)).Succeeded;

            await GetDairyExpensesAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetDairyExpensesAsync()
        {
            var response = await DairyExpenseManager.GetAllAsync();
            if (response.Succeeded)
            {
                _dairyexpenseList = response.Data.ToList();
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
                var response = await DairyExpenseManager.DeleteAsync(id);
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
            var response = await DairyExpenseManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(DairyExpenses).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Dairy Expenses Record exported"]
                    : _localizer["Filtered Dairy Expenses Record exported"], Severity.Success);
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
                _dairyexpense = _dairyexpenseList.FirstOrDefault(c => c.Id == id);
                if (_dairyexpense != null)
                {
                    parameters.Add(nameof(AddEditDairyExpenseModal.AddEditDairyExpenseModel), new AddEditDairyExpenseCommand
                    {
                        Id = _dairyexpense.Id,
                        Description = _dairyexpense.Description,
                        Quantity = _dairyexpense.Quantity,
                        UnitCost = _dairyexpense.UnitCost,
                        Transport = _dairyexpense.Transport
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditDairyExpenseModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportDairyExpensesCommand { UploadRequest = uploadFile };
            var result = await DairyExpenseManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), _localizer["Dairy Expenses Record"].ToString() }
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
            _dairyexpense = new GetAllDairyExpensesResponse();
            await GetDairyExpensesAsync();
        }

        private bool Search(GetAllDairyExpensesResponse dairyexpense)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return dairyexpense.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}