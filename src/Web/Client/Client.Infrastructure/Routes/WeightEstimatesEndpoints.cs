namespace Famtela.Client.Infrastructure.Routes
{
    public static class WeightEstimatesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/weightestimates/export";
        public static string GetAll = "api/v1/weightestimates";
        public static string Delete = "api/v1/weightestimates";
        public static string Save = "api/v1/weightestimates";
        public static string GetCount = "api/v1/weightestimates/count";
        public static string Import = "api/v1/weightestimates/import";
    }
}