namespace Famtela.Shared.Constants.Application
{
    public static class ApplicationConstants
    {
        public static class SignalR
        {
            public const string HubUrl = "/signalRHub";
            public const string SendUpdateDashboard = "UpdateDashboardAsync";
            public const string ReceiveUpdateDashboard = "UpdateDashboard";
            public const string SendRegenerateTokens = "RegenerateTokensAsync";
            public const string ReceiveRegenerateTokens = "RegenerateTokens";
            public const string ReceiveChatNotification = "ReceiveChatNotification";
            public const string SendChatNotification = "ChatNotificationAsync";
            public const string ReceiveMessage = "ReceiveMessage";
            public const string SendMessage = "SendMessageAsync";

            public const string OnConnect = "OnConnectAsync";
            public const string ConnectUser = "ConnectUser";
            public const string OnDisconnect = "OnDisconnectAsync";
            public const string DisconnectUser = "DisconnectUser";
            public const string OnChangeRolePermissions = "OnChangeRolePermissions";
            public const string LogoutUsersByRole = "LogoutUsersByRole";

            public const string PingRequest = "PingRequestAsync";
            public const string PingResponse = "PingResponseAsync";
        }
        public static class Cache
        {
            public const string GetAllAgesCacheKey = "all-ages";
            public const string GetAllBreedsCacheKey = "all-breeds";
            public const string GetAllChickenExpensesCacheKey = "all-chickenexpenses";
            public const string GetAllChicksCacheKey = "all-chicks";
            public const string GetAllColorsCacheKey = "all-colors";
            public const string GetAllCountiesCacheKey = "all-counties";
            public const string GetAllCowsCacheKey = "all-cows";
            public const string GetAllDairyExpensesCacheKey = "all-dairyexpenses";
            public const string GetAllDiseasesCacheKey = "all-diseases";
            public const string GetAllDocumentTypesCacheKey = "all-document-types";
            public const string GetAllEggsCacheKey = "all-eggs";
            public const string GetAllGrowersCacheKey = "all-growers";
            public const string GetAllLayersCacheKey = "all-layers";
            public const string GetAllStatusesCacheKey = "all-statuses";
            public const string GetAllTagsCacheKey = "all-tags";
            public const string GetAllTypesofFarmingCacheKey = "all-typesoffarming";
            public const string GetAllTypesofFeedCacheKey = "all-typesoffeed";
            public const string GetAllWeightEstimatesCacheKey = "all-weightestimates";

            public static string GetAllEntityExtendedAttributesCacheKey(string entityFullName)
            {
                return $"all-{entityFullName}-extended-attributes";
            }

            public static string GetAllEntityExtendedAttributesByEntityIdCacheKey<TEntityId>(string entityFullName, TEntityId entityId)
            {
                return $"all-{entityFullName}-extended-attributes-{entityId}";
            }
        }

        public static class MimeTypes
        {
            public const string OpenXml = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        }
    }
}