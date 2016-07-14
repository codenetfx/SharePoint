// -----------------------------------------------------------------------
// <copyright file="EngagementsSharePointDal_Engagements.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsSharePointDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;
    using Acme.SharePointCore.Helpers;

    /// <summary>
    /// SharePoint DataAccess Layer for AcmeCorp Engagements
    /// </summary>
    public partial class EngagementsSharePointDal : AcmeCorp.Engagements.EngagementsDomain.IAcmeCorpEngagementsDalInterface
    {
        /// <summary>
        /// Creates a engagement site.
        /// </summary>
        /// <param name="wbId">The wb id.</param>
        /// <param name="engagementFolders">Engagement folders type.</param>
        /// <param name="engagementOwners">Engagement owners.</param>
        /// <param name="engagementPartners">Engagement partners.</param>
        /// <param name="engagementStaff">Engagement staff.</param>
        /// <param name="engagementProperties">Engagement properties.</param>
        /// <returns>
        /// Path to the newly created engagement site
        /// </returns>
        public string CreateEngagementSite(int wbId, EngagementsDomain.Enums.EngagementFoldersType engagementFolders, string[] engagementOwners, string[] engagementPartners, string[] engagementStaff, Dictionary<string, object> engagementProperties)
        {
            string engagementSitePath = string.Empty;

            try
            {
                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Entered CreateNewProjectSite method in dal");

                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Trying to establish the context in: " + this.rootSiteCollection + ";");

                // STEP 1: CURRENT USER INFO
                ContextHelper context = new ContextHelper(this.rootSiteCollection + ";");

                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Context established");

                if (context.Web.CurrentUser == null)
                {
                    this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Current user is null");
                }

                string currentUserLogin = context.Web.CurrentUser.LoginName;

                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Current user login: " + currentUserLogin);

                string currentUserName = context.Web.CurrentUser.Name;

                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Current user name: " + currentUserName);

                string currentUserEmail = context.Web.CurrentUser.Email;

                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Current user mail: " + currentUserEmail);

                context.DisposeContext();

                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Got current user");

                // Create site under elevated permissions
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite elevatedSiteCollection = new SPSite(this.rootSiteCollection))
                    {
                        // STEP 2: CREATE SITE COLLECTION
                        this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Got under elevated priviledges");

                        SPContentDatabase rootDatabase = elevatedSiteCollection.ContentDatabase;

                        this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Root database is " + rootDatabase.Name);

                        // get default owner info
                        SPUser defaultOwner = elevatedSiteCollection.RootWeb.EnsureUser(this.defaultSiteOwner);

                        string defaultUserLogin = defaultOwner.LoginName;
                        string defaultUserName = defaultOwner.Name;
                        string defaultUserEmail = defaultOwner.Email;

                        this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Got default user");

                        // SPContentDatabase contentDatabase = CreateContentDb(elevatedSiteCollection, rootDatabase);
                        SPContentDatabase contentDatabase = rootDatabase;

                        // sharepoint system is not allowed to be the site collection owner
                        if (currentUserLogin.ToUpper() == "SHAREPOINT\\SYSTEM")
                        {
                            currentUserLogin = defaultUserLogin;
                            currentUserEmail = defaultUserEmail;
                            currentUserName = defaultUserName;
                        }

                        this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Normalized current user");

                        string siteColUrl = string.Format("{0}/{1}/{2}", this.rootSiteCollection, this.defaultProjectSitePath, wbId);

                        this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Built the site collection path " + siteColUrl);

                        SPSite engagementSite = this.CreateEngagementSiteCollection(wbId, engagementProperties, currentUserLogin, currentUserName, currentUserEmail, defaultUserLogin, defaultUserName, defaultUserEmail, contentDatabase, siteColUrl);

                        if (engagementSite != null)
                        {
                            engagementSitePath = engagementSite.Url;

                            // delete all groups
                            Acme.SharePointCore.Helpers.SecurityHelpers.DeleteAllSecurityGroups(engagementSite);
                            this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "All default security groups are deleted");

                            // create new default groups
                            SPGroup managersGroup = SecurityHelpers.CreateSecurityGroup(engagementSite, "Engagement Manager", "Engagement Manager - AcmeCorp Engagements Related SP Group", false, false, false, engagementOwners[0], engagementOwners);
                            SPGroup partnersGroup = SecurityHelpers.CreateSecurityGroup(engagementSite, "Engagement Partner", "Engagement Partner - AcmeCorp Engagements Related SP Group", false, false, false, engagementPartners[0], engagementPartners);
                            SPGroup staffGroup = SecurityHelpers.CreateSecurityGroup(engagementSite, "Engagement Staff", "Engagement Srtaff - AcmeCorp Engagements Related SP Group", false, false, false, engagementOwners[0], engagementStaff);

                            this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "New security groups are created");

                            // create AcmeCorp Engagements Related Permission Levels
                            // TODO: Make configurable
                            SPRoleDefinition readersPermissionLevel = SecurityHelpers.CreateNewPermissionLevel(engagementSite.RootWeb, "AcmeCorp_Read", "AcmeCorp Engagements Read Only Permissions", engagementSite.RootWeb.RoleDefinitions.GetByType(SPRoleType.Reader).BasePermissions);
                            SPRoleDefinition contributorsPermissionLevel = SecurityHelpers.CreateNewPermissionLevel(engagementSite.RootWeb, "AcmeCorp_Contribute", "AcmeCorp Engagements Contributor Permissions", engagementSite.RootWeb.RoleDefinitions.GetByType(SPRoleType.Contributor).BasePermissions);
                            SPRoleDefinition administratorsPermissionLevel = SecurityHelpers.CreateNewPermissionLevel(engagementSite.RootWeb, "AcmeCorp_Manage", "AcmeCorp Engagements Manager Permissions", engagementSite.RootWeb.RoleDefinitions.GetByType(SPRoleType.Administrator).BasePermissions);

                            // extend the contributors permissions level
                            // SecurityHelpers.AddPermissionToPermissionLevel(contributorsPermissionLevel, SPBasePermissions.AddListItems);
                            // SecurityHelpers.AddPermissionToPermissionLevel(contributorsPermissionLevel, SPBasePermissions.EditListItems);
                            // SecurityHelpers.AddPermissionToPermissionLevel(contributorsPermissionLevel, SPBasePermissions.DeleteListItems);

                            // set permissions for site
                            SecurityHelpers.AddPermissionToSecurableObject(engagementSite.RootWeb, partnersGroup, administratorsPermissionLevel, false);
                            SecurityHelpers.AddPermissionToSecurableObject(engagementSite.RootWeb, managersGroup, contributorsPermissionLevel, false);
                            SecurityHelpers.AddPermissionToSecurableObject(engagementSite.RootWeb, staffGroup, readersPermissionLevel, false);

                            // create folders for the deliver library
                            SPDocumentLibrary deliverLib = (SPDocumentLibrary)engagementSite.RootWeb.Lists.TryGetList(this.engagementDeliverLibraryName);
                            this.SetLibraryPermissions(deliverLib, staffGroup, managersGroup, partnersGroup, readersPermissionLevel, contributorsPermissionLevel);

                            //SPFolder startUpFolder = CreateStaffFolder("Start up", deliverLib.RootFolder, staffGroup, managersGroup, partnersGroup, contributorsPermissionLevel, readersPermissionLevel, administratorsPermissionLevel);
                            //SPFolder communicationsFolder = CreateStaffFolder("Communications Reporting", deliverLib.RootFolder, staffGroup, managersGroup, partnersGroup, contributorsPermissionLevel, readersPermissionLevel, administratorsPermissionLevel);
                            //SPFolder clientCorrespondenceFolder = CreateStaffFolder("Client Correspondence", communicationsFolder, staffGroup, managersGroup, partnersGroup, contributorsPermissionLevel, readersPermissionLevel, administratorsPermissionLevel);
                            //SPFolder meetingspresentationsFolderCorrespondenceFolder = CreateStaffFolder("Meetings Presentations Minutes", communicationsFolder, staffGroup, managersGroup, partnersGroup, contributorsPermissionLevel, readersPermissionLevel, administratorsPermissionLevel);
                            //SPFolder pmControllingFolder = CreatePartnersFolder("PM and Controlling", deliverLib.RootFolder, staffGroup, managersGroup, partnersGroup, contributorsPermissionLevel, readersPermissionLevel, administratorsPermissionLevel);
                            //SPFolder progressStatusFolder = CreatePartnersFolder("Progress and Status", pmControllingFolder, staffGroup, managersGroup, partnersGroup, contributorsPermissionLevel, readersPermissionLevel, administratorsPermissionLevel);
                            //SPFolder clientInvoicesFolder = CreatePartnersFolder("Client invoices", pmControllingFolder, staffGroup, managersGroup, partnersGroup, contributorsPermissionLevel, readersPermissionLevel, administratorsPermissionLevel);
                            //SPFolder informationFolder = CreateStaffFolder("Informations Received", deliverLib.RootFolder, staffGroup, managersGroup, partnersGroup, contributorsPermissionLevel, readersPermissionLevel, administratorsPermissionLevel);
                            //SPFolder clientFolder = CreateStaffFolder("Client", informationFolder, staffGroup, managersGroup, partnersGroup, contributorsPermissionLevel, readersPermissionLevel, administratorsPermissionLevel);
                            //SPFolder internalResearchFolder = CreateStaffFolder("Internal and Research", informationFolder, staffGroup, managersGroup, partnersGroup, contributorsPermissionLevel, readersPermissionLevel, administratorsPermissionLevel);
                            //SPFolder externalFolder = CreateStaffFolder("External", informationFolder, staffGroup, managersGroup, partnersGroup, contributorsPermissionLevel, readersPermissionLevel, administratorsPermissionLevel);

                            deliverLib.Update();

                            SPDocumentLibrary closeLib = (SPDocumentLibrary)engagementSite.RootWeb.Lists.TryGetList(this.engagementCloseLibraryName);
                            this.SetLibraryPermissions(closeLib, staffGroup, managersGroup, partnersGroup, readersPermissionLevel, contributorsPermissionLevel);

                            //SPFolder benefitsFolder = CreateStaffFolder("Benefits Revision and Cleints Satisfaction", closeLib.RootFolder, staffGroup, managersGroup, partnersGroup, contributorsPermissionLevel, readersPermissionLevel, administratorsPermissionLevel);
                            //SPFolder debriefingFolder = CreateStaffFolder("Debriefing", closeLib.RootFolder, staffGroup, managersGroup, partnersGroup, contributorsPermissionLevel, readersPermissionLevel, administratorsPermissionLevel);
                            //SPFolder lessonsFolder = CreateStaffFolder("Lessons Learned", closeLib.RootFolder, staffGroup, managersGroup, partnersGroup, contributorsPermissionLevel, readersPermissionLevel, administratorsPermissionLevel);
                            //SPFolder opportunitiesFolder = CreateStaffFolder("Opportunities and Follow Up", closeLib.RootFolder, staffGroup, managersGroup, partnersGroup, contributorsPermissionLevel, readersPermissionLevel, administratorsPermissionLevel);

                            closeLib.Update();

                            // set indexed properties (internal)
                            engagementProperties["status"] = "open";
                            engagementProperties["statusdate"] = DateTime.Now;
                            engagementProperties["created"] = DateTime.Now;

                            this.AddIndexedProperty(engagementSite.RootWeb, "status", "open");
                            this.AddIndexedProperty(engagementSite.RootWeb, "statusdate", DateTime.Now);
                            this.AddIndexedProperty(engagementSite.RootWeb, "created", DateTime.Now);

                            // set indexed properties (public)
                            SPUser userPartner = engagementSite.RootWeb.EnsureUser(engagementProperties["Eng.Partner"].ToString());
                            SPUser userManager = engagementSite.RootWeb.EnsureUser(engagementProperties["Eng.Manager"].ToString());
                            SPUser userConcPartner = engagementSite.RootWeb.EnsureUser(engagementProperties["Concurring Partner"].ToString());

                            engagementProperties["engpartner"] = userPartner.LoginName;
                            engagementProperties["engmanager"] = userManager.LoginName;
                            engagementProperties["engconcpartner"] = userConcPartner.LoginName;

                            this.AddIndexedProperty(engagementSite.RootWeb, "Name des Mandanten", engagementProperties["Name des Mandanten"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "Opportunity Nr", engagementProperties["Opportunity Nr"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "Account", engagementProperties["Account"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "engconcpartner", engagementProperties["Concurring Partner"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "Concurring Partner", userConcPartner.Name);
                            this.AddIndexedProperty(engagementSite.RootWeb, "Niederlassung", engagementProperties["Niederlassung"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "Bezeichnung", engagementProperties["Bezeichnung"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "WB-Auftrags-Nr", engagementProperties["WB-Auftrags-Nr"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "Eng.Partner", userPartner.Name);
                            this.AddIndexedProperty(engagementSite.RootWeb, "Eng.Manager", userManager.Name);
                            this.AddIndexedProperty(engagementSite.RootWeb, "engpartner", engagementProperties["Eng.Partner"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "engmanager", engagementProperties["Eng.Manager"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "WB-Auftrag Status", engagementProperties["WB-Auftrag Status"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "WB-Auftrag Status Datum", engagementProperties["WB-Auftrag Status Datum"]);

                            engagementSite.RootWeb.Update();

                            // save engagement into database
                            this.SaveEngagementIntoDb(engagementSite, engagementOwners, engagementPartners, engagementStaff, engagementProperties);

                            try
                            {
                                // dispose engagements web and site
                                engagementSite.RootWeb.Dispose();
                                engagementSite.Dispose();
                            }
                            catch
                            {
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Error, 10, "Error provisioning site:" + ex.Message + " - " + ex.InnerException + " - " + ex.StackTrace);
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeDalException(1000, "Error in creating new site collection", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }

            return engagementSitePath;
        }

        /// <summary>
        /// Updates properties of a engagement site.
        /// </summary>
        /// <param name="wbId">The wb id.</param>
        /// <param name="engagementFolders">Engagement folders type.</param>
        /// <param name="engagementOwners">Engagement owners.</param>
        /// <param name="engagementPartners">Engagement partners.</param>
        /// <param name="engagementStaff">Engagement staff.</param>
        /// <param name="engagementProperties">Engagement properties.</param>
        /// <returns>
        /// Path to the newly created engagement site
        /// </returns>
        public string UpdateEngagementSiteProperties(int wbId, EngagementsDomain.Enums.EngagementFoldersType engagementFolders, string[] engagementOwners, string[] engagementPartners, string[] engagementStaff, Dictionary<string, object> engagementProperties)
        {
            string engagementSitePath = string.Empty;

            try
            {
                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Entered UpdateEngagementSiteProperties method in dal");

                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Trying to establish the context in: " + this.rootSiteCollection + ";");

                // STEP 1: CURRENT USER INFO
                ContextHelper context = new ContextHelper(this.rootSiteCollection + ";");

                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Context established");

                if (context.Web.CurrentUser == null)
                {
                    this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Current user is null");
                }

                string currentUserLogin = context.Web.CurrentUser.LoginName;

                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Current user login: " + currentUserLogin);

                string currentUserName = context.Web.CurrentUser.Name;

                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Current user name: " + currentUserName);

                string currentUserEmail = context.Web.CurrentUser.Email;

                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Current user mail: " + currentUserEmail);

                context.DisposeContext();

                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Got current user");

                // Create site under elevated permissions
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite elevatedSiteCollection = new SPSite(this.rootSiteCollection))
                    {
                        // STEP 2: CREATE SITE COLLECTION
                        this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Got under elevated priviledges");

                        SPContentDatabase rootDatabase = elevatedSiteCollection.ContentDatabase;

                        this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Root database is " + rootDatabase.Name);

                        // get default owner info
                        SPUser defaultOwner = elevatedSiteCollection.RootWeb.EnsureUser(this.defaultSiteOwner);

                        string defaultUserLogin = defaultOwner.LoginName;
                        string defaultUserName = defaultOwner.Name;
                        string defaultUserEmail = defaultOwner.Email;

                        this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Got default user");

                        // SPContentDatabase contentDatabase = CreateContentDb(elevatedSiteCollection, rootDatabase);
                        SPContentDatabase contentDatabase = rootDatabase;

                        // sharepoint system is not allowed to be the site collection owner
                        if (currentUserLogin.ToUpper() == "SHAREPOINT\\SYSTEM")
                        {
                            currentUserLogin = defaultUserLogin;
                            currentUserEmail = defaultUserEmail;
                            currentUserName = defaultUserName;
                        }

                        this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Normalized current user");

                        string siteColUrl = string.Format("{0}/{1}/{2}", this.rootSiteCollection, this.defaultProjectSitePath, wbId);

                        this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Built the site collection path " + siteColUrl);

                        SPSite engagementSite = new SPSite(siteColUrl);
                        SPGroup managersGroup;
                        SPGroup partnersGroup;
                        SPGroup staffGroup;

                        if (engagementSite != null)
                        {
                            engagementSitePath = engagementSite.Url;

                            if (engagementOwners != null && engagementOwners.Length != 0)
                            {
                                // Update Engagement Owners
                                managersGroup = SecurityHelpers.UpdateUsersInSecurityGroup(engagementSite, "Engagement Manager", "Engagement Manager - AcmeCorp Engagements Related SP Group", false, false, false, engagementOwners[0], engagementOwners);
                            }

                            if (engagementPartners != null && engagementPartners.Length != 0)
                            {
                                // Update Engagement Partners
                                partnersGroup = SecurityHelpers.UpdateUsersInSecurityGroup(engagementSite, "Engagement Partner", "Engagement Partner - AcmeCorp Engagements Related SP Group", false, false, false, engagementPartners[0], engagementPartners);
                            }

                            if (engagementStaff != null && engagementStaff.Length != 0)
                            {
                                // Update Engagement Staff
                                staffGroup = SecurityHelpers.UpdateUsersInSecurityGroup(engagementSite, "Engagement Staff", "Engagement Staff - AcmeCorp Engagements Related SP Group", false, false, false, engagementOwners[0], engagementStaff);
                            }
                            

                            // extend the contributors permissions level
                            // SecurityHelpers.AddPermissionToPermissionLevel(contributorsPermissionLevel, SPBasePermissions.AddListItems);
                            // SecurityHelpers.AddPermissionToPermissionLevel(contributorsPermissionLevel, SPBasePermissions.EditListItems);
                            // SecurityHelpers.AddPermissionToPermissionLevel(contributorsPermissionLevel, SPBasePermissions.DeleteListItems);

                            // set permissions for site
                           

                            // TODO: set configurable parameters folders for the documents library     

                            

                            // set indexed properties (internal)
                            engagementProperties["status"] = "open";
                            engagementProperties["statusdate"] = DateTime.Now;


                            this.AddIndexedProperty(engagementSite.RootWeb, "status", engagementProperties["status"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "statusdate", DateTime.Now);
                            // this.AddIndexedProperty(engagementSite.RootWeb, "created", Convert.ToDateTime(engagementProperties["statusdate"]));

                            // set indexed properties (public)
                            SPUser userPartner = engagementSite.RootWeb.EnsureUser(engagementProperties["Eng.Partner"].ToString());
                            SPUser userManager = engagementSite.RootWeb.EnsureUser(engagementProperties["Eng.Manager"].ToString());
                            SPUser userConcPartner = engagementSite.RootWeb.EnsureUser(engagementProperties["Concurring Partner"].ToString());

                            engagementProperties["engpartner"] = userPartner.LoginName;
                            engagementProperties["engmanager"] = userManager.LoginName;
                            engagementProperties["engconcpartner"] = userConcPartner.LoginName;

                            this.AddIndexedProperty(engagementSite.RootWeb, "Name des Mandanten", engagementProperties["Name des Mandanten"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "Opportunity Nr", engagementProperties["Opportunity Nr"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "Account", engagementProperties["Account"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "engconcpartner", engagementProperties["Concurring Partner"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "Concurring Partner", userConcPartner.Name);
                            this.AddIndexedProperty(engagementSite.RootWeb, "Niederlassung", engagementProperties["Niederlassung"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "Bezeichnung", engagementProperties["Bezeichnung"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "WB-Auftrags-Nr", engagementProperties["WB-Auftrags-Nr"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "Eng.Partner", userPartner.Name);
                            this.AddIndexedProperty(engagementSite.RootWeb, "Eng.Manager", userManager.Name);
                            this.AddIndexedProperty(engagementSite.RootWeb, "engpartner", engagementProperties["Eng.Partner"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "engmanager", engagementProperties["Eng.Manager"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "WB-Auftrag Status", engagementProperties["WB-Auftrag Status"]);
                            this.AddIndexedProperty(engagementSite.RootWeb, "WB-Auftrag Status Datum", engagementProperties["WB-Auftrag Status Datum"]);

                            engagementSite.RootWeb.Update();

                            // save engagement into database
                            //this.SaveEngagementIntoDb(engagementSite, engagementOwners, engagementPartners, engagementStaff, engagementProperties);

                            try
                            {
                                // dispose engagements web and site
                                engagementSite.RootWeb.Dispose();
                                engagementSite.Dispose();
                            }
                            catch
                            {
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Error, 11, "Error updating site properties:" + ex.Message + " - " + ex.InnerException + " - " + ex.StackTrace);
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeDalException(1000, "Error in updating properties of engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }

            return engagementSitePath;
        }


        /// <summary>
        /// Creates a folder accessible to whole engagement staff
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="rootFolder">The root folder.</param>
        /// <param name="staffGroup">The staff group.</param>
        /// <param name="managersGroup">The managers group.</param>
        /// <param name="partnersGroup">The partners group.</param>
        /// <param name="contributorsPermissionLevel">The contributors permission level.</param>
        /// <param name="readersPermissionLevel">Readers permission level</param>
        /// <param name="adminsPermissionLevel">Administrators permission level</param>
        /// <returns>Folder accessible to whole staff</returns>
        private static SPFolder CreateStaffFolder(string folderName, SPFolder rootFolder, SPGroup staffGroup, SPGroup managersGroup, SPGroup partnersGroup, SPRoleDefinition contributorsPermissionLevel, SPRoleDefinition readersPermissionLevel, SPRoleDefinition adminsPermissionLevel)
        {
            SPFolder newFolder = FilesAndFolders.CreateNewFolder(folderName, rootFolder);
            SecurityHelpers.ClearAllRolesFromSecurableObject(newFolder.Item);

            // add staff permissions
            SecurityHelpers.AddPermissionToSecurableObject(newFolder.Item, staffGroup, contributorsPermissionLevel, false);

            // add managers permissions
            SecurityHelpers.AddPermissionToSecurableObject(newFolder.Item, managersGroup, contributorsPermissionLevel, false);

            // add partners permissions
            SecurityHelpers.AddPermissionToSecurableObject(newFolder.Item, partnersGroup, contributorsPermissionLevel, false);

            newFolder.Update();
            return newFolder;
        }

        /// <summary>
        /// Creates a folder visible and accessible only to managers and partners
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="rootFolder">The root folder.</param>
        /// <param name="staffGroup">The staff group.</param>
        /// <param name="managersGroup">The managers group.</param>
        /// <param name="partnersGroup">The partners group.</param>
        /// <param name="contributorsPermissionLevel">The contributors permission level.</param>
        /// <param name="readersPermissionLevel">Readers permission level</param>
        /// <param name="adminsPermissionLevel">Administrators permission level</param>
        /// <returns>Folder created</returns>
        private static SPFolder CreatePartnersFolder(string folderName, SPFolder rootFolder, SPGroup staffGroup, SPGroup managersGroup, SPGroup partnersGroup, SPRoleDefinition contributorsPermissionLevel, SPRoleDefinition readersPermissionLevel, SPRoleDefinition adminsPermissionLevel)
        {
            SPFolder newFolder = FilesAndFolders.CreateNewFolder(folderName, rootFolder);
            SecurityHelpers.ClearAllRolesFromSecurableObject(newFolder.Item);

            // add staff permissions
            // SecurityHelpers.AddPermissionToSecurableObject(newFolder.Item, staffGroup, contributorsPermissionLevel, false);

            // add managers permissions
            SecurityHelpers.AddPermissionToSecurableObject(newFolder.Item, managersGroup, contributorsPermissionLevel, false);

            // add partners permissions
            SecurityHelpers.AddPermissionToSecurableObject(newFolder.Item, partnersGroup, contributorsPermissionLevel, false);

            newFolder.Update();
            return newFolder;
        }

        /// <summary>
        /// Adds the indexed property.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <param name="propName">Name of the prop.</param>
        /// <param name="propValue">The prop value.</param>
        private void AddIndexedProperty(SPWeb web, string propName, object propValue)
        {
            web.AllProperties[propName] = propValue;
            web.IndexedPropertyKeys.Add(propName);
        }

        /// <summary>
        /// Sets the unique permissions for engagement libraries.
        /// </summary>
        /// <param name="library">The engagement library.</param>
        /// <param name="staffGroup">The staff group.</param>
        /// <param name="managersGroup">The managers group.</param>
        /// <param name="partnersGroup">The partners group.</param>
        /// <param name="readersPermissionLevel">The readers permission level.</param>
        /// <param name="contributorsPermissionLevel">The contributors permission level.</param>
        private void SetLibraryPermissions(SPList library, SPGroup staffGroup, SPGroup managersGroup, SPGroup partnersGroup, SPRoleDefinition readersPermissionLevel, SPRoleDefinition contributorsPermissionLevel)
        {
            SecurityHelpers.ClearAllRolesFromSecurableObject(library);

            // add staff permissions
            SecurityHelpers.AddPermissionToSecurableObject(library, staffGroup, readersPermissionLevel, false);

            // add managers permissions
            SecurityHelpers.AddPermissionToSecurableObject(library, managersGroup, contributorsPermissionLevel, false);

            // add partners permissions
            SecurityHelpers.AddPermissionToSecurableObject(library, partnersGroup, contributorsPermissionLevel, false);
        }

        /// <summary>
        /// Creates a folder with contribute rights for managers and partners, and read only for staff
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="rootFolder">The root folder.</param>
        /// <param name="staffGroup">The staff group.</param>
        /// <param name="managersGroup">The managers group.</param>
        /// <param name="partnersGroup">The partners group.</param>
        /// <param name="contributorsPermissionLevel">The contributors permission level.</param>
        /// <param name="readersPermissionLevel">Readers permission level</param>
        /// <param name="adminsPermissionLevel">Administrators permission level</param>
        /// <returns>Folder created</returns>
        private SPFolder CreateStaffReadOnlyFolder(string folderName, SPFolder rootFolder, SPGroup staffGroup, SPGroup managersGroup, SPGroup partnersGroup, SPRoleDefinition contributorsPermissionLevel, SPRoleDefinition readersPermissionLevel, SPRoleDefinition adminsPermissionLevel)
        {
            SPFolder newFolder = FilesAndFolders.CreateNewFolder(folderName, rootFolder);
            SecurityHelpers.ClearAllRolesFromSecurableObject(newFolder.Item);

            // add staff permissions
            SecurityHelpers.AddPermissionToSecurableObject(newFolder.Item, staffGroup, readersPermissionLevel, false);

            // add managers permissions
            SecurityHelpers.AddPermissionToSecurableObject(newFolder.Item, managersGroup, contributorsPermissionLevel, false);

            // add partners permissions
            SecurityHelpers.AddPermissionToSecurableObject(newFolder.Item, partnersGroup, contributorsPermissionLevel, false);

            newFolder.Update();
            return newFolder;
        }

        /// <summary>
        /// Creates the engagement site collection.
        /// </summary>
        /// <param name="wbId">The wb id.</param>
        /// <param name="engagementProperties">The engagement properties.</param>
        /// <param name="currentUserLogin">The current user login.</param>
        /// <param name="currentUserName">Name of the current user.</param>
        /// <param name="currentUserEmail">The current user email.</param>
        /// <param name="defaultUserLogin">The default user login.</param>
        /// <param name="defaultUserName">Default name of the user.</param>
        /// <param name="defaultUserEmail">The default user email.</param>
        /// <param name="contentDatabase">The content database.</param>
        /// <param name="siteColUrl">The site col URL.</param>
        /// <returns>SiteCollection object</returns>
        private SPSite CreateEngagementSiteCollection(int wbId, Dictionary<string, object> engagementProperties, string currentUserLogin, string currentUserName, string currentUserEmail, string defaultUserLogin, string defaultUserName, string defaultUserEmail, SPContentDatabase contentDatabase, string siteColUrl)
        {
            // After we have the content database, please create the site collection in the database
            // First prove if we need two admins
            SPSite newSite;
            if (currentUserLogin.ToUpper().EndsWith(defaultUserLogin.ToUpper()))
            {
                // if the current user is default user, add only him as the site collection owner
                newSite = contentDatabase.Sites.Add(siteColUrl, wbId.ToString(), engagementProperties["Bezeichnung"].ToString(), this.engagementProjectSiteTemplateLanguage, this.engagementProjectSiteTemplateName, currentUserLogin, currentUserName, currentUserEmail);
            }
            else
            {
                // if the current user is not the default user, add both as the site collection owners
                newSite = contentDatabase.Sites.Add(siteColUrl, wbId.ToString(), engagementProperties["Bezeichnung"].ToString(), this.engagementProjectSiteTemplateLanguage, this.engagementProjectSiteTemplateName, currentUserLogin, currentUserName, currentUserEmail, defaultUserLogin, defaultUserName, defaultUserEmail);
            }

            this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Site collection created");

            if (newSite != null)
            {
                // SPQuota quota = new SPQuota();
                // quota.StorageMaximumLevel = this.siteCollectionQuotaTemplate;
                // newSite.Quota = quota;
                newSite.RootWeb.Update();
                //contentDatabase.Update();

                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Content database updated");
            }

            return newSite;
        }

        /// <summary>
        /// Creates or selects appropriate the content database.
        /// </summary>
        /// <param name="elevatedSiteCollection">The elevated site collection.</param>
        /// <param name="rootDatabase">The root database.</param>
        /// <returns>Content database that is created</returns>
        private SPContentDatabase CreateContentDb(SPSite elevatedSiteCollection, SPContentDatabase rootDatabase)
        {
            // get all content databases
            SPContentDatabaseCollection contentDbs = elevatedSiteCollection.WebApplication.ContentDatabases;

            this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Got under elevated priviledges");

            List<SPContentDatabase> databases = new List<SPContentDatabase>();
            foreach (SPContentDatabase contentDb in contentDbs)
            {
                if (contentDb.Name.StartsWith(this.contentDatabaseTemplateName))
                {
                    string stringIndexCounter = contentDb.Name.Substring(this.contentDatabaseTemplateName.Length);
                    int intIndexCounter = 0;
                    bool indexResolved = int.TryParse(stringIndexCounter, out intIndexCounter);
                    stringIndexCounter = this.PrependCharactersToString(intIndexCounter.ToString(), '0', 3);

                    if (!contentDb.Properties.ContainsKey("IntIndexCounter"))
                    {
                        contentDb.Properties.Add("IntIndexCounter", intIndexCounter);
                    }

                    if (!contentDb.Properties.ContainsKey("StringIndexCounter"))
                    {
                        contentDb.Properties.Add("StringIndexCounter", stringIndexCounter);
                    }

                    databases.Add(contentDb);
                }
            }

            this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Found all active content databases");

            // Sort databases
            List<SPContentDatabase> sortedDatabases = databases.OrderByDescending(x => x.GetDbIndexAsInt()).ToList();

            bool createNewDb = false;
            string newName = string.Empty;

            if (sortedDatabases.Count == 0)
            {
                createNewDb = true;
                newName = this.contentDatabaseTemplateName + "001";
            }
            else
            {
                SPContentDatabase lastDatabase = sortedDatabases[0];

                if (lastDatabase.DiskSizeRequired > this.contentDatabaseMaxTresholdSize)
                {
                    createNewDb = true;
                    int newIndex = lastDatabase.GetDbIndexAsInt() + 1;
                    newName = this.contentDatabaseTemplateName + this.PrependCharactersToString(newIndex.ToString(), '0', 3);
                }

                if (lastDatabase.Sites.Count >= lastDatabase.MaximumSiteCount)
                {
                    createNewDb = true;
                }
            }

            this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Create new database? " + createNewDb.ToString());

            SPContentDatabase contentDatabase;
            if (!createNewDb)
            {
                contentDatabase = sortedDatabases[0];
            }
            else
            {
                contentDatabase = elevatedSiteCollection.WebApplication.ContentDatabases.Add(rootDatabase.Server, newName, rootDatabase.Username, rootDatabase.Password, this.contentDatabaseWarningSiteCollections, this.contentDatabaseMaxSiteCollections, 0);
                contentDatabase.Update();
                elevatedSiteCollection.WebApplication.Update();

                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "New content db created");
            }

            return contentDatabase;
        }

        /// <summary>
        /// Adds the leading zeros in front of the string.
        /// </summary>
        /// <param name="originalString">The original</param>
        /// <param name="characterToPrepend">The character to prepend.</param>
        /// <param name="finalLength">The length of the destination string (after prepending)</param>
        /// <returns>
        /// String with prepended characters
        /// </returns>
        private string PrependCharactersToString(string originalString, char characterToPrepend, int finalLength)
        {
            string toPrepend = string.Empty;
            if (originalString.Length < finalLength)
            {
                int prependLength = finalLength - originalString.Length;
                toPrepend = new string(characterToPrepend, prependLength);
            }

            return string.Concat(toPrepend, originalString);
        }
    }
}
