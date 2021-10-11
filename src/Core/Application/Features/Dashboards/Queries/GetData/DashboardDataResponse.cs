using System.Collections.Generic;

namespace Famtela.Application.Features.Dashboards.Queries.GetData
{
    public class DashboardDataResponse
    {
        public int FarmProfileCount { get; set; }
        public int CountyCount { get; set; }
        public int TypeofFarmingCount { get; set; }
        public int AgeCount { get; set; }
        public int ChicksCount { get; set; }
        public int ChickenExpenseCount { get; set; }
        public int ConsumptionCount { get; set; }
        public int DiseaseCount { get; set; }
        public int EggCount { get; set; }
        public int GrowersCount { get; set; }
        public int LayersCount { get; set; }
        public int TypeofFeedCount { get; set; }
        public int VaccinationCount { get; set; }
        public int BreedCount { get; set; }
        public int ColorCount { get; set; }
        public int CowCount { get; set; }
        public int DairyExpenseCount { get; set; }
        public int MilkCount { get; set; }
        public int StatusCount { get; set; }
        public int TagCount { get; set; }
        public int WeightEstimateCount { get; set; }
        public int DocumentCount { get; set; }
        public int DocumentTypeCount { get; set; }
        public int DocumentExtendedAttributeCount { get; set; }
        public int UserCount { get; set; }
        public int RoleCount { get; set; }
        public List<ChartSeries> DataEnterBarChart { get; set; } = new();
        public Dictionary<string, double> FarmProfileByCountyTypePieChart { get; set; }
    }

    public class ChartSeries
    {
        public ChartSeries() { }

        public string Name { get; set; }
        public double[] Data { get; set; }
    }

}