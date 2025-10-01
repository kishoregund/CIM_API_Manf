using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class appDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Transactions");

            migrationBuilder.EnsureSchema(
                name: "Masters");

            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.EnsureSchema(
                name: "Academics");

            migrationBuilder.CreateTable(
                name: "AdvanceRequest",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistributorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnderTaking = table.Column<bool>(type: "bit", nullable: false),
                    IsBillable = table.Column<bool>(type: "bit", nullable: false),
                    AdvanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdvanceCurrency = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportingManager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientNameLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficeLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvanceRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AMC",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BillTo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustSite = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceQuote = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    SqDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Project = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondVisitDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstVisitDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Zerorate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    BaseCurrencyAmt = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ConversionAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    IsMultipleBreakdown = table.Column<bool>(type: "bit", nullable: false),
                    TnC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMC", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AMCInstrument",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SerialNos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    InsTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AMCId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMCInstrument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AMCItems",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SqNo = table.Column<int>(type: "int", nullable: false),
                    AMCId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceRequestId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstStartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstEndDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMCItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AMCStages",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AMCId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StageIndex = table.Column<int>(type: "int", nullable: false),
                    Stage = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    PaymentTypeId = table.Column<Guid>(type: "nvarchar(100)", nullable: true),
                    PayAmtCurrencyId = table.Column<Guid>(type: "nvarchar(100)", nullable: true),
                    PayAmt = table.Column<double>(type: "float", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AMCStages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankDetails",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BankAccountNo = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameInBank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IBANNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankSwiftCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brand",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    BusinessUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessUnit",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessUnitName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessUnit", x => x.Id);
                });


            migrationBuilder.CreateTable(
                name: "ManfBusinessUnit",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ManfId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessUnitName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManfBusinessUnit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigTypeValues",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListTypeItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigValue = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigTypeValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Iso_2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Iso_3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Formal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sub_Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capital = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContinentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MCId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    N_Code = table.Column<int>(type: "int", nullable: true),
                    Minor_Unit = table.Column<int>(type: "int", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndustrySegment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefDistRegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefDistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddrCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeoLat = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    GeoLong = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInstrument",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustSiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateOfPurchase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BaseCurrencyAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsMfgDt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipDt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstallDt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstallBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstallByOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngNameOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstruEngineerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Warranty = table.Column<bool>(type: "bit", nullable: false),
                    WrntyStDt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WrntyEnDt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInstrument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSatisfactionSurvey",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceRequestNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsProfessional = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNotified = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSatisfied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAreaClean = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSatisfactionSurvey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustSPInventory",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QtyAvailable = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SparePartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustSPInventory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Distributor",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Distname = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Payterms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManufacturerIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddrCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ManfBusinessUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),                    
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeoLat = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    GeoLong = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EngScheduler",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAllDay = table.Column<bool>(type: "bit", nullable: false),
                    IsBlock = table.Column<bool>(type: "bit", nullable: false),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: false),
                    RoomId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerReqId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTimezone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndTimezone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecurrenceRule = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecurrenceException = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngScheduler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileShare",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    FileFor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileShare", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instrument",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ManufId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SerialNos = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    InsMfgDt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instrument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentAccessory",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccessoryName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModelNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearOfPurchase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccessoryDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentAccessory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentSpares",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SparepartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsQty = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentSpares", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListType",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ListName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListTypeItems",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEscalationSupervisor = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListTypeItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturer",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ManfName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Payterms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddrCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeoLat = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    GeoLong = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterData",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEscalationSupervisor = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RaisedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfferRequest",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistributorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerSiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OffReqNo = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    OtherSpareDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpareQuoteNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Instruments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AirFreightChargesAmt = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    InspectionChargesAmt = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LcAdministrativeChargesAmt = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalAmt = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BasePCurrencyAmt = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalCurr = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AirFreightChargesCurr = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectionChargesCurr = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LcadministrativeChargesCurr = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsDistUpdated = table.Column<bool>(type: "bit", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfferRequestProcess",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Stage = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: true),
                    StageIndex = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    PaymentTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PayAmt = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BaseCurrencyAmt = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PayAmtCurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferRequestProcess", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PastServiceReport",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Of = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PastServiceReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionContact",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryEmail = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    SecondaryContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesignationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WhatsappNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsFieldEngineer = table.Column<bool>(type: "bit", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddrCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeoLat = table.Column<string>(type: "nvarchar(max)", precision: 6, scale: 2, nullable: true),
                    GeoLong = table.Column<string>(type: "nvarchar(max)", precision: 6, scale: 2, nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionContact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    DistRegName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    PayTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    Countries = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPrincipal = table.Column<bool>(type: "bit", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddrCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeoLat = table.Column<string>(type: "nvarchar(max)", precision: 6, scale: 2, nullable: true),
                    GeoLong = table.Column<string>(type: "nvarchar(max)", precision: 6, scale: 2, nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesRegion",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesRegionName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    PayTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManfId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    Countries = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPrincipal = table.Column<bool>(type: "bit", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddrCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeoLat = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    GeoLong = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesRegion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesRegionContact",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryEmail = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    SecondaryContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesignationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesRegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WhatsappNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddrCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeoLat = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    GeoLong = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesRegionContact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                schema: "Academics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    EstablishedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceReport",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceReportNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Town = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstrumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LabChief = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SrOf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RespInstrumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComputerArlsn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Software = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Firmaware = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Installation = table.Column<bool>(type: "bit", nullable: false),
                    AnalyticalAssit = table.Column<bool>(type: "bit", nullable: false),
                    PrevMaintenance = table.Column<bool>(type: "bit", nullable: false),
                    Rework = table.Column<bool>(type: "bit", nullable: false),
                    CorrMaintenance = table.Column<bool>(type: "bit", nullable: false),
                    Problem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkFinished = table.Column<bool>(type: "bit", nullable: false),
                    WorkCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Interrupted = table.Column<bool>(type: "bit", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NextVisitScheduled = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngineerComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SignEngName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignCustName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequest",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsReportGenerated = table.Column<bool>(type: "bit", nullable: false),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Distributor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerReqNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerReqDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerResolutionDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperatorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperatorEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachmodelName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XrayGenerator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BreakdownType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecurringComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BreakoccurDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResolveAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComplaintRegisName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedTo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VisitType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachEngineer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperatorNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachinesNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SampleHandlingType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    AlarmDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentInstrustatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Escalation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrarPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accepted = table.Column<bool>(type: "bit", nullable: false),
                    AcceptedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DelayedReasons = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCritical = table.Column<bool>(type: "bit", nullable: false),
                    RequestTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubRequestTypeId = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNotUnderAmc = table.Column<bool>(type: "bit", nullable: false),
                    AmcId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CostInUsd = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    BaseCurrency = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BaseAmt = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    AmcServiceQuote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalCostCurrency = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Site",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustRegName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    PayTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeoLat = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    GeoLong = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Site", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteContact",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryEmail = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    SecondaryContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesignationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WhatsappNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddrCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeoLat = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    GeoLong = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteContact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sparepart",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNo = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ItemDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    PartType = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DescCatalogue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HsCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsObselete = table.Column<bool>(type: "bit", nullable: false),
                    ReplacePartNoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sparepart", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SparepartsOfferRequest",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false, defaultValue:0),
                    AfterDiscount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue:0),
                    HsCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SparePartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SparepartsOfferRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SPConsumed",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigType = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigValue = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNo = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    HscCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QtyConsumed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QtyAvailable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerSPInventoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPConsumed", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SPRecommended",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigType = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigValue = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartNo = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    HscCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QtyRecommended = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPRecommended", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SRAssignedHistory",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TicketStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SRAssignedHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SRAuditTrail",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Values = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SRAuditTrail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SREngAction",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Actiontaken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamviewRecording = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SREngAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SREngComments",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nextdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SREngComments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SRPEngWorkDone",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Workdone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SRPEngWorkDone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SRPEngWorkTime",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkTimeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerDayHrs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDays = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalHrs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SRPEngWorkTime", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TravelExpense",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistributorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Designation = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrandCompanyTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    GrandEngineerTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelExpense", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TravelExpenseItems",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TravelExpenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpNature = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsBillsAttached = table.Column<bool>(type: "bit", nullable: false),
                    Currency = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpenseBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BcyAmt = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UsdAmt = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelExpenseItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TravelInvoice",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistributorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountBuild = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelInvoice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserContactMapping",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContactMapping", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                schema: "Masters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrandIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileFor = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessUnitIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistRegions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustSites = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManfSalesRegions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "Identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "Identity",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Identity",
                table: "Roles",
                columns: new[] { "NormalizedName", "TenantId" },
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                schema: "Identity",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "Identity",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Identity",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Identity",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Identity",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.Sql("Create    \r\nVIEW [dbo].[VW_ListItems] AS\r\n    SELECT \r\n        l.code AS ListCode,\r\n        l.id AS ListTypeId,\r\n        l.listname AS ListName,\r\n        li.id AS ListTypeItemId,\r\n        li.code AS ItemCode,\r\n        li.itemname AS ItemName,\r\n        li.isdeleted AS IsDeleted,\r\n        li.isescalationsupervisor AS IsEscalationSupervisor,\r\n        0 AS IsMaster\r\n    FROM\r\n        [Masters].ListType l\r\n        JOIN [Masters].ListTypeItems li ON l.Id = li.ListTypeId \r\n    UNION SELECT \r\n        l.code AS ListCode,\r\n        l.id AS ListTypeId,\r\n        l.listname AS ListName,\r\n        li.id AS ListTypeItemId,\r\n        li.code AS ItemCode,\r\n        li.itemname AS ItemName,\r\n        li.isdeleted AS IsDeleted,\r\n        li.isescalationsupervisor AS IsEscalationSupervisor,\r\n        1 AS IsMaster\r\n    FROM\r\n        [Masters].ListType l\r\n        JOIN [Masters].MasterData li ON l.Id = li.ListTypeId ;\r\nGO\r\n");
            migrationBuilder.Sql("CREATE     \r\nVIEW [dbo].[VW_InstrumentSpares] AS\r\n    SELECT \r\n        sp.id AS id,\r\n        sp.isactive AS isactive,\r\n        sp.configtypeid AS configtypeid,\r\n        ctv.itemname AS configtypename,\r\n        sp.partno AS partno,\r\n        sp.itemdesc AS itemdesc,\r\n        sp.qty AS qty,\r\n        sp.parttype AS parttype,\r\n        sp.desccatalogue AS desccatalogue,\r\n        sp.hscode AS hscode,\r\n        sp.countryid AS countryid,\r\n        c.name AS countryname,\r\n        sp.price AS price,\r\n        sp.currencyid AS currencyid,\r\n        cr.name AS currencyname,\r\n        mdpt.itemname AS parttypename,\r\n        sp.image AS image,\r\n        sp.isobselete AS isobselete,\r\n        sp.replacepartnoid AS replacepartnoid,\r\n        sp.configvalueid AS configvalueid,\r\n        '' AS configvaluename,\r\n        ic.instrumentid AS instrumentid,\r\n        c.isdeleted AS couisdeleted,\r\n        cr.isdeleted AS currisdeleted,\r\n        isnull(ic.insqty, 0) AS insqty,\r\n\t\tsp.PartNo + ' => '+ sp.ItemDesc as PartNoDesc\r\n    FROM\r\n        [Masters].Sparepart sp\r\n        LEFT JOIN [Masters].InstrumentSpares ic ON sp.configtypeid = ic.configtypeid \r\n            AND sp.id = ic.sparepartid\r\n        LEFT JOIN [Masters].country c ON sp.countryid = c.id\r\n        LEFT JOIN [Masters].currency cr ON sp.currencyid = cr.id\r\n        LEFT JOIN [Masters].listtypeitems licg ON sp.configtypeid = licg.id\r\n        LEFT JOIN [Masters].listtypeitems lipt ON sp.parttype = lipt.id\r\n        LEFT JOIN [Masters].listtypeitems ctv ON sp.configtypeid = ctv.id\r\n        LEFT JOIN [Masters].masterdata mdpt ON sp.parttype = mdpt.id\r\n\t\twhere sp.IsActive = 1 and sp.IsDeleted = 0\r\nGO\r\n");
            migrationBuilder.Sql("CREATE  VIEW [dbo].[VW_ServiceReport] AS  \r\n    SELECT   \r\n        srrp.id AS ServiceReportId,  \r\n        srrp.customer AS Customer,  \r\n        li.ItemName AS Department,  \r\n        srrp.town AS Town,  \r\n        srrp.labchief AS LabChief,  \r\n        isnull(srrq.MachmodelName,'') + ' - ' + isnull(i.SerialNos, '') AS Instrument,  \r\n        lti.brandname AS BrandName,  \r\n        srrp.srof AS SrOf,  \r\n        srrp.country AS Country,  \r\n        con.FirstName AS RespInstrumentFName,  \r\n        con.LastName AS RespInstrumentLName,  \r\n        srrp.computerarlsn AS ComputerArlsn,  \r\n        srrp.software AS Software,  \r\n        srrp.firmaware AS Firmaware,  \r\n        srrp.installation AS Installation,  \r\n        srrp.analyticalassit AS AnalyticalAssit,  \r\n        srrp.prevmaintenance AS PrevMaintenance,  \r\n        srrp.rework AS Rework,  \r\n        srrp.corrmaintenance AS CorrMaintenance,  \r\n        srrp.problem AS Problem,  \r\n        srrp.workfinished AS WorkFinished,  \r\n        srrp.interrupted AS Interrupted,  \r\n        srrp.reason AS Reason,  \r\n        srrp.nextvisitscheduled AS NextVisitScheduled,  \r\n        srrp.engineercomments AS EngineerComments,  \r\n        srrp.servicereportdate AS ServiceReportDate,  \r\n        srrp.servicerequestid AS ServiceRequestId,  \r\n        srrp.signengname AS SignEngName,  \r\n        srrp.signcustname AS SignCustName,  \r\n        srrp.engsignature AS EngSignature,  \r\n        srrp.custsignature AS CustSignature,  \r\n        srrq.serreqno AS SerReqNo,  \r\n        rqid.itemname AS RequestType,  \r\n        srrp.workcompleted AS WorkCompleted,  \r\n        srrp.isdeleted AS SrpIsDeleted,  \r\n        srrq.isdeleted AS SrqIsDeleted,  \r\n        cust.defdistregionid AS DefDistRegionId,  \r\n        srrp.iscompleted AS IsCompleted,  \r\n        srrq.machinesno AS MachinesNo,  \r\n        srrq.siteid AS SiteId,  \r\n        srrq.visittype AS VisitType,  \r\n  CAST( 0 as bit) as Attachment,  \r\n  CAST( 0 as bit) as IsWorkDone,  \r\n  0 as TotalDays  \r\n    FROM  \r\n        Transactions.servicereport srrp  \r\n        JOIN Transactions.servicerequest srrq ON srrp.servicerequestid = srrq.id  \r\n        LEFT JOIN Masters.listtypeitems li ON srrp.department = li.id  \r\n        LEFT JOIN Masters.SiteContact con ON srrp.RespInstrumentId = con.id  \r\n        LEFT JOIN Masters.brand lti ON srrp.BrandId = lti.id  \r\n        LEFT JOIN Masters.MasterData rqid ON srrq.visittype = rqid.id  \r\n        LEFT JOIN Masters.customer cust ON srrq.custid = cust.id  \r\n  Left join Masters.Instrument i on srrp.InstrumentId = i.Id  \r\n\r\n");
            migrationBuilder.Sql("CREATE     \r\nVIEW [dbo].[VW_SparepartConsumedHistory] AS\r\n    SELECT \r\n        spcon.QtyConsumed ,\r\n        srq.SerReqNo,\r\n        srep.ServiceReportDate,\r\n        srq.CustId AS customerid,\r\n        spcon.CustomerSPInventoryId ,\r\n        spcon.IsDeleted ,\r\n        cust.DefDistRegionId AS DefDistRegionId\r\n    FROM\r\n        Transactions.ServiceRequest srq\r\n        left JOIN Transactions.servicereport srep ON srep.servicerequestid = srq.id\r\n        left JOIN Transactions.SPConsumed spcon ON spcon.servicereportid = srep.id\r\n        LEFT JOIN Masters.Customer cust ON srq.custid = cust.id\r\nGO\r\n");
            migrationBuilder.Sql("CREATE view [dbo].[VW_Spareparts]  \r\n as  \r\nselect distinct  \r\n s.Id,  \r\n s.ConfigTypeId,  \r\n s.ConfigValueId,  \r\n ct.ItemName as ConfigTypeName,  \r\n isnull(cv.ConfigValue,'') as ConfigValueName,  \r\n s.CountryId,  \r\n s.CurrencyId,  \r\n s.DescCatalogue,  \r\n s.HsCode,  \r\n s.Image,  \r\n s.IsActive,  \r\n s.IsDeleted,  \r\n s.IsObselete,  \r\n s.PartNo,  \r\n s.PartType,  \r\n pt.ItemName as PartTypeName,  \r\n s.ItemDesc,  \r\n s.Price,  \r\n s.Qty,  \r\n s.ReplacePartNoId ,\r\n s.PartNo + ' => '+ s.ItemDesc as PartNoDesc\r\nfrom [Masters].Sparepart s  \r\nleft join VW_ListItems pt on s.PartType = pt.ListTypeItemId  \r\nleft join VW_ListItems ct on s.ConfigTypeId = ct.ListTypeItemId  \r\nleft join [Masters].ConfigTypeValues cv on s.ConfigValueId = cv.Id  \r\nwhere s.isactive = 1;  \r\nGO\r\n");
            migrationBuilder.Sql("CREATE \r\nVIEW [dbo].[VW_SparesRecommended] AS\r\n    SELECT \r\n        sr.id AS ServiceRequestId,\r\n        srp.id AS ServiceReportId,\r\n        sr.serreqno AS SerReqNo,\r\n        sr.assignedto AS AssignedToId,\r\n        c.FirstName AS AssignedToFName,\r\n        c.LastName AS AssignedToLName,\r\n        srp.servicereportdate AS ServiceReportDate,\r\n        spre.id AS SpareRecomId,\r\n        spre.partno AS PartNo,\r\n        spre.hsccode AS HscCode,\r\n        spre.qtyrecommended AS QtyRecommended,\r\n        sr.custid AS CustId,\r\n        spre.isdeleted AS IsDeleted,\r\n        cust.defdistregionid AS DefDistRegionId,\r\n        dist.distname AS DefaultDistributor,\r\n        sr.machinesno AS Instrument,\r\n        spre.createdon AS CreatedOn,\r\n        spre.createdby AS CreatedBy,\r\n        s.DistId AS SiteRegion, -- distributor\r\n        ins.businessunitid AS BusinessUnitId,\r\n        ins.brandid AS BrandId,\r\n\t\ts.id as SiteId\r\n    FROM\r\n        Transactions.servicerequest sr\r\n        JOIN Transactions.servicereport srp ON sr.id = srp.servicerequestid\r\n        JOIN Transactions.SPRecommended spre ON srp.id = spre.servicereportid\r\n        LEFT JOIN Masters.RegionContact c ON sr.assignedto = c.id\r\n        LEFT JOIN Masters.customer cust ON sr.custid = cust.id\r\n        LEFT JOIN Masters.distributor dist ON sr.distid = dist.id\r\n        LEFT JOIN Masters.site s ON sr.siteid = s.id\r\n        LEFT JOIN Masters.instrument ins ON sr.machinesno = ins.id\r\n\r\nGO\r\n");
            migrationBuilder.Sql("\r\n\r\nCREATE\r\n View [dbo].[VW_UserProfile]\r\nas\r\n\tselect up.id as UserProfileId,\r\n\tup.Description,\r\n\tup.BrandIds,\r\n\tup.BusinessUnitIds,\r\n\tup.CustSites,\r\n\tup.DistRegions,\r\n\tup.IsActive as ActiveUserProfile,\r\n\tup.ManfSalesRegions,\r\n\tup.ProfileFor,\r\n\tup.RoleId,\r\n\tup.SegmentId,\r\n\tup.UserId,\r\n\tu.FirstName + ' ' + u.LastName as UserName,\r\n\tu.FirstName,\r\n\tu.LastName,\r\n\tu.Email,\r\n\tu.IsActive as ActiveUser,\r\n\tr.Name as UserRole,\r\n\tucm.ContactId,\r\n\tucm.ContactType,\r\n\trg.Id as EntityChildId,\r\n\trg.DistRegName as EntityChildName,\r\n\td.Id as EntityParentId,\r\n\td.Distname as EntityParentName,\r\n\tlti.ItemName as Designation,\r\n\tlti.Id as DesignationId,\r\n\tseg.Code as SegmentCode,\r\n\t'' as SelectedBusinessUnitId,\r\n\t'' as SelectedBrandId,\r\n\tu.TenantId as Company,\r\n (select SubscribedBy from CIMSaaS.Multitenancy.Tenants where id = u.TenantId ) as IsManfSubscribed\r\n\tfrom \r\n\t[Masters].UserProfiles up\r\n\tinner join [Identity].users u on up.UserId = u.Id\r\n\tinner join [Identity].Roles r on up.RoleId = r.Id\r\n\tinner join [Masters].MasterData seg on up.SegmentId = seg.Id\r\n\t--inner join [Masters].BusinessUnit bu on bu.Id = up.BusinessUnitId\r\n\t--inner join [Masters].Brand br on br.Id = up.BrandId\r\n\tinner join [Identity].UserContactMapping ucm on u.Id = ucm.UserId and ucm.ContactType = 'DR'\r\n\tinner join [Masters].RegionContact rc on ucm.ContactId = rc.Id\r\n\tinner join [Masters].ListTypeItems lti on rc.DesignationId = lti.Id\r\n\tinner join [Masters].Regions rg on rc.RegionId = rg.id\r\n\tinner join [Masters].Distributor d on rg.DistId = d.Id\r\n\tunion all\r\n\tselect up.id as UserProfileId,\r\n\tup.Description,\r\n\tup.BrandIds,\r\n\tup.BusinessUnitIds,\r\n\tup.CustSites,\r\n\tup.DistRegions,\r\n\tup.IsActive as ActiveUserProfile,\r\n\tup.ManfSalesRegions,\r\n\tup.ProfileFor,\r\n\tup.RoleId,\r\n\tup.SegmentId,\r\n\tup.UserId,\r\n\tu.FirstName + ' ' + u.LastName as UserName,\r\n\tu.FirstName,\r\n\tu.LastName,\r\n\tu.Email,\r\n\tu.IsActive as ActiveUser,\r\n\tr.Name as UserRole,\r\n\tucm.ContactId,\r\n\tucm.ContactType,\r\n\ts.Id as EntityChildId,\r\n\ts.CustRegName as EntityChildName,\r\n\tc.Id  as EntityParentId,\r\n\tc.CustName as EntityParentName,\r\n\tlti.ItemName as Designation,\r\n\tlti.Id as DesignationId,\r\n\tseg.Code as SegmentCode,\r\n\t'' as SelectedBusinessUnitId,\r\n\t'' as SelectedBrandId,\r\n\tu.TenantId as Company,\r\n (select SubscribedBy from CIMSaaS.Multitenancy.Tenants where id = u.TenantId ) as IsManfSubscribed\r\n\tfrom \r\n\t[Masters].UserProfiles up\r\n\tinner join [Identity].users u on up.UserId = u.Id\r\n\tinner join [Identity].Roles r on up.RoleId = r.Id\r\n\tinner join [Masters].MasterData seg on up.SegmentId = seg.Id\r\n\t--inner join [Masters].BusinessUnit bu on bu.Id = up.BusinessUnitId\r\n\t--inner join [Masters].Brand br on br.Id = up.BrandId\r\n\tinner join [Identity].UserContactMapping ucm on u.Id = ucm.UserId  and ucm.ContactType = 'CS'\r\n\tinner join [Masters].SiteContact sc on ucm.ContactId = sc.Id\r\n\tinner join [Masters].ListTypeItems lti on sc.DesignationId = lti.Id\r\n\tinner join [Masters].Site s on sc.SiteId =s.id\r\n\tinner join [Masters].Customer c on s.CustomerId = c.Id\r\n\tunion all\r\n\tselect up.id as UserProfileId,\r\n\tup.Description,\r\n\tup.BrandIds,\r\n\tup.BusinessUnitIds,\r\n\tup.CustSites,\r\n\tup.DistRegions,\r\n\tup.IsActive as ActiveUserProfile,\r\n\tup.ManfSalesRegions,\r\n\tup.ProfileFor,\r\n\tup.RoleId,\r\n\tup.SegmentId,\r\n\tup.UserId,\r\n\tu.FirstName + ' ' + u.LastName as UserName,\r\n\tu.FirstName,\r\n\tu.LastName,\r\n\tu.Email,\r\n\tu.IsActive as ActiveUser,\r\n\tr.Name as UserRole,\r\n\tucm.ContactId,\r\n\tucm.ContactType,\r\n\tsr.Id as EntityChildId,\r\n\tsr.SalesRegionName as EntityChildName,\r\n\tm.Id  as EntityParentId,\r\n\tm.ManfName as EntityParentName,\r\n\tlti.ItemName as Designation,\r\n\tlti.Id as DesignationId,\r\n\tseg.Code as SegmentCode,\r\n\t'' as SelectedBusinessUnitId,\r\n\t'' as SelectedBrandId,\r\n\tu.TenantId as Company,\r\n (select SubscribedBy from CIMSaaS.Multitenancy.Tenants where id = u.TenantId ) as IsManfSubscribed\r\n\tfrom \r\n\t[Masters].UserProfiles up\r\n\tinner join [Identity].users u on up.UserId = u.Id\r\n\tinner join [Identity].Roles r on up.RoleId = r.Id\r\n\tinner join [Masters].MasterData seg on up.SegmentId = seg.Id\r\n\t--inner join [Masters].BusinessUnit bu on bu.Id = up.BusinessUnitId\r\n\t--inner join [Masters].Brand br on br.Id = up.BrandId\r\n\tinner join [Identity].UserContactMapping ucm on u.Id = ucm.UserId and ucm.ContactType = 'MSR'\r\n\tinner join [Masters].SalesRegionContact src on ucm.ContactId = src.Id\r\n\tinner join [Masters].ListTypeItems lti on src.DesignationId = lti.Id\r\n\tinner join [Masters].SalesRegion sr on src.SalesRegionId =sr.id\r\n\tinner join [Masters].Manufacturer m on sr.ManfId = m.Id;\r\nGO\r\n\r\n\r\n");

            /// UNIQUE CONSTRAINTS
            migrationBuilder.Sql(@"
                    ALTER TABLE MASTERS.BUSINESSUNIT ADD CONSTRAINT INDUQ_BUSINESSUNIT UNIQUE (BUSINESSUNITNAME);
                    ALTER TABLE MASTERS.BRAND ADD CONSTRAINT INDUQ_BRAND UNIQUE (BUSINESSUNITID, BRANDNAME);
                    ALTER TABLE MASTERS.CONFIGTYPEVALUES ADD CONSTRAINT INDUQ_CONFIGTYPEVALUES UNIQUE (LISTTYPEITEMID, CONFIGVALUE);
                    ALTER TABLE MASTERS.COUNTRY ADD CONSTRAINT INDUQ_COUNTRY UNIQUE ([NAME]);
                    ALTER TABLE MASTERS.CURRENCY ADD CONSTRAINT INDUQ_CURRENCY UNIQUE ([NAME]);
                    ALTER TABLE MASTERS.CUSTOMER ADD CONSTRAINT INDUQ_CUSTOMER UNIQUE (CUSTNAME);
                    ALTER TABLE MASTERS.DISTRIBUTOR ADD CONSTRAINT INDUQ_DISTRIBUTOR UNIQUE (DISTNAME);
                    ALTER TABLE MASTERS.INSTRUMENT ADD CONSTRAINT INDUQ_INSTRUMENT UNIQUE (SERIALNOS);
                    ALTER TABLE MASTERS.INSTRUMENTACCESSORY ADD CONSTRAINT INDUQ_INSTRUMENTACCESS UNIQUE (ACCESSORYNAME);
                    ALTER TABLE MASTERS.INSTRUMENTSPARES ADD CONSTRAINT INDUQ_INSTRUMENTSPARES UNIQUE (INSTRUMENTID, SPAREPARTID);
                    ALTER TABLE MASTERS.LISTTYPEITEMS ADD CONSTRAINT INDUQ_LISTTYPEITEMS UNIQUE (LISTTYPEID, ITEMNAME);
                    ALTER TABLE MASTERS.MANUFACTURER ADD CONSTRAINT INDUQ_MANUFACTURER UNIQUE (MANFNAME);
                   -- ALTER TABLE MASTERS.MASTERDATA ADD CONSTRAINT INDUQ_MASTERDATA UNIQUE (LISTTYPEID, ITEMNAME);
                    ALTER TABLE MASTERS.REGIONS ADD CONSTRAINT INDUQ_REGION UNIQUE (DISTID, DISTREGNAME);
                    ALTER TABLE MASTERS.REGIONCONTACT ADD CONSTRAINT INDUQ_REGIONCONTACT UNIQUE (REGIONID, PRIMARYEMAIL);
                    ALTER TABLE MASTERS.SALESREGION ADD CONSTRAINT INDUQ_SALESREGION UNIQUE (MANFID, SALESREGIONNAME);
                    ALTER TABLE MASTERS.SALESREGIONCONTACT ADD CONSTRAINT INDUQ_SALESREGIONCONTACT UNIQUE (SALESREGIONID, PRIMARYEMAIL);
                    ALTER TABLE MASTERS.SITE ADD CONSTRAINT INDUQ_SITE UNIQUE (CUSTOMERID, CUSTREGNAME);
                    ALTER TABLE MASTERS.SITECONTACT ADD CONSTRAINT INDUQ_SITECONTACT UNIQUE (SITEID, PRIMARYEMAIL);
                    ALTER TABLE MASTERS.SPAREPART ADD CONSTRAINT INDUQ_SPAREPART UNIQUE (PARTNO);
                    ALTER TABLE TRANSACTIONS.ADVANCEREQUEST ADD CONSTRAINT INDUQ_ADVANCEREQUEST UNIQUE (ENGINEERID, SERVICEREQUESTID);
                    ALTER TABLE TRANSACTIONS.AMC ADD CONSTRAINT INDUQ_AMC UNIQUE (BILLTO, CUSTSITE,SERVICEQUOTE);
                    ALTER TABLE TRANSACTIONS.AMCINSTRUMENT ADD CONSTRAINT INDUQ_AMCINSTRUMENT UNIQUE (AMCID, INSTRUMENTID);
                    ALTER TABLE TRANSACTIONS.AMCSTAGES ADD CONSTRAINT INDUQ_AMCSTAGES UNIQUE (AMCID, STAGE);
                    ALTER TABLE TRANSACTIONS.BANKDETAILS ADD CONSTRAINT INDUQ_BANKDETAILS UNIQUE (CONTACTID, BANKACCOUNTNO);
                    ALTER TABLE TRANSACTIONS.CUSTOMERINSTRUMENT ADD CONSTRAINT INDUQ_CUSTOMERINSTRUMENT UNIQUE (CUSTSITEID, INSTRUMENTID);
                    ALTER TABLE TRANSACTIONS.CUSTSPINVENTORY ADD CONSTRAINT INDUQ_CUSTSPINVENTORY UNIQUE (CUSTOMERID,SITEID, INSTRUMENTID, SPAREPARTID);
                    ALTER TABLE TRANSACTIONS.OFFERREQUEST ADD CONSTRAINT INDUQ_OFFERREQUEST UNIQUE (CUSTOMERID,CUSTOMERSITEID, DISTRIBUTORID, OFFREQNO);
                    ALTER TABLE TRANSACTIONS.OFFERREQUESTPROCESS ADD CONSTRAINT INDUQ_OFFERREQUESTPROCESS UNIQUE (OFFERREQUESTID,STAGE);
                    ALTER TABLE TRANSACTIONS.PASTSERVICEREPORT ADD CONSTRAINT INDUQ_PASTSERVICEREPORT UNIQUE (CUSTOMERID,SITEID, INSTRUMENTID,[OF]);
                    ALTER TABLE TRANSACTIONS.SERVICEREPORT ADD CONSTRAINT INDUQ_SERVICEREPORT UNIQUE (SERVICEREQUESTID);
                    ALTER TABLE TRANSACTIONS.SPAREPARTSOFFERREQUEST ADD CONSTRAINT INDUQ_SPAREPARTSOFFERREQUEST UNIQUE (OFFERREQUESTID, SPAREPARTID);
                    ALTER TABLE TRANSACTIONS.SPCONSUMED ADD CONSTRAINT INDUQ_SPCONSUMED UNIQUE (SERVICEREPORTID,PARTNO);
                    ALTER TABLE TRANSACTIONS.SPRECOMMENDED ADD CONSTRAINT INDUQ_SPRECOMMENDED UNIQUE (SERVICEREPORTID,PARTNO);
                    ALTER TABLE TRANSACTIONS.TRAVELEXPENSE ADD CONSTRAINT INDUQ_TRAVELEXPENSE UNIQUE (ENGINEERID, SERVICEREQUESTID);
                    ALTER TABLE TRANSACTIONS.TRAVELINVOICE ADD CONSTRAINT INDUQ_TRAVELINVOICE UNIQUE (ENGINEERID, SERVICEREQUESTID);
                    GO
                    ");



            /// FK constriaints
            migrationBuilder.Sql(@"
            ALTER TABLE Masters.Brand ADD CONSTRAINT [FK_Brand_BU_BUId] FOREIGN KEY (BusinessUnitId) REFERENCES Masters.BusinessUnit([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.ConfigTypeValues ADD CONSTRAINT [FK_ConfgVal_ListItem_ListItemId] FOREIGN KEY (ListTypeItemId) REFERENCES Masters.ListTypeItems([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.Country ADD CONSTRAINT [FK_Country_Curr_CurrId] FOREIGN KEY (CurrencyId) REFERENCES Masters.Currency([Id]) ON DELETE NO ACTION;  ---- blank record issue
            ALTER TABLE Masters.Customer ADD CONSTRAINT [FK_Cust_Country_CountryId] FOREIGN KEY (CountryId) REFERENCES Masters.Country([Id]) ON DELETE NO ACTION; 
            -- ALTER TABLE Masters.Customer ADD CONSTRAINT [FK_Cust_Country_AddrCountryId] FOREIGN KEY (AddrCountryId) REFERENCES Masters.Country([Id]) ON DELETE NO ACTION;  -- coln not in use
            ALTER TABLE Masters.Customer ADD CONSTRAINT [FK_Cust_Dist_DistId] FOREIGN KEY (DefDistId) REFERENCES Masters.Distributor([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Masters.Site ADD CONSTRAINT [FK_Site_Cust_CustId] FOREIGN KEY (CustomerId) REFERENCES Masters.Customer([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.Site ADD CONSTRAINT [FK_Site_Country_CountryId] FOREIGN KEY (CountryId) REFERENCES Masters.Country([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.Site ADD CONSTRAINT [FK_Site_Dist_DistId] FOREIGN KEY (DistId) REFERENCES Masters.Distributor([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.Site ADD CONSTRAINT [FK_Site_Region_RegionId] FOREIGN KEY (RegionId) REFERENCES Masters.Regions([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Masters.SiteContact ADD CONSTRAINT [FK_SiteCon_Site_SiteId] FOREIGN KEY (SiteId) REFERENCES Masters.Site([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.SiteContact ADD CONSTRAINT [FK_SiteCon_ListItem_DesigId] FOREIGN KEY (DesignationId) REFERENCES Masters.ListTypeItems([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.SiteContact ADD CONSTRAINT [FK_SiteCon_Country_AddrCountryId] FOREIGN KEY (AddrCountryId) REFERENCES Masters.Country([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Masters.Distributor ADD CONSTRAINT [FK_Dist_Country_AddrCountryId] FOREIGN KEY (AddrCountryId) REFERENCES Masters.Country([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.Regions ADD CONSTRAINT [FK_Region_Dist_DistId] FOREIGN KEY (DistId) REFERENCES Masters.Distributor([Id]) ON DELETE NO ACTION; 
             -- ALTER TABLE Masters.Regions ADD CONSTRAINT [FK_Region_Country_Countries] FOREIGN KEY (Countries) REFERENCES Masters.Country([Id]) ON DELETE NO ACTION; -- datatype mismatch
             -- ALTER TABLE Masters.Regions ADD CONSTRAINT [FK_Region_Country_AddrCountryId] FOREIGN KEY (AddrCountryId) REFERENCES Masters.Country([Id]) ON DELETE NO ACTION; -- not used coln

            ALTER TABLE Masters.RegionContact ADD CONSTRAINT [FK_RegionCon_Region_RegionId] FOREIGN KEY (RegionId) REFERENCES Masters.Regions([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.RegionContact ADD CONSTRAINT [FK_RegionCon_ListItem_DesigId] FOREIGN KEY (DesignationId) REFERENCES Masters.ListTypeItems([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.RegionContact ADD CONSTRAINT [FK_RegionCon_Country_AddrCountryId] FOREIGN KEY (AddrCountryId) REFERENCES Masters.Country([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Masters.Manufacturer ADD CONSTRAINT [FK_Manf_Country_AddrCountryId] FOREIGN KEY (AddrCountryId) REFERENCES Masters.Country([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Masters.SalesRegion ADD CONSTRAINT [FK_SalesReg_Manf_ManfId] FOREIGN KEY (ManfId) REFERENCES Masters.Manufacturer([Id]) ON DELETE NO ACTION; 
            -- -- ALTER TABLE Masters.SalesRegion ADD CONSTRAINT [FK_SalesReg_Country_Countries] FOREIGN KEY (Countries) REFERENCES Masters.Country([Id]) ON DELETE NO ACTION;  -- datatype mismatch
            ALTER TABLE Masters.SalesRegion ADD CONSTRAINT [FK_SalesReg_Country_AddrCountryId] FOREIGN KEY (AddrCountryId) REFERENCES Masters.Country([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Masters.SalesRegionContact ADD CONSTRAINT [FK_SalRegionCon_SalRegion_SalesRegionId] FOREIGN KEY (SalesRegionId) REFERENCES Masters.SalesRegion([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.SalesRegionContact ADD CONSTRAINT [FK_SalRegionCon_ListItem_DesigId] FOREIGN KEY (DesignationId) REFERENCES Masters.ListTypeItems([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.SalesRegionContact ADD CONSTRAINT [FK_SalRegionCon_Country_AddrCountryId] FOREIGN KEY (AddrCountryId) REFERENCES Masters.Country([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Masters.Instrument ADD CONSTRAINT [FK_Instr_BU_BUId] FOREIGN KEY (BusinessUnitId) REFERENCES Masters.BusinessUnit([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.Instrument ADD CONSTRAINT [FK_Instr_Brand_BrandId] FOREIGN KEY (BrandId) REFERENCES Masters.Brand([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.Instrument ADD CONSTRAINT [FK_Instr_Manf_ManufId] FOREIGN KEY (ManufId) REFERENCES Masters.Manufacturer([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Masters.InstrumentAccessory ADD CONSTRAINT [FK_InstrAcc_Instr_InstrId] FOREIGN KEY (InstrumentId) REFERENCES Masters.Instrument([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Masters.InstrumentSpares ADD CONSTRAINT [FK_InstrSpare_Instr_InstrId] FOREIGN KEY (InstrumentId) REFERENCES Masters.Instrument([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.InstrumentSpares ADD CONSTRAINT [FK_InstrSpare_Spare_SpareId] FOREIGN KEY (SparepartId) REFERENCES Masters.Sparepart([Id]) ON DELETE NO ACTION; 
            -- ALTER TABLE Masters.InstrumentSpares ADD CONSTRAINT [FK_InstrSpare_ListItem_ConfTypeId] FOREIGN KEY (ConfigTypeId) REFERENCES Masters.ListTypeItems([Id]) ON DELETE NO ACTION; 
            -- ALTER TABLE Masters.InstrumentSpares ADD CONSTRAINT [FK_InstrSpare_ConfVal_ConfValId] FOREIGN KEY (ConfigValueId) REFERENCES Masters.ConfigTypeValues([Id]) ON DELETE NO ACTION; 

            -- ALTER TABLE Masters.Sparepart ADD CONSTRAINT [FK_Spare_ConfType_ConfTypeId] FOREIGN KEY (ConfigTypeId) REFERENCES Masters.ListTypeItems([Id]) ON DELETE NO ACTION; 
            -- ALTER TABLE Masters.Sparepart ADD CONSTRAINT [FK_Spare_ConfVal_ConfValId] FOREIGN KEY (ConfigValueId) REFERENCES Masters.ConfigTypeValues([Id]) ON DELETE NO ACTION; -- blankrecord
            ALTER TABLE Masters.Sparepart ADD CONSTRAINT [FK_Spare_Country_CountryId] FOREIGN KEY (CountryId) REFERENCES Masters.Country([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Masters.Sparepart ADD CONSTRAINT [FK_Spare_Currency_CurrencyId] FOREIGN KEY (CurrencyId) REFERENCES Masters.Currency([Id]) ON DELETE NO ACTION; 

            ---- Transactions -----
            ALTER TABLE Transactions.AdvanceRequest ADD CONSTRAINT [FK_Adv_RegCon_EngineerId] FOREIGN KEY (EngineerId) REFERENCES Masters.RegionContact([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.AdvanceRequest ADD CONSTRAINT [FK_Adv_Dist_DistributorId] FOREIGN KEY (DistributorId) REFERENCES Masters.Distributor([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.AdvanceRequest ADD CONSTRAINT [FK_Adv_Cust_CustomerId] FOREIGN KEY (CustomerId) REFERENCES Masters.Customer([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.AdvanceRequest ADD CONSTRAINT [FK_Adv_SerReq_SerReqId] FOREIGN KEY (ServiceRequestId) REFERENCES Transactions.ServiceRequest([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.AMC ADD CONSTRAINT [FK_AMC_Cust_BillTo] FOREIGN KEY (BillTo) REFERENCES Masters.Customer([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.AMC ADD CONSTRAINT [FK_AMC_Site_CustSite] FOREIGN KEY (CustSite) REFERENCES Masters.Site([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.AMC ADD CONSTRAINT [FK_AMC_Brand_BrandId] FOREIGN KEY (BrandId) REFERENCES Masters.Brand([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.AMC ADD CONSTRAINT [FK_AMC_Currency_CurrencyId] FOREIGN KEY (CurrencyId) REFERENCES Masters.Currency([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.AMCInstrument ADD CONSTRAINT [FK_AMCIns_ListItem_InsTypeId] FOREIGN KEY (InsTypeId) REFERENCES Masters.ListTypeItems([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.AMCInstrument ADD CONSTRAINT [FK_AMCIns_AMC_AMCId] FOREIGN KEY (AMCId) REFERENCES Transactions.AMC([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.AMCInstrument ADD CONSTRAINT [FK_AMCIns_Instr_InstrumentId] FOREIGN KEY (InstrumentId) REFERENCES Masters.Instrument([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.AMCItems ADD CONSTRAINT [FK_AMCItem_AMC_AMCId] FOREIGN KEY (AMCId) REFERENCES Transactions.AMC([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.AMCStages ADD CONSTRAINT [FK_AMCStage_AMC_AMCId] FOREIGN KEY (AMCId) REFERENCES Transactions.AMC([Id]) ON DELETE NO ACTION; 
           -- ALTER TABLE Transactions.AMCStages ADD CONSTRAINT [FK_AMCStage_ListItem_PaymentTypeId] FOREIGN KEY (PaymentTypeId) REFERENCES Masters.ListTypeItems([Id]) ON DELETE NO ACTION;  -- datatype mismatch
           -- ALTER TABLE Transactions.AMCStages ADD CONSTRAINT [FK_AMCStage_Currency_PayAmtCurrId] FOREIGN KEY (PayAmtCurrencyId) REFERENCES Masters.Currency([Id]) ON DELETE NO ACTION;  -- datatype mismatch

            ALTER TABLE Transactions.BankDetails ADD CONSTRAINT [FK_BankDet_RegContact_ContactId] FOREIGN KEY (ContactId) REFERENCES Masters.RegionContact([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.CustomerInstrument ADD CONSTRAINT [FK_CustInst_Site_CustSiteId] FOREIGN KEY (CustSiteId) REFERENCES Masters.Site([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.CustomerInstrument ADD CONSTRAINT [FK_CustInst_Instr_InstrumentId] FOREIGN KEY (InstrumentId) REFERENCES Masters.Instrument([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.CustomerInstrument ADD CONSTRAINT [FK_CustInst_Currency_CurrencyId] FOREIGN KEY (CurrencyId) REFERENCES Masters.Currency([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.CustomerInstrument ADD CONSTRAINT [FK_CustInst_SiteCon_OperatorId] FOREIGN KEY (OperatorId) REFERENCES Masters.SiteContact([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.CustomerInstrument ADD CONSTRAINT [FK_CustInst_SiteCon_InstrEngId] FOREIGN KEY (InstruEngineerId) REFERENCES Masters.SiteContact([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.CustomerSatisfactionSurvey ADD CONSTRAINT [FK_CustSurvey_Dist_DistId] FOREIGN KEY (DistId) REFERENCES Masters.Distributor([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.CustomerSatisfactionSurvey ADD CONSTRAINT [FK_CustSurvey_RegCon_EngineerId] FOREIGN KEY (EngineerId) REFERENCES Masters.RegionContact([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.CustomerSatisfactionSurvey ADD CONSTRAINT [FK_CustSurvey_ServiceRequest_SerReqId] FOREIGN KEY (ServiceRequestId) REFERENCES Transactions.ServiceRequest([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.CustSPInventory ADD CONSTRAINT [FK_CustInven_Customer_CustId] FOREIGN KEY (CustomerId) REFERENCES Masters.Customer([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.CustSPInventory ADD CONSTRAINT [FK_CustInven_Spare_SpareId] FOREIGN KEY (SparePartId) REFERENCES Masters.Sparepart([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.CustSPInventory ADD CONSTRAINT [FK_CustInven_Site_SiteId] FOREIGN KEY (SiteId) REFERENCES Masters.Site([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.CustSPInventory ADD CONSTRAINT [FK_CustInven_Instr_InstrumentId] FOREIGN KEY (InstrumentId) REFERENCES Masters.Instrument([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.EngScheduler ADD CONSTRAINT [FK_EngSch_SerReq_SerReqId] FOREIGN KEY (SerReqId) REFERENCES Transactions.ServiceRequest([Id]) ON DELETE NO ACTION; 
            ALTER TABLE [Transactions].[EngScheduler]  WITH CHECK ADD  CONSTRAINT [FK_EngSch_EngAction_ActionId] FOREIGN KEY ([ActionId]) REFERENCES [Transactions].SREngAction ([Id])
            ALTER TABLE Transactions.EngScheduler ADD CONSTRAINT [FK_EngSch_RegCon_EngId] FOREIGN KEY (EngId) REFERENCES Masters.RegionContact([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.OfferRequest ADD CONSTRAINT [FK_OffReq_Dist_DistId] FOREIGN KEY (DistributorId) REFERENCES Masters.Distributor([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.OfferRequest ADD CONSTRAINT [FK_OffReq_Curr_CurrencyId] FOREIGN KEY (CurrencyId) REFERENCES Masters.Currency([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.OfferRequest ADD CONSTRAINT [FK_OffReq_Cust_CustId] FOREIGN KEY (CustomerId) REFERENCES Masters.Customer([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.OfferRequest ADD CONSTRAINT [FK_OffReq_Site_SiteId] FOREIGN KEY (CustomerSiteId) REFERENCES Masters.Site([Id]) ON DELETE NO ACTION; 
             -- ALTER TABLE Transactions.OfferRequest ADD CONSTRAINT [FK_OffReq_ListItem_PayTerms] FOREIGN KEY (PaymentTerms) REFERENCES Masters.ListTypeItems([Id]) ON DELETE NO ACTION;  -- datatype mismatch
            ALTER TABLE Transactions.OfferRequest ADD CONSTRAINT [FK_OffReq_Curr_AirFreightCurr] FOREIGN KEY (AirFreightChargesCurr) REFERENCES Masters.Currency([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.OfferRequest ADD CONSTRAINT [FK_OffReq_Curr_InspCurr] FOREIGN KEY (InspectionChargesCurr) REFERENCES Masters.Currency([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.OfferRequest ADD CONSTRAINT [FK_OffReq_Curr_LCAAdminCurr] FOREIGN KEY (LcadministrativeChargesCurr) REFERENCES Masters.Currency([Id]) ON DELETE NO ACTION;  -- datatype mismatch
            ALTER TABLE Transactions.OfferRequestProcess ADD CONSTRAINT [FK_OffReqPro_OffReq_OfferRequestId] FOREIGN KEY (OfferRequestId) REFERENCES Transactions.OfferRequest([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.OfferRequestProcess ADD CONSTRAINT [FK_OffReq_Curr_PayAmtCurr] FOREIGN KEY (PayAmtCurrencyId) REFERENCES Masters.Currency([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.OfferRequestProcess ADD CONSTRAINT [FK_OffReq_ListItems_PaymentTypeId] FOREIGN KEY (PaymentTypeId) REFERENCES Masters.ListTypeItems([Id]) ON DELETE NO ACTION;  -- datatype mismatch
            ALTER TABLE Transactions.OfferRequestProcess ADD CONSTRAINT [FK_OffReq_MasterData_Stage] FOREIGN KEY (Stage) REFERENCES Masters.MasterData([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.PastServiceReport ADD CONSTRAINT [FK_PastSerRep_Cust_CustId] FOREIGN KEY (CustomerId) REFERENCES Masters.Customer([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.PastServiceReport ADD CONSTRAINT [FK_PastSerRep_Site_SiteId] FOREIGN KEY (SiteId) REFERENCES Masters.Site([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.PastServiceReport ADD CONSTRAINT [FK_PastSerRep_Instr_InstrId] FOREIGN KEY (InstrumentId) REFERENCES Masters.Instrument([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.PastServiceReport ADD CONSTRAINT [FK_PastSerRep_Brand_BrandId] FOREIGN KEY (BrandId) REFERENCES Masters.Brand([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.ServiceRequest ADD CONSTRAINT [FK_SerReq_Dist_DistId] FOREIGN KEY (DistId) REFERENCES Masters.Distributor([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.ServiceRequest ADD CONSTRAINT [FK_SerReq_Cust_CustId] FOREIGN KEY (CustId) REFERENCES Masters.Customer([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.ServiceRequest ADD CONSTRAINT [FK_SerReq_Site_SiteId] FOREIGN KEY (SiteId) REFERENCES Masters.Site([Id]) ON DELETE NO ACTION; 
           ALTER TABLE [Transactions].[ServiceRequest]  WITH CHECK ADD  CONSTRAINT [FK_SerReq_ListItem_BreakoccurId] FOREIGN KEY([BreakoccurDetailsId]) REFERENCES [Masters].[ListTypeItems] ([Id]) 

            ALTER TABLE Transactions.ServiceRequest ADD CONSTRAINT [FK_SerReq_MasterData_StatusId] FOREIGN KEY (StatusId) REFERENCES Masters.MasterData([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.ServiceRequest ADD CONSTRAINT [FK_SerReq_MasterData_StageId] FOREIGN KEY (StageId) REFERENCES Masters.MasterData([Id]) ON DELETE NO ACTION; 
            --ALTER TABLE Transactions.ServiceRequest ADD CONSTRAINT [FK_SerReq_AMC_AMCId] FOREIGN KEY (AMCId) REFERENCES Transactions.AMC([Id]) ON DELETE NO ACTION;  -- not all records have amc
            ALTER TABLE Transactions.ServiceRequest ADD CONSTRAINT [FK_SerReq_Curr_BaseCurrency] FOREIGN KEY (BaseCurrency) REFERENCES Masters.Currency([Id]) ON DELETE NO ACTION; -- blank record
            ALTER TABLE Transactions.ServiceRequest ADD CONSTRAINT [FK_SerReq_Curr_TotalCostCurrency] FOREIGN KEY (TotalCostCurrency) REFERENCES Masters.Currency([Id]) ON DELETE NO ACTION; -- blank record

            ALTER TABLE Transactions.SRAssignedHistory ADD CONSTRAINT [FK_SRAssignedHistory_SerReq_SerReqId] FOREIGN KEY (ServiceRequestId) REFERENCES Transactions.ServiceRequest([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.SRAssignedHistory ADD CONSTRAINT [FK_SRAssignedHistory_RegCon_EngId] FOREIGN KEY (EngineerId) REFERENCES Masters.RegionContact([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.SREngAction ADD CONSTRAINT [FK_SREngAction_SerReq_SerReqId] FOREIGN KEY (ServiceRequestId) REFERENCES Transactions.ServiceRequest([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.SREngAction ADD CONSTRAINT [FK_SREngAction_RegCon_EngId] FOREIGN KEY (EngineerId) REFERENCES Masters.RegionContact([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.SREngComments ADD CONSTRAINT [FK_SREngComments_SerReq_SerReqId] FOREIGN KEY (ServiceRequestId) REFERENCES Transactions.ServiceRequest([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.SREngComments ADD CONSTRAINT [FK_SREngComments_RegCon_EngId] FOREIGN KEY (EngineerId) REFERENCES Masters.RegionContact([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.ServiceReport ADD CONSTRAINT [FK_SerRep_SerReq_SerReqId] FOREIGN KEY (ServiceRequestId) REFERENCES Transactions.ServiceRequest([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.SPConsumed ADD CONSTRAINT [FK_SPConsumed_SerRep_SerRepId] FOREIGN KEY (ServiceReportId) REFERENCES Transactions.ServiceReport([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.SPConsumed ADD CONSTRAINT [FK_SPConsumed_SerRep_CustSPInvenId] FOREIGN KEY (CustomerSPInventoryId) REFERENCES Transactions.CustSpInventory([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.SPRecommended ADD CONSTRAINT [FK_SPRecommended_SerRep_SerRepId] FOREIGN KEY (ServiceReportId) REFERENCES Transactions.ServiceReport([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.SRPEngWorkDone ADD CONSTRAINT [FK_SRPEngWorkDone_SerRep_SerRepId] FOREIGN KEY (ServiceReportId) REFERENCES Transactions.ServiceReport([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.SRPEngWorkTime ADD CONSTRAINT [FK_SRPEngWorkTime_SerRep_SerRepId] FOREIGN KEY (ServiceReportId) REFERENCES Transactions.ServiceReport([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.SparepartsOfferRequest ADD CONSTRAINT [FK_SPOffReq_Sparepart_SparepartId] FOREIGN KEY (SparepartId) REFERENCES Masters.Sparepart([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.SparepartsOfferRequest ADD CONSTRAINT [FK_SPOffReq_OffReq_OfferRequestId] FOREIGN KEY (OfferRequestId) REFERENCES Transactions.OfferRequest([Id]) ON DELETE NO ACTION; 
            -- ALTER TABLE Transactions.SparepartsOfferRequest ADD CONSTRAINT [FK_SPOffReq_Curr_CurrencyId] FOREIGN KEY (CurrencyId) REFERENCES Masters.Currency([Id]) ON DELETE NO ACTION;  -- additional col
            -- ALTER TABLE Transactions.SparepartsOfferRequest ADD CONSTRAINT [FK_SPOffReq_Country_CountryId] FOREIGN KEY (CountryId) REFERENCES Masters.Country([Id]) ON DELETE NO ACTION;  -- additional col

            ALTER TABLE Transactions.TravelExpense ADD CONSTRAINT [FK_TravelExpense_Dist_DistId] FOREIGN KEY (DistributorId) REFERENCES Masters.Distributor([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.TravelExpense ADD CONSTRAINT [FK_TravelExpense_Cust_CustId] FOREIGN KEY (CustomerId) REFERENCES Masters.Customer([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.TravelExpense ADD CONSTRAINT [FK_TravelExpense_RegCon_EngineerId] FOREIGN KEY (EngineerId) REFERENCES Masters.RegionContact([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.TravelExpense ADD CONSTRAINT [FK_TravelExpense_SerReq_SerReqId] FOREIGN KEY (ServiceRequestId) REFERENCES Transactions.ServiceRequest([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.TravelExpense ADD CONSTRAINT [FK_TravelExpense_ListItem_Designation] FOREIGN KEY (Designation) REFERENCES Masters.ListTypeItems([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.TravelExpenseItems ADD CONSTRAINT [FK_TravelExpItem_TravelExp_TravelExpId] FOREIGN KEY (TravelExpenseId) REFERENCES Transactions.TravelExpense([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.TravelExpenseItems ADD CONSTRAINT [FK_TravelExpItem_MasterData_ExpNature] FOREIGN KEY (ExpNature) REFERENCES Masters.MasterData([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.TravelExpenseItems ADD CONSTRAINT [FK_TravelExpItem_Curr_Currency] FOREIGN KEY (Currency) REFERENCES Masters.Currency([Id]) ON DELETE NO ACTION;  -- blank record
            ALTER TABLE Transactions.TravelExpenseItems ADD CONSTRAINT [FK_TravelExpItem_MasterData_ExpenseBy] FOREIGN KEY (ExpenseBy) REFERENCES Masters.MasterData([Id]) ON DELETE NO ACTION; 

            ALTER TABLE Transactions.TravelInvoice ADD CONSTRAINT [FK_TravelInvoice_Dist_DistId] FOREIGN KEY (DistributorId) REFERENCES Masters.Distributor([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.TravelInvoice ADD CONSTRAINT [FK_TravelInvoice_RegCon_EngineerId] FOREIGN KEY (EngineerId) REFERENCES Masters.RegionContact([Id]) ON DELETE NO ACTION; 
            ALTER TABLE Transactions.TravelInvoice ADD CONSTRAINT [FK_TravelInvoice_SerReq_SerReqId] FOREIGN KEY (ServiceRequestId) REFERENCES Transactions.ServiceRequest([Id]) ON DELETE NO ACTION; 
            go            
            ");



            /// temporal tables
            /// 
            migrationBuilder.Sql(@"ALTER TABLE Masters.ListTypeItems ADD  ValidFrom DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL,
                                ValidTo DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL,   PERIOD FOR SYSTEM_TIME(ValidFrom, ValidTo);          
                            ALTER TABLE Masters.ListTypeItems SET( SYSTEM_VERSIONING = ON  (HISTORY_TABLE = Masters.ListTypeItemsHistory) );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("drop view VW_ListItems;");
            migrationBuilder.Sql("drop view VW_ServiceReport;");
            migrationBuilder.Sql("drop view VW_SparepartConsumedHistory;");
            migrationBuilder.Sql("drop view VW_SparesRecommended;");
            migrationBuilder.Sql("drop view VW_UserProfile;");
            migrationBuilder.Sql("drop view VW_Spareparts;");
            migrationBuilder.Sql("drop view VW_InstrumentSpares;");

            migrationBuilder.DropTable(
                name: "AdvanceRequest",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "AMC",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "AMCInstrument",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "AMCItems",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "AMCStages",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "BankDetails",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "Brand",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "BusinessUnit",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "ManfBusinessUnit",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "ConfigTypeValues",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "Currency",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "CustomerInstrument",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "CustomerSatisfactionSurvey",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "CustSPInventory",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "Distributor",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "EngScheduler",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "FileShare",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "Instrument",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "InstrumentAccessory",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "InstrumentSpares",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "ListType",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "ListTypeItems",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "Manufacturer",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "MasterData",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OfferRequest",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "OfferRequestProcess",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "PastServiceReport",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "RegionContact",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "Regions",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "SalesRegion",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "SalesRegionContact",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "Schools",
                schema: "Academics");

            migrationBuilder.DropTable(
                name: "ServiceReport",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "ServiceRequest",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "Site",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "SiteContact",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "Sparepart",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "SparepartsOfferRequest",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "SPConsumed",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "SPRecommended",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "SRAssignedHistory",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "SRAuditTrail",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "SREngAction",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "SREngComments",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "SRPEngWorkDone",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "SRPEngWorkTime",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "TravelExpense",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "TravelExpenseItems",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "TravelInvoice",
                schema: "Transactions");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserContactMapping",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserProfiles",
                schema: "Masters");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Identity");
        }
    }
}
