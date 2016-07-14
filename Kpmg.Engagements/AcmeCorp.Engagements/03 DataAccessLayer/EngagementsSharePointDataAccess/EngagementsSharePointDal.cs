// -----------------------------------------------------------------------
// <copyright file="EngagementsSharePointDal.cs" company="AcmeCorp">
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
        #region Fully qualified Assembly name...
        /*
        AcmeCorp.Engagements.EngagementsSharePointDataAccess.EngagementsSharePointDal, AcmeCorp.Engagements.EngagementsSharePointDataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=76ab1eb817808800
        */
        #endregion

        #region Private fields
        /// <summary>
        /// DAL configuration
        /// </summary>
        private Acme.Core.Configuration.InterfaceConfigurationSection dalConfiguration;

        /// <summary>
        /// SharePoint context, either SPWeb, or as the string web url. To be resolved by the context helper class
        /// </summary>
        private object sharepointContext;

        /// <summary>
        /// Default owner when to be set when new sites are created
        /// </summary>
        private string defaultSiteOwner;

        /// <summary>
        /// Default path to be used when new project sites are created, instead of "sites" in "http://server/sites/projectsite"
        /// </summary>
        private string defaultProjectSitePath;

        /// <summary>
        /// Path to the root site collection
        /// </summary>
        private string rootSiteCollection;

        /// <summary>
        /// Name of the Search Service application proxy
        /// </summary>
        private string searchServiceApplicationProxy;

        /// <summary>
        /// Default taxonomy group name for AcmeCorp Engagements
        /// </summary>
        private string projectsTxonomyGroupName;

        /// <summary>
        /// Name of the Term set where the folder structures are stored
        /// </summary>
        private string folderStructuresTermSetName;

        /// <summary>
        /// Naming convention for the content databases. Indexer will be appended on the end of the database name
        /// </summary>
        private string contentDatabaseTemplateName;

        /// <summary>
        /// Maximum content database size (in bytes) before a new content database is created for storing the project site collections
        /// </summary>
        private ulong contentDatabaseMaxTresholdSize;

        /// <summary>
        /// Maximum site collection size (in bytes) - quota template
        /// </summary>
        private long siteCollectionQuotaTemplate;

        /// <summary>
        /// Maximal number of site collections per database
        /// </summary>
        private int contentDatabaseMaxSiteCollections;

        /// <summary>
        /// Warning threshold number of site collections per database
        /// </summary>
        private int contentDatabaseWarningSiteCollections;

        /// <summary>
        /// Name of the template on which the engagement projects site collections are based
        /// </summary>
        private string engagementProjectSiteTemplateName;

        /// <summary>
        /// Language code of the template on which engagement project site collections are based
        /// </summary>
        private uint engagementProjectSiteTemplateLanguage;

        /// <summary>
        /// Name of the template on which the engagement projects site collections are based
        /// </summary>
        private string internalProjectSiteTemplateName;

        /// <summary>
        /// Language code of the template on which engagement project site collections are based
        /// </summary>
        private uint internalProjectSiteTemplateLanguage;


        /// <summary>
        /// "Plan" library name for opportunity
        /// </summary>
        private string opportunityPlanLibraryName;

        /// <summary>
        /// "Initiate" library name for opportunity
        /// </summary>
        private string opportunityInitiateLibraryName;

        /// <summary>
        /// "Close" library name for engagement
        /// </summary>
        private string engagementCloseLibraryName;

        /// <summary>
        /// "Deliver" library name for engagement
        /// </summary>
        private string engagementDeliverLibraryName;

        /// <summary>
        /// Engagements DB connection string
        /// </summary>
        private string engagementsDbConnectionString;

        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the logger to be used in the Data Access Layer
        /// </summary>
        public Acme.Core.Logger.ILogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the cache to be used in the Data Access Layer
        /// </summary>
        public Acme.Core.Caching.ICaching Cache { get; set; }

        /// <summary>
        /// Gets or sets the settings implementation to be used in the Data Access Layer
        /// </summary>
        public Acme.Core.Settings.ISettings Settings { get; set; }

        /// <summary>
        /// Gets the data context used by the Data Access Layer.
        /// </summary>
        public object DataContext
        {
            get
            {
                return this.sharepointContext;
            }
        }
        #endregion

        /// <summary>
        /// Initializes the MOCK DAL with appropriate configuration.
        /// </summary>
        /// <param name="interfaceConfiguration">DAL configuration object</param>
        /// <param name="sharepointContext">SharePoint context, being passed either as a SPWeb or as the string web url. To be resolved by the context helper class</param>
        public void InitializeConfiguration(Acme.Core.Configuration.InterfaceConfigurationSection interfaceConfiguration, object sharepointContext)
        {
            try
            {
                this.dalConfiguration = interfaceConfiguration;
                this.sharepointContext = sharepointContext;

                // Now look at the initialization parameters, if all are there and valid. If they are, store their values in the member variables.
                // If not, throw an critical initialization exception that API could not be initialized
                if (!this.dalConfiguration.ImplementationParameters.KeyExists("DefaultSiteOwner") || this.dalConfiguration.ImplementationParameters["DefaultSiteOwner"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: DefaultSiteOwner", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("DefaultProjectSitePath") || this.dalConfiguration.ImplementationParameters["DefaultProjectSitePath"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: DefaultProjectSitePath", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("RootSiteCollection") || this.dalConfiguration.ImplementationParameters["RootSiteCollection"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: RootSiteCollection", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("SearchServiceApplicationProxy") || this.dalConfiguration.ImplementationParameters["SearchServiceApplicationProxy"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: SearchServiceApplicationProxy", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("ProjectsTxonomyGroupName") || this.dalConfiguration.ImplementationParameters["ProjectsTxonomyGroupName"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: ProjectsTxonomyGroupName", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("FolderStructuresTermSetName") || this.dalConfiguration.ImplementationParameters["FolderStructuresTermSetName"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: FolderStructuresTermSetName", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("ContentDatabaseTemplateName") || this.dalConfiguration.ImplementationParameters["ContentDatabaseTemplateName"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: ContentDatabaseTemplateName", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("ContentDatabaseMaxTresholdSize") || this.dalConfiguration.ImplementationParameters["ContentDatabaseMaxTresholdSize"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: ContentDatabaseMaxTresholdSize", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("SiteCollectionQuotaTemplate") || this.dalConfiguration.ImplementationParameters["SiteCollectionQuotaTemplate"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: SiteCollectionQuotaTemplate", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("ContentDatabaseMaxSiteCollections") || this.dalConfiguration.ImplementationParameters["ContentDatabaseMaxSiteCollections"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: ContentDatabaseMaxSiteCollections", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("ContentDatabaseWarningSiteCollections") || this.dalConfiguration.ImplementationParameters["ContentDatabaseWarningSiteCollections"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: EngagementDatabaseWarningSiteCollections", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("EngagementProjectSiteTemplateName") || this.dalConfiguration.ImplementationParameters["EngagementProjectSiteTemplateName"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: EngagementProjectSiteTemplateName", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("EngagementProjectSiteTemplateLanguage") || this.dalConfiguration.ImplementationParameters["EngagementProjectSiteTemplateLanguage"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: EngagementProjectSiteTemplateLanguage", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("EngagementDeliverLibraryName") || this.dalConfiguration.ImplementationParameters["EngagementDeliverLibraryName"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: EngagementDeliverLibraryName", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("EngagementCloseLibraryName") || this.dalConfiguration.ImplementationParameters["EngagementCloseLibraryName"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: EngagementCloseLibraryName", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("OpportunityInitiateLibraryName") || this.dalConfiguration.ImplementationParameters["OpportunityInitiateLibraryName"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: OpportunityInitiateLibraryName", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("OpportunityPlanLibraryName") || this.dalConfiguration.ImplementationParameters["OpportunityPlanLibraryName"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: OpportunityPlanLibraryName", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("EngagementsDbConnectionString"))
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: EngagementsDbConnectionString", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("InternalProjectSiteTemplateName") || this.dalConfiguration.ImplementationParameters["InternalProjectSiteTemplateName"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: InternalProjectSiteTemplateName", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                if (!this.dalConfiguration.ImplementationParameters.KeyExists("InternalProjectSiteTemplateLanguage") || this.dalConfiguration.ImplementationParameters["InternalProjectSiteTemplateLanguage"].Value == string.Empty)
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: InternalProjectSiteTemplateLanguage", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                this.rootSiteCollection = this.dalConfiguration.ImplementationParameters["RootSiteCollection"].Value;
                this.searchServiceApplicationProxy = this.dalConfiguration.ImplementationParameters["SearchServiceApplicationProxy"].Value;
                this.defaultProjectSitePath = this.dalConfiguration.ImplementationParameters["DefaultProjectSitePath"].Value;
                this.defaultSiteOwner = this.dalConfiguration.ImplementationParameters["DefaultSiteOwner"].Value;
                this.projectsTxonomyGroupName = this.dalConfiguration.ImplementationParameters["ProjectsTxonomyGroupName"].Value;
                this.folderStructuresTermSetName = this.dalConfiguration.ImplementationParameters["FolderStructuresTermSetName"].Value;
                this.contentDatabaseTemplateName = this.dalConfiguration.ImplementationParameters["ContentDatabaseTemplateName"].Value;
                this.engagementProjectSiteTemplateName = this.dalConfiguration.ImplementationParameters["EngagementProjectSiteTemplateName"].Value;
                this.opportunityPlanLibraryName = this.dalConfiguration.ImplementationParameters["OpportunityPlanLibraryName"].Value;
                this.opportunityInitiateLibraryName = this.dalConfiguration.ImplementationParameters["OpportunityInitiateLibraryName"].Value;
                this.engagementDeliverLibraryName = this.dalConfiguration.ImplementationParameters["EngagementDeliverLibraryName"].Value;
                this.engagementCloseLibraryName = this.dalConfiguration.ImplementationParameters["EngagementCloseLibraryName"].Value;
                this.engagementsDbConnectionString = this.dalConfiguration.ImplementationParameters["EngagementsDbConnectionString"].Value;
                this.internalProjectSiteTemplateName = this.dalConfiguration.ImplementationParameters["InternalProjectSiteTemplateName"].Value;

                // Now retrieve the language code for newly created site engagement collections and convert it to uint
                string languageAsString = this.dalConfiguration.ImplementationParameters["EngagementProjectSiteTemplateLanguage"].Value;
                uint language = 0;
                bool languageOk = uint.TryParse(languageAsString, out language);

                if (languageOk)
                {
                    this.engagementProjectSiteTemplateLanguage = language;
                }
                else
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or inproperly set: EngagementProjectSiteTemplateLanguage", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                // Now retrieve the language code for newly created internal site collections and convert it to uint
                string interanlLanguageAsString = this.dalConfiguration.ImplementationParameters["InternalProjectSiteTemplateLanguage"].Value;
                uint internalLanguage = 0;
                bool internalLanguageOk = uint.TryParse(interanlLanguageAsString, out internalLanguage);

                if (internalLanguageOk)
                {
                    this.internalProjectSiteTemplateLanguage = internalLanguage;
                }
                else
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or inproperly set: InternalProjectSiteTemplateLanguage", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                // Now retrieve the max db treshold size, and convert it to ulong
                string sizeAsString = this.dalConfiguration.ImplementationParameters["ContentDatabaseMaxTresholdSize"].Value;
                ulong tresholdSize = 0;
                bool sizeOk = ulong.TryParse(sizeAsString, out tresholdSize);
                
                if (sizeOk)
                {
                    this.contentDatabaseMaxTresholdSize = tresholdSize;
                }
                else
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or inproperly set: ContentDatabaseMaxTresholdSize", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                // Now retrieve the site collection quota template - max size for one site collection
                string scquotaAsString = this.dalConfiguration.ImplementationParameters["SiteCollectionQuotaTemplate"].Value;
                long scquotaSize = 0;
                bool quotaOk = long.TryParse(scquotaAsString, out scquotaSize);

                if (quotaOk)
                {
                    this.siteCollectionQuotaTemplate = scquotaSize;
                }
                else
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or inproperly set: SiteCollectionQuotaTemplate", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                // Now retrieve the max site collection per content db, and convert it to int
                string maxSiteColPerDbAsString = this.dalConfiguration.ImplementationParameters["ContentDatabaseMaxSiteCollections"].Value;
                int maxSiteColDb = 0;
                bool maxSiteColOk = int.TryParse(maxSiteColPerDbAsString, out maxSiteColDb);
                if (maxSiteColOk)
                {
                    this.contentDatabaseMaxSiteCollections = maxSiteColDb;
                }
                else
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or inproperly set: ContentDatabaseMaxSiteCollections", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                // Now retrieve the warning trashold for site collection per content db limit, and convert it to int
                string warningSiteColTresholdAsString = this.dalConfiguration.ImplementationParameters["ContentDatabaseWarningSiteCollections"].Value;
                int warningSiteColTreshold = 0;
                bool warningTrasholdOk = int.TryParse(warningSiteColTresholdAsString, out warningSiteColTreshold);
                if (warningTrasholdOk)
                {
                    this.contentDatabaseWarningSiteCollections = warningSiteColTreshold;
                }
                else
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or inproperly set: ContentDatabaseWarningSiteCollections", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                // check if the sharePoint context has been passed as a int (project Id), and convert it to the url
                this.CheckContext();
            }
            catch (Exception ex)
            {
                ExceptionManager.Manager.CatchException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "SharePoint Data Access Layer could not be initialized - parameters missing or empty: SearchServiceApplicationProxy", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
            }
        }

        /// <summary>
        /// Retrieves the dal configuration parameter.
        /// </summary>
        /// <param name="parameter">The parameter we want to retrieve.</param>
        /// <returns></returns>
        public string GetDalConfigurationParameter(string parameter)
        {
            try
            {
                if (!this.dalConfiguration.ImplementationParameters.KeyExists(parameter))
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.DalParametersNotInitialized, "Parameter could not be retreived - parameters missing or empty: " + parameter, null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }

                string value = this.dalConfiguration.ImplementationParameters[parameter].Value;

                return value;

            }
            catch (Exception ex)
            {
                throw new AcmeDalException(Enums.ExceptionCode.GenericDAL, "DAL parameter could not be retreived", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Checks the context. If context is integer, then it means that the context is ProjectId, and we need to create the string context out of it, based on the pattern below.
        /// If the context is null or empty string, use the root web as a context
        /// All other patterns (string, SPContext, SPSite, SPWeb, SPList, SPListItem - please look at the rules below)
        /// </summary>
        private void CheckContext()
        {
            if (this.sharepointContext == null)
            {
                this.sharepointContext = string.Empty;
            }

            if (this.sharepointContext is string)
            {
                if (string.IsNullOrEmpty((string)this.sharepointContext))
                {
                    this.sharepointContext = string.Format("{0}/;", this.rootSiteCollection);
                }
            }

            if (!(this.sharepointContext is string))
            {
                if (this.sharepointContext is int)
                {
                    this.sharepointContext = string.Format("{0}/{1}/{2}/;", this.rootSiteCollection, this.defaultProjectSitePath, this.sharepointContext);
                }
                else if (this.sharepointContext is SPContext)
                {
                    this.sharepointContext = ((SPContext)this.sharepointContext).Web;
                }
                else if (this.sharepointContext is SPSite)
                {
                    this.sharepointContext = ((SPSite)this.sharepointContext).RootWeb;
                }
                else if (this.sharepointContext is SPWeb)
                {
                    this.sharepointContext = (SPWeb)this.sharepointContext;
                }
                else if (this.sharepointContext is SPList)
                {
                    this.sharepointContext = ((SPList)this.sharepointContext).ParentWeb;
                }
                else if (this.sharepointContext is SPListItem)
                {
                    this.sharepointContext = ((SPListItem)this.sharepointContext).ParentList.ParentWeb;
                }
                else
                {
                    ExceptionManager.Manager.ThrowException(new AcmeDalException(Enums.ExceptionCode.GenericDAL, "Could not understand the Data Context", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
                }
            }
        }
    }
}
