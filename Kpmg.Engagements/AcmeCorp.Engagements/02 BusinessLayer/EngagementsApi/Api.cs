// -----------------------------------------------------------------------
// <copyright file="Api.cs" company="AcmeCorp">
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
    using AcmeCorp.Engagements.EngagementsDomain;
    using Acme.Core;
    using Acme.Core.DiagnosticSystem.Enums;
    using Acme.Core.DiagnosticSystem.ExceptionEntities;
    using Acme.Core.DiagnosticSystem.ExceptionManager;

    /// <summary>
    /// Initializes engagements API
    /// </summary>
    public partial class Api
    {
        #region Private members
        /// <summary>
        /// Data Layer implementation 
        /// </summary>
        private IAcmeCorpEngagementsDalInterface dataLayer;

        /// <summary>
        /// Logger implementation
        /// </summary>
        private Acme.Core.Logger.ILogger logger;

        /// <summary>
        /// Settings implementation
        /// </summary>
        private Acme.Core.Settings.ISettings settings;

        /// <summary>
        /// Localization implementation
        /// </summary>
        private Acme.Core.Localization.ILocalization localization;

        /// <summary>
        /// Caching implementation
        /// </summary>
        private Acme.Core.Caching.ICaching caching;

        /// <summary>
        /// Data Access Layer Context
        /// </summary>
        private object dalContext;

        /// <summary>
        /// OU in the AD where the deputy groups are stored
        /// </summary>
        private string adDeputyGroupsOu;

        /// <summary>
        /// Prefix for Deputies group names in AD
        /// </summary>
        private string adDeputiesGroupPrefix;

        /// <summary>
        /// OU Container in the AD where Deputy Groups OU will be stored.
        /// </summary>
        private string adDeputiesOuContainer;

        /// <summary>
        /// Container in the AD where usernames are stored
        /// </summary>
        private string adUsersOuContainer;

        #endregion

        #region Public properties
        /// <summary>
        /// Gets the localization implementation
        /// </summary>
        /// <value>
        /// The localization.
        /// </value>
        public Acme.Core.Localization.ILocalization Localization
        {
            get
            {
                return this.localization;
            }
        }

        /// <summary>
        /// Gets OU in the AD where the deputy groups are stored
        /// </summary>
        /// <value>
        /// The OU name
        /// </value>
        public string AdDeputyGroupsOu
        {
            get
            {
                return this.adDeputyGroupsOu;
            }
        }

        /// <summary>
        /// Gets prefix for names of AD groups in the OU
        /// </summary>
        /// <value>
        /// The ad deputies group prefix.
        /// </value>
        public string AdDeputiesGroupPrefix
        {
            get
            {
                return this.adDeputiesGroupPrefix;
            }
        }

        /// <summary>
        /// Gets the Container in the OU where Deputy Groups container will be stored
        /// </summary>
        /// <value>
        /// The AD deputies OU container.
        /// </value>
        public string AdDeputiesOuContainer
        {
            get
            {
                return this.adDeputiesOuContainer;
            }
        }

        /// <summary>
        /// Gets the container in the AD where users are stored.
        /// </summary>
        /// <value>
        /// The AD users OU container.
        /// </value>
        public string AdUsersOuContainer 
        { 
            get 
            { 
                return this.adUsersOuContainer;
            } 
        }
        #endregion

        #region Constructor and configuration

        /// <summary>
        /// Initializes a new instance of the <see cref="Api"/> class (AcmeCorp Engagements API), with the configuration objects which are set through the config files
        /// </summary>
        /// <param name="dalContext">
        /// The context, usually needed by a data access layer, passed from the UI layer. Set it to null if the DAL does not need the context.
        /// Possible DAL types are
        /// SPWeb - passing the SharePoint Context from the UI
        /// string - creating the DAL context from the site (site collection) and web (site) URLs, separated by semicolon, i.e. "http://server;MySiteName" 
        /// integer - creating the DAL context from the project number
        /// </param>
        public Api(object dalContext)
        {
            try
            {
                this.dalContext = dalContext;

                // Create interface implementation instances on the API initialization
                this.CreateInterfaceImplementationInstances();

                Acme.Core.DiagnosticSystem.ExceptionManager.ExceptionManager.Manager.SetDefaultLogger(this.logger);
            }
            catch (Exception ex)
            {
                ExceptionManager.Manager.CatchException(new AcmeApiException(Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionCode.FailedToInitializeApi, "Error has occured when trying to initialize the Api", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Api"/> class, with the manually set configuration of interface implementations.
        /// </summary>
        /// <param name="loggingConfiguration">The logging configuration.</param>
        /// <param name="localizationConfiguration">The localization configuration.</param>
        /// <param name="settingsConfiguration">The settings configuration.</param>
        /// <param name="cachingConfiguration">The caching configuration.</param>
        /// <param name="dataAccessLayerConfiguration">The data access layer configuration.</param>
        /// <param name="dalContext">The context, usually needed by a data access layer, passed from the UI layer. Set it to null if the DAL does not need the context</param>
        public Api(
            Acme.Core.Logger.LoggingConfiguration loggingConfiguration,
            Acme.Core.Localization.LocalizationConfiguration localizationConfiguration,
            Acme.Core.Configuration.InterfaceConfigurationSection settingsConfiguration,
            Acme.Core.Configuration.InterfaceConfigurationSection cachingConfiguration,
            Acme.Core.Configuration.InterfaceConfigurationSection dataAccessLayerConfiguration,
            object dalContext)
        {
            try
            {
                // Set the context
                this.dalContext = dalContext;

                this.CreateInterfaceImplementationInstances(loggingConfiguration, localizationConfiguration, settingsConfiguration, cachingConfiguration, dataAccessLayerConfiguration);

                Acme.Core.DiagnosticSystem.ExceptionManager.ExceptionManager.Manager.SetDefaultLogger(this.logger);
            }
            finally
            {
                Acme.Core.DiagnosticSystem.ExceptionManager.ExceptionManager.Manager.ProcessExceptions();
            }
        }
        #endregion

        #region Private methods - configuration and interfaces
        /// <summary>
        /// Creates the interface implementation instances from known, manually set configuration
        /// </summary>
        /// <param name="loggingConfiguration">The logging configuration.</param>
        /// <param name="localizationConfiguration">The localization configuration.</param>
        /// <param name="settingsConfiguration">The settings configuration.</param>
        /// <param name="cachingConfiguration">The caching configuration.</param>
        /// <param name="dataAccessLayerConfiguration">The data access layer configuration.</param>
        private void CreateInterfaceImplementationInstances(Acme.Core.Logger.LoggingConfiguration loggingConfiguration, Acme.Core.Localization.LocalizationConfiguration localizationConfiguration, Acme.Core.Configuration.InterfaceConfigurationSection settingsConfiguration, Acme.Core.Configuration.InterfaceConfigurationSection cachingConfiguration, Acme.Core.Configuration.InterfaceConfigurationSection dataAccessLayerConfiguration)
        {
            try
            {
                // Create DAL instance
                AcmeCorp.Engagements.EngagementsDomain.EngagementsDalFactory.DestroyInstance();
                this.dataLayer = AcmeCorp.Engagements.EngagementsDomain.EngagementsDalFactory.CreateInstance(dataAccessLayerConfiguration, this.dalContext);

                // It might happen that the dataLayer corrects the data context on some way, so use the corrected data context
                // This is possible for example with SharePoint data layer, and passing integer (project Id) as a context - we need to resolve the project site path 
                // as a context out of project id
                this.dalContext = this.dataLayer.DataContext;

                // Create Logging instance
                Acme.Core.Logger.LogFactory.DestroyInstance();
                this.logger = Acme.Core.Logger.LogFactory.CreateInstance(loggingConfiguration, this.dalContext);

                // Create Settings instance
                Acme.Core.Settings.SettingsFactory.DestroyInstance();
                this.settings = Acme.Core.Settings.SettingsFactory.CreateInstance(settingsConfiguration, this.dalContext);

                // Create Caching instance
                Acme.Core.Caching.CacheFactory.DestroyInstance();
                this.caching = Acme.Core.Caching.CacheFactory.CreateInstance(cachingConfiguration);

                // Create localization instance
                Acme.Core.Localization.LocalizationFactory.DestroyInstance();
                this.localization = Acme.Core.Localization.LocalizationFactory.CreateInstance(localizationConfiguration);

                // And on the end set the different interface implementations also to different intefaces, 
                // since they need to speak to each other (Data access layer and localization both need logging and caching)
                this.SetConfigurationInInterfaceImplementations();
            }
            catch (Exception ex)
            {
                ExceptionManager.Manager.CatchException(new AcmeApiException(Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionCode.FailedToInstantiateInterfaces, "Error has occured when trying to instantiate the interfaces the Api", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
            }
        }

        /// <summary>
        /// Creates the interface implementation instances from configuration set through the config files (app.config, web.config)
        /// </summary>
        private void CreateInterfaceImplementationInstances()
        {
            try
            {
                // Create DAL instance
                AcmeCorp.Engagements.EngagementsDomain.EngagementsDalFactory.DestroyInstance();
                this.dataLayer = AcmeCorp.Engagements.EngagementsDomain.EngagementsDalFactory.CreateInstance(this.dalContext);

                // It might happen that the dataLayer corrects the data context on some way, so use the corrected data context
                // This is possible for example with SharePoint data layer, and passing integer (project Id) as a context - we need to resolve the project site path 
                // as a context out of project id
                this.dalContext = this.dataLayer.DataContext;

                // Create Logging instance
                Acme.Core.Logger.LogFactory.DestroyInstance();
                this.logger = Acme.Core.Logger.LogFactory.CreateInstance(this.dalContext);

                // Create Settings instance
                Acme.Core.Settings.SettingsFactory.DestroyInstance();
                this.settings = Acme.Core.Settings.SettingsFactory.CreateInstance(this.dalContext);

                // Create Caching instance
                Acme.Core.Caching.CacheFactory.DestroyInstance();
                this.caching = Acme.Core.Caching.CacheFactory.CreateInstance();

                // Create localization instance
                Acme.Core.Localization.LocalizationFactory.DestroyInstance();
                this.localization = Acme.Core.Localization.LocalizationFactory.CreateInstance();

                // And on the end set the different interface implementations also to different intefaces, 
                // since they need to speak to each other (Data access layer and localization both need logging and caching)
                this.SetConfigurationInInterfaceImplementations();
            }
            catch (Exception ex)
            {
                ExceptionManager.Manager.CatchException(new AcmeApiException(Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionCode.FailedToInstantiateInterfaces, "Error has occured when trying to instantiate the interfaces the Api", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical));
            }
        }

        /// <summary>
        /// Sets the configuration in interface implementations.
        /// </summary>
        private void SetConfigurationInInterfaceImplementations()
        {
            this.caching.Logger = this.logger;

            this.localization.Cache = this.caching;
            this.localization.Logger = this.logger;

            this.settings.Logger = this.logger;
            this.settings.Cache = this.caching;

            this.dataLayer.Cache = this.caching;
            this.dataLayer.Logger = this.logger;
            this.dataLayer.Settings = this.settings;

            // set the API (BL) Only paramerets
            this.adDeputyGroupsOu = this.dataLayer.GetDalConfigurationParameter("DeputiesGroupOu");
            this.adDeputiesGroupPrefix = this.dataLayer.GetDalConfigurationParameter("DeputiesGroupPrefix");
            this.adDeputiesOuContainer = this.dataLayer.GetDalConfigurationParameter("DeputiesOuContainer");
            this.adUsersOuContainer = this.dataLayer.GetDalConfigurationParameter("UsersOuContainer");
        }
        #endregion
    }
}
