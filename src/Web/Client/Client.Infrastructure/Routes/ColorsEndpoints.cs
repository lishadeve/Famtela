namespace Famtela.Client.Infrastructure.Routes
{
    public static class ColorsEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/colors/export";
        public static string GetAll = "api/v1/colors";
        public static string Delete = "api/v1/colors";
        public static string Save = "api/v1/colors";
        public static string GetCount = "api/v1/colors/count";
        public static string Import = "api/v1/colors/import";
    }
}