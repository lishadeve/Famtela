﻿using System.Linq;

namespace Famtela.Client.Infrastructure.Routes
{
    public static class MilkEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/milk?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            if (orderBy?.Any() == true)
            {
                foreach (var orderByPart in orderBy)
                {
                    url += $"{orderByPart},";
                }
                url = url[..^1]; // loose training ,
            }
            return url;
        }

        public static string GetCount = "api/v1/milk/count";

        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Save = "api/v1/milk";
        public static string Delete = "api/v1/milk";
        public static string Export = "api/v1/milk/export";
        public static string GetAll = "api/v1/milk";
    }
}