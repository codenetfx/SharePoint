// -----------------------------------------------------------------------
// <copyright file="IAcmeCorpEngagementsDalInterface.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsDomain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AcmeCorp.Engagements;

    /// <summary>
    /// AcmeCorp Engagements DAL Interface
    /// </summary>
    public interface IAcmeCorpEngagementsDalInterface
    {

        #region Lookup Tables

        CustomTableQueryResponseRows[] GetDataFromLookupTable(string tableName, string language);
        void AddTerm(string tablename, string term, string language, string SPSiteURL);
        void EditTerm(string tablename, string term, string language, string SPSiteURL);
        void DeprecateTerm(string tablename, string term, string SPSiteURL);
        void ProcessTermsSync(string termset, string language);
        void ProcessTermsAsync(string termset, string language);
        #endregion

        #region Configuration
        /// <summary>
        /// Gets or sets the logger to be used in the Data Access Layer
        /// </summary>
        Acme.Core.Logger.ILogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the cache to be used in the Data Access Layer
        /// </summary>
        Acme.Core.Caching.ICaching Cache { get; set; }

        /// <summary>
        /// Gets or sets the settings implementation to be used in the Data Access Layer
        /// </summary>
        Acme.Core.Settings.ISettings Settings { get; set; }

        /// <summary>
        /// Gets the data context used by the Data Access Layer.
        /// </summary>
        object DataContext { get; }

        /// <summary>
        /// Initializes the DAL with appropriate configuration.  
        /// </summary>
        /// <param name="interfaceConfiguration">DAL configuration object</param>
        /// <param name="dalContext">The context, usually needed by a data access layer, passed from the UI layer. Set it to null if the DAL does not need the context</param>
        void InitializeConfiguration(Acme.Core.Configuration.InterfaceConfigurationSection interfaceConfiguration, object dalContext);

        /// <summary>
        /// Retrieves the dal configuration parameter.
        /// </summary>
        /// <param name="parameter">The parameter we want to retrieve.</param>
        /// <returns></returns>
        string GetDalConfigurationParameter(string parameter);

        #endregion

        #region Engagement provisioning
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
        string CreateEngagementSite(int wbId, EngagementsDomain.Enums.EngagementFoldersType engagementFolders, string[] engagementOwners, string[] engagementPartners, string[] engagementStaff, Dictionary<string, object> engagementProperties);

        /// <summary>
        /// Updates properties of the engagement site.
        /// </summary>
        /// <param name="wbId">The wb id.</param>
        /// <param name="engagementFolders">Engagement folders type.</param>
        /// <param name="engagementOwners">Engagement owners.</param>
        /// <param name="engagementPartners">Engagement partners.</param>
        /// <param name="engagementStaff">Engagement staff.</param>
        /// <param name="engagementProperties">Engagement properties.</param>
        /// <returns>
        /// Path to the engagement site
        /// </returns>
        string UpdateEngagementSiteProperties(int wbId, EngagementsDomain.Enums.EngagementFoldersType engagementFolders, string[] engagementOwners, string[] engagementPartners, string[] engagementStaff, Dictionary<string, object> engagementProperties);

        /// <summary>
        /// Creates the opportunity site.
        /// </summary>
        /// <param name="opportunityId">The opportunity id.</param>
        /// <param name="opportunityManagers">The opportunity managers.</param>
        /// <param name="opportunityPartners">The opportunity partners.</param>
        /// <param name="opportunityStaff">The opportunity staff.</param>
        /// <param name="opportunityProperties">The opportunity properties.</param>
        /// <returns></returns>
        string CreateOpportunitySite(int opportunityId, string[] opportunityManagers, string[] opportunityPartners, string[] opportunityStaff, Dictionary<string, object> opportunityProperties);


        /// <summary>
        /// Updates the opportunity site properties.
        /// </summary>
        /// <param name="opportuityId">The opportuity id.</param>
        /// <param name="opportunityManagers">The opportunity managers.</param>
        /// <param name="opportunityPartners">The opportunity partners.</param>
        /// <param name="opportunityStaff">The opportunity staff.</param>
        /// <param name="opportunityProperties">The opportunity properties.</param>
        /// <returns></returns>
        string UpdateOpportunitySiteProperties(int opportuityId, string[] opportunityManagers, string[] opportunityPartners, string[] opportunityStaff, Dictionary<string, object> opportunityProperties);

        /// <summary>
        /// Creates an engagement site from a opportunity site.
        /// </summary>
        /// <param name="opportunityId">The opportunity id.</param>
        /// <param name="wbId">The wb id.</param>
        /// <returns></returns>
        string CreateEngagementFromOpportunity(int opportunityId, int wbId);

        /// <summary>
        /// Creates the internal site.
        /// </summary>
        /// <param name="siteOwners">The site owners.</param>
        /// <param name="siteDeputies">The site deputies.</param>
        /// <param name="title">The site title.</param>
        /// <param name="description">The site description.</param>
        /// <returns></returns>
        string CreateInternalSite(string[] siteOwners, string[] siteDeputies, string title, string description);

        /// <summary>
        /// Gets the engagement status.
        /// </summary>
        /// <param name="engagementId">The engagement id.</param>
        /// <returns>Engagement Status</returns>
        string GetEngagementStatus(long engagementId);

        /// <summary>
        /// Closes the engagement.
        /// </summary>
        /// <param name="engagementId">The engagement id.</param>
        void CloseEngagement(long engagementId);

        /// <summary>
        /// Reopens the engagement.
        /// </summary>
        /// <param name="engagementId">The engagement id.</param>
        void ReopenEngagement(long engagementId);

        #endregion
    }
}
