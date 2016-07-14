using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Security.Cryptography;

namespace AcmeCorp.Engagements.FarmConfiguration.Features.PropertyBagSettings
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>
    [Guid("e27a5014-a70f-4969-8239-a1efdcf27b15")]
    public class PropertyBagSettingsEventReceiver : SPFeatureReceiver
    {

        /// <summary>
        /// The configuration settings
        /// </summary>
        Dictionary<string, string> configurationSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBagSettingsEventReceiver"/> class.
        /// </summary>
        public PropertyBagSettingsEventReceiver()
        {
            //create default set of the ProjectSpace configuration properties which are to be stored in the farm property bags
            configurationSettings = new Dictionary<string, string>();

            //logger
            configurationSettings.Add("PS_CONFIG_Acme_Logger_InterfaceImplementation_name", "Mock Logger");
            configurationSettings.Add("PS_CONFIG_Acme_Logger_InterfaceImplementation_type", "Acme.Core.MockImplementations.MockLogger, Acme.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=057a447e7ed7d0fb");
            configurationSettings.Add("PS_CONFIG_Acme_Logger_LoggingSettings_traceLevel", "Verbose");
            configurationSettings.Add("PS_CONFIG_Acme_Logger_LoggingSettings_eventId", "1-9999");
            configurationSettings.Add("PS_CONFIG_Acme_Logger_LoggingSettings_innerExceptions", "All");

            configurationSettings.Add("PS_CONFIG_Acme_Logger_ImplementationParameters_Area", "ProjectSpace");

            // localization
            configurationSettings.Add("PS_CONFIG_Acme_Localization_InterfaceImplementation_name", "SQL Db localization");
            configurationSettings.Add("PS_CONFIG_Acme_Localization_InterfaceImplementation_type", "Acme.Core.MultiLanguageDb.MultiLanguageDb, Acme.Core.MultiLanguageDb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=057a447e7ed7d0fb");

            configurationSettings.Add("PS_CONFIG_Acme_Localization_LocalizationSettings_defaultLanguage", "ENG");
            configurationSettings.Add("PS_CONFIG_Acme_Localization_LocalizationSettings_implementFallback", "false");
            configurationSettings.Add("PS_CONFIG_Acme_Localization_LocalizationSettings_fallbackLanguage", "ENG");
            configurationSettings.Add("PS_CONFIG_Acme_Localization_LocalizationSettings_fallbackToKey", "false");
            configurationSettings.Add("PS_CONFIG_Acme_Localization_LocalizationSettings_createMissingKeys", "false");

            configurationSettings.Add("PS_CONFIG_Acme_Localization_ImplementationParameters_DbConnectionString", "metadata=res://*/MultiLanguageEntities.csdl|res://*/MultiLanguageEntities.ssdl|res://*/MultiLanguageEntities.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=localhost;initial catalog=MultiLanguage;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;");

            //settings
            configurationSettings.Add("PS_CONFIG_Acme_Settings_InterfaceImplementation_name", "Mock settings");
            configurationSettings.Add("PS_CONFIG_Acme_Settings_InterfaceImplementation_type", "Acme.Core.MockImplementations.MockSettings, Acme.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=057a447e7ed7d0fb");
            configurationSettings.Add("PS_CONFIG_Acme_Settings_ImplementationParameters_TaxonomyGroupName", "AcmeCorpEngagements");
            configurationSettings.Add("PS_CONFIG_Acme_Settings_ImplementationParameters_TaxonomyTermSetName", "Settings");

            //caching
            configurationSettings.Add("PS_CONFIG_Acme_Caching_InterfaceImplementation_name", "Mock Caching");
            configurationSettings.Add("PS_CONFIG_Acme_Caching_InterfaceImplementation_type", "Acme.Core.MockImplementations.MockCaching, Acme.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=057a447e7ed7d0fb");
            configurationSettings.Add("PS_CONFIG_Acme_Caching_ImplementationParameters_CacheName", "default");
            configurationSettings.Add("PS_CONFIG_Acme_Caching_ImplementationParameters_NumberOfServers", "1");
            configurationSettings.Add("PS_CONFIG_Acme_Caching_ImplementationParameters_CacheServer1", "WIN-H1G8DUH8PUT;22233");
            configurationSettings.Add("PS_CONFIG_Acme_Caching_ImplementationParameters_DataCacheSecurityMode", "Transport");
            configurationSettings.Add("PS_CONFIG_Acme_Caching_ImplementationParameters_DataCacheProtectionLevel", "EncryptAndSign");
            //  configurationSettings.Add("Acme_Caching_ImplementationParameters_CacheServer2", "TESTSERVER;12345");

            // AcmeCorp ProjectSpace DAL Configuration
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_InterfaceImplementation_name", "SharePoint DAL Implementation");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_InterfaceImplementation_type", "AcmeCorp.Engagements.EngagementsSharePointDataAccess.EngagementsSharePointDal, AcmeCorp.Engagements.EngagementsSharePointDataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=76ab1eb817808800");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_DefaultSiteOwner", "SPFARM\\Administrator");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_DefaultProjectSitePath", "engagements");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_RootSiteCollection", "https://appsrv1");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_SearchServiceApplicationProxy", "FAST Query SSA");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_ProjectsTxonomyGroupName", "AcmeCorpEngagements");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_FolderStructuresTermSetName", "Folder Structure Templates");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_ContentDatabaseTemplateName", "T_SPCONTENT_PORTAL_");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_ContentDatabaseMaxTresholdSize", "19327352832");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_SiteCollectionQuotaTemplate", "10737418240");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_ContentDatabaseMaxSiteCollections", "20");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_ContentDatabaseWarningSiteCollections", "15");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_EngagementProjectSiteTemplateName", "EngagementsSiteDef#0");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_EngagementProjectSiteTemplateLanguage", "1033");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_InternalProjectSiteTemplateName", "STS#0");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_InternalProjectSiteTemplateLanguage", "1033");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_EngagementDeliverLibraryName", "03 Deliver");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_EngagementCloseLibraryName", "04 Close");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_OpportunityInitiateLibraryName", "Documents");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_OpportunityPlanLibraryName", "Documents");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_EngagementsDbConnectionString", "Data Source=APPSRV1\\MSSQLSP;Initial Catalog=EngagementsDb;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_DeputiesGroupOu", "EngagementDeputies");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_DeputiesGroupPrefix", "Deputies");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_DeputiesOuContainer", "OU=AcmeCorp,DC=spfarm,DC=local");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_UsersOuContainer", "CN=Users,DC=spfarm,DC=local");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_ServiceBusConnectionString", "Endpoint=sb://appsrv1.spfarm.local/WorkflowDefaultNamespace;StsEndpoint=https://appsrv1.spfarm.local:9355/WorkflowDefaultNamespace;RuntimePort=9354;ManagementPort=9355");
            configurationSettings.Add("PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_ServiceBusQueueName", "EngagementsServiceCalls");

            //<add name="DeputiesGroupOu" value="EngagementDeputies" />
            //<add name="DeputiesGroupPrefix" value="Deputies" />
            //<add name="DeputiesOuContainer" value="OU=AcmeCorp,DC=spfarm,DC=local" />
            //<add name="UsersOuContainer" value="CN=Users,DC=spfarm,DC=local" />

        }

        /// <summary>
        /// Occurs after a Feature is activated.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties" /> object that represents the properties of the event.</param>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            TripleDESCryptoServiceProvider tDESalg = new TripleDESCryptoServiceProvider();

            byte[] key = tDESalg.Key;
            string sKey = string.Empty;
            byte[] iv = tDESalg.IV;
            string sIV = string.Empty;
            SPFarm farm;
            farm = SPFarm.Local;
            for (int i = 1; i < 25; i++)
            {
                sKey += key[i - 1].ToString();
                if (i != 24)
                {
                    sKey += "-";
                }
            }

            if (!farm.Properties.ContainsKey("pbs_secret_key"))
            {
                farm.Properties.Add("pbs_secret_key", sKey);
                farm.Update();
            }
            for (int i = 1; i < 9; i++)
            {
                sIV += iv[i - 1].ToString();
                if (i != 8)
                {
                    sIV += "-";
                }

            }
            if (!farm.Properties.ContainsKey("pbs_initialization_vector"))
            {
                farm.Properties.Add("pbs_initialization_vector", sIV);
                farm.Update();
            }

            AddFarmPropertiesFromConfig(configurationSettings, farm);
        }

        /// <summary>
        /// Adds the farm properties from config.
        /// </summary>
        /// <param name="configurationSettings">The configuration settings.</param>
        /// <param name="farm">The farm.</param>
        private void AddFarmPropertiesFromConfig(Dictionary<string, string> configurationSettings, SPFarm farm)
        {
            foreach (KeyValuePair<string, string> item in configurationSettings)
            {
                if (!farm.Properties.ContainsKey(item.Key))
                {
                    farm.Properties.Add(item.Key, item.Value);
                    farm.Update();
                }
            }
        }

        /// <summary>
        /// Occurs when a Feature is deactivated.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties" /> object that represents the properties of the event.</param>
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            //Do nothing if the Property Bag Settings feature is deactivated 

        }

        /// <summary>
        /// Occurs after a Feature is installed.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties" /> object that represents the properties of the event.</param>
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
        }

        /// <summary>
        /// Occurs when a Feature is uninstalled.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties" /> object that represents the properties of the event.</param>
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
        }
    }
}
