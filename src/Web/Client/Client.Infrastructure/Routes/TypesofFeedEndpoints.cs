namespace Famtela.Client.Infrastructure.Routes
{
    public static class TypesofFeedEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/typesoffeed/export";
        public static string GetAll = "api/v1/typesoffeed";
        public static string Delete = "api/v1/typesoffeed";
        public static string Save = "api/v1/typesoffeed";
        public static string GetCount = "api/v1/typesoffeed/count";
        public static string Import = "api/v1/typesoffeed/import";
    }
}