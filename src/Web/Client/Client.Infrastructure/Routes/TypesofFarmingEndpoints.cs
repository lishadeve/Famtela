namespace Famtela.Client.Infrastructure.Routes
{
    public static class TypesofFarmingEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/typesoffarming/export";
        public static string GetAll = "api/v1/typesoffarming";
        public static string Delete = "api/v1/typesoffarming";
        public static string Save = "api/v1/typesoffarming";
        public static string GetCount = "api/v1/typesoffarming/count";
        public static string Import = "api/v1/typesoffarming/import";
    }
}