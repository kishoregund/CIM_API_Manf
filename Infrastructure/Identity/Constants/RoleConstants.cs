using System.Collections.ObjectModel;

namespace Infrastructure.Identity.Constants
{
    public static class RoleConstants
    {
        public const string Admin = nameof(Admin);
        public const string Basic = nameof(Basic);
        public const string Customer = nameof(Customer);
        public const string Site = nameof(Site);
        public const string Distributor_Operations = nameof(Distributor_Operations);
        public const string Distributor_Operations_Region = nameof(Distributor_Operations_Region);
        public const string Engineer = nameof(Engineer);

        public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(
        [
            Admin,
            Basic,
            Customer,
            Site,
            Distributor_Operations,
            Distributor_Operations_Region,
            Engineer
        ]);

        public static bool IsDefault(string roleName) => DefaultRoles.Any(role => role == roleName);
    }

    //public static class ProfileConstants
    //{        
    //    public const string Customer = nameof(Customer);
    //    public const string DistributorSupport = nameof(DistributorSupport);
    //    public const string Engineer = nameof(Engineer);
    //    public const string Manufacturer = nameof(Manufacturer);

    //    public static IReadOnlyList<string> DefaultProfiles { get; } = new ReadOnlyCollection<string>(
    //    [
    //        Customer,
    //        DistributorSupport,
    //        Engineer,
    //        Manufacturer
    //    ]);

    //    public static bool IsDefault(string profileName) => DefaultProfiles.Any(profile => profile == profileName);
    //}
}
