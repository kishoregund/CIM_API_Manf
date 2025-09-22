using Domain.Entities;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace Infrastructure.Identity.Constants
{
    public static class CimAction
    {
        public const string View = nameof(View);
        public const string Create = nameof(Create);
        public const string Update = nameof(Update);
        public const string Delete = nameof(Delete);
        public const string UpgradeSubscription = nameof(UpgradeSubscription);
    }

    public static class CimFeature
    {
        public const string Tenants = nameof(Tenants);
        public const string Base = nameof(Base);
        public const string Users = nameof(Users);
        public const string UserRoles = nameof(UserRoles);
        public const string Role = nameof(Role);
        public const string RoleClaims = nameof(RoleClaims);
        public const string ListTypeItems = nameof(ListTypeItems);
        public const string Country = nameof(Country);
        public const string Currency = nameof(Currency);
        public const string Spare_Part = nameof(Spare_Part);
        public const string BusinessUnit = nameof(BusinessUnit);
        public const string Brand = nameof(Brand);
        public const string User_Profile = nameof(User_Profile);
        public const string Customer = nameof(Customer);
        public const string Distributor = nameof(Distributor);
        public const string Manufacturer = nameof(Manufacturer);
        public const string Instrument = nameof(Instrument);
        public const string AMC = nameof(AMC);
        public const string Customer_Instrument = nameof(Customer_Instrument);
        public const string Service_Request = nameof(Service_Request);
        public const string Service_Report = nameof(Service_Report);
        public const string Sparepart_Quotation = nameof(Sparepart_Quotation);
        public const string Customer_Spareparts_Inventory = nameof(Customer_Spareparts_Inventory);
        public const string Spareparts_Consumed = nameof(Spareparts_Consumed);
        public const string Spareparts_Recommended = nameof(Spareparts_Recommended);
        public const string Customer_Satisfaction_Survey = nameof(Customer_Satisfaction_Survey);
        public const string Scheduler = nameof(Scheduler);
        public const string Past_Service_Report = nameof(Past_Service_Report);        
        public const string ConfigTypeValues = nameof(ConfigTypeValues);
        public const string Customer_Dashboard = nameof(Customer_Dashboard);
        public const string Distributor_Dashboard = nameof(Distributor_Dashboard);
        public const string Engineer_Dashboard = nameof(Engineer_Dashboard);
        public const string Manufacturer_Dashboard = nameof(Manufacturer_Dashboard);
        public const string Travel_Expenses = nameof(Travel_Expenses);
        public const string Travel_Invoice = nameof(Travel_Invoice);
        public const string Audit_Trail = nameof(Audit_Trail);
        public const string Advance_Request_Form = nameof(Advance_Request_Form);
        



        //public const string SRAssignedHistory = nameof(SRAssignedHistory);
        //public const string SRAuditTrails = nameof(SRAuditTrails);
        //public const string SREngActions = nameof(SREngActions);
        //public const string SREngComments = nameof(SREngComments);


        public const string Schools = nameof(Schools);
    }

    public record CimPermission(string Description, string Action, string Feature, bool IsBasic = false, bool IsRoot = false)
    {
        public string Name => NameFor(Action, Feature);
        public static string NameFor(string action, string feature) => $"Permission.{feature}.{action}";
    }

    public static class CimPermissions
    {
        private static readonly CimPermission[] AllPermissions =
        [
            new CimPermission("View Users", CimAction.View, CimFeature.Users),
            new CimPermission("Create Users", CimAction.Create, CimFeature.Users),
            new CimPermission("Update Users", CimAction.Update, CimFeature.Users),
            new CimPermission("Delete Users", CimAction.Delete, CimFeature.Users),

            new CimPermission("View User Roles", CimAction.View, CimFeature.UserRoles),
            new CimPermission("Update User Roles", CimAction.Update, CimFeature.UserRoles),

            new CimPermission("View Roles", CimAction.View, CimFeature.Role),
            new CimPermission("Create Roles", CimAction.Create, CimFeature.Role),
            new CimPermission("Update Roles", CimAction.Update, CimFeature.Role),
            new CimPermission("Delete Roles", CimAction.Delete, CimFeature.Role),

            new CimPermission("View Role Claims/Permissions", CimAction.View, CimFeature.RoleClaims),
            new CimPermission("Update Role Claims/Permissions", CimAction.Update, CimFeature.RoleClaims),

            new CimPermission("View ListTypeItems", CimAction.View, CimFeature.ListTypeItems),
            new CimPermission("Create ListTypeItems", CimAction.Create, CimFeature.ListTypeItems),
            new CimPermission("Update ListTypeItems", CimAction.Update, CimFeature.ListTypeItems),
            new CimPermission("Delete ListTypeItems", CimAction.Delete, CimFeature.ListTypeItems),

            new CimPermission("View Country", CimAction.View, CimFeature.Country),
            new CimPermission("Create Country", CimAction.Create, CimFeature.Country),
            new CimPermission("Update Country", CimAction.Update, CimFeature.Country),
            new CimPermission("Delete Country", CimAction.Delete, CimFeature.Country),

            new CimPermission("View Currency", CimAction.View, CimFeature.Currency),
            new CimPermission("Create Currency", CimAction.Create, CimFeature.Currency),
            new CimPermission("Update Currency", CimAction.Update, CimFeature.Currency),
            new CimPermission("Delete Currency", CimAction.Delete, CimFeature.Currency),

            new CimPermission("View Sparepart", CimAction.View, CimFeature.Spare_Part),
            new CimPermission("Create Sparepart", CimAction.Create, CimFeature.Spare_Part),
            new CimPermission("Update Sparepart", CimAction.Update, CimFeature.Spare_Part),
            new CimPermission("Delete Sparepart", CimAction.Delete, CimFeature.Spare_Part),
            
            new CimPermission("View UserProfile", CimAction.View, CimFeature.User_Profile),
            new CimPermission("Create UserProfile", CimAction.Create, CimFeature.User_Profile),
            new CimPermission("Update UserProfile", CimAction.Update, CimFeature.User_Profile),
            new CimPermission("Delete UserProfile", CimAction.Delete, CimFeature.User_Profile),

            new CimPermission("View BusinessUnit", CimAction.View, CimFeature.BusinessUnit),
            new CimPermission("Create BusinessUnit", CimAction.Create, CimFeature.BusinessUnit),
            new CimPermission("Update BusinessUnit", CimAction.Update, CimFeature.BusinessUnit),
            new CimPermission("Delete BusinessUnit", CimAction.Delete, CimFeature.BusinessUnit),

            new CimPermission("View Brand", CimAction.View, CimFeature.Brand),
            new CimPermission("Create Brand", CimAction.Create, CimFeature.Brand),
            new CimPermission("Update Brand", CimAction.Update, CimFeature.Brand),
            new CimPermission("Delete Brand", CimAction.Delete, CimFeature.Brand),

            new CimPermission("View Customer", CimAction.View, CimFeature.Customer),
            new CimPermission("Create Customer", CimAction.Create, CimFeature.Customer),
            new CimPermission("Update Customer", CimAction.Update, CimFeature.Customer),
            new CimPermission("Delete Customer", CimAction.Delete, CimFeature.Customer),

            new CimPermission("View Distributor", CimAction.View, CimFeature.Distributor),
            new CimPermission("Create Distributor", CimAction.Create, CimFeature.Distributor),
            new CimPermission("Update Distributor", CimAction.Update, CimFeature.Distributor),
            new CimPermission("Delete Distributor", CimAction.Delete, CimFeature.Distributor),

            new CimPermission("View Manufacturer", CimAction.View, CimFeature.Manufacturer),
            new CimPermission("Create Manufacturer", CimAction.Create, CimFeature.Manufacturer),
            new CimPermission("Update Manufacturer", CimAction.Update, CimFeature.Manufacturer),
            new CimPermission("Delete Manufacturer", CimAction.Delete, CimFeature.Manufacturer),

            new CimPermission("View Instrument", CimAction.View, CimFeature.Instrument),
            new CimPermission("Create Instrument", CimAction.Create, CimFeature.Instrument),
            new CimPermission("Update Instrument", CimAction.Update, CimFeature.Instrument),
            new CimPermission("Delete Instrument", CimAction.Delete, CimFeature.Instrument),

            new CimPermission("View AMC", CimAction.View, CimFeature.AMC),
            new CimPermission("Create AMC", CimAction.Create, CimFeature.AMC),
            new CimPermission("Update AMC", CimAction.Update, CimFeature.AMC),
            new CimPermission("Delete AMC", CimAction.Delete, CimFeature.AMC),

            new CimPermission("View CustomerInstrument", CimAction.View, CimFeature.Customer_Instrument),
            new CimPermission("Create CustomerInstrument", CimAction.Create, CimFeature.Customer_Instrument),
            new CimPermission("Update CustomerInstrument", CimAction.Update, CimFeature.Customer_Instrument),
            new CimPermission("Delete CustomerInstrument", CimAction.Delete, CimFeature.Customer_Instrument),

            new CimPermission("View ServiceRequest", CimAction.View, CimFeature.Service_Request),
            new CimPermission("Create ServiceRequest", CimAction.Create, CimFeature.Service_Request),
            new CimPermission("Update ServiceRequest", CimAction.Update, CimFeature.Service_Request),
            new CimPermission("Delete ServiceRequest", CimAction.Delete, CimFeature.Service_Request),

            new CimPermission("View ServiceReport", CimAction.View, CimFeature.Service_Report),
            new CimPermission("Create ServiceReport", CimAction.Create, CimFeature.Service_Report),
            new CimPermission("Update ServiceReport", CimAction.Update, CimFeature.Service_Report),
            new CimPermission("Delete ServiceReport", CimAction.Delete, CimFeature.Service_Report),

            new CimPermission("View SparepartQuotation", CimAction.View, CimFeature.Sparepart_Quotation),
            new CimPermission("Create SparepartQuotation", CimAction.Create, CimFeature.Sparepart_Quotation),
            new CimPermission("Update SparepartQuotation", CimAction.Update, CimFeature.Sparepart_Quotation),
            new CimPermission("Delete SparepartQuotation", CimAction.Delete, CimFeature.Sparepart_Quotation),

            new CimPermission("View SparepartInventory", CimAction.View, CimFeature.Customer_Spareparts_Inventory),
            new CimPermission("Create SparepartInventory", CimAction.Create, CimFeature.Customer_Spareparts_Inventory),
            new CimPermission("Update SparepartInventory", CimAction.Update, CimFeature.Customer_Spareparts_Inventory),
            new CimPermission("Delete SparepartInventory", CimAction.Delete, CimFeature.Customer_Spareparts_Inventory),

            new CimPermission("View SparepartConsumed", CimAction.View, CimFeature.Spareparts_Consumed),
            new CimPermission("Create SparepartConsumed", CimAction.Create, CimFeature.Spareparts_Consumed),
            new CimPermission("Update SparepartConsumed", CimAction.Update, CimFeature.Spareparts_Consumed),
            new CimPermission("Delete SparepartConsumed", CimAction.Delete, CimFeature.Spareparts_Consumed),

            new CimPermission("View SparepartRecommended", CimAction.View, CimFeature.Spareparts_Recommended),
            new CimPermission("Create SparepartRecommended", CimAction.Create, CimFeature.Spareparts_Recommended),
            new CimPermission("Update SparepartRecommended", CimAction.Update, CimFeature.Spareparts_Recommended),
            new CimPermission("Delete SparepartRecommended", CimAction.Delete, CimFeature.Spareparts_Recommended),

            new CimPermission("View CustomerSatisfactionSurvey", CimAction.View, CimFeature.Customer_Satisfaction_Survey),
            new CimPermission("Create CustomerSatisfactionSurvey", CimAction.Create, CimFeature.Customer_Satisfaction_Survey),
            new CimPermission("Update CustomerSatisfactionSurvey", CimAction.Update, CimFeature.Customer_Satisfaction_Survey),
            new CimPermission("Delete CustomerSatisfactionSurvey", CimAction.Delete, CimFeature.Customer_Satisfaction_Survey),

            new CimPermission("View Scheduler", CimAction.View, CimFeature.Scheduler),
            new CimPermission("Create Scheduler", CimAction.Create, CimFeature.Scheduler),
            new CimPermission("Update Scheduler", CimAction.Update, CimFeature.Scheduler),
            new CimPermission("Delete Scheduler", CimAction.Delete, CimFeature.Scheduler),

            new CimPermission("View PastServiceReport", CimAction.View, CimFeature.Past_Service_Report),
            new CimPermission("Create PastServiceReport", CimAction.Create, CimFeature.Past_Service_Report),
            new CimPermission("Update PastServiceReport", CimAction.Update, CimFeature.Past_Service_Report),
            new CimPermission("Delete PastServiceReport", CimAction.Delete, CimFeature.Past_Service_Report),

            new CimPermission("View ConfigTypeValues", CimAction.View, CimFeature.ConfigTypeValues),
            new CimPermission("Create ConfigTypeValues", CimAction.Create, CimFeature.ConfigTypeValues),
            new CimPermission("Update ConfigTypeValues", CimAction.Update, CimFeature.ConfigTypeValues),
            new CimPermission("Delete ConfigTypeValues", CimAction.Delete, CimFeature.ConfigTypeValues),

            new CimPermission("View Distributor Dashboard", CimAction.View, CimFeature.Distributor_Dashboard),
            new CimPermission("View Customer Dashboard", CimAction.View, CimFeature.Customer_Dashboard),
            new CimPermission("View Engineer Dashboard", CimAction.View, CimFeature.Engineer_Dashboard),
            new CimPermission("View Manufacturer Dashboard", CimAction.View, CimFeature.Manufacturer_Dashboard),

            //new CimPermission("View Schools", CimAction.View, CimFeature.Schools, IsBasic: true),
            //new CimPermission("Create Schools", CimAction.Create, CimFeature.Schools),
            //new CimPermission("Update Schools", CimAction.Update, CimFeature.Schools),
            //new CimPermission("Delete Schools", CimAction.Delete, CimFeature.Schools),

            new CimPermission("View Base", CimAction.View, CimFeature.Base),

            new CimPermission("View Tenants", CimAction.View, CimFeature.Tenants, IsRoot: true),
            new CimPermission("Create Tenants", CimAction.Create, CimFeature.Tenants, IsRoot: true),
            new CimPermission("Update Tenants", CimAction.Update, CimFeature.Tenants, IsRoot: true),
            new CimPermission("Upgrade Tenants Subscription", CimAction.UpgradeSubscription, CimFeature.Tenants, IsRoot: true)
        ];

        public static IReadOnlyList<CimPermission> All { get; } = new ReadOnlyCollection<CimPermission>(AllPermissions);
        public static IReadOnlyList<CimPermission> Root { get; } = new ReadOnlyCollection<CimPermission>(AllPermissions.Where(p => p.IsRoot).ToArray());
        public static IReadOnlyList<CimPermission> Admin { get; } = new ReadOnlyCollection<CimPermission>(AllPermissions.Where(p => !p.IsRoot).ToArray());
        public static IReadOnlyList<CimPermission> Basic { get; } = new ReadOnlyCollection<CimPermission>(AllPermissions.Where(p => p.IsBasic).ToArray());
    }
}
