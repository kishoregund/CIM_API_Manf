using Application.Features.AMCS;
using Application.Features.AppBasic;
using Application.Features.Customers;
using Application.Features.Dashboards;
using Application.Features.Distributors;
using Application.Features.Instruments;
using Application.Features.Manufacturers;
using Application.Features.Masters;
using Application.Features.Notifications;
using Application.Features.Schools;
using Application.Features.ServiceReports;
using Application.Features.ServiceRequests;
using Application.Features.Spares;
using Application.Features.Travels;
using Application.Features.UserProfiles;
using Infrastructure.Identity;
using Infrastructure.OpenApi;
using Infrastructure.Persistence;
using Infrastructure.Schools;
using Infrastructure.Services;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddMultitenancyServices(configuration)
                .AddPersistenceService(configuration)
                .AddIdentityServices()
                .AddPermissions()
                .AddJwtAuthentication()
                .AddOpenApiDocumentation(configuration)
                .AddScoped<ISchoolService, SchoolService>()
                .AddScoped<IAmcService, AmcService>()
                .AddScoped<IAmcItemsService, AmcItemsService>()
                .AddScoped<IAmcInstrumentService, AmcInstrumentService>()
                .AddScoped<IAmcStagesService, AmcStagesService>()
                .AddScoped<ISparepartService, SparepartService>()
                .AddScoped<IConfigTypeValuesService, ConfigTypeValuesService>()
                .AddScoped<ICountryService, CountryService>()
                .AddScoped<ICurrencyService, CurrencyService>()
                .AddScoped<ICustInstrumentService, CustomerInstrumentService>()
                .AddScoped<ICustomerService, CustomerService>()
                .AddScoped<IDistributorService, DistributorService>()
                .AddScoped<IEngSchedulerService, EngSchedulerService>()
                .AddScoped<IInstrumentAccessoryService, InstrumentAccessoryService>()
                .AddScoped<IInstrumentService, InstrumentService>()
                .AddScoped<IInstrumentSparesService, InstrumentSparesService>()
                .AddScoped<IListTypeItemsService, ListTypeItemsService>()
                .AddScoped<IManufacturerService, ManufacturerService>()
                .AddScoped<IPastServiceReportService, PastServiceReportService>()
                .AddScoped<IRegionContactService, RegionContactService>()
                .AddScoped<IRegionService, RegionService>()
                .AddScoped<ISalesRegionContactService, SalesRegionContactService>()
                .AddScoped<ISalesRegionService, SalesRegionService>()
                .AddScoped<IServiceReportService, ServiceReportService>()
                .AddScoped<IServiceRequestService, ServiceRequestService>()
                .AddScoped<ISiteContactService, SiteContactService>()
                .AddScoped<ISiteService, SiteService>()
                .AddScoped<ISparepartService, SparepartService>()
                .AddScoped<ICustSPInventoryService, SparesInventoryService>()
                .AddScoped<ISPConsumedService, SPConsumedService>()
                .AddScoped<ISPRecommendedService, SPRecommendedService>()
                .AddScoped<ISRAssignedHistoryService, SRAssignedHistoryService>()
                .AddScoped<ISRAuditTrailService, SRAuditTrailService>()
                .AddScoped<ISREngActionService, SREngActionService>()
                .AddScoped<ISREngCommentsService, SREngCommentsService>()
                .AddScoped<ISRPEngWorkDoneService, SRPEngWorkDoneService>()
                .AddScoped<ISRPEngWorkTimeService, SRPEngWorkTimeService>()
                .AddScoped<IUserProfilesService, UserProfilesService>()
                .AddScoped<IBusinessUnitService, BusinessUnitService>()
                .AddScoped<IManfBusinessUnitService, ManfBusinessUnitService>()
                .AddScoped<IBrandService, BrandService>()
                .AddScoped<IAppBasicService, AppBasicService>()
                .AddScoped<ICustomerSurveyService, CustomerSurveyService>()
                .AddScoped<IOfferRequestService, OfferRequestService>()
                .AddScoped<IOfferRequestProcessService, OfferRequestProcessService>()
                .AddScoped<ISparepartsOfferRequestService, SparepartsOfferRequestService>()
                .AddScoped<ITravelExpenseService, TravelExpenseService>()
                .AddScoped<ITravelExpenseItemsService, TravelExpenseItemservice>()
                .AddScoped<ITravelInvoiceService, TravelInvoiceService>()
                .AddScoped<IDistributorDashboardService, DistributorDashboardService>()
                .AddScoped<ICustomerDashboardService, CustomerDashboardService>()
                .AddScoped<IEngineerDashboardService, EngineerDashboardService>()
                .AddScoped<IAdvanceRequestService, AdvanceRequestService>()
                .AddScoped<IBankDetailsService, BankDetailsService>()
                .AddScoped<IPastServiceReportService, PastServiceReportService>()
                .AddScoped<INotificationsService, NotificationsService>();
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            return app
                .UseAuthentication()
                .UseCurrentUser()
                .UseMultitenancy()
                .UseAuthorization()
                .UseOpenApiDocumentation();
        }
    }
}
