using Domain.Entities;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.DbConfigurations
{
    internal class ListTypeConfig : IEntityTypeConfiguration<ListType>
    {
        public void Configure(EntityTypeBuilder<ListType> builder)
        {
            builder
                .ToTable("ListType", SchemaNames.Masters)
                .IsMultiTenant();          
        }
    }

    internal class ListTypeItemsConfig : IEntityTypeConfiguration<ListTypeItems>
    {
        public void Configure(EntityTypeBuilder<ListTypeItems> builder)
        {
            builder
                .ToTable("ListTypeItems", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class MasterDataConfig : IEntityTypeConfiguration<MasterData>
    {
        public void Configure(EntityTypeBuilder<MasterData> builder)
        {
            builder
                .ToTable("MasterData", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class CountryConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder
                .ToTable("Country", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class CurrencyConfig : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder
                .ToTable("Currency", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }


    internal class SparepartConfig : IEntityTypeConfiguration<Sparepart>
    {
        public void Configure(EntityTypeBuilder<Sparepart> builder)
        {
            builder
                .ToTable("Sparepart", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class ConfigTypeValuesConfig : IEntityTypeConfiguration<ConfigTypeValues>
    {
        public void Configure(EntityTypeBuilder<ConfigTypeValues> builder)
        {
            builder
                .ToTable("ConfigTypeValues", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class UserProfilesConfig : IEntityTypeConfiguration<UserProfiles>
    {
        public void Configure(EntityTypeBuilder<UserProfiles> builder)
        {
            builder
                .ToTable("UserProfiles", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class BusinessUnitConfig : IEntityTypeConfiguration<BusinessUnit>
    {
        public void Configure(EntityTypeBuilder<BusinessUnit> builder)
        {
            builder
                .ToTable("BusinessUnit", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class ManfBusinessUnitConfig : IEntityTypeConfiguration<ManfBusinessUnit>
    {
        public void Configure(EntityTypeBuilder<ManfBusinessUnit> builder)
        {
            builder
                .ToTable("ManfBusinessUnit", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class BrandConfig : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder
                .ToTable("Brand", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder
                .ToTable("Customer", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class DistributorConfig : IEntityTypeConfiguration<Distributor>
    {
        public void Configure(EntityTypeBuilder<Distributor> builder)
        {
            builder
                .ToTable("Distributor", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class ManufacturerConfig : IEntityTypeConfiguration<Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            builder
                .ToTable("Manufacturer", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class InstrumentConfig : IEntityTypeConfiguration<Instrument>
    {
        public void Configure(EntityTypeBuilder<Instrument> builder)
        {
            builder
                .ToTable("Instrument", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class InstrumentAccessoryConfig : IEntityTypeConfiguration<InstrumentAccessory>
    {
        public void Configure(EntityTypeBuilder<InstrumentAccessory> builder)
        {
            builder
                .ToTable("InstrumentAccessory", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class InstrumentSparesConfig : IEntityTypeConfiguration<InstrumentSpares>
    {
        public void Configure(EntityTypeBuilder<InstrumentSpares> builder)
        {
            builder
                .ToTable("InstrumentSpares", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class SiteConfig : IEntityTypeConfiguration<Site>
    {
        public void Configure(EntityTypeBuilder<Site> builder)
        {
            builder
                .ToTable("Site", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class SiteContactConfig : IEntityTypeConfiguration<SiteContact>
    {
        public void Configure(EntityTypeBuilder<SiteContact> builder)
        {
            builder
                .ToTable("SiteContact", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class RegionContactConfig : IEntityTypeConfiguration<RegionContact>
    {
        public void Configure(EntityTypeBuilder<RegionContact> builder)
        {
            builder
                .ToTable("RegionContact", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class RegionsConfig : IEntityTypeConfiguration<Regions>
    {
        public void Configure(EntityTypeBuilder<Regions> builder)
        {
            builder
                .ToTable("Regions", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class SalesRegionConfig : IEntityTypeConfiguration<SalesRegion>
    {
        public void Configure(EntityTypeBuilder<SalesRegion> builder)
        {
            builder
                .ToTable("SalesRegion", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class SalesRegionContactConfig : IEntityTypeConfiguration<SalesRegionContact>
    {
        public void Configure(EntityTypeBuilder<SalesRegionContact> builder)
        {
            builder
                .ToTable("SalesRegionContact", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }

    internal class FileShareConfig : IEntityTypeConfiguration<Domain.Entities.FileShare>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.FileShare> builder)
        {
            builder
                .ToTable("FileShare", SchemaNames.Masters)
                .IsMultiTenant();
        }
    }
}
