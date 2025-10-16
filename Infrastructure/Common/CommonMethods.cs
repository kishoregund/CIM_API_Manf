using Application.Features.Dashboards.Requests;
using Application.Features.Identity.Roles;
using Application.Features.Identity.Users;
using Application.Features.UserProfiles.Responses;
using Application.Models;
using Domain.Entities;
using Domain.Views;
using Infrastructure.Persistence.Contexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class CommonMethods(ApplicationDbContext context, ICurrentUserService currentUserService,
        IConfiguration configuration)
    {
        public List<string> GetFormatScreenPermissionsToRoleClaims(List<ScreenPermissions> screenPersmissions)
        {
            var roleClaims = new List<string>();
            string roleClaim = string.Empty;
            string CRUD = string.Empty;
            foreach (ScreenPermissions screenPermission in screenPersmissions)
            {
                roleClaim = "Permission." + (string.IsNullOrEmpty(screenPermission.ScreenName) ? context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == Guid.Parse(screenPermission.ScreenId)).ItemName : screenPermission.ScreenName).Trim().Replace(" ", "_");
                if (screenPermission.Create)
                {
                    CRUD = string.Empty;
                    CRUD = roleClaim + ".Create";
                    roleClaims.Add(CRUD);
                }
                if (screenPermission.Update)
                {
                    CRUD = string.Empty;
                    CRUD = roleClaim + ".Update";
                    roleClaims.Add(CRUD);
                }
                if (screenPermission.Delete)
                {
                    CRUD = string.Empty;
                    CRUD = roleClaim + ".Delete";
                    roleClaims.Add(CRUD);
                }
                if (screenPermission.Read)
                {
                    CRUD = string.Empty;
                    CRUD = roleClaim + ".View";
                    roleClaims.Add(CRUD);
                }
                if (!string.IsNullOrEmpty(screenPermission.Privilages))
                {
                    CRUD = string.Empty;
                    CRUD = roleClaim + "." + screenPermission.Privilages;
                    roleClaims.Add(CRUD);
                }
                if (screenPermission.Commercial)
                {
                    CRUD = string.Empty;
                    CRUD = roleClaim + ".Commercial";
                    roleClaims.Add(CRUD);
                }
            }
            return roleClaims;
        }
        public List<ScreenPermissions> GetFormatRoleClaimsToScreenPermissions(List<string> roleClaims)
        {
            List<ScreenPermissions> ScreenPermissions = new();
            ScreenPermissions screenPermission = new();
            string screenName = string.Empty;
            string roleClaim = string.Empty;
            for (int i = 0; i < roleClaims.Count; i++) // string roleClaim in roleClaims)
            {
                roleClaim = roleClaims[i];
                string[] strPerm = roleClaim.Split('.');
                if (strPerm[1].ToUpper() != "BASE")
                {
                    if (screenName == "" || screenName == strPerm[1])
                    {
                        screenName = strPerm[1];                       
                        screenPermission.ScreenName = screenName.Replace("_", " ");
                        if (ScreenPermissions.Any(x => x.ScreenName == screenPermission.ScreenName))
                        {
                            screenPermission = ScreenPermissions.FirstOrDefault(x => x.ScreenName == screenPermission.ScreenName);
                        }

                        if (strPerm[2] == "Create")
                        {
                            screenPermission.Create = true;
                        }
                        else if (strPerm[2] == "Update")
                        {
                            screenPermission.Update = true;
                        }
                        else if (strPerm[2] == "Delete")
                        {
                            screenPermission.Delete = true;
                        }
                        else if (strPerm[2] == "View")
                        {
                            screenPermission.Read = true;
                        }
                        else if (strPerm[2] == "Commercial")
                        {
                            screenPermission.Commercial = true;
                        }
                        else if (context.VW_ListItems.Any(x => x.ListTypeItemId == Guid.Parse(strPerm[2])))
                        {
                            //if (strPerm[2] == "All Data" || strPerm[2] == "User Data Only")
                            screenPermission.Privilages = strPerm[2];//  context.VW_ListItems.FirstOrDefault(x => x.ListTypeItemId == Guid.Parse(strPerm[2])).ItemName;
                                                                     //screenPermission.Privilages = strPerm[2];
                        }
                        if (i == roleClaims.Count - 1)
                        {
                            if (!ScreenPermissions.Any(x => x.ScreenName == screenPermission.ScreenName))
                            {
                                var listItem = context.VW_ListItems.FirstOrDefault(x => x.ItemName == screenPermission.ScreenName && x.ListCode == "SCRNS");
                                if (listItem != null)
                                {
                                    screenPermission.ScreenId = listItem.ListTypeItemId.ToString();
                                    screenPermission.ScreenCode = listItem.ItemCode;
                                    screenPermission.ScreenName = listItem.ItemName;

                                    ScreenPermissions.Add(getCategory(screenPermission));
                                }
                            }
                        }
                    }
                    else
                    {
                        var listItem = context.VW_ListItems.FirstOrDefault(x => x.ItemName == screenPermission.ScreenName && x.ListCode == "SCRNS");
                        if (listItem != null)
                        {
                            screenPermission.ScreenId = listItem.ListTypeItemId.ToString();
                            screenPermission.ScreenCode = listItem.ItemCode;
                            screenPermission.ScreenName = listItem.ItemName;

                            ScreenPermissions.Add(getCategory(screenPermission));
                        }
                        screenPermission = new();
                        screenName = strPerm[1];
                        i--;
                    }
                }
            }
            return ScreenPermissions;
        }
        public ScreenPermissions getCategory(ScreenPermissions screen)
        {
            VW_ListItems lItem = new();
            if (screen.ScreenCode == "PROF" || screen.ScreenCode == "URPRF")
            {
                lItem = context.VW_ListItems.FirstOrDefault(x => x.ListCode == "PRGRP" && x.ItemCode == "ADMIN");
                screen.Category = lItem.ListTypeItemId.ToString();
                screen.CategoryName = lItem.ItemName;

            }
            else if (screen.ScreenCode == "SCURR" || screen.ScreenCode == "SCOUN" || screen.ScreenCode == "SCUST" || screen.ScreenCode == "SDIST" || screen.ScreenCode == "SINST" || screen.ScreenCode == "SSPAR" || screen.ScreenCode == "SMANF" || screen.ScreenCode == "SINAL")
            {
                lItem = context.VW_ListItems.FirstOrDefault(x => x.ListCode == "PRGRP" && x.ItemCode == "MSTRS");
                screen.Category = lItem.ListTypeItemId.ToString();
                screen.CategoryName = lItem.ItemName;
            }

            else if (screen.ScreenCode == "PSRRP" || screen.ScreenCode == "CUSDH" || screen.ScreenCode == "DHSET" || screen.ScreenCode == "DISDH") //|| screen.ScreenCode == "SIMXP" || screen.ScreenCode == "SSRCH" || screen.ScreenCode == "AUDIT" 
            {
                lItem = context.VW_ListItems.FirstOrDefault(x => x.ListCode == "PRGRP" && x.ItemCode == "UTILS");
                screen.Category = lItem.ListTypeItemId.ToString();
                screen.CategoryName = lItem.ItemName;
            }

            else if (screen.ScreenCode == "SAMC" || screen.ScreenCode == "CTSPI" || screen.ScreenCode == "OFREQ" || screen.ScreenCode == "SCDLE" || screen.ScreenCode == "SRREQ"
                || screen.ScreenCode == "SPRCM" || screen.ScreenCode == "SRREP" || screen.ScreenCode == "TREXP" || screen.ScreenCode == "TRINV" || screen.ScreenCode == "ADREQ"
                || screen.ScreenCode == "CTSS" || screen.ScreenCode == "SCINS")
            {
                lItem = context.VW_ListItems.FirstOrDefault(x => x.ListCode == "PRGRP" && x.ItemCode == "TRANS");
                screen.Category = lItem.ListTypeItemId.ToString();
                screen.CategoryName = lItem.ItemName;
            }

            //else if (screen.ScreenCode == "SRQRP" || screen.ScreenCode == "SRCMR" || screen.ScreenCode == "PDQRQ" || screen.ScreenCode == "SRCRR")
            //{
            //    lItem = context.VW_ListItems.FirstOrDefault(x => x.ListCode == "PRGRP" && x.ItemCode == "REPTS");
            //    screen.Category = lItem.ListTypeItemId.ToString();
            //    screen.CategoryName = lItem.ItemName;
            //}

            return screen;
        }
        public Guid CreateUserProfile(CreateUserRequest userRequest, Guid id)
        {
            //bool isValid = true;
            string buName = "", brandName = "";
            var p = new UserProfiles();
            p.UserId = id;
            p.IsDeleted = false;
            p.IsActive = true;
            p.CreatedBy = Guid.Parse(currentUserService.GetUserId());
            p.UpdatedBy = Guid.Parse(currentUserService.GetUserId());
            p.UpdatedOn = DateTime.Now;
            p.CreatedOn = DateTime.Now;
            p.Description = "System created user profile";

            UserByContactResponse userContact;
            //create user profile
            switch (userRequest.ContactType)
            {
                case "MSR":
                    userContact = GetManfUserByContact(userRequest.ContactId);
                    p.Id = Guid.NewGuid();
                    p.ProfileFor = context.VW_ListItems.FirstOrDefault(x => x.ListName.ToUpper() == "RECORDSFOR" && x.ItemName.ToUpper() == "MANUFACTURER").ListTypeItemId;
                    p.RoleId = Guid.Parse(context.Roles.FirstOrDefault(x => x.Name.ToUpper().Trim() == "MANUFACTURER").Id);
                    p.SegmentId = context.VW_ListItems.FirstOrDefault(x => x.ListName.ToUpper() == "SEGMENTS" && x.ItemCode.ToUpper() == "RMANF").ListTypeItemId;
                    p.DistRegions = null;
                    p.CustSites = null;  
                    p.ManfSalesRegions = userContact.ChildId.ToString();
                    p.ManfBUIds = context.ManfBusinessUnit.Any() ? context.ManfBusinessUnit.FirstOrDefault().Id.ToString() : null;
                    
                    break;
                case "DR":
                    userContact = GetDistUserByContact(userRequest.ContactId);
                    p.Id = Guid.NewGuid();
                    p.ProfileFor = context.VW_ListItems.FirstOrDefault(x => x.ListName.ToUpper() == "RECORDSFOR" && x.ItemName.ToUpper() == "REGION").ListTypeItemId;

                    p.RoleId = Guid.Parse(context.Roles.FirstOrDefault(x => x.Name.ToUpper().Trim() == "DISTRIBUTOR_OPERATIONS_REGION").Id);
                    p.SegmentId = context.VW_ListItems.FirstOrDefault(x => x.ListName.ToUpper() == "SEGMENTS" && x.ItemCode.ToUpper() == "RDTSP").ListTypeItemId;

                    p.DistRegions = userContact.ChildId.ToString();
                    p.CustSites = null;

                    if (userContact.IsFieldEngineer)  //.Designation.ToUpper().Contains("ENGINEER"))
                    {
                        p.RoleId = Guid.Parse(context.Roles.FirstOrDefault(x => x.Name.ToUpper().Trim() == "ENGINEER").Id);
                        p.SegmentId = context.VW_ListItems.FirstOrDefault(x => x.ListName.ToUpper() == "SEGMENTS" && x.ItemCode.ToUpper() == "RENG").ListTypeItemId;

                        p.DistRegions = GetAllRegionsForEngineerProfile(userContact.ContactId);
                    }


                    p.BusinessUnitIds = context.BusinessUnit.FirstOrDefault(x => x.DistributorId == userContact.ParentId).Id.ToString();
                    p.BrandIds = context.Brand.FirstOrDefault(x => x.BusinessUnitId == Guid.Parse(p.BusinessUnitIds)).Id.ToString();

                    buName = context.BusinessUnit.FirstOrDefault(x => x.DistributorId == userContact.ParentId).BusinessUnitName;
                    brandName = context.Brand.FirstOrDefault(x => x.BusinessUnitId == Guid.Parse(p.BusinessUnitIds)).BrandName;
                    break;

                case "CS":
                    userContact = GetCustUserByContact(userRequest.ContactId);
                    p.Id = Guid.NewGuid();
                    p.ProfileFor = context.VW_ListItems.FirstOrDefault(x => x.ListName.ToUpper() == "RECORDSFOR" && x.ItemName.ToUpper() == "SITE").ListTypeItemId;
                    p.RoleId = Guid.Parse(context.Roles.FirstOrDefault(x => x.Name.ToUpper() == "SITE").Id);
                    p.SegmentId = context.VW_ListItems.FirstOrDefault(x => x.ListName.ToUpper() == "SEGMENTS" && x.ItemCode.ToUpper() == "RCUST").ListTypeItemId;
                    p.DistRegions = null;
                    p.CustSites = userContact.ChildId.ToString();

                    break;

            }

            context.UserProfiles.Add(p);
            context.SaveChanges();

            SendEmail(userRequest, buName, brandName);

            return p.RoleId;
        }

        public UserByContactResponse GetManfUserByContact(Guid contactId)
        {
            var regions = (from rc in context.SalesRegionContact
                           join r1 in context.SalesRegion on rc.SalesRegionId equals r1.Id
                           join d1 in context.Manufacturer on r1.ManfId equals d1.Id
                           join des in context.VW_ListItems on rc.DesignationId equals des.ListTypeItemId
                           where rc.Id == contactId
                           select new UserByContactResponse()
                           {
                               ContactId = rc.Id,
                               ChildId = r1.Id,
                               ChildName = r1.SalesRegionName,
                               ContactType = "MF",
                               Designation = des.ItemName,
                               DesignationId = des.ListTypeItemId,
                               Email = rc.PrimaryEmail,
                               FirstName = rc.FirstName,
                               IsActive = rc.IsActive,
                               LastName = rc.LastName,
                               ParentId = d1.Id,
                               ParentName = d1.ManfName,
                               PhoneNumber = rc.PrimaryContactNo
                           }).FirstOrDefault();

            return regions;
        }

        public UserByContactResponse GetDistUserByContact(Guid contactId)
        {
            var regions = (from rc in context.RegionContact
                           join r1 in context.Regions on rc.RegionId equals r1.Id
                           join d1 in context.Distributor on r1.DistId equals d1.Id
                           join des in context.VW_ListItems on rc.DesignationId equals des.ListTypeItemId
                           where rc.Id == contactId
                           select new UserByContactResponse()
                           {
                               ContactId = rc.Id,
                               ChildId = r1.Id,
                               ChildName = r1.DistRegName,
                               ContactType = "DR",
                               Designation = des.ItemName,
                               DesignationId = des.ListTypeItemId,
                               Email = rc.PrimaryEmail,
                               FirstName = rc.FirstName,
                               IsActive = rc.IsActive,
                               LastName = rc.LastName,
                               ParentId = d1.Id,
                               ParentName = d1.DistName,
                               PhoneNumber = rc.PrimaryContactNo,
                               IsFieldEngineer = rc.IsFieldEngineer
                           }).FirstOrDefault();

            return regions;
        }
       
        public UserByContactResponse GetCustUserByContact(Guid contactId)
        {
            var regions = (from sc in context.SiteContact
                           join s1 in context.Site on sc.SiteId equals s1.Id
                           join c1 in context.Customer on s1.CustomerId equals c1.Id
                           join des in context.VW_ListItems on sc.DesignationId equals des.ListTypeItemId
                           where sc.Id == contactId
                           select new UserByContactResponse()
                           {
                               ContactId = sc.Id,
                               ChildId = s1.Id,
                               ChildName = s1.CustRegName,
                               ContactType = "CS",
                               Designation = des.ItemName,
                               DesignationId = des.ListTypeItemId,
                               Email = sc.PrimaryEmail,
                               FirstName = sc.FirstName,
                               IsActive = sc.IsActive,
                               LastName = sc.LastName,
                               ParentId = s1.Id,
                               ParentName = s1.CustRegName,
                               PhoneNumber = sc.PrimaryContactNo
                           }).FirstOrDefault();

            return regions;
        }
        public UserByContactResponse GetManfUserByContactAsync(Guid contactId)
        {
            var regions = (from rc in context.SalesRegionContact
                           join r1 in context.SalesRegion on rc.SalesRegionId equals r1.Id
                           join d1 in context.Manufacturer on r1.ManfId equals d1.Id
                           join des in context.VW_ListItems on rc.DesignationId equals des.ListTypeItemId
                           where rc.Id == contactId
                           select new UserByContactResponse()
                           {
                               ContactId = rc.Id,
                               ChildId = r1.Id,
                               ChildName = r1.SalesRegionName,
                               ContactType = "MSR",
                               Designation = des.ItemName,
                               DesignationId = des.ListTypeItemId,
                               Email = rc.PrimaryEmail,
                               FirstName = rc.FirstName,
                               IsActive = rc.IsActive,
                               LastName = rc.LastName,
                               ParentId = d1.Id,
                               ParentName = d1.ManfName,
                               PhoneNumber = rc.PrimaryContactNo
                           }).FirstOrDefault();

            return regions;
        }
        public async Task<List<string>> GetDistRegionsByUserIdAsync()
        {
            var distRegions = "";
#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                var regionsprofile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == currentUserService.GetUserId());

                if (user.FirstName == "Admin") // if user is admin
                {
                    var lstAllRegions = await (from region in context.Regions
                                               join dist in context.Distributor
                                               on region.DistId equals dist.Id
                                               select region).ToListAsync();

                    foreach (var region in lstAllRegions)
                    {
                        distRegions = distRegions + region.Id + ",";
                    }
                }
                else
                {
                    //this is when the user is a customer 
                    if (regionsprofile?.DistRegions == null || regionsprofile?.DistRegions == "")
                    {

                        var siteLst = await context.Site.Where(x => x.CustomerId == regionsprofile.EntityParentId).ToListAsync();

                        foreach (var site in siteLst)
                        {
                            distRegions += site.DistId + ",";
                        }

                        if (string.IsNullOrEmpty(distRegions)) distRegions = context.Site.FirstOrDefault(x => x.Id == regionsprofile.EntityChildId)?.DistId.ToString();
                    }
                    else
                    {
                        distRegions = regionsprofile.DistRegions;
                    }
                }
            }
            catch (Exception ex)
            {
                distRegions = "";
            }
