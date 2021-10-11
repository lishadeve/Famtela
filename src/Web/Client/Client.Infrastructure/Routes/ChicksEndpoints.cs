namespace Famtela.Client.Infrastructure.Routes
{
    public static class ChicksEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/chicks/export";
        public static string GetAll = "api/v1/chicks";
        public static string Delete = "api/v1/chicks";
        public static string Save = "api/v1/chicks";
        public static string GetCount = "api/v1/chicks/count";
        public static string Import = "api/v1/chicks/import";
    }
}