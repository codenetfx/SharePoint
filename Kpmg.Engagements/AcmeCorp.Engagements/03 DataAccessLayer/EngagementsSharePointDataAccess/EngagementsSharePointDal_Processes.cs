// -----------------------------------------------------------------------
// <copyright file="EngagementsSharePointDal_Processes.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsSharePointDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    using Acme.Core.DiagnosticSystem.Enums;
    using Acme.Core.DiagnosticSystem.ExceptionEntities;
    using Acme.Core.DiagnosticSystem.ExceptionManager;    

    /// <summary>
    /// SharePoint DataAccess Layer for AcmeCorp Engagements
    /// </summary>
    public partial class EngagementsSharePointDal : AcmeCorp.Engagements.EngagementsDomain.IAcmeCorpEngagementsDalInterface
    {
        /// <summary>
        /// Gets the engagement status.
        /// </summary>
        /// <param name="engagementId">The engagement id.</param>
        /// <returns>Engagement status</returns>
        public string GetEngagementStatus(long engagementId)
        {
            try
            {
                string siteColUrl = string.Format("{0}/{1}/{2}", this.rootSiteCollection, this.defaultProjectSitePath, engagementId);

                string status = "Unknown";

                // Create site under elevated permissions
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite elevatedSiteCollection = new SPSite(siteColUrl))
                    {
                        status = elevatedSiteCollection.RootWeb.AllProperties["status"].ToString();
                    }
                });

                return status;
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeDalException(1000, "Error reading engagement site status", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Closes the engagement.
        /// </summary>
        /// <param name="engagementId">The engagement id.</param>
        public void CloseEngagement(long engagementId)
        {
            try
            {
                string siteColUrl = string.Format("{0}/{1}/{2}", this.rootSiteCollection, this.defaultProjectSitePath, engagementId);

                // Create site under elevated permissions
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite elevatedSiteCollection = new SPSite(siteColUrl))
                    {
                        SPRoleDefinition contributors = elevatedSiteCollection.RootWeb.RoleDefinitions["AcmeCorp_Contribute"];

                        // change to readers!
                        contributors.BasePermissions = elevatedSiteCollection.RootWeb.RoleDefinitions.GetByType(SPRoleType.Reader).BasePermissions;

                        contributors.Update();

                        elevatedSiteCollection.RootWeb.AllProperties["status"] = "closed";
                        elevatedSiteCollection.RootWeb.AllProperties["statusdate"] = DateTime.Now;

                        elevatedSiteCollection.RootWeb.Update();
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeDalException(1000, "Error in closing the engagement", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Reopens the engagement.
        /// </summary>
        /// <param name="engagementId">The engagement id.</param>
        public void ReopenEngagement(long engagementId)
        {
            try
            {
                string siteColUrl = string.Format("{0}/{1}/{2}", this.rootSiteCollection, this.defaultProjectSitePath, engagementId);

                // Create site under elevated permissions
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite elevatedSiteCollection = new SPSite(siteColUrl))
                    {
                        SPRoleDefinition contributors = elevatedSiteCollection.RootWeb.RoleDefinitions["AcmeCorp_Contribute"];

                        // change to readers!
                        contributors.BasePermissions = elevatedSiteCollection.RootWeb.RoleDefinitions.GetByType(SPRoleType.Contributor).BasePermissions;

                        contributors.Update();

                        elevatedSiteCollection.RootWeb.AllProperties["status"] = "open";
                        elevatedSiteCollection.RootWeb.AllProperties["statusdate"] = DateTime.Now;

                        elevatedSiteCollection.RootWeb.Update();
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeDalException(1000, "Error in reopening the engagement", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }
    }
}
