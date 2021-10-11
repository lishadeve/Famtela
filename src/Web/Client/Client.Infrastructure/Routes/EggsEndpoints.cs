namespace Famtela.Client.Infrastructure.Routes
{
    public static class EggsEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/eggs/export";
        public static string GetAll = "api/v1/eggs";
        public static string Delete = "api/v1/eggs";
        public static string Save = "api/v1/eggs";
        public static string GetCount = "api/v1/eggs/count";
        public static string Import = "api/v1/eggs/import";
    }
}