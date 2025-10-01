using Application.Features.Masters.Responses;
using Domain.Entities;
using Domain.Views;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Persistence.DbConfigurations;
using Infrastructure.Services;
using Infrastructure.Tenancy;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext(IMultiTenantContextAccessor<CIMTenantInfo> tenantInfoContextAccessor, DbContextOptions<ApplicationDbContext> options)
        : BaseDbContext(tenantInfoContextAccessor, options)    
    {
        public DbSet<School> Schools => Set<School>();
        public DbSet<ListType> ListTypes => Set<ListType>();
        public DbSet<ListTypeItems> ListTypeItems => Set<ListTypeItems>();
        public DbSet<MasterData> MasterData => Set<MasterData>();
        public DbSet<Country> Country => Set<Country>();
        public DbSet<Currency> Currency => Set<Currency>();
        public DbSet<Sparepart> Spareparts => Set<Sparepart>();
        public DbSet<Brand> Brand => Set<Brand>();
        public DbSet<BusinessUnit> BusinessUnit => Set<BusinessUnit>();
        public DbSet<ManfBusinessUnit> ManfBusinessUnit => Set<ManfBusinessUnit>();
        public DbSet<ConfigTypeValues> ConfigTypeValues => Set<ConfigTypeValues>();
        public DbSet<UserProfiles> UserProfiles => Set<UserProfiles>();
        public DbSet<Domain.Entities.Instrument> Instrument => Set<Domain.Entities.Instrument>();
        public DbSet<InstrumentAccessory> InstrumentAccessory => Set<InstrumentAccessory>();
        public DbSet<InstrumentSpares> InstrumentSpares => Set<InstrumentSpares>();
        public DbSet<Domain.Entities.Customer> Customer => Set<Domain.Entities.Customer>();
        public DbSet<Site> Site => Set<Site>();
        public DbSet<SiteContact> SiteContact => Set<SiteContact>();
        public DbSet<Distributor> Distributor => Set<Distributor>();
        public DbSet<Regions> Regions => Set<Regions>();
        public DbSet<RegionContact> RegionContact => Set<RegionContact>();
        public DbSet<Domain.Entities.Manufacturer> Manufacturer => Set<Domain.Entities.Manufacturer>();
        public DbSet<SalesRegion> SalesRegion => Set<SalesRegion>();
        public DbSet<SalesRegionContact> SalesRegionContact => Set<SalesRegionContact>();
        public DbSet<Notifications> Notifications => Set<Notifications>();
        public DbSet<UserContactMapping> UserContactMappings => Set<UserContactMapping>();
        public DbSet<Domain.Entities.FileShare> FileShare => Set<Domain.Entities.FileShare>();
        public DbSet<CustomerSatisfactionSurvey> CustomerSatisfactionSurvey => Set<CustomerSatisfactionSurvey>();



        public DbSet<Domain.Entities.AMC> AMC => Set<Domain.Entities.AMC>();
        public DbSet<AMCInstrument> AMCInstrument => Set<AMCInstrument>();
        public DbSet<AMCItems> AMCItems => Set<AMCItems>();
        public DbSet<AMCStages> AMCStages => Set<AMCStages>();
        public DbSet<CustomerInstrument> CustomerInstrument => Set<CustomerInstrument>();
        public DbSet<CustSPInventory> CustSPInventory => Set<CustSPInventory>();
        public DbSet<Domain.Entities.ServiceRequest> ServiceRequest => Set<Domain.Entities.ServiceRequest>();
        public DbSet<SRAssignedHistory> SRAssignedHistory => Set<SRAssignedHistory>();
        public DbSet<SRAuditTrail> SRAuditTrail => Set<SRAuditTrail>();
        public DbSet<SREngAction> SREngAction => Set<SREngAction>();
        public DbSet<SREngComments> SREngComments => Set<SREngComments>();
        public DbSet<Domain.Entities.ServiceReport> ServiceReport => Set<Domain.Entities.ServiceReport>();
        public DbSet<SRPEngWorkDone> SRPEngWorkDone => Set<SRPEngWorkDone>();
        public DbSet<SRPEngWorkTime> SRPEngWorkTime => Set<SRPEngWorkTime>();
        public DbSet<SPConsumed> SPConsumed => Set<SPConsumed>();
        public DbSet<SPRecommended> SPRecommended => Set<SPRecommended>();
        public DbSet<PastServiceReport> PastServiceReport => Set<PastServiceReport>();
        public DbSet<EngScheduler> EngScheduler => Set<EngScheduler>();
        public DbSet<OfferRequest> OfferRequest => Set<OfferRequest>();
        public DbSet<OfferRequestProcess> OfferRequestProcess => Set<OfferRequestProcess>();
        public DbSet<SparepartsOfferRequest> SparepartsOfferRequest => Set<SparepartsOfferRequest>();
        public DbSet<TravelExpense> TravelExpenses => Set<TravelExpense>();
        public DbSet<TravelExpenseItems> TravelExpenseItems => Set<TravelExpenseItems>();
        public DbSet<TravelInvoice> TravelInvoice => Set<TravelInvoice>();
        public DbSet<AdvanceRequest> AdvanceRequest => Set<AdvanceRequest>();
        public DbSet<BankDetails> BankDetails => Set<BankDetails>();
        

        /// Views
        public DbSet<VW_ListItems> VW_ListItems => Set<VW_ListItems>();
        public DbSet<VW_UserProfile> VW_UserProfile => Set<VW_UserProfile>();
        public DbSet<VW_Spareparts> VW_Spareparts => Set<VW_Spareparts>();
        public DbSet<VW_InstrumentSpares> VW_InstrumentSpares => Set<VW_InstrumentSpares>();
        public DbSet<VW_SparepartConsumedHistory> VW_SparepartConsumedHistory => Set<VW_SparepartConsumedHistory>();
        public DbSet<VW_ServiceReport> VW_ServiceReport => Set<VW_ServiceReport>();
        public DbSet<VW_SparesRecommended> VW_SparesRecommended => Set<VW_SparesRecommended>();

        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RegionContact>().HasIndex(x => x.PrimaryEmail).IsUnique().HasDatabaseName("INDUQ_REGIONCONTACT");
            modelBuilder.Entity<RegionContact>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique().HasDatabaseName("INDUQ_COUNTRY");
            modelBuilder.Entity<Country>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique().HasDatabaseName("INDUQ_CURRENCY");
            modelBuilder.Entity<Currency>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<Customer>().HasIndex(x => x.CustName).IsUnique().HasDatabaseName("INDUQ_CUSTOMER");
            modelBuilder.Entity<Customer>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<Regions>().HasIndex(x => new { x.DistId, x.DistRegName }).IsUnique().HasDatabaseName("INDUQ_REGIONS");
            modelBuilder.Entity<Regions>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<Distributor>().HasIndex(x => x.DistName).IsUnique().HasDatabaseName("INDUQ_DISTRIBUTOR");
            modelBuilder.Entity<Distributor>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<Instrument>().HasIndex(x => new { x.InsType, x.SerialNos }).IsUnique().HasDatabaseName("INDUQ_INSTRUMENT");
            modelBuilder.Entity<Instrument>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<InstrumentSpares>().HasIndex(x => new { x.InstrumentId, x.SparepartId }).IsUnique().HasDatabaseName("INDUQ_INSTRUMENTSPARES");
            modelBuilder.Entity<InstrumentSpares>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<ListType>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ListTypeItems>().HasIndex(x => new { x.ListTypeId, x.ItemName}).IsUnique().HasDatabaseName("INDUQ_LISTTYPEITEMS");
            modelBuilder.Entity<ListTypeItems>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<ConfigTypeValues>().HasIndex(x => new { x.ListTypeItemId, x.ConfigValue}).IsUnique().HasDatabaseName("INDUQ_CONFIGTYPEVALUES");
            modelBuilder.Entity<ConfigTypeValues>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);            
            modelBuilder.Entity<UserProfiles>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<Site>().HasIndex(x => new { x.CustomerId, x.CustRegName}).IsUnique().HasDatabaseName("INDUQ_SITE");
            modelBuilder.Entity<Site>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<Sparepart>().HasIndex(x => x.PartNo).IsUnique().HasDatabaseName("INDUQ_SPAREPART");
            modelBuilder.Entity<Sparepart>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);

            modelBuilder.Entity<Domain.Entities.FileShare>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<ServiceRequest>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<SREngComments>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<SRAssignedHistory>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<SREngAction>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<OfferRequest>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<AMC>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<AMCItems>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<AMCInstrument>().HasIndex(x => new { x.InstrumentId, x.AMCId}).IsUnique().HasDatabaseName("INDUQ_AMCINSTRUMENT");
            modelBuilder.Entity<TravelInvoice>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<ServiceReport>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<SRPEngWorkDone>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<SRPEngWorkTime>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<SPConsumed>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<CustomerSatisfactionSurvey>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<SPRecommended>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<CustSPInventory>().HasIndex(x => new { x.InstrumentId, x.SparePartId }).IsUnique().HasDatabaseName("INDUQ_CUSTSPINVENTORY");
            modelBuilder.Entity<CustSPInventory>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<SparepartsOfferRequest>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<EngScheduler>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<SRAuditTrail>().HasQueryFilter(x => x.IsActive);
            modelBuilder.Entity<OfferRequestProcess>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<AMCStages>().HasIndex(x => new { x.AMCId, x.Stage}).IsUnique().HasDatabaseName("INDUQ_AMCSTAGES");
            modelBuilder.Entity<AMCStages>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<TravelExpense>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<TravelExpenseItems>().HasIndex(x => new { x.TravelExpenseId, x.ExpNature, x.ExpDate}).IsUnique().HasDatabaseName("INDUQ_TRAVELEXPENSEITEMS");
            modelBuilder.Entity<TravelExpenseItems>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<AdvanceRequest>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<BankDetails>().HasQueryFilter(x => x.IsActive);
            modelBuilder.Entity<InstrumentAccessory>().HasIndex(x => new { x.InstrumentId, x.AccessoryName}).IsUnique().HasDatabaseName("INDUQ_INSTRUMENTACCESSORY");
            modelBuilder.Entity<InstrumentAccessory>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<PastServiceReport>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<Brand>().HasIndex(x => new { x.BusinessUnitId, x.BrandName }).IsUnique().HasDatabaseName("INDUQ_BRAND");
            modelBuilder.Entity<Brand>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<BusinessUnit>().HasIndex(x =>x.BusinessUnitName).IsUnique().HasDatabaseName("INDUQ_BUSINESSUNIT");
            modelBuilder.Entity<BusinessUnit>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            modelBuilder.Entity<ManfBusinessUnit>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);



            //modelBuilder.Entity<VW_ListItems>().HasQueryFilter(x => !x.IsDeleted && (x.IsActive || x.CompanyId == null));
            //modelBuilder.Entity<VW_SparesRecommended>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            //modelBuilder.Entity<VW_ServiceReport>().HasQueryFilter(x => !x.SrqIsdeleted && x.IsActive);
            //modelBuilder.Entity<VW_InstrumentSpares>().HasQueryFilter(x => !x.IsDeleted && x.IsActive);
            //modelBuilder.Entity<VW_SparepartConsumedHistory>().HasQueryFilter(x => x.IsActive);
            //modelBuilder.Entity<VW_Spareparts>().HasQueryFilter(x => x.IsActive);
            //modelBuilder.Entity<VW_UserProfile>().HasQueryFilter(x => x.IsActive);
            //modelBuilder.Entity<VW_ListTypeItemsResponse>().HasQueryFilter(x => x.IsActive);



            modelBuilder.Entity<Domain.Entities.Distributor>().Property(e => e.GeoLat).HasPrecision(6, 2);
            modelBuilder.Entity<Domain.Entities.Distributor>().Property(e => e.GeoLong).HasPrecision(6, 2);
            modelBuilder.Entity<Domain.Entities.Regions>().Property(e => e.GeoLat).HasPrecision(6, 2);
            modelBuilder.Entity<Domain.Entities.Regions>().Property(e => e.GeoLong).HasPrecision(6, 2);
            modelBuilder.Entity<Domain.Entities.RegionContact>().Property(e => e.GeoLat).HasPrecision(6, 2);
            modelBuilder.Entity<Domain.Entities.RegionContact>().Property(e => e.GeoLong).HasPrecision(6, 2);

            modelBuilder.Entity<Domain.Entities.Customer>().Property(e => e.GeoLat).HasPrecision(6, 2);
            modelBuilder.Entity<Domain.Entities.Customer>().Property(e => e.GeoLong).HasPrecision(6, 2);
            modelBuilder.Entity<Domain.Entities.Site>().Property(e => e.GeoLat).HasPrecision(6, 2);
            modelBuilder.Entity<Domain.Entities.Site>().Property(e => e.GeoLong).HasPrecision(6, 2);
            modelBuilder.Entity<Domain.Entities.SiteContact>().Property(e => e.GeoLat).HasPrecision(6, 2);
            modelBuilder.Entity<Domain.Entities.SiteContact>().Property(e => e.GeoLong).HasPrecision(6, 2);

            modelBuilder.Entity<Domain.Entities.Manufacturer>().Property(e => e.GeoLat).HasPrecision(6, 2);
            modelBuilder.Entity<Domain.Entities.Manufacturer>().Property(e => e.GeoLong).HasPrecision(6, 2);
            modelBuilder.Entity<Domain.Entities.SalesRegion>().Property(e => e.GeoLat).HasPrecision(6, 2);
            modelBuilder.Entity<Domain.Entities.SalesRegion>().Property(e => e.GeoLong).HasPrecision(6, 2);
            modelBuilder.Entity<Domain.Entities.SalesRegionContact>().Property(e => e.GeoLat).HasPrecision(6, 2);
            modelBuilder.Entity<Domain.Entities.SalesRegionContact>().Property(e => e.GeoLong).HasPrecision(6, 2);

            modelBuilder.Entity<Domain.Entities.AMC>().Property(e => e.BaseCurrencyAmt).HasPrecision(18, 2);
            modelBuilder.Entity<Domain.Entities.AMC>().Property(e => e.ConversionAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Domain.Entities.AMC>().Property(e => e.Zerorate).HasPrecision(18, 2);
            modelBuilder.Entity<AMCInstrument>().Property(e => e.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<AMCInstrument>().Property(e => e.Rate).HasPrecision(18, 2);
            modelBuilder.Entity<Domain.Entities.ServiceRequest>().Property(e => e.BaseAmt).HasPrecision(18, 2);
            modelBuilder.Entity<Domain.Entities.ServiceRequest>().Property(e => e.CostInUsd).HasPrecision(18, 2);
            modelBuilder.Entity<Domain.Entities.ServiceRequest>().Property(e => e.TotalCost).HasPrecision(18, 2);
            modelBuilder.Entity<Sparepart>().Property(e => e.Price).HasPrecision(18, 2);

            modelBuilder.Entity<OfferRequest>().Property(e => e.AirFreightChargesAmt).HasPrecision(18, 2);
            modelBuilder.Entity<OfferRequest>().Property(e => e.BasePCurrencyAmt).HasPrecision(18, 2);
            modelBuilder.Entity<OfferRequest>().Property(e => e.InspectionChargesAmt).HasPrecision(18, 2);
            modelBuilder.Entity<OfferRequest>().Property(e => e.LcAdministrativeChargesAmt).HasPrecision(18, 2);
            modelBuilder.Entity<OfferRequest>().Property(e => e.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<OfferRequest>().Property(e => e.TotalAmt).HasPrecision(18, 2);

            modelBuilder.Entity<OfferRequestProcess>().Property(e => e.PayAmt).HasPrecision(18, 2);
            modelBuilder.Entity<OfferRequestProcess>().Property(e => e.BaseCurrencyAmt).HasPrecision(18, 2);

            modelBuilder.Entity<SparepartsOfferRequest>().Property(e => e.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<SparepartsOfferRequest>().Property(e => e.Price).HasPrecision(18, 2);
            modelBuilder.Entity<SparepartsOfferRequest>().Property(e => e.DiscountPercentage).HasPrecision(5, 2);
            modelBuilder.Entity<SparepartsOfferRequest>().Property(e => e.AfterDiscount).HasPrecision(18, 2);

            modelBuilder.Entity<TravelExpense>().Property(e => e.GrandEngineerTotal).HasPrecision(18, 2);
            modelBuilder.Entity<TravelExpense>().Property(e => e.GrandCompanyTotal).HasPrecision(18, 2);

            modelBuilder.Entity<TravelExpenseItems>().Property(e => e.BcyAmt).HasPrecision(18, 2);
            modelBuilder.Entity<TravelExpenseItems>().Property(e => e.UsdAmt).HasPrecision(18, 2);

            modelBuilder.Entity<TravelInvoice>().Property(e => e.AmountBuild).HasPrecision(18, 2);



            //// Views
            modelBuilder.Entity<VW_ListItems>().ToView("VW_ListItems").HasNoKey();
            modelBuilder.Entity<VW_UserProfile>().ToView("VW_UserProfile").HasNoKey();
            modelBuilder.Entity<VW_Spareparts>().ToView("VW_Spareparts").HasNoKey();
            modelBuilder.Entity<VW_Spareparts>().Property(e => e.Price).HasPrecision(18, 2);
            modelBuilder.Entity<VW_InstrumentSpares>().ToView("VW_InstrumentSpares").HasNoKey();
            modelBuilder.Entity<VW_InstrumentSpares>().Property(e => e.Price).HasPrecision(18, 2);
            modelBuilder.Entity<VW_SparepartConsumedHistory>().ToView("VW_SparepartConsumedHistory").HasNoKey();
            modelBuilder.Entity<VW_ServiceReport>().ToView("VW_ServiceReport").HasNoKey();
            modelBuilder.Entity<VW_SparesRecommended>().ToView("VW_SparesRecommended").HasNoKey();

           
        }
    }
}
