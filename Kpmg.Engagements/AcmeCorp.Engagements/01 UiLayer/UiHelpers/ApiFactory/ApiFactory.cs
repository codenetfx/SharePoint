using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.Core;
using AcmeCorp.Engagements.EngagementsApi;
using Microsoft.SharePoint.Administration;

namespace AcmeCorp.Engagements.UiHelpers.ApiFactory
{
    /// <summary>
    /// Creates an AcmeCorp Engagements APi instance configured from Farm Property Bag
    /// </summary>
    public class ApiFactory
    {

        /// <summary>
        /// The propperty bag configuration helper
        /// </summary>
        private Acme.SharePointCore.Helpers.PropertyBagConfiguration propBagHelper;

        /// <summary>
        /// Instance of AcmeCorp Engagements API
        /// </summary>
        private EngagementsApi.Api api;

        /// <summary>
        /// Gets the Engagements.
        /// </summary>
        /// <value>
        /// The API.
        /// </value>
        public EngagementsApi.Api Api
        {
            get
            {
                return api;
            }
        }

        /// <summary>
        /// Initializes the API Factory
        /// </summary>
        /// <param name="propertyBagConfigurationContext">The property bag configuration context (object where the property bags for API configuration are stored).</param>
        /// <param name="apiDataContext">The API data context.</param>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException">0;Could not initialize API Factory</exception>
        public ApiFactory(object propertyBagConfigurationContext, object apiDataContext)
        {
            try
            {

                Acme.SharePointCore.Helpers.PropertyBagConfiguration.PropertyBagLevel propBagLevel = Acme.SharePointCore.Helpers.PropertyBagConfiguration.PropertyBagLevel.NotInitialized;
                if (propertyBagConfigurationContext is SPWeb) propBagLevel = Acme.SharePointCore.Helpers.PropertyBagConfiguration.PropertyBagLevel.Web;
                if (propertyBagConfigurationContext is SPSite) propBagLevel = Acme.SharePointCore.Helpers.PropertyBagConfiguration.PropertyBagLevel.Site;
                if (propertyBagConfigurationContext is SPWebApplication) propBagLevel = Acme.SharePointCore.Helpers.PropertyBagConfiguration.PropertyBagLevel.WebApplication;
                if (propertyBagConfigurationContext is SPFarm) propBagLevel = Acme.SharePointCore.Helpers.PropertyBagConfiguration.PropertyBagLevel.Farm;

                if (propBagLevel == null || propBagLevel == Acme.SharePointCore.Helpers.PropertyBagConfiguration.PropertyBagLevel.NotInitialized)
                {
                    throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Unknown context for Proeprty bag level", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
                }


                //get the property bag configuration helper
                propBagHelper = new Acme.SharePointCore.Helpers.PropertyBagConfiguration(propBagLevel, propertyBagConfigurationContext);

                //read the configurations
                Acme.Core.Logger.LoggingConfiguration loggingConfiguration = GetLoggerConfiguration();
                Acme.Core.Configuration.InterfaceConfigurationSection cachingConfiguration = GetCachingConfiguration();
                Acme.Core.Configuration.InterfaceConfigurationSection settingsConfiguration = GetSettingsConfiguration();
                Acme.Core.Localization.LocalizationConfiguration localizationConfiguration = GetLocalizationConfiguration();
                Acme.Core.Configuration.InterfaceConfigurationSection dalConfiguration = GetDalConfiguration();

                //create the api
                api = new Api(loggingConfiguration, localizationConfiguration, settingsConfiguration, cachingConfiguration, dalConfiguration, apiDataContext);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not initialize API Factory", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Gets the dal configuration.
        /// </summary>
        /// <returns>The DAL configuration</returns>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException">0;Could not initialize DAL configuration</exception>
        private Acme.Core.Configuration.InterfaceConfigurationSection GetDalConfiguration()
        {
            try
            {

                AcmeCorp.Engagements.EngagementsDomain.EngagementsDalFactory.DestroyInstance();

                Acme.Core.Configuration.InterfaceConfigurationSection dalConfiguration = new Acme.Core.Configuration.InterfaceConfigurationSection();

                Acme.Core.Configuration.InterfaceImplementationSection dalImplementation = new Acme.Core.Configuration.InterfaceImplementationSection();
                dalImplementation.Name = propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_InterfaceImplementation_name");
                dalImplementation.Type = propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_InterfaceImplementation_type");

                dalConfiguration.InterfaceImplementation = dalImplementation;

                Acme.Core.Configuration.ConfigParameterCollection parameters = new Acme.Core.Configuration.ConfigParameterCollection();
                parameters.AddNewParameter("DefaultSiteOwner", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_DefaultSiteOwner"));
                parameters.AddNewParameter("DefaultProjectSitePath", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_DefaultProjectSitePath"));
                parameters.AddNewParameter("RootSiteCollection", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_RootSiteCollection"));
                parameters.AddNewParameter("SearchServiceApplicationProxy", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_SearchServiceApplicationProxy"));
                parameters.AddNewParameter("ProjectsTxonomyGroupName", propBagHelper.GetStringProperty("PS_CONFIG_Acme_Settings_ImplementationParameters_TaxonomyGroupName"));
                parameters.AddNewParameter("FolderStructuresTermSetName", propBagHelper.GetStringProperty("PS_CONFIG_Acme_Settings_ImplementationParameters_TaxonomyTermSetName"));
                parameters.AddNewParameter("ContentDatabaseTemplateName", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_ContentDatabaseTemplateName"));
                parameters.AddNewParameter("ContentDatabaseMaxTresholdSize", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_ContentDatabaseMaxTresholdSize"));
                parameters.AddNewParameter("SiteCollectionQuotaTemplate", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_SiteCollectionQuotaTemplate"));
                parameters.AddNewParameter("ContentDatabaseMaxSiteCollections", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_ContentDatabaseMaxSiteCollections"));
                parameters.AddNewParameter("ContentDatabaseWarningSiteCollections", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_ContentDatabaseWarningSiteCollections"));
                parameters.AddNewParameter("EngagementProjectSiteTemplateName", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_EngagementProjectSiteTemplateName"));
                parameters.AddNewParameter("EngagementProjectSiteTemplateLanguage", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_EngagementProjectSiteTemplateLanguage"));
                parameters.AddNewParameter("InternalProjectSiteTemplateName", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_InternalProjectSiteTemplateName"));
                parameters.AddNewParameter("InternalProjectSiteTemplateLanguage", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_InternalProjectSiteTemplateLanguage"));
                parameters.AddNewParameter("EngagementDeliverLibraryName", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_EngagementDeliverLibraryName"));
                parameters.AddNewParameter("EngagementCloseLibraryName", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_EngagementCloseLibraryName"));
                parameters.AddNewParameter("OpportunityInitiateLibraryName", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_OpportunityInitiateLibraryName"));
                parameters.AddNewParameter("OpportunityPlanLibraryName", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_OpportunityPlanLibraryName"));
                parameters.AddNewParameter("EngagementsDbConnectionString", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_EngagementsDbConnectionString"));
                parameters.AddNewParameter("DeputiesGroupOu", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_DeputiesGroupOu"));
                parameters.AddNewParameter("DeputiesGroupPrefix", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_DeputiesGroupPrefix"));
                parameters.AddNewParameter("DeputiesOuContainer", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_DeputiesOuContainer"));
                parameters.AddNewParameter("UsersOuContainer", propBagHelper.GetStringProperty("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_UsersOuContainer"));

                dalConfiguration.ImplementationParameters = parameters;

                return dalConfiguration;
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not initialize DAL configuration", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Gets the settings configuration.
        /// </summary>
        /// <returns>
        /// Settings configuration
        /// </returns>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException">0;Could not initialize Settings configuration</exception>
        private Acme.Core.Configuration.InterfaceConfigurationSection GetSettingsConfiguration()
        {
            try
            {
                Acme.Core.Settings.SettingsFactory.DestroyInstance();

                Acme.Core.Configuration.InterfaceConfigurationSection settingsConfiguration = new Acme.Core.Configuration.InterfaceConfigurationSection();

                Acme.Core.Configuration.InterfaceImplementationSection settingsImplementation = new Acme.Core.Configuration.InterfaceImplementationSection();
                settingsImplementation.Name = propBagHelper.GetStringProperty("PS_CONFIG_Acme_Settings_InterfaceImplementation_name");
                settingsImplementation.Type = propBagHelper.GetStringProperty("PS_CONFIG_Acme_Settings_InterfaceImplementation_type");

                settingsConfiguration.InterfaceImplementation = settingsImplementation;

                return settingsConfiguration;
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not initialize Settings configuration", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Gets the logger configuration.
        /// </summary>
        /// <returns>
        /// Logger configuration
        /// </returns>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException">0;Could not initialize Logger configuration</exception>
        private Acme.Core.Logger.LoggingConfiguration GetLoggerConfiguration()
        {
            try
            {
                Acme.Core.Logger.LogFactory.DestroyInstance();

                Acme.Core.Logger.LoggingConfiguration loggerConfiguration;

                loggerConfiguration = new Acme.Core.Logger.LoggingConfiguration();

                Acme.Core.Configuration.InterfaceImplementationSection implementation = new Acme.Core.Configuration.InterfaceImplementationSection();
                implementation.Name = propBagHelper.GetStringProperty("PS_CONFIG_Acme_Logger_InterfaceImplementation_name");
                implementation.Type = propBagHelper.GetStringProperty("PS_CONFIG_Acme_Logger_InterfaceImplementation_type");

                Acme.Core.Logger.LoggingSettings settings = new Acme.Core.Logger.LoggingSettings();
                settings.EventId = propBagHelper.GetStringProperty("PS_CONFIG_Acme_Logger_LoggingSettings_eventId");
                settings.InnerExceptions = propBagHelper.GetStringProperty("PS_CONFIG_Acme_Logger_LoggingSettings_innerExceptions");
                settings.TraceLevel = propBagHelper.GetStringProperty("PS_CONFIG_Acme_Logger_LoggingSettings_traceLevel");

                Acme.Core.Configuration.ConfigParameterCollection parameters = new Acme.Core.Configuration.ConfigParameterCollection();
                parameters.AddNewParameter("MyDummyParamName1", "MyDummyParamValue1");
                parameters.AddNewParameter("MyDummyParamName2", "MyDummyParamValue2");

                loggerConfiguration.LoggingSettings = settings;
                loggerConfiguration.InterfaceImplementation = implementation;
                loggerConfiguration.ImplementationParameters = parameters;

                return loggerConfiguration;
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not initialize Logger configuration", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Gets the localization configuration.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException">0;Could not initialize Localization configuration</exception>
        private Acme.Core.Localization.LocalizationConfiguration GetLocalizationConfiguration()
        {
            try
            {
                Acme.Core.Localization.LocalizationFactory.DestroyInstance();

                Acme.Core.Localization.LocalizationConfiguration localizationConfiguration;

                localizationConfiguration = new Acme.Core.Localization.LocalizationConfiguration();

                Acme.Core.Configuration.InterfaceImplementationSection implementation = new Acme.Core.Configuration.InterfaceImplementationSection();
                implementation.Name = propBagHelper.GetStringProperty("PS_CONFIG_Acme_Localization_InterfaceImplementation_name");
                implementation.Type = propBagHelper.GetStringProperty("PS_CONFIG_Acme_Localization_InterfaceImplementation_type");

                Acme.Core.Localization.LocalizationSettings settings = new Acme.Core.Localization.LocalizationSettings();
                settings.CreateMissingKeys = propBagHelper.GetBoolProperty("PS_CONFIG_Acme_Localization_LocalizationSettings_createMissingKeys");
                settings.DefaultLanguage = propBagHelper.GetStringProperty("PS_CONFIG_Acme_Localization_LocalizationSettings_defaultLanguage");
                settings.FallbackLanguage = propBagHelper.GetStringProperty("PS_CONFIG_Acme_Localization_LocalizationSettings_fallbackLanguage");
                settings.FallbackToKey = propBagHelper.GetBoolProperty("PS_CONFIG_Acme_Localization_LocalizationSettings_fallbackToKey");
                settings.ImplementFallback = propBagHelper.GetBoolProperty("PS_CONFIG_Acme_Localization_LocalizationSettings_implementFallback");

                Acme.Core.Configuration.ConfigParameterCollection parameters = new Acme.Core.Configuration.ConfigParameterCollection();
                parameters.AddNewParameter("DbConnection", propBagHelper.GetStringProperty("PS_CONFIG_Acme_Localization_ImplementationParameters_DbConnectionString"));

                localizationConfiguration.LocalizationSettings = settings;
                localizationConfiguration.InterfaceImplementation = implementation;
                localizationConfiguration.ImplementationParameters = parameters;

                return localizationConfiguration;
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not initialize Localization configuration", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        //Caching
        /// <summary>
        /// Gets the caching configuration.
        /// </summary>
        /// <returns>
        /// Settings configuration
        /// </returns>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException">0;Could not initialize Caching configuration</exception>
        private Acme.Core.Configuration.InterfaceConfigurationSection GetCachingConfiguration()
        {
            try
            {
                Acme.Core.Caching.CacheFactory.DestroyInstance();

                Acme.Core.Configuration.InterfaceConfigurationSection cachingConfiguration = new Acme.Core.Configuration.InterfaceConfigurationSection();

                Acme.Core.Configuration.InterfaceImplementationSection cachingImplementation = new Acme.Core.Configuration.InterfaceImplementationSection();
                cachingImplementation.Name = propBagHelper.GetStringProperty("PS_CONFIG_Acme_Caching_InterfaceImplementation_name");
                cachingImplementation.Type = propBagHelper.GetStringProperty("PS_CONFIG_Acme_Caching_InterfaceImplementation_type");

                Acme.Core.Configuration.ConfigParameterCollection parameters = new Acme.Core.Configuration.ConfigParameterCollection();
                parameters.AddNewParameter("CacheName", propBagHelper.GetStringProperty("PS_CONFIG_Acme_Caching_ImplementationParameters_CacheName"));
                parameters.AddNewParameter("NumberOfServers", propBagHelper.GetStringProperty("PS_CONFIG_Acme_Caching_ImplementationParameters_NumberOfServers"));
                parameters.AddNewParameter("CacheServer1", propBagHelper.GetStringProperty("PS_CONFIG_Acme_Caching_ImplementationParameters_CacheServer1"));
                parameters.AddNewParameter("DataCacheSecurityMode", propBagHelper.GetStringProperty("PS_CONFIG_Acme_Caching_ImplementationParameters_DataCacheSecurityMode"));
                parameters.AddNewParameter("DataCacheProtectionLevel", propBagHelper.GetStringProperty("PS_CONFIG_Acme_Caching_ImplementationParameters_DataCacheProtectionLevel"));

                cachingConfiguration.ImplementationParameters = parameters;

                cachingConfiguration.InterfaceImplementation = cachingImplementation;

                return cachingConfiguration;
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not initialize Caching configuration", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

    }
}
