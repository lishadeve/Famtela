namespace Famtela.Client.Infrastructure.Routes
{
    public static class DiseasesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/diseases/export";
        public static string GetAll = "api/v1/diseases";
        public static string Delete = "api/v1/diseases";
        public static string Save = "api/v1/diseases";
        public static string GetCount = "api/v1/diseases/count";
        public static string Import = "api/v1/diseases/import";
    }
}