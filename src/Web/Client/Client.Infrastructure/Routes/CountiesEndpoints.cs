namespace Famtela.Client.Infrastructure.Routes
{
    public static class CountiesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/counties/export";
        public static string GetAll = "api/v1/counties";
        public static string Delete = "api/v1/counties";
        public static string Save = "api/v1/counties";
        public static string GetCount = "api/v1/counties/count";
        public static string Import = "api/v1/counties/import";
  }
}