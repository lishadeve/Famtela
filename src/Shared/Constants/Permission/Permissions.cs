using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Famtela.Shared.Constants.Permission
{
    public static class Permissions
    {
        [DisplayName("Audit Trails")]
        [Description("Audit Trails Permissions")]
        public static class AuditTrails
        {
            public const string View = "Permissions.AuditTrails.View";
            public const string Export = "Permissions.AuditTrails.Export";
            public const string Search = "Permissions.AuditTrails.Search";
        }

        [DisplayName("Ages")]
        [Description("Ages Permissions")]
        public static class Ages
        {
            public const string View = "Permissions.Ages.View";
            public const string Create = "Permissions.Ages.Create";
            public const string Edit = "Permissions.Ages.Edit";
            public const string Delete = "Permissions.Ages.Delete";
            public const string Export = "Permissions.Ages.Export";
            public const string Search = "Permissions.Ages.Search";
            public const string Import = "Permissions.Ages.Import";
        }

        [DisplayName("Breeds")]
        [Description("Breeds Permissions")]
        public static class Breeds
        {
            public const string View = "Permissions.Breeds.View";
            public const string Create = "Permissions.Breeds.Create";
            public const string Edit = "Permissions.Breeds.Edit";
            public const string Delete = "Permissions.Breeds.Delete";
            public const string Export = "Permissions.Breeds.Export";
            public const string Search = "Permissions.Breeds.Search";
            public const string Import = "Permissions.Breeds.Import";
        }

        [DisplayName("Chicks")]
        [Description("Chicks Permissions")]
        public static class Chicks
        {
            public const string View = "Permissions.Chicks.View";
            public const string Create = "Permissions.Chicks.Create";
            public const string Edit = "Permissions.Chicks.Edit";
            public const string Delete = "Permissions.Chicks.Delete";
            public const string Export = "Permissions.Chicks.Export";
            public const string Search = "Permissions.Chicks.Search";
            public const string Import = "Permissions.Chicks.Import";
        }

        [DisplayName("Chicken Expenses")]
        [Description("Chicken Expenses Permissions")]
        public static class ChickenExpenses
        {
            public const string View = "Permissions.ChickenExpenses.View";
            public const string Create = "Permissions.ChickenExpenses.Create";
            public const string Edit = "Permissions.ChickenExpenses.Edit";
            public const string Delete = "Permissions.ChickenExpenses.Delete";
            public const string Export = "Permissions.ChickenExpenses.Export";
            public const string Search = "Permissions.ChickenExpenses.Search";
            public const string Import = "Permissions.ChickenExpenses.Import";
        }

        [DisplayName("Colors")]
        [Description("Colors Permissions")]
        public static class Colors
        {
            public const string View = "Permissions.Colors.View";
            public const string Create = "Permissions.Colors.Create";
            public const string Edit = "Permissions.Colors.Edit";
            public const string Delete = "Permissions.Colors.Delete";
            public const string Export = "Permissions.Colors.Export";
            public const string Search = "Permissions.Colors.Search";
            public const string Import = "Permissions.Colors.Import";
        }

        [DisplayName("Communication")]
        [Description("Communication Permissions")]
        public static class Communication
        {
            public const string Chat = "Permissions.Communication.Chat";
        }

        [DisplayName("Consumptions")]
        [Description("Consumptions Permissions")]
        public static class Consumptions
        {
            public const string View = "Permissions.Consumptions.View";
            public const string Create = "Permissions.Consumptions.Create";
            public const string Edit = "Permissions.Consumptions.Edit";
            public const string Delete = "Permissions.Consumptions.Delete";
            public const string Export = "Permissions.Consumptions.Export";
            public const string Search = "Permissions.Consumptions.Search";
            public const string Import = "Permissions.Consumptions.Import";
        }

        [DisplayName("Counties")]
        [Description("Counties Permissions")]
        public static class Counties
        {
            public const string View = "Permissions.Counties.View";
            public const string Create = "Permissions.Counties.Create";
            public const string Edit = "Permissions.Counties.Edit";
            public const string Delete = "Permissions.Counties.Delete";
            public const string Export = "Permissions.Counties.Export";
            public const string Search = "Permissions.Counties.Search";
            public const string Import = "Permissions.Counties.Import";
        }

        [DisplayName("Cows")]
        [Description("Cows Permissions")]
        public static class Cows
        {
            public const string View = "Permissions.Cows.View";
            public const string Create = "Permissions.Cows.Create";
            public const string Edit = "Permissions.Cows.Edit";
            public const string Delete = "Permissions.Cows.Delete";
            public const string Export = "Permissions.Cows.Export";
            public const string Search = "Permissions.Cows.Search";
            public const string Import = "Permissions.Cows.Import";
        }

        [DisplayName("Dashboards")]
        [Description("Dashboards Permissions")]
        public static class Dashboards
        {
            public const string View = "Permissions.Dashboards.View";
        }

        [DisplayName("Documents")]
        [Description("Documents Permissions")]
        public static class Documents
        {
            public const string View = "Permissions.Documents.View";
            public const string Create = "Permissions.Documents.Create";
            public const string Edit = "Permissions.Documents.Edit";
            public const string Delete = "Permissions.Documents.Delete";
            public const string Search = "Permissions.Documents.Search";
        }

        [DisplayName("Document Types")]
        [Description("Document Types Permissions")]
        public static class DocumentTypes
        {
            public const string View = "Permissions.DocumentTypes.View";
            public const string Create = "Permissions.DocumentTypes.Create";
            public const string Edit = "Permissions.DocumentTypes.Edit";
            public const string Delete = "Permissions.DocumentTypes.Delete";
            public const string Export = "Permissions.DocumentTypes.Export";
            public const string Search = "Permissions.DocumentTypes.Search";
        }

        [DisplayName("Document Extended Attributes")]
        [Description("Document Extended Attributes Permissions")]
        public static class DocumentExtendedAttributes
        {
            public const string View = "Permissions.DocumentExtendedAttributes.View";
            public const string Create = "Permissions.DocumentExtendedAttributes.Create";
            public const string Edit = "Permissions.DocumentExtendedAttributes.Edit";
            public const string Delete = "Permissions.DocumentExtendedAttributes.Delete";
            public const string Export = "Permissions.DocumentExtendedAttributes.Export";
            public const string Search = "Permissions.DocumentExtendedAttributes.Search";
        }

        [DisplayName("Dairy Expenses")]
        [Description("Dairy Expenses Permissions")]
        public static class DairyExpenses
        {
            public const string View = "Permissions.DairyExpenses.View";
            public const string Create = "Permissions.DairyExpenses.Create";
            public const string Edit = "Permissions.DairyExpenses.Edit";
            public const string Delete = "Permissions.DairyExpenses.Delete";
            public const string Export = "Permissions.DairyExpenses.Export";
            public const string Search = "Permissions.DairyExpenses.Search";
            public const string Import = "Permissions.DairyExpenses.Import";
        }

        [DisplayName("Diseases")]
        [Description("Diseases Permissions")]
        public static class Diseases
        {
            public const string View = "Permissions.Diseases.View";
            public const string Create = "Permissions.Diseases.Create";
            public const string Edit = "Permissions.Diseases.Edit";
            public const string Delete = "Permissions.Diseases.Delete";
            public const string Export = "Permissions.Diseases.Export";
            public const string Search = "Permissions.Diseases.Search";
            public const string Import = "Permissions.Diseases.Import";
        }

        [DisplayName("Eggs")]
        [Description("Eggs Permissions")]
        public static class Eggs
        {
            public const string View = "Permissions.Eggs.View";
            public const string Create = "Permissions.Eggs.Create";
            public const string Edit = "Permissions.Eggs.Edit";
            public const string Delete = "Permissions.Eggs.Delete";
            public const string Export = "Permissions.Eggs.Export";
            public const string Search = "Permissions.Eggs.Search";
            public const string Import = "Permissions.Eggs.Import";
        }

        [DisplayName("Farm Profiles")]
        [Description("Farm Profiles Permissions")]
        public static class FarmProfiles
        {
            public const string View = "Permissions.FarmProfiles.View";
            public const string Create = "Permissions.FarmProfiles.Create";
            public const string Edit = "Permissions.FarmProfiles.Edit";
            public const string Delete = "Permissions.FarmProfiles.Delete";
            public const string Export = "Permissions.FarmProfiles.Export";
            public const string Search = "Permissions.FarmProfiles.Search";
        }

        [DisplayName("Growers")]
        [Description("Growers Permissions")]
        public static class Growers
        {
            public const string View = "Permissions.Growers.View";
            public const string Create = "Permissions.Growers.Create";
            public const string Edit = "Permissions.Growers.Edit";
            public const string Delete = "Permissions.Growers.Delete";
            public const string Export = "Permissions.Growers.Export";
            public const string Search = "Permissions.Growers.Search";
            public const string Import = "Permissions.Growers.Import";
        }

        [DisplayName("Hangfire")]
        [Description("Hangfire Permissions")]
        public static class Hangfire
        {
            public const string View = "Permissions.Hangfire.View";
        }

        [DisplayName("Layers")]
        [Description("Layers Permissions")]
        public static class Layers
        {
            public const string View = "Permissions.Layers.View";
            public const string Create = "Permissions.Layers.Create";
            public const string Edit = "Permissions.Layers.Edit";
            public const string Delete = "Permissions.Layers.Delete";
            public const string Export = "Permissions.Layers.Export";
            public const string Search = "Permissions.Layers.Search";
            public const string Import = "Permissions.Layers.Import";
        }

        [DisplayName("Milk")]
        [Description("Milk Permissions")]
        public static class Milk
        {
            public const string View = "Permissions.Milk.View";
            public const string Create = "Permissions.Milk.Create";
            public const string Edit = "Permissions.Milk.Edit";
            public const string Delete = "Permissions.Milk.Delete";
            public const string Export = "Permissions.Milk.Export";
            public const string Search = "Permissions.Milk.Search";
            public const string Import = "Permissions.Milk.Import";
        }

        [DisplayName("Preferences")]
        [Description("Preferences Permissions")]
        public static class Preferences
        {
            public const string ChangeLanguage = "Permissions.Preferences.ChangeLanguage";
        }

        [DisplayName("Roles")]
        [Description("Roles Permissions")]
        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
            public const string Search = "Permissions.Roles.Search";
        }

        [DisplayName("Role Claims")]
        [Description("Role Claims Permissions")]
        public static class RoleClaims
        {
            public const string View = "Permissions.RoleClaims.View";
            public const string Create = "Permissions.RoleClaims.Create";
            public const string Edit = "Permissions.RoleClaims.Edit";
            public const string Delete = "Permissions.RoleClaims.Delete";
            public const string Search = "Permissions.RoleClaims.Search";
        }

        [DisplayName("Tags")]
        [Description("Tags Permissions")]
        public static class Tags
        {
            public const string View = "Permissions.Tags.View";
            public const string Create = "Permissions.Tags.Create";
            public const string Edit = "Permissions.Tags.Edit";
            public const string Delete = "Permissions.Tags.Delete";
            public const string Export = "Permissions.Tags.Export";
            public const string Search = "Permissions.Tags.Search";
            public const string Import = "Permissions.Tags.Import";
        }

        [DisplayName("Types of Farming")]
        [Description("Types of Farming Permissions")]
        public static class TypesofFarming
        {
            public const string View = "Permissions.TypesofFarming.View";
            public const string Create = "Permissions.TypesofFarming.Create";
            public const string Edit = "Permissions.TypesofFarming.Edit";
            public const string Delete = "Permissions.TypesofFarming.Delete";
            public const string Export = "Permissions.TypesofFarming.Export";
            public const string Search = "Permissions.TypesofFarming.Search";
            public const string Import = "Permissions.TypesofFarming.Import";
        }

        [DisplayName("Types of Feed")]
        [Description("Types of Feed Permissions")]
        public static class TypesofFeed
        {
            public const string View = "Permissions.TypesofFeed.View";
            public const string Create = "Permissions.TypesofFeed.Create";
            public const string Edit = "Permissions.TypesofFeed.Edit";
            public const string Delete = "Permissions.TypesofFeed.Delete";
            public const string Export = "Permissions.TypesofFeed.Export";
            public const string Search = "Permissions.TypesofFeed.Search";
            public const string Import = "Permissions.TypesofFeed.Import";
        }

        [DisplayName("Statuses")]
        [Description("Statuses Permissions")]
        public static class Statuses
        {
            public const string View = "Permissions.Statuses.View";
            public const string Create = "Permissions.Statuses.Create";
            public const string Edit = "Permissions.Statuses.Edit";
            public const string Delete = "Permissions.Statuses.Delete";
            public const string Export = "Permissions.Statuses.Export";
            public const string Search = "Permissions.Statuses.Search";
            public const string Import = "Permissions.Statuses.Import";
        }

        [DisplayName("Users")]
        [Description("Users Permissions")]
        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
            public const string Export = "Permissions.Users.Export";
            public const string Search = "Permissions.Users.Search";
        }

        [DisplayName("Vaccinations")]
        [Description("Vaccinations Permissions")]
        public static class Vaccinations
        {
            public const string View = "Permissions.Vaccinations.View";
            public const string Create = "Permissions.Vaccinations.Create";
            public const string Edit = "Permissions.Vaccinations.Edit";
            public const string Delete = "Permissions.Vaccinations.Delete";
            public const string Export = "Permissions.Vaccinations.Export";
            public const string Search = "Permissions.Vaccinations.Search";
            public const string Import = "Permissions.Vaccinations.Import";
        }

        [DisplayName("Weight Estimates")]
        [Description("Weight Estimates Permissions")]
        public static class WeightEstimates
        {
            public const string View = "Permissions.WeightEstimates.View";
            public const string Create = "Permissions.WeightEstimates.Create";
            public const string Edit = "Permissions.WeightEstimates.Edit";
            public const string Delete = "Permissions.WeightEstimates.Delete";
            public const string Export = "Permissions.WeightEstimates.Export";
            public const string Search = "Permissions.WeightEstimates.Search";
            public const string Import = "Permissions.WeightEstimates.Import";
        }

        /// <summary>
        /// Returns a list of Permissions.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRegisteredPermissions()
        {
            var permissions = new List<string>();
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    permissions.Add(propertyValue.ToString());
            }
            return permissions;
        }
    }
}