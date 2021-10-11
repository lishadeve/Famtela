namespace Famtela.Client.Infrastructure.Routes
{
    public static class LayersEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/layers/export";
        public static string GetAll = "api/v1/layers";
        public static string Delete = "api/v1/layers";
        public static string Save = "api/v1/layers";
        public static string GetCount = "api/v1/layers/count";
        public static string Import = "api/v1/layers/import";
    }
}