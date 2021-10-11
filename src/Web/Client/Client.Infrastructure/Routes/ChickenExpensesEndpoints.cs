namespace Famtela.Client.Infrastructure.Routes
{
    public static class ChickenExpensesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/chickenexpenses/export";
        public static string GetAll = "api/v1/chickenexpenses";
        public static string Delete = "api/v1/chickenexpenses";
        public static string Save = "api/v1/chickenexpenses";
        public static string GetCount = "api/v1/chickenexpenses/count";
        public static string Import = "api/v1/chickenexpenses/import";
    }
}