namespace Famtela.Client.Infrastructure.Routes
{
    public static class DairyExpensesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/dairyexpenses/export";

        public static string GetAll = "api/v1/dairyexpenses";
        public static string Delete = "api/v1/dairyexpenses";
        public static string Save = "api/v1/dairyexpenses";
        public static string GetCount = "api/v1/dairyexpenses/count";
        public static string Import = "api/v1/dairyexpenses/import";
    }
}