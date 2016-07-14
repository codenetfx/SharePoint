// -----------------------------------------------------------------------
// <copyright file="Api_Opportunities.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Initializes engagements API
    /// </summary>
    public partial class Api
    {

        /// <summary>
        /// Creates the new opportunity site.
        /// </summary>
        /// <param name="wbId">The wb id.</param>
        /// <param name="engagementFolders">Engagement Folders</param>
        /// <param name="engagementOwners">Engagement Owners</param>
        /// <param name="engagementPartners">Engagement partners</param>
        /// <param name="engagementStaff">Engagement Staff</param>
        /// <param name="engagementProperties">Engagement Properties</param>
        /// <returns>Path to the engagement site</returns>
        public string CreateNewOpportunitySite(int wbId, string[] engagementOwners, string[] engagementPartners, string[] engagementStaff, Dictionary<string, object> engagementProperties)
        {
            try
            {
                this.logger.LogMessage(this.logger.DefaultArea, "Test", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Hello there");

                string engSite = this.dataLayer.CreateOpportunitySite(wbId, engagementOwners, engagementPartners, engagementStaff, engagementProperties);

                return engSite;
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in creating engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        public string CreateEngagementFromOpportunity(int opportunityId, int wbId)
        {
            try
            {
                this.logger.LogMessage(this.logger.DefaultArea, "Test", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Hello there");

                string engSite = this.dataLayer.CreateEngagementFromOpportunity(opportunityId, wbId);

                return engSite;
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in updating engagement site properties", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }


        /// <summary>
        /// Updates properties of the opportunity site.
        /// </summary>
        /// <param name="wbId">The wb id.</param>
        /// <param name="engagementFolders">Engagement Folders</param>
        /// <param name="engagementOwners">Engagement Owners</param>
        /// <param name="engagementPartners">Engagement partners</param>
        /// <param name="engagementStaff">Engagement Staff</param>
        /// <param name="engagementProperties">Engagement Properties</param>
        /// <returns>
        /// Path to the engagement site
        /// </returns>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException">0;Error in updating engagement site properties</exception>
        public string UpdateOpportunitySiteProperties(int wbId, string[] engagementOwners, string[] engagementPartners, string[] engagementStaff, Dictionary<string, object> engagementProperties)
        {
            try
            {
                this.logger.LogMessage(this.logger.DefaultArea, "Test", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Hello there");
                Utilities.ActiveDirectoryHelpers adHelpers = new Utilities.ActiveDirectoryHelpers(this);

                string[] ownerDeputies = adHelpers.GetDeputiesForUsers(engagementOwners);
                string[] partnerDeputies = adHelpers.GetDeputiesForUsers(engagementPartners);
                engagementOwners = AddDeputiesToUsersArray(engagementOwners, ownerDeputies);
                engagementPartners = AddDeputiesToUsersArray(engagementPartners, partnerDeputies);
                string engSite = this.dataLayer.UpdateOpportunitySiteProperties(wbId, engagementOwners, engagementPartners, engagementStaff, engagementProperties);

                return engSite;
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in updating engagement site properties", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

    }
}