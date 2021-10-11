namespace Famtela.Client.Infrastructure.Routes
{
    public static class StatusesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/statuses/export";
        public static string GetAll = "api/v1/statuses";
        public static string Delete = "api/v1/statuses";
        public static string Save = "api/v1/statuses";
        public static string GetCount = "api/v1/statuses/count";
        public static string Import = "api/v1/statuses/import";
    }
}