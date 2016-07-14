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
        /// Creates the internal site.
        /// </summary>
        /// <param name="siteOwners">The site owners.</param>
        /// <param name="siteDeputies">The site deputies.</param>
        /// <param name="title">The site title.</param>
        /// <param name="description">The site description.</param>
        /// <returns></returns>
        public string CreateInternalSite(string[] siteOwners, string[] siteDeputies, string title, string description)
        {
            string interalSitePath = string.Empty;

            try
            {
                string siteUrl = GetSiteUrl();

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

                        string siteColUrl = string.Format("{0}/{1}/{2}", this.rootSiteCollection, this.defaultProjectSitePath, siteUrl);

                        this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Built the site collection path " + siteColUrl);

                        SPSite internalSite = this.CreateInternalSiteCollection(title, description, currentUserLogin, currentUserName, currentUserEmail, defaultUserLogin, defaultUserName, defaultUserEmail, contentDatabase, siteColUrl);

                        if (internalSite != null)
                        {
                            interalSitePath = internalSite.Url;

                            // delete all groups
                            Acme.SharePointCore.Helpers.SecurityHelpers.DeleteAllSecurityGroups(internalSite);
                            this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "All default security groups are deleted");

                            // create new default groups
                            SPGroup managersGroup = SecurityHelpers.CreateSecurityGroup(internalSite, "Engagement Manager", "Engagement Manager - AcmeCorp Engagements Related SP Group", false, false, false, siteOwners[0], siteOwners);
                            SPGroup staffGroup = SecurityHelpers.CreateSecurityGroup(internalSite, "Engagement Staff", "Engagement Srtaff - AcmeCorp Engagements Related SP Group", false, false, false, siteOwners[0], siteOwners);

                            this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "New security groups are created");

                            // create AcmeCorp Engagements Related Permission Levels
                            // TODO: Make configurable
                            SPRoleDefinition readersPermissionLevel = SecurityHelpers.CreateNewPermissionLevel(internalSite.RootWeb, "AcmeCorp_Read", "AcmeCorp Engagements Read Only Permissions", internalSite.RootWeb.RoleDefinitions.GetByType(SPRoleType.Reader).BasePermissions);
                            SPRoleDefinition contributorsPermissionLevel = SecurityHelpers.CreateNewPermissionLevel(internalSite.RootWeb, "AcmeCorp_Contribute", "AcmeCorp Engagements Contributor Permissions", internalSite.RootWeb.RoleDefinitions.GetByType(SPRoleType.Contributor).BasePermissions);
                            SPRoleDefinition administratorsPermissionLevel = SecurityHelpers.CreateNewPermissionLevel(internalSite.RootWeb, "AcmeCorp_Manage", "AcmeCorp Engagements Manager Permissions", internalSite.RootWeb.RoleDefinitions.GetByType(SPRoleType.Administrator).BasePermissions);

                            // set permissions for site
                            SecurityHelpers.AddPermissionToSecurableObject(internalSite.RootWeb, managersGroup, administratorsPermissionLevel, false);
                            SecurityHelpers.AddPermissionToSecurableObject(internalSite.RootWeb, staffGroup, contributorsPermissionLevel, false);

                            internalSite.RootWeb.Update();

                            try
                            {
                                // dispose engagements web and site
                                internalSite.RootWeb.Dispose();
                                internalSite.Dispose();
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
                this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Error, 10, "Error provisioning internal site:" + ex.Message + " - " + ex.InnerException + " - " + ex.StackTrace);
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeDalException(1000, "Error in creating new internal site collection", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }

            return interalSitePath;
        }

        /// <summary>
        /// Gets the site URL. This is a dummy random method at the moment, needs to be expanded in the future
        /// </summary>
        /// <returns>site url</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string GetSiteUrl()
        {
            //TODO: Implement real logic for numbering the internal sites
            Random rand = new Random(DateTime.Now.Millisecond);
            int rnd = rand.Next(90001, 99999);
            return rnd.ToString();
        }


        /// <summary>
        /// Creates the internal site collection.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="currentUserLogin">The current user login.</param>
        /// <param name="currentUserName">Name of the current user.</param>
        /// <param name="currentUserEmail">The current user email.</param>
        /// <param name="defaultUserLogin">The default user login.</param>
        /// <param name="defaultUserName">Default name of the user.</param>
        /// <param name="defaultUserEmail">The default user email.</param>
        /// <param name="contentDatabase">The content database.</param>
        /// <param name="siteColUrl">The site col URL.</param>
        /// <returns></returns>
        private SPSite CreateInternalSiteCollection(string title, string description, string currentUserLogin, string currentUserName, string currentUserEmail, string defaultUserLogin, string defaultUserName, string defaultUserEmail, SPContentDatabase contentDatabase, string siteColUrl)
        {
            // After we have the content database, please create the site collection in the database
            // First prove if we need two admins
            SPSite newSite;
            if (currentUserLogin.ToUpper().EndsWith(defaultUserLogin.ToUpper()))
            {
                // if the current user is default user, add only him as the site collection owner
                newSite = contentDatabase.Sites.Add(siteColUrl, title, description, this.internalProjectSiteTemplateLanguage, this.internalProjectSiteTemplateName, currentUserLogin, currentUserName, currentUserEmail);
            }
            else
            {
                // if the current user is not the default user, add both as the site collection owners
                newSite = contentDatabase.Sites.Add(siteColUrl, title, description, this.engagementProjectSiteTemplateLanguage, this.engagementProjectSiteTemplateName, currentUserLogin, currentUserName, currentUserEmail, defaultUserLogin, defaultUserName, defaultUserEmail);
            }

            this.Logger.LogMessage(this.Logger.DefaultArea, "Site Provisioining", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Internal site collection created");

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



    }
}
