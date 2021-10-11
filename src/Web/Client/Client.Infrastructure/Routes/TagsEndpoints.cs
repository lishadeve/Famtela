namespace Famtela.Client.Infrastructure.Routes
{
    public static class TagsEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/tags/export";
        public static string GetAll = "api/v1/tags";
        public static string Delete = "api/v1/tags";
        public static string Save = "api/v1/tags";
        public static string GetCount = "api/v1/tags/count";
        public static string Import = "api/v1/tags/import";
    }
}