namespace Famtela.Client.Infrastructure.Routes
{
    public static class AgesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/ages/export";
        public static string GetAll = "api/v1/ages";
        public static string Delete = "api/v1/ages";
        public static string Save = "api/v1/ages";
        public static string GetCount = "api/v1/ages/count";
        public static string Import = "api/v1/ages/import";
    }
}