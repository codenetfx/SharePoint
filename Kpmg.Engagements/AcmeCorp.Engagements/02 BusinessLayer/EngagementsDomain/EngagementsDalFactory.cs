// -----------------------------------------------------------------------
// <copyright file="EngagementsDalFactory.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsDomain
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Factory class for Engagements DAL
    /// </summary>
    public class EngagementsDalFactory
    {
        #region Example DAL config in .config file
        /*
        <configuration>
              <configSections>
                <sectionGroup name="Acme">
                  <section name="Settings" type="Acme.Core.Configuration.InterfaceConfigurationSection, Acme.Core"></section>
                  <!-- This is for app.config -->
                  <section name="Settings" type="Acme.Core.Configuration.InterfaceConfigurationSection, Acme.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd40f1f0005062a"></section>
                  <!-- This is for web.config -->
                </sectionGroup>
              </configSections>

              <Acme>
                <Settings>
                  <InterfaceImplementation
                    name="Mock Settings from App.Config"
                    type="Acme.Core.MockImplementations.MockSettings, Acme.Core" />
                  <!-- This is the example for the app.config, in web.config fully qualified name with Version, Culture and Token must be used -->
                </Settings>
              </Acme>
            </configuration>
        */
        #endregion

        /// <summary>
        /// Private variable which holds the DAL implementation instance
        /// </summary>
        private static IAcmeCorpEngagementsDalInterface dalInstance;

        /// <summary>
        /// Gets the DAL implementation instance
        /// </summary>
        private static IAcmeCorpEngagementsDalInterface Instance
        {
            get
            {
                return dalInstance;
            }
        }

        /// <summary>
        /// Create an instance of IArgoProjectsDalInterface, based on the config file (app.config, web.config)
        /// </summary>
        /// <param name="dalContext">The context, usually needed by a data access layer, passed from the UI layer. Set it to null if the DAL does not need the context</param>
        /// <returns>Implementation of IArgoProjectsDalInterface instance</returns>
        public static IAcmeCorpEngagementsDalInterface CreateInstance(object dalContext)
        {
            // If a DAL instance has already been created, just return the instance, we can not have more than one DAL instance running per API
            if (dalInstance != null)
            {
                return dalInstance;
            }

            object configurationSection = ConfigurationManager.GetSection("AcmeCorp/DataAccessLayer");
            Acme.Core.Configuration.InterfaceConfigurationSection dalConfigurationSection = (Acme.Core.Configuration.InterfaceConfigurationSection)configurationSection;

            if (dalConfigurationSection != null)
            {
                return CreateInstance(dalConfigurationSection, dalContext);
            }
            else
            {
                // DAL configuration is not loaded
                throw Acme.Core.DiagnosticSystem.ExceptionFactory.ExceptionFactory.CreateException(Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionCode.CannotCreateInstance, "Could not read or understand the Data Access Layer configuration from the configuration file. Data Access Layer implementation has not been created.", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical, Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionType.AcmeDalException);
            }
        }

        /// <summary>
        /// Creates an instance of IArgoProjectsDalInterface implementation, based on the InterfaceConfigurationSection parameter
        /// </summary>
        /// <param name="configurationSection">DataAccessLayer Configuration section</param>
        /// <param name="dalContext">The context, usually needed by a data access layer, passed from the UI layer. Set it to null if the DAL does not need the context</param>
        /// <returns>Implementation of IArgoProjectsDalInterface instance</returns>
        public static IAcmeCorpEngagementsDalInterface CreateInstance(Acme.Core.Configuration.InterfaceConfigurationSection configurationSection, object dalContext)
        {
            try
            {
                // If a DAL instance has already been created, just return the instance, we can not have more than one DAL instance running per API
                if (dalInstance != null)
                {
                    return dalInstance;
                }

                if (configurationSection != null)
                {
                    System.Reflection.Assembly assembly = Acme.Core.Helpers.ReflectionHelpers.GetAssemblyFromFullyQualifiedName(configurationSection.InterfaceImplementation.Type);

                    // instantiate class if the assembly has been properly loaded
                    if (assembly != null)
                    {
                        string classType = configurationSection.InterfaceImplementation.Type.Split(',')[0].Trim();
                        object instance = assembly.CreateInstance(classType);

                        // check if this class implements IArgoProjectsDalInterface interface
                        if (instance is IAcmeCorpEngagementsDalInterface)
                        {
                            IAcmeCorpEngagementsDalInterface dal = (IAcmeCorpEngagementsDalInterface)instance;
                            dal.InitializeConfiguration(configurationSection, dalContext);
                            dalInstance = dal;
                            return dal;
                        }
                        else
                        {
                            // Class does not implement interface
                            throw Acme.Core.DiagnosticSystem.ExceptionFactory.ExceptionFactory.CreateException(Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionCode.DoesNotImplementAcmeInterface, "Class instance specified in the Data Access Layer configuration does not implement DeltaAccess.ArgoProjects.BusinessLayer.ArgoProjectsDomain.DalInterface.IArgoProjectsDalInterface interface.", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical, Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionType.AcmeDalException);
                        }
                    }
                    else
                    {
                        // Assembly is not loaded
                        throw Acme.Core.DiagnosticSystem.ExceptionFactory.ExceptionFactory.CreateException(Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionCode.LoadAssemblyFailed, "Could not find or load the Data Access Layer interface implementation assembly stated in the Data Access Layer configuration.", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical, Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionType.AcmeDalException);
                    }
                }
                else
                {
                    // Cannot create instance
                    throw Acme.Core.DiagnosticSystem.ExceptionFactory.ExceptionFactory.CreateException(Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionCode.CannotCreateInstance, "Configuration section has not been found or set. Can not instantiate Data Access Layer Interface.", null, Acme.Core.Logger.Enums.EventServerity.ErrorCritical, Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionType.AcmeDalException);
                }
            }
            catch (Exception ex)
            {
                // Error in creating instance of the Data Access Layer
                throw Acme.Core.DiagnosticSystem.ExceptionFactory.ExceptionFactory.CreateException(Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionCode.DalParametersNotInitialized, "Error in creating instance of the Data Access Layer", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical, Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionType.AcmeDalException);
            }
        }

        /// <summary>
        /// Destroys the already created DAL instance
        /// </summary>
        public static void DestroyInstance()
        {
            dalInstance = null;
        }
    }
}
