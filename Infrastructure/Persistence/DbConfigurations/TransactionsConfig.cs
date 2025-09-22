using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Finbuckle.MultiTenant;

namespace Infrastructure.Persistence.DbConfigurations
{
    internal class AMCConfig : IEntityTypeConfiguration<AMC>
    {
        public void Configure(EntityTypeBuilder<AMC> builder)
        {
            builder
                .ToTable("AMC", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class AMCInstrumentConfig : IEntityTypeConfiguration<AMCInstrument>
    {
        public void Configure(EntityTypeBuilder<AMCInstrument> builder)
        {
            builder
                .ToTable("AMCInstrument", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class AMCItemsConfig : IEntityTypeConfiguration<AMCItems>
    {
        public void Configure(EntityTypeBuilder<AMCItems> builder)
        {
            builder
                .ToTable("AMCItems", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class AMCStagesConfig : IEntityTypeConfiguration<AMCStages>
    {
        public void Configure(EntityTypeBuilder<AMCStages> builder)
        {
            builder
                .ToTable("AMCStages", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class CustomerInstrumentConfig : IEntityTypeConfiguration<CustomerInstrument>
    {
        public void Configure(EntityTypeBuilder<CustomerInstrument> builder)
        {
            builder
                .ToTable("CustomerInstrument", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class CustSPInventoryConfig : IEntityTypeConfiguration<CustSPInventory>
    {
        public void Configure(EntityTypeBuilder<CustSPInventory> builder)
        {
            builder
                .ToTable("CustSPInventory", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class EngSchedulerConfig : IEntityTypeConfiguration<EngScheduler>
    {
        public void Configure(EntityTypeBuilder<EngScheduler> builder)
        {
            builder
                .ToTable("EngScheduler", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class PastServiceReportConfig : IEntityTypeConfiguration<PastServiceReport>
    {
        public void Configure(EntityTypeBuilder<PastServiceReport> builder)
        {
            builder
                .ToTable("PastServiceReport", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class ServiceRequestConfig : IEntityTypeConfiguration<ServiceRequest>
    {
        public void Configure(EntityTypeBuilder<ServiceRequest> builder)
        {
            builder
                .ToTable("ServiceRequest", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class ServiceReportConfig : IEntityTypeConfiguration<ServiceReport>
    {
        public void Configure(EntityTypeBuilder<ServiceReport> builder)
        {
            builder
                .ToTable("ServiceReport", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class SPConsumedConfig : IEntityTypeConfiguration<SPConsumed>
    {
        public void Configure(EntityTypeBuilder<SPConsumed> builder)
        {
            builder
                .ToTable("SPConsumed", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class SPRecommendedConfig : IEntityTypeConfiguration<SPRecommended>
    {
        public void Configure(EntityTypeBuilder<SPRecommended> builder)
        {
            builder
                .ToTable("SPRecommended", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class SRAssignedHistoryConfig : IEntityTypeConfiguration<SRAssignedHistory>
    {
        public void Configure(EntityTypeBuilder<SRAssignedHistory> builder)
        {
            builder
                .ToTable("SRAssignedHistory", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class SRAuditTrailConfig : IEntityTypeConfiguration<SRAuditTrail>
    {
        public void Configure(EntityTypeBuilder<SRAuditTrail> builder)
        {
            builder
                .ToTable("SRAuditTrail", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class SREngActionConfig : IEntityTypeConfiguration<SREngAction>
    {
        public void Configure(EntityTypeBuilder<SREngAction> builder)
        {
            builder
                .ToTable("SREngAction", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class SREngCommentsConfig : IEntityTypeConfiguration<SREngComments>
    {
        public void Configure(EntityTypeBuilder<SREngComments> builder)
        {
            builder
                .ToTable("SREngComments", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class SRPEngWorkDoneConfig : IEntityTypeConfiguration<SRPEngWorkDone>
    {
        public void Configure(EntityTypeBuilder<SRPEngWorkDone> builder)
        {
            builder
                .ToTable("SRPEngWorkDone", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class SRPEngWorkTimeConfig : IEntityTypeConfiguration<SRPEngWorkTime>
    {
        public void Configure(EntityTypeBuilder<SRPEngWorkTime> builder)
        {
            builder
                .ToTable("SRPEngWorkTime", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class CustomerSatisfactionSurveyConfig : IEntityTypeConfiguration<CustomerSatisfactionSurvey>
    {
        public void Configure(EntityTypeBuilder<CustomerSatisfactionSurvey> builder)
        {
            builder
                .ToTable("CustomerSatisfactionSurvey", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class OfferRequestConfig : IEntityTypeConfiguration<OfferRequest>
    {
        public void Configure(EntityTypeBuilder<OfferRequest> builder)
        {
            builder
                .ToTable("OfferRequest", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class OfferRequestProcessConfig : IEntityTypeConfiguration<OfferRequestProcess>
    {
        public void Configure(EntityTypeBuilder<OfferRequestProcess> builder)
        {
            builder
                .ToTable("OfferRequestProcess", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class SparepartsOfferRequestConfig : IEntityTypeConfiguration<SparepartsOfferRequest>
    {
        public void Configure(EntityTypeBuilder<SparepartsOfferRequest> builder)
        {
            builder
                .ToTable("SparepartsOfferRequest", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class TravelExpenseConfig : IEntityTypeConfiguration<TravelExpense>
    {
        public void Configure(EntityTypeBuilder<TravelExpense> builder)
        {
            builder
                .ToTable("TravelExpense", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class TravelExpenseItemsConfig : IEntityTypeConfiguration<TravelExpenseItems>
    {
        public void Configure(EntityTypeBuilder<TravelExpenseItems> builder)
        {
            builder
                .ToTable("TravelExpenseItems", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class TravelInvoiceConfig : IEntityTypeConfiguration<TravelInvoice>
    {
        public void Configure(EntityTypeBuilder<TravelInvoice> builder)
        {
            builder
                .ToTable("TravelInvoice", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class AdvanceRequestConfig : IEntityTypeConfiguration<AdvanceRequest>
    {
        public void Configure(EntityTypeBuilder<AdvanceRequest> builder)
        {
            builder
                .ToTable("AdvanceRequest", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }

    internal class BankDetailsConfig : IEntityTypeConfiguration<BankDetails>
    {
        public void Configure(EntityTypeBuilder<BankDetails> builder)
        {
            builder
                .ToTable("BankDetails", SchemaNames.Transactions)
                .IsMultiTenant();
        }
    }   
}
