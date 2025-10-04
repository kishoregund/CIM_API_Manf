using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Masters.DataFiles
{
    internal class ViewScripts
    {
        #region  Roles
        /*
        INSERT INTO[Identity].[RoleClaims]
        ([RoleId],[ClaimType],[ClaimValue],[TenantId])
   VALUES((select id from [Identity].Roles where NormalizedName = 'DISTRIBUTOR_OPERATIONS_REGION')
	 ,'permission','Permission.Customer_Instrument.View',(SELECT DB_NAME() AS DatabaseName));

	 
  INSERT INTO[Identity].[RoleClaims]
        ([RoleId],[ClaimType],[ClaimValue],[TenantId])
     VALUES((select id from [Identity].Roles where NormalizedName = 'DISTRIBUTOR_OPERATIONS_REGION')
	 ,'permission','Permission.Customer_Instrument.Create',(SELECT DB_NAME() AS DatabaseName));
	 
  INSERT INTO[Identity].[RoleClaims]
        ([RoleId],[ClaimType],[ClaimValue],[TenantId])
     VALUES((select id from [Identity].Roles where NormalizedName = 'DISTRIBUTOR_OPERATIONS_REGION')
	 ,'permission','Permission.Customer_Instrument.Update',(SELECT DB_NAME() AS DatabaseName));
	 
	 
  INSERT INTO[Identity].[RoleClaims]
        ([RoleId],[ClaimType],[ClaimValue],[TenantId])
     VALUES((select id from [Identity].Roles where NormalizedName = 'DISTRIBUTOR_OPERATIONS_REGION')
	 ,'permission','Permission.Customer_Instrument.Delete',(SELECT DB_NAME() AS DatabaseName));

	 
  INSERT INTO[Identity].[RoleClaims]
        ([RoleId],[ClaimType],[ClaimValue],[TenantId])
     VALUES((select id from [Identity].Roles where NormalizedName = 'DISTRIBUTOR_OPERATIONS_REGION')
	 ,'permission','Permission.Users.Create',(SELECT DB_NAME() AS DatabaseName));
	 	 
  INSERT INTO[Identity].[RoleClaims]
        ([RoleId],[ClaimType],[ClaimValue],[TenantId])
     VALUES((select id from [Identity].Roles where NormalizedName = 'DISTRIBUTOR_OPERATIONS_REGION')
	 ,'permission','Permission.Users.Update',(SELECT DB_NAME() AS DatabaseName));

	 
  INSERT INTO[Identity].[RoleClaims]
        ([RoleId],[ClaimType],[ClaimValue],[TenantId])
     VALUES((select id from [Identity].Roles where NormalizedName = 'DISTRIBUTOR_OPERATIONS_REGION')
	 ,'permission','Permission.Users.Delete',(SELECT DB_NAME() AS DatabaseName));

	 
  INSERT INTO[Identity].[RoleClaims]
        ([RoleId],[ClaimType],[ClaimValue],[TenantId])
     VALUES((select id from [Identity].Roles where NormalizedName = 'DISTRIBUTOR_OPERATIONS_REGION')
	 ,'permission','Permission.Users.View',(SELECT DB_NAME() AS DatabaseName));



	 
  INSERT INTO[Identity].[RoleClaims]
        ([RoleId],[ClaimType],[ClaimValue],[TenantId])
     VALUES((select id from [Identity].Roles where NormalizedName = 'SITE')
	 ,'permission','Permission.Customer_Instrument.View',(SELECT DB_NAME() AS DatabaseName));

	 
	 
  INSERT INTO[Identity].[RoleClaims]
        ([RoleId],[ClaimType],[ClaimValue],[TenantId])
     VALUES((select id from [Identity].Roles where NormalizedName = 'ENGINEER')
	 ,'permission','Permission.Engineer_Dashboard.View',(SELECT DB_NAME() AS DatabaseName));
	 	 
  INSERT INTO[Identity].[RoleClaims]
        ([RoleId],[ClaimType],[ClaimValue],[TenantId])
     VALUES((select id from [Identity].Roles where NormalizedName = 'ENGINEER')
	 ,'permission','Permission.Scheduler.Delete',(SELECT DB_NAME() AS DatabaseName));
*/
        #endregion

        #region views
/*        
            migrationBuilder.Sql("Create    \r\nVIEW [dbo].[VW_ListItems] AS\r\n    SELECT \r\n        l.code AS ListCode,\r\n        l.id AS ListTypeId,\r\n        l.listname AS ListName,\r\n        li.id AS ListTypeItemId,\r\n        li.code AS ItemCode,\r\n        li.itemname AS ItemName,\r\n        li.isdeleted AS IsDeleted,\r\n        li.isescalationsupervisor AS IsEscalationSupervisor,\r\n        0 AS IsMaster\r\n    FROM\r\n        [Masters].ListType l\r\n        JOIN [Masters].ListTypeItems li ON l.Id = li.ListTypeId \r\n    UNION SELECT \r\n        l.code AS ListCode,\r\n        l.id AS ListTypeId,\r\n        l.listname AS ListName,\r\n        li.id AS ListTypeItemId,\r\n        li.code AS ItemCode,\r\n        li.itemname AS ItemName,\r\n        li.isdeleted AS IsDeleted,\r\n        li.isescalationsupervisor AS IsEscalationSupervisor,\r\n        1 AS IsMaster\r\n    FROM\r\n        [Masters].ListType l\r\n        JOIN [Masters].MasterData li ON l.Id = li.ListTypeId ;\r\nGO\r\n");
            migrationBuilder.Sql("CREATE     \r\nVIEW [dbo].[VW_InstrumentSpares] AS\r\n    SELECT \r\n        sp.id AS id,\r\n        sp.isactive AS isactive,\r\n        sp.configtypeid AS configtypeid,\r\n        ctv.itemname AS configtypename,\r\n        sp.partno AS partno,\r\n        sp.itemdesc AS itemdesc,\r\n        sp.qty AS qty,\r\n        sp.parttype AS parttype,\r\n        sp.desccatalogue AS desccatalogue,\r\n        sp.hscode AS hscode,\r\n        sp.countryid AS countryid,\r\n        c.name AS countryname,\r\n        sp.price AS price,\r\n        sp.currencyid AS currencyid,\r\n        cr.name AS currencyname,\r\n        mdpt.itemname AS parttypename,\r\n        sp.image AS image,\r\n        sp.isobselete AS isobselete,\r\n        sp.replacepartnoid AS replacepartnoid,\r\n        sp.configvalueid AS configvalueid,\r\n        '' AS configvaluename,\r\n        ic.instrumentid AS instrumentid,\r\n        c.isdeleted AS couisdeleted,\r\n        cr.isdeleted AS currisdeleted,\r\n        isnull(ic.insqty, 0) AS insqty,\r\n\t\tsp.PartNo + ' => '+ sp.ItemDesc as PartNoDesc\r\n    FROM\r\n        [Masters].Sparepart sp\r\n        LEFT JOIN [Masters].InstrumentSpares ic ON sp.configtypeid = ic.configtypeid \r\n            AND sp.id = ic.sparepartid\r\n        LEFT JOIN [Masters].country c ON sp.countryid = c.id\r\n        LEFT JOIN [Masters].currency cr ON sp.currencyid = cr.id\r\n        LEFT JOIN [Masters].listtypeitems licg ON sp.configtypeid = licg.id\r\n        LEFT JOIN [Masters].listtypeitems lipt ON sp.parttype = lipt.id\r\n        LEFT JOIN [Masters].listtypeitems ctv ON sp.configtypeid = ctv.id\r\n        LEFT JOIN [Masters].masterdata mdpt ON sp.parttype = mdpt.id\r\n\t\twhere sp.IsActive = 1 and sp.IsDeleted = 0\r\nGO\r\n");
            migrationBuilder.Sql("CREATE  VIEW [dbo].[VW_ServiceReport] AS  \r\n    SELECT   \r\n        srrp.id AS ServiceReportId,  \r\n        srrp.customer AS Customer,  \r\n        li.ItemName AS Department,  \r\n        srrp.town AS Town,  \r\n        srrp.labchief AS LabChief,  \r\n        srrq.MachmodelName + ' - ' + i.SerialNos AS Instrument,  \r\n        lti.brandname AS BrandName,  \r\n        srrp.srof AS SrOf,  \r\n        srrp.country AS Country,  \r\n        con.FirstName AS RespInstrumentFName,  \r\n        con.LastName AS RespInstrumentLName,  \r\n        srrp.computerarlsn AS ComputerArlsn,  \r\n        srrp.software AS Software,  \r\n        srrp.firmaware AS Firmaware,  \r\n        srrp.installation AS Installation,  \r\n        srrp.analyticalassit AS AnalyticalAssit,  \r\n        srrp.prevmaintenance AS PrevMaintenance,  \r\n        srrp.rework AS Rework,  \r\n        srrp.corrmaintenance AS CorrMaintenance,  \r\n        srrp.problem AS Problem,  \r\n        srrp.workfinished AS WorkFinished,  \r\n        srrp.interrupted AS Interrupted,  \r\n        srrp.reason AS Reason,  \r\n        srrp.nextvisitscheduled AS NextVisitScheduled,  \r\n        srrp.engineercomments AS EngineerComments,  \r\n        srrp.servicereportdate AS ServiceReportDate,  \r\n        srrp.servicerequestid AS ServiceRequestId,  \r\n        srrp.signengname AS SignEngName,  \r\n        srrp.signcustname AS SignCustName,  \r\n        srrp.engsignature AS EngSignature,  \r\n        srrp.custsignature AS CustSignature,  \r\n        srrq.serreqno AS SerReqNo,  \r\n        rqid.itemname AS RequestType,  \r\n        srrp.workcompleted AS WorkCompleted,  \r\n        srrp.isdeleted AS SrpIsDeleted,  \r\n        srrq.isdeleted AS SrqIsDeleted,  \r\n        cust.defdistregionid AS DefDistRegionId,  \r\n        srrp.iscompleted AS IsCompleted,  \r\n        srrq.machinesno AS MachinesNo,  \r\n        srrq.siteid AS SiteId,  \r\n        srrq.visittype AS VisitType,  \r\n  CAST( 0 as bit) as Attachment,  \r\n  CAST( 0 as bit) as IsWorkDone,  \r\n  0 as TotalDays  \r\n    FROM  \r\n        Transactions.servicereport srrp  \r\n        JOIN Transactions.servicerequest srrq ON srrp.servicerequestid = srrq.id  \r\n        LEFT JOIN Masters.listtypeitems li ON srrp.department = li.id  \r\n        LEFT JOIN Masters.SiteContact con ON srrp.RespInstrumentId = con.id  \r\n        LEFT JOIN Masters.brand lti ON srrp.BrandId = lti.id  \r\n        LEFT JOIN Masters.MasterData rqid ON srrq.visittype = rqid.id  \r\n        LEFT JOIN Masters.customer cust ON srrq.custid = cust.id  \r\n  Left join Masters.Instrument i on srrp.InstrumentId = i.Id  \r\n\r\n");
            migrationBuilder.Sql("CREATE     \r\nVIEW [dbo].[VW_SparepartConsumedHistory] AS\r\n    SELECT \r\n        spcon.QtyConsumed ,\r\n        srq.SerReqNo,\r\n        srep.ServiceReportDate,\r\n        srq.CustId AS customerid,\r\n        spcon.CustomerSPInventoryId ,\r\n        spcon.IsDeleted ,\r\n        cust.DefDistRegionId AS DefDistRegionId\r\n    FROM\r\n        Transactions.ServiceRequest srq\r\n        left JOIN Transactions.servicereport srep ON srep.servicerequestid = srq.id\r\n        left JOIN Transactions.SPConsumed spcon ON spcon.servicereportid = srep.id\r\n        LEFT JOIN Masters.Customer cust ON srq.custid = cust.id\r\nGO\r\n");
            migrationBuilder.Sql("CREATE view [dbo].[VW_Spareparts]  \r\n as  \r\nselect distinct  \r\n s.Id,  \r\n s.ConfigTypeId,  \r\n s.ConfigValueId,  \r\n ct.ItemName as ConfigTypeName,  \r\n isnull(cv.ConfigValue,'') as ConfigValueName,  \r\n s.CountryId,  \r\n s.CurrencyId,  \r\n s.DescCatalogue,  \r\n s.HsCode,  \r\n s.Image,  \r\n s.IsActive,  \r\n s.IsDeleted,  \r\n s.IsObselete,  \r\n s.PartNo,  \r\n s.PartType,  \r\n pt.ItemName as PartTypeName,  \r\n s.ItemDesc,  \r\n s.Price,  \r\n s.Qty,  \r\n s.ReplacePartNoId ,\r\n s.PartNo + ' => '+ s.ItemDesc as PartNoDesc\r\nfrom [Masters].Sparepart s  \r\nleft join VW_ListItems pt on s.PartType = pt.ListTypeItemId  \r\nleft join VW_ListItems ct on s.ConfigTypeId = ct.ListTypeItemId  \r\nleft join [Masters].ConfigTypeValues cv on s.ConfigValueId = cv.Id  \r\nwhere s.isactive = 1;  \r\nGO\r\n");
            migrationBuilder.Sql("CREATE \r\nVIEW [dbo].[VW_SparesRecommended] AS\r\n    SELECT \r\n        sr.id AS ServiceRequestId,\r\n        srp.id AS ServiceReportId,\r\n        sr.serreqno AS SerReqNo,\r\n        sr.assignedto AS AssignedToId,\r\n        c.FirstName AS AssignedToFName,\r\n        c.LastName AS AssignedToLName,\r\n        srp.servicereportdate AS ServiceReportDate,\r\n        spre.id AS SpareRecomId,\r\n        spre.partno AS PartNo,\r\n        spre.hsccode AS HscCode,\r\n        spre.qtyrecommended AS QtyRecommended,\r\n        sr.custid AS CustId,\r\n        spre.isdeleted AS IsDeleted,\r\n        cust.defdistregionid AS DefDistRegionId,\r\n        dist.distname AS DefaultDistributor,\r\n        sr.machinesno AS Instrument,\r\n        spre.createdon AS CreatedOn,\r\n        spre.createdby AS CreatedBy,\r\n        s.DistId AS SiteRegion, -- distributor\r\n        ins.businessunitid AS BusinessUnitId,\r\n        ins.brandid AS BrandId,\r\n\t\ts.id as SiteId\r\n    FROM\r\n        Transactions.servicerequest sr\r\n        JOIN Transactions.servicereport srp ON sr.id = srp.servicerequestid\r\n        JOIN Transactions.SPRecommended spre ON srp.id = spre.servicereportid\r\n        LEFT JOIN Masters.RegionContact c ON sr.assignedto = c.id\r\n        LEFT JOIN Masters.customer cust ON sr.custid = cust.id\r\n        LEFT JOIN Masters.distributor dist ON sr.distid = dist.id\r\n        LEFT JOIN Masters.site s ON sr.siteid = s.id\r\n        LEFT JOIN Masters.instrument ins ON sr.machinesno = ins.id\r\n\r\nGO\r\n");
            migrationBuilder.Sql("\r\nCreate\r\n View [dbo].[VW_UserProfile]\r\nas\r\n\tselect up.id as UserProfileId,\r\n\tup.Description,\r\n\tup.BrandIds,\r\n\tup.BusinessUnitIds,\r\n\tup.CustSites,\r\n\tup.DistRegions,\r\n\tup.IsActive as ActiveUserProfile,\r\n\tup.ManfSalesRegions,\r\n\tup.ManfBUIds,\r\n\tup.ProfileFor,\r\n\tup.RoleId,\r\n\tup.SegmentId,\r\n\tup.UserId,\r\n\tu.FirstName + ' ' + u.LastName as UserName,\r\n\tu.FirstName,\r\n\tu.LastName,\r\n\tu.Email,\r\n\tu.IsActive as ActiveUser,\r\n\tr.Name as UserRole,\r\n\tucm.ContactId,\r\n\tucm.ContactType,\r\n\trg.Id as EntityChildId,\r\n\trg.DistRegName as EntityChildName,\r\n\td.Id as EntityParentId,\r\n\td.Distname as EntityParentName,\r\n\tlti.ItemName as Designation,\r\n\tlti.Id as DesignationId,\r\n\tseg.Code as SegmentCode,\r\n\t'' as SelectedBusinessUnitId,\r\n\t'' as SelectedBrandId,\r\n\tu.TenantId as Company,\r\n (select SubscribedBy from CIMSaaS.Multitenancy.Tenants where id = u.TenantId ) as IsManfSubscribed\r\n\tfrom \r\n\t[Masters].UserProfiles up\r\n\tinner join [Identity].users u on up.UserId = u.Id\r\n\tinner join [Identity].Roles r on up.RoleId = r.Id\r\n\tinner join [Masters].MasterData seg on up.SegmentId = seg.Id\r\n\t--inner join [Masters].BusinessUnit bu on bu.Id = up.BusinessUnitId\r\n\t--inner join [Masters].Brand br on br.Id = up.BrandId\r\n\tinner join [Identity].UserContactMapping ucm on u.Id = ucm.UserId and ucm.ContactType = 'DR'\r\n\tinner join [Masters].RegionContact rc on ucm.ContactId = rc.Id\r\n\tinner join [Masters].ListTypeItems lti on rc.DesignationId = lti.Id\r\n\tinner join [Masters].Regions rg on rc.RegionId = rg.id\r\n\tinner join [Masters].Distributor d on rg.DistId = d.Id\r\n\tunion all\r\n\tselect up.id as UserProfileId,\r\n\tup.Description,\r\n\tup.BrandIds,\r\n\tup.BusinessUnitIds,\r\n\tup.CustSites,\r\n\tup.DistRegions,\r\n\tup.IsActive as ActiveUserProfile,\r\n\tup.ManfSalesRegions,\r\n\tup.ManfBUIds,\r\n\tup.ProfileFor,\r\n\tup.RoleId,\r\n\tup.SegmentId,\r\n\tup.UserId,\r\n\tu.FirstName + ' ' + u.LastName as UserName,\r\n\tu.FirstName,\r\n\tu.LastName,\r\n\tu.Email,\r\n\tu.IsActive as ActiveUser,\r\n\tr.Name as UserRole,\r\n\tucm.ContactId,\r\n\tucm.ContactType,\r\n\ts.Id as EntityChildId,\r\n\ts.CustRegName as EntityChildName,\r\n\tc.Id  as EntityParentId,\r\n\tc.CustName as EntityParentName,\r\n\tlti.ItemName as Designation,\r\n\tlti.Id as DesignationId,\r\n\tseg.Code as SegmentCode,\r\n\t'' as SelectedBusinessUnitId,\r\n\t'' as SelectedBrandId,\r\n\tu.TenantId as Company,\r\n (select SubscribedBy from CIMSaaS.Multitenancy.Tenants where id = u.TenantId ) as IsManfSubscribed\r\n\tfrom \r\n\t[Masters].UserProfiles up\r\n\tinner join [Identity].users u on up.UserId = u.Id\r\n\tinner join [Identity].Roles r on up.RoleId = r.Id\r\n\tinner join [Masters].MasterData seg on up.SegmentId = seg.Id\r\n\t--inner join [Masters].BusinessUnit bu on bu.Id = up.BusinessUnitId\r\n\t--inner join [Masters].Brand br on br.Id = up.BrandId\r\n\tinner join [Identity].UserContactMapping ucm on u.Id = ucm.UserId  and ucm.ContactType = 'CS'\r\n\tinner join [Masters].SiteContact sc on ucm.ContactId = sc.Id\r\n\tinner join [Masters].ListTypeItems lti on sc.DesignationId = lti.Id\r\n\tinner join [Masters].Site s on sc.SiteId =s.id\r\n\tinner join [Masters].Customer c on s.CustomerId = c.Id\r\n\tunion all\r\n\tselect up.id as UserProfileId,\r\n\tup.Description,\r\n\tup.BrandIds,\r\n\tup.BusinessUnitIds,\r\n\tup.CustSites,\r\n\tup.DistRegions,\r\n\tup.IsActive as ActiveUserProfile,\r\n\tup.ManfSalesRegions,\r\n\tup.ManfBUIds,\r\n\tup.ProfileFor,\r\n\tup.RoleId,\r\n\tup.SegmentId,\r\n\tup.UserId,\r\n\tu.FirstName + ' ' + u.LastName as UserName,\r\n\tu.FirstName,\r\n\tu.LastName,\r\n\tu.Email,\r\n\tu.IsActive as ActiveUser,\r\n\tr.Name as UserRole,\r\n\tucm.ContactId,\r\n\tucm.ContactType,\r\n\tsr.Id as EntityChildId,\r\n\tsr.SalesRegionName as EntityChildName,\r\n\tm.Id  as EntityParentId,\r\n\tm.ManfName as EntityParentName,\r\n\tlti.ItemName as Designation,\r\n\tlti.Id as DesignationId,\r\n\tseg.Code as SegmentCode,\r\n\t'' as SelectedBusinessUnitId,\r\n\t'' as SelectedBrandId,\r\n\tu.TenantId as Company,\r\n (select SubscribedBy from CIMSaaS.Multitenancy.Tenants where id = u.TenantId ) as IsManfSubscribed\r\n\tfrom \r\n\t[Masters].UserProfiles up\r\n\tinner join [Identity].users u on up.UserId = u.Id\r\n\tinner join [Identity].Roles r on up.RoleId = r.Id\r\n\tinner join [Masters].MasterData seg on up.SegmentId = seg.Id\r\n\t--inner join [Masters].BusinessUnit bu on bu.Id = up.BusinessUnitId\r\n\t--inner join [Masters].Brand br on br.Id = up.BrandId\r\n\tinner join [Identity].UserContactMapping ucm on u.Id = ucm.UserId and ucm.ContactType = 'MSR'\r\n\tinner join [Masters].SalesRegionContact src on ucm.ContactId = src.Id\r\n\tinner join [Masters].ListTypeItems lti on src.DesignationId = lti.Id\r\n\tinner join [Masters].SalesRegion sr on src.SalesRegionId =sr.id\r\n\tinner join [Masters].Manufacturer m on sr.ManfId = m.Id;\r\nGO\r\n\r\n\r\n");


            migrationBuilder.Sql("drop view VW_ListItems;");
            migrationBuilder.Sql("drop view VW_ServiceReport;");
            migrationBuilder.Sql("drop view VW_SparepartConsumedHistory;");
            migrationBuilder.Sql("drop view VW_SparesRecommended;");
            migrationBuilder.Sql("drop view VW_UserProfile;");
            migrationBuilder.Sql("drop view VW_Spareparts;");
            migrationBuilder.Sql("drop view VW_InstrumentSpares;");
            */
        #endregion

        #region unique constraints
        /* /// UNIQUE CONSTRAINTS

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
        */
        #endregion

        #region FK Constraints
        /*
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
        ALTER TABLE [Transactions].[EngScheduler]  WITH CHECK ADD  CONSTRAINT [FK_EngSch_EngAction_ActionId] FOREIGN KEY([ActionId]) REFERENCES [Transactions].SREngAction ([Id])
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
        ALTER TABLE Transactions.ServiceRequest ADD CONSTRAINT [FK_SerReq_ListItem_BreakoccurId] FOREIGN KE[FK_SerReq_ListItem_StageId]Y (BreakoccurDetailsId) REFERENCES Masters.ListTypeItems([Id]) ON DELETE NO ACTION;  -- datatype mismatch
        -- ALTER TABLE Transactions.ServiceRequest ADD CONSTRAINT [FK_SerReq_RegCon_AssignedTo] FOREIGN KEY (AssignedTo) REFERENCES Masters.RegionContact([Id]) ON DELETE NO ACTION;  -- EMPTY RECORD INITIALLY 
       -- ALTER TABLE Transactions.ServiceRequest ADD CONSTRAINT [FK_SerReq_ListItem_SubReqId] FOREIGN KEY (SubRequestTypeId) REFERENCES Masters.ListTypeItems([Id]) ON DELETE NO ACTION;  -- uses code
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
    */

        #endregion


    }

}
