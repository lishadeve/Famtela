namespace Famtela.Client.Infrastructure.Routes
{
    public static class GrowersEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/growers/export";
        public static string GetAll = "api/v1/growers";
        public static string Delete = "api/v1/growers";
        public static string Save = "api/v1/growers";
        public static string GetCount = "api/v1/growers/count";
        public static string Import = "api/v1/growers/import";
    }
}