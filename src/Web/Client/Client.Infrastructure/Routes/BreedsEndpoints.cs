namespace Famtela.Client.Infrastructure.Routes
{
    public static class BreedsEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/breeds/export";
        public static string GetAll = "api/v1/breeds";
        public static string Delete = "api/v1/breeds";
        public static string Save = "api/v1/breeds";
        public static string GetCount = "api/v1/breeds/count";
        public static string Import = "api/v1/breeds/import";
    }
}