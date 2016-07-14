// -----------------------------------------------------------------------
// <copyright file="Api_Engagements.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsApi
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AcmeCorp.Engagements.EngagementsDomain;

    /// <summary>
    /// Initializes engagements API
    /// </summary>
    public partial class Api
    {
        /// <summary>
        /// Creates the new engagement site.
        /// </summary>
        /// <param name="wbId">The wb id.</param>
        /// <param name="engagementFolders">Engagement Folders</param>
        /// <param name="engagementOwners">Engagement Owners</param>
        /// <param name="engagementPartners">Engagement partners</param>
        /// <param name="engagementStaff">Engagement Staff</param>
        /// <param name="engagementProperties">Engagement Properties</param>
        /// <returns>Path to the engagement site</returns>
        public string CreateNewEngagementSite(int wbId, EngagementsDomain.Enums.EngagementFoldersType engagementFolders, string[] engagementOwners, string[] engagementPartners, string[] engagementStaff, Dictionary<string, object> engagementProperties)
        {
            try
            {
                this.logger.LogMessage(this.logger.DefaultArea, "Test", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Hello there");

                string engSite = this.dataLayer.CreateEngagementSite(wbId, engagementFolders, engagementOwners, engagementPartners, engagementStaff, engagementProperties);

                return engSite;
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in creating engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Updates properties of the engagement site.
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
        public string UpdateEngagementSiteProperties(int wbId, EngagementsDomain.Enums.EngagementFoldersType engagementFolders, string[] engagementOwners, string[] engagementPartners, string[] engagementStaff, Dictionary<string, object> engagementProperties)
        {
            try
            {
                this.logger.LogMessage(this.logger.DefaultArea, "Test", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Hello there");
                Utilities.ActiveDirectoryHelpers adHelpers = new Utilities.ActiveDirectoryHelpers(this);

                string[] ownerDeputies = adHelpers.GetDeputiesForUsers(engagementOwners);
                string[] partnerDeputies = adHelpers.GetDeputiesForUsers(engagementPartners);
                engagementOwners = AddDeputiesToUsersArray(engagementOwners, ownerDeputies);
                engagementPartners = AddDeputiesToUsersArray(engagementPartners, partnerDeputies);
                string engSite = this.dataLayer.UpdateEngagementSiteProperties(wbId, engagementFolders, engagementOwners, engagementPartners, engagementStaff, engagementProperties);

                return engSite;
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in updating engagement site properties", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Adds the deputies usernames to users array.
        /// </summary>
        /// <param name="usernamesArray">The usernames array.</param>
        /// <param name="deputiesArray">The deputies array.</param>
        /// <returns></returns>
        private static string[] AddDeputiesToUsersArray(string[] usernamesArray, string[] deputiesArray)
        {
            try
            {
            if (deputiesArray != null && deputiesArray.Length != 0)
            {
                List<string> engagementOwnersList = usernamesArray.ToList<string>();
                foreach (var item in deputiesArray)
                {
                    engagementOwnersList.Add(item);
                }
                usernamesArray = engagementOwnersList.ToArray();
            }
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in adding deputies to username array", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
            return usernamesArray;
        }

        /// <summary>
        /// Gets the engagement status.
        /// </summary>
        /// <param name="wbId">The wb id of the engagement</param>
        /// <returns>Engagement status</returns>
        public string GetEngagementStatus(long wbId)
        {
            try
            {
                string status = this.dataLayer.GetEngagementStatus(wbId);

                return status;
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in creating engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Closes the engagement.
        /// </summary>
        /// <param name="wbId">The engagement id.</param>
        /// <exception cref="Acme">Exception to return</exception>
        public void CloseEngagement(long wbId)
        {
            try
            {
                this.dataLayer.CloseEngagement(wbId);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in creating engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Reopens the engagement.
        /// </summary>
        /// <param name="wbId">The engagement id.</param>
        /// <exception cref="Acme">Exception to return</exception>
        public void ReopenEngagement(long wbId)
        {
            try
            {
                this.dataLayer.ReopenEngagement(wbId);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in creating engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }
    }
}
