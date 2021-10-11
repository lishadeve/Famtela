﻿using Famtela.Application.Features.FarmProfiles.Queries.GetAllPaged;
using Famtela.Application.Requests.Catalog;
using Famtela.Client.Extensions;
using Famtela.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Famtela.Application.Features.FarmProfiles.Commands.AddEdit;
using Famtela.Client.Infrastructure.Managers.Catalog.FarmProfile;
using Famtela.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;

namespace Famtela.Client.Pages.Catalog
{
    public partial class FarmProfiles
    {
        [Inject] private IFarmProfileManager FarmProfileManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private IEnumerable<GetAllPagedFarmProfilesResponse> _pagedData;
        private MudTable<GetAllPagedFarmProfilesResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateFarmProfiles;
        private bool _canEditFarmProfiles;
        private bool _canDeleteFarmProfiles;
        private bool _canExportFarmProfiles;
        private bool _canSearchFarmProfiles;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateFarmProfiles = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.FarmProfiles.Create)).Succeeded;
            _canEditFarmProfiles = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.FarmProfiles.Edit)).Succeeded;
            _canDeleteFarmProfiles = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.FarmProfiles.Delete)).Succeeded;
            _canExportFarmProfiles = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.FarmProfiles.Export)).Succeeded;
            _canSearchFarmProfiles = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.FarmProfiles.Search)).Succeeded;

            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task<TableData<GetAllPagedFarmProfilesResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedFarmProfilesResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] {$"{state.SortLabel} {state.SortDirection}"} : new[] {$"{state.SortLabel}"};
            }

            var request = new GetAllPagedFarmProfilesRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await FarmProfileManager.GetFarmProfilesAsync(request);
            if (response.Succeeded)
            {
                _totalItems = response.TotalCount;
                _currentPage = response.CurrentPage;
                _pagedData = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void OnSearch(string text)
        {
            _searchString = text;
            _table.ReloadServerData();
        }

        private async Task ExportToExcel()
        {
            var response = await FarmProfileManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(FarmProfiles).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Farm Profiles exported"]
                    : _localizer["Filtered Farm Profiles exported"], Severity.Success);
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
                var farmprofile = _pagedData.FirstOrDefault(c => c.Id == id);
                if (farmprofile != null)
                {
                    parameters.Add(nameof(AddEditFarmProfileModal.AddEditFarmProfileModel), new AddEditFarmProfileCommand
                    {
                        Id = farmprofile.Id,
                        FarmName = farmprofile.FarmName,
                        CountyId = farmprofile.CountyId,
                        TypeofFarmingId = farmprofile.TypeofFarmingId
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditFarmProfileModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch("");
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await FarmProfileManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    OnSearch("");
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    OnSearch("");
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
    }
}