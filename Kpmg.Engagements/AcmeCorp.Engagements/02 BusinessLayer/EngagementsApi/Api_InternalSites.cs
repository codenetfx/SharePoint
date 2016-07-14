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
        /// Creates the new internal site.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="siteOwners">The site owners.</param>
        /// <param name="siteDeputies">The site deputies.</param>
        /// <returns>
        /// Path to the internal site
        /// </returns>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException">0;Error in creating engagement site</exception>
        public string CreateNewInternalSite(string title, string description, string[] siteOwners, string[] siteDeputies)
        {
            try
            {
                this.logger.LogMessage(this.logger.DefaultArea, "InternalSites", Acme.Core.Logger.Enums.EventServerity.Information, 0, "Creating new internal site");

                string engSite = this.dataLayer.CreateInternalSite(siteOwners, siteDeputies, title, description);

                return engSite;
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in creating internal site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        

    }
}