#pragma warning restore CS0168 // Variable is declared but never used
            if (distRegions != null && distRegions.Length > 0 && distRegions.EndsWith(","))
            {
                distRegions = distRegions.Remove(distRegions.Length - 1, 1);
            }

            return distRegions.Split(',').ToList();
        }

        public async Task<List<string>> GetSitesByUserIdAsync()
        {
            var sites = "";
            var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));

            if (userProfile != null)
            {
                if (userProfile.ContactType.ToLower() == "cs")
                {
                    sites = userProfile.CustSites;
                }
                else if (userProfile.ContactType.ToLower() == "dr")
                {
                    var lstRegion = await GetDistRegionsByUserIdAsync();
                    var siteList = await GetCustomerSitesForDistUser(userProfile);

                    sites = "";
                    foreach (var site in siteList)
                    {
                        if (lstRegion.Contains(site.RegionId.ToString())) sites += site.Id + ",";
                    }

                    if (sites.Length > 1) sites = sites.Remove(sites.Length - 1, 1);
                }
                else if (userProfile.FirstName.Equals("Admin"))

                {
                    var CustSites = context.Site.Select(x => x.Id).ToList();
                    sites = "";
                    foreach (var site in CustSites) sites += site + ",";
                    if (sites.Length > 1) sites = sites.Remove(sites.Length - 1, 1);
                }
            }

            return sites.Split(',').ToList();
        }
        public async Task<List<Site>> GetCustomerSitesForDistUser(VW_UserProfile userProfile)
        {
            var sites = new List<Site>();
            //var userProfile = await context.VW_UserProfile.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(currentUserService.GetUserId()));
            var Customers = await context.Customer.Where(x => x.DefDistId == userProfile.EntityParentId).OrderBy(x => x.CustName).ToListAsync();

            //var privilage = _context.Vw_Privilages.FirstOrDefault(x => x.UserId == userId && x.ScreenCode == "SCUST" && x.UserName != "admin");

            //if (privilage != null && privilage.PrivilageCode != "PARTS" && (privilage._create || privilage._read || privilage._update || privilage._delete))
            //{
            //    customer = customer.Where(x => x.Createdby == userId);
            //}

            foreach (var c in Customers)
            {
                var s = context.Site.Where(x => x.CustomerId == c.Id).ToList();
                sites.AddRange(s);
            }

            return sites;
        }

        private string GetAllRegionsForEngineerProfile(Guid contactId)
        {
            var distRegions = "";
            var lstAllRegions = (from d in context.Distributor
                                 join r in context.Regions on d.Id equals r.DistId
                                 where d.Id == (
                                     from rc in context.RegionContact
                                     join r1 in context.Regions on rc.RegionId equals r1.Id
                                     join d1 in context.Distributor on r1.DistId equals d1.Id
                                     where rc.Id == contactId
                                     select d1.Id
                                     ).FirstOrDefault()
                                 select r).ToList();

            foreach (var region in lstAllRegions)
            {
                distRegions = distRegions + region.Id + ",";
            }

            return distRegions.Substring(0, distRegions.Length - 1);
        }

        public bool GetDateDiff(DashboardDateRequest dashboardDate)
        {
            var sCompare = DateTime.Compare(dashboardDate.CreatedOn.Date, dashboardDate.SDate.Date);
            var eCompare = DateTime.Compare(dashboardDate.CreatedOn.Date, dashboardDate.EDate.Date);
            var seCompare = DateTime.Compare(dashboardDate.SDate.Date, dashboardDate.EDate.Date);

            if (seCompare > 0) return false;
            if (sCompare < 0) return false;
            if (eCompare > 0) return false;

            return true;
        }

        private bool SendEmail(CreateUserRequest usr, string BU, string brand)
        {
#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
                var message = new MailMessage();

                #region set To,Cc abd Bcc
                message.To.Add(new MailAddress(usr.Email));
                message.CC.Add(new MailAddress("kishoregund@gmail.com"));
                //message.Bcc.Add(new MailAddress(arrStr.Trim()));
                //message.ReplyToList.Add(new MailAddress(arrStr.Trim(), "reply-to"));
                #endregion

                #region set Email body
                message.From = new MailAddress(appSettings.EmailSettings.SMTPUser, appSettings.EmailSettings.DisplayName);
                message.Subject = appSettings.EmailSettings.Subject;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                var body = "Hi,<br><br> Welcome to Avante Grade! <br><br> Please use below credentials to login to Avante.<br> Username: " + usr.Email + "<br> Password: " + usr.Password + "<br><br>";
                if (BU != "")
                    body += " Business Unit: " + BU + "<br> Brand: " + brand;
                body += "<br><br><a href='https://service.avantgardeinc.com/'>Please Click here to Login.</a>" +
                    "<br><br><br>Thank you,<br>Avante Team<br>";
                body += "<br><br><br><br> *This is a system generated email and you will get the exact schedule date and time from engineer and service coordinator";

                message.Body = body;
                #endregion


                #region set Credential
                var client = new SmtpClient
                {
                    EnableSsl = Convert.ToBoolean(appSettings.EmailSettings.SSL),
                    Host = appSettings.EmailSettings.Host,
                    Port = Convert.ToInt32(appSettings.EmailSettings.Port)
                };

                if (!string.IsNullOrEmpty(appSettings.EmailSettings.SMTPPassword))
                {
                    client.Credentials = new System.Net.NetworkCredential(appSettings.EmailSettings.SMTPUser, appSettings.EmailSettings.SMTPPassword);
                    //client.UseDefaultCredentials = false;
                }
                else
                    client.UseDefaultCredentials = true;
                #endregion

                client.Send(message);
                message.Dispose();
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
#pragma warning restore CS0168 // Variable is declared but never used
        }


        public bool SendEmailMethod(string email, string emailBody, string subject)
        {
            try
            {
                var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
                var message = new MailMessage();

                #region set To,Cc abd Bcc
                message.To.Add(new MailAddress(email));
                message.CC.Add(new MailAddress("kishoregund@gmail.com"));
                //message.Bcc.Add(new MailAddress(arrStr.Trim()));
                //message.ReplyToList.Add(new MailAddress(arrStr.Trim(), "reply-to"));
                #endregion

                #region set Email body
                message.From = new MailAddress(appSettings.EmailSettings.SMTPUser, appSettings.EmailSettings.DisplayName);
                message.Subject = subject;
                message.IsBodyHtml = true;
                //message.Priority = MailPriority.High;
                
                message.Body = emailBody;
                #endregion


                #region set Credential
                var client = new SmtpClient
                {
                    EnableSsl = Convert.ToBoolean(appSettings.EmailSettings.SSL),
                    Host = appSettings.EmailSettings.Host,
                    Port = Convert.ToInt32(appSettings.EmailSettings.Port)
                };

                if (!string.IsNullOrEmpty(appSettings.EmailSettings.SMTPPassword))
                {
                    client.Credentials = new System.Net.NetworkCredential(appSettings.EmailSettings.SMTPUser, appSettings.EmailSettings.SMTPPassword);
                    //client.UseDefaultCredentials = false;
                }
                else
                    client.UseDefaultCredentials = true;
                #endregion

                client.Send(message);
                message.Dispose();
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool IsManfSubscribed()
        {
            SqlConnection con = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select SubscribedBy from CIMSaaS.Multitenancy.Tenants where id ='" + currentUserService.GetUserTenant() + "'", con);
            da.Fill(dt);
            var isManfSubscribed = dt.Rows.Count > 0 ? bool.Parse(dt.Rows[0][0].ToString()) : false;
            con.Close();
            return isManfSubscribed;
        }

        ///// not neeeded
        ///
        private string GetRegionContactDesignation(Guid contactId)
        {
            return (from c in context.RegionContact
                    join des in context.VW_ListItems on c.DesignationId equals des.ListTypeItemId
                    where c.Id == contactId
                    select des.ItemName).ToString();
        }

        private string GetSiteContactDesignation(Guid contactId)
        {
            return (from c in context.SiteContact
                    join des in context.VW_ListItems on c.DesignationId equals des.ListTypeItemId
                    where c.Id == contactId
                    select des.ItemName).ToString();
        }

        private string GetSalesRegionContactDesignation(Guid contactId)
        {
            return (from c in context.SalesRegionContact
                    join des in context.VW_ListItems on c.DesignationId equals des.ListTypeItemId
                    where c.Id == contactId
                    select des.ItemName).ToString();
        }

    }
}
