using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Famtela.Client.Infrastructure.Managers.Dashboard;
using Famtela.Shared.Constants.Application;
using Famtela.Client.Extensions;

namespace Famtela.Client.Pages.Content
{
    public partial class Dashboard
    {
        [Inject] private IDashboardManager DashboardManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Parameter] public int FarmProfileCount { get; set; }
        //[Parameter] public int CountyCount { get; set; }
        //[Parameter] public int TypeofFarmingCount { get; set; }
        //[Parameter] public int AgeCount { get; set; }
        [Parameter] public int ChicksCount { get; set; }
        [Parameter] public int ChickenExpenseCount { get; set; }
        //[Parameter] public int ConsumptionCount { get; set; }
        //[Parameter] public int DiseaseCount { get; set; }
        [Parameter] public int EggCount { get; set; }
        [Parameter] public int GrowersCount { get; set; }
        [Parameter] public int LayersCount { get; set; }
        //[Parameter] public int TypeofFeedCount { get; set; }
        //[Parameter] public int VaccinationCount { get; set; }
        //[Parameter] public int BreedCount { get; set; }
        //[Parameter] public int ColorCount { get; set; }
        [Parameter] public int CowCount { get; set; }
        [Parameter] public int DairyExpenseCount { get; set; }
        [Parameter] public int MilkCount { get; set; }
        //[Parameter] public int StatusCount { get; set; }
        //[Parameter] public int TagCount { get; set; }
        //[Parameter] public int WeightEstimateCount { get; set; }
        //[Parameter] public int DocumentCount { get; set; }
        //[Parameter] public int DocumentTypeCount { get; set; }
        //[Parameter] public int DocumentExtendedAttributeCount { get; set; }
        [Parameter] public int UserCount { get; set; }
        //[Parameter] public int RoleCount { get; set; }

        private readonly string[] _dataEnterBarChartXAxisLabels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        private readonly List<ChartSeries> _dataEnterBarChartSeries = new();
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            HubConnection.On(ApplicationConstants.SignalR.ReceiveUpdateDashboard, async () =>
            {
                await LoadDataAsync();
                StateHasChanged();
            });
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            var response = await DashboardManager.GetDataAsync();
            if (response.Succeeded)
            {
                FarmProfileCount = response.Data.FarmProfileCount;
                //CountyCount = response.Data.CountyCount;
                //TypeofFarmingCount = response.Data.TypeofFarmingCount;
                //AgeCount = response.Data.AgeCount;
                ChicksCount = response.Data.ChicksCount;
                ChickenExpenseCount = response.Data.ChickenExpenseCount;
                //ConsumptionCount = response.Data.ConsumptionCount;
                //DiseaseCount = response.Data.DiseaseCount;
                EggCount = response.Data.EggCount;
                GrowersCount = response.Data.GrowersCount;
                LayersCount = response.Data.LayersCount;
                //TypeofFeedCount = response.Data.TypeofFeedCount;
                //VaccinationCount = response.Data.VaccinationCount;
                //BreedCount = response.Data.BreedCount;
                //ColorCount = response.Data.ColorCount;
                CowCount = response.Data.CowCount;
                DairyExpenseCount = response.Data.DairyExpenseCount;
                MilkCount = response.Data.MilkCount;
                //StatusCount = response.Data.StatusCount;
                //TagCount = response.Data.TagCount;
                //WeightEstimateCount = response.Data.WeightEstimateCount;
                //DocumentCount = response.Data.DocumentCount;
                //DocumentTypeCount = response.Data.DocumentTypeCount;
                //DocumentExtendedAttributeCount = response.Data.DocumentExtendedAttributeCount;
                UserCount = response.Data.UserCount;
                //RoleCount = response.Data.RoleCount;
                foreach (var item in response.Data.DataEnterBarChart)
                {
                    _dataEnterBarChartSeries
                        .RemoveAll(x => x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                    _dataEnterBarChartSeries.Add(new ChartSeries { Name = item.Name, Data = item.Data });
                }
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
    }
}