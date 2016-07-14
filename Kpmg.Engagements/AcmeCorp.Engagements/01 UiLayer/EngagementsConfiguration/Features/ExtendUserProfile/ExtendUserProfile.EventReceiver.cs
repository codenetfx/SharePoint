using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Administration;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint.Taxonomy;

namespace AcmeCorp.Engagements.EngagementsConfiguration.Features.ExtendUserProfile
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("2c33ff98-23b2-456a-a0a8-c293ad03bd84")]
    public class ExtendUserProfileEventReceiver : SPFeatureReceiver
    {
        private string siteUrl = "https://pspace/";
        private string adImportConnectionName = "sharedove";
        private SPSite _site;
        private SPFarm _farm;
        private SPServiceContext _serviceContext;
        private UserProfileManager _profileManager;
        private ProfileSubtypePropertyManager _profileSubtypePropertyManager;
        private UserProfileConfigManager _userProfileConfigManager;
        private ProfilePropertyManager _profilePropertyManager;
        private CorePropertyManager _corePropertyManager;
        private ProfileTypePropertyManager _profileTypePropertyManager;
        private ActiveDirectoryImportConnection _synchronizationConnection;


        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            _farm = properties.Feature.Parent as SPFarm;
            _site = new SPSite(siteUrl);
            _serviceContext = SPServiceContext.Current;
            _userProfileConfigManager = new UserProfileConfigManager(_serviceContext);
            _profilePropertyManager = _userProfileConfigManager.ProfilePropertyManager;

            _profileManager = new UserProfileManager(_serviceContext);
            _profileSubtypePropertyManager = _profileManager.DefaultProfileSubtypeProperties;
            // if you need another profile subtype 
            //_profileSubtypePropertyManager = _profilePropertyManager.GetProfileSubtypeProperties("ProfileSubtypeName"); 

            _corePropertyManager = _profilePropertyManager.GetCoreProperties();
            _profileTypePropertyManager = _profilePropertyManager.GetProfileTypeProperties(ProfileType.User);

            _synchronizationConnection = (ActiveDirectoryImportConnection)_userProfileConfigManager.ConnectionManager[adImportConnectionName];

            this.ProcessProps();
        }

        private void ProcessProps()
        {
            try
            {
                ProfileSubtypeProperty AcmeCorpProperties = CreateOrUpdateSection("AcmeCorpProperties", "AcmeCorp Properties");

                ProfileSubtypeProperty AcmeCorpPropertyGlobalId = CreateOrUpdateStringProperty("AcmeCorpUserGlobalId", "AcmeCorp_UserGlobalId", "imported from AcmeCorp-User-GPID AD field", 50, false, null, null, null, null, false, Privacy.Manager, true, true, false, AcmeCorpProperties);
                _synchronizationConnection.AddPropertyMapping("AcmeCorpUserGPID", AcmeCorpPropertyGlobalId.Name);

                ProfileSubtypeProperty AcmeCorpPropertyUserManagementLevel = CreateOrUpdateIntProperty("AcmeCorpUserManagementLevel", "AcmeCorp_UserManagementLevel", "imported from AcmeCorp-User-GOManagementLevel AD field", false, Privacy.Manager, true, true, false, AcmeCorpProperties);
                _synchronizationConnection.AddPropertyMapping("AcmeCorpUserGOManagementLevel", AcmeCorpPropertyUserManagementLevel.Name);

                ProfileSubtypeProperty AcmeCorpPropertyEmployeeId = CreateOrUpdateStringProperty("AcmeCorpEmployeeId", "AcmeCorp_EmployeeId", "imported from employeeID AD dield", 100, false, null, null, null, null, false, Privacy.Manager, true, true, false, AcmeCorpProperties);
                _synchronizationConnection.AddPropertyMapping("employeeID", AcmeCorpPropertyEmployeeId.Name);

                ProfileSubtypeProperty AcmeCorpPropertyDeputy1 = CreateOrUpdatePeopleProperty("AcmeCorpDeputy1", "Deputy 1", "AcmeCorp Deputy 1 field", null, null, null, false, Privacy.Public, true, true, true, AcmeCorpProperties);
                ProfileSubtypeProperty AcmeCorpPropertyDeputy2 = CreateOrUpdatePeopleProperty("AcmeCorpDeputy2", "Deputy 2", "AcmeCorp Deputy 2 field", null, null, null, false, Privacy.Public, true, true, true, AcmeCorpProperties);
                ProfileSubtypeProperty AcmeCorpPropertyDeputy3 = CreateOrUpdatePeopleProperty("AcmeCorpDeputy3", "Deputy 3", "AcmeCorp Deputy 3 field", null, null, null, false, Privacy.Public, true, true, true, AcmeCorpProperties);
                ProfileSubtypeProperty AcmeCorpPropertyDeputy4 = CreateOrUpdatePeopleProperty("AcmeCorpDeputy4", "Deputy 4", "AcmeCorp Deputy 4 field", null, null, null, false, Privacy.Public, true, true, true, AcmeCorpProperties);
                ProfileSubtypeProperty AcmeCorpPropertyDeputy5 = CreateOrUpdatePeopleProperty("AcmeCorpDeputy5", "Deputy 5", "AcmeCorp Deputy 5 field", null, null, null, false, Privacy.Public, true, true, true, AcmeCorpProperties);

                ProfileSubtypeProperty AcmeCorpPropertyDeputiesGroupId = CreateOrUpdateStringProperty("AcmeCorpDeputiesGroupId", "AcmeCorp_DeputiesGroupId", "Deputies", 100, false, null, null, null, null, false, Privacy.Manager, true, true, false, AcmeCorpProperties);

                //  start UserProfile – Profile Synchronization Job 
                StartServiceJob("User Profile Service", "UserProfile_ProfileSynchronizationJob");

            }

            catch (Exception ex)
            {
                // TODO: ex.Message);
            }

        }

        private void StartServiceJob(string serviceTypeName, string jobName)
        {
            foreach (SPService service in _farm.Services)
            {
                if ((serviceTypeName == null) || (serviceTypeName == service.TypeName))
                {
                    foreach (SPJobDefinition jobDefinition in service.JobDefinitions)
                    {
                        if (jobDefinition.Name == jobName)
                        {
                            jobDefinition.RunNow();
                            break;
                        }
                    }
                }
            }
        }

        private ProfileSubtypeProperty CreateOrUpdateSection(string name, string displayName)
        {
            if ((_corePropertyManager.GetPropertyByName(name) != null)
                || (_profileTypePropertyManager.GetPropertyByName(name) != null)
                || (_profileSubtypePropertyManager.GetPropertyByName(name) != null))
            {
                throw new Exception(String.Format("WARNING! Property with the same name ‘{0}’ exists.", name));
            }

            CoreProperty coreProperty = _corePropertyManager.GetSectionByName(name);
            bool exist = (coreProperty != null);
            if (!exist)
            {
                coreProperty = _corePropertyManager.Create(true);
                coreProperty.Name = name;
            }

            coreProperty.DisplayName = displayName;

            if (exist)
            {
                coreProperty.Commit();
            }
            else
            {
                _corePropertyManager.Add(coreProperty);
            }

            ProfileTypeProperty profileTypeProperty = _profileTypePropertyManager.GetSectionByName(name);
            if (profileTypeProperty == null)
            {
                profileTypeProperty = _profileTypePropertyManager.Create(coreProperty);
                _profileTypePropertyManager.Add(profileTypeProperty);
            }

            ProfileSubtypeProperty profileSubTypeProperty = _profileSubtypePropertyManager.GetSectionByName(coreProperty.Name);
            if (profileSubTypeProperty == null)
            {
                // we should get the display order before creating our new section 
                // otherwise its display order will be returned when trying to get 
                // the display order of the last section 
                int lastSectionDisplayOrder = GetDisplayOrderForLastSection();
                profileSubTypeProperty = _profileSubtypePropertyManager.Create(profileTypeProperty);
                _profileSubtypePropertyManager.Add(profileSubTypeProperty);

                // set the position for new section by adding 100 to the last section 
                _profileSubtypePropertyManager.SetDisplayOrderBySectionName(name, lastSectionDisplayOrder + 100);
                _profileSubtypePropertyManager.CommitDisplayOrder();
            }

            return profileSubTypeProperty;

        }

        private int GetDisplayOrderForLastSection()
        {
            int displayOrder = 0;

            foreach (ProfileSubtypeProperty profileSubtypeProperty in _profileSubtypePropertyManager.PropertiesWithSection)
            {
                if (profileSubtypeProperty.IsSection)
                {
                    if (profileSubtypeProperty.DisplayOrder > displayOrder)
                    {
                        displayOrder = profileSubtypeProperty.DisplayOrder;
                    }
                }
            }
            return displayOrder;
        }



        private ProfileSubtypeProperty CreateOrUpdateStringProperty(string name, string displayName,
         string description, int length, bool isMultiValued, string separator, string termStoreName,
         string groupName, string termSetName, bool required, Privacy privacy, bool isVisibleOnViewer,
         bool isVisibleOnEditor, bool isUserEditable, ProfileSubtypeProperty section)
        {
            TermSet termSet = null;
            if ((!String.IsNullOrEmpty(termStoreName))
                && (!String.IsNullOrEmpty(groupName))
                && (!String.IsNullOrEmpty(termSetName)))
            {
                TaxonomySession session = new TaxonomySession(_site);
                TermStore termStore = session.TermStores[termStoreName];
                Group group = termStore.Groups[groupName];
                termSet = group.TermSets[termSetName];
            }

            Action<CoreProperty> customSettingsOnCreate =
                coreProperty =>
                {
                    // for multivalue and taxonomic properties the length 
                    // will be set to 3600 chars by default 
                    if ((!isMultiValued) && (termSet == null))
                    {
                        coreProperty.Length = length;
                    }
                    coreProperty.IsMultivalued = isMultiValued;
                    if ((isMultiValued) && (!String.IsNullOrEmpty(separator)))
                    {
                        coreProperty.Separator = (MultiValueSeparator)Enum.Parse(typeof(MultiValueSeparator), separator);
                    }
                    if (termSet != null)
                    {
                        coreProperty.TermSet = termSet;
                    }
                };

            Action<CoreProperty> customSettingsOnUpdate =
                coreProperty =>
                {
                    if (termSet != null)
                    {
                        coreProperty.TermSet = termSet;
                    }
                };

            return CreateOrUpdateProperty(name, String.Format("string ({0} Value)",
                isMultiValued ? "Multi" : "Single"),
                displayName, description, required, privacy, isVisibleOnViewer,
                isVisibleOnEditor, isUserEditable, section, customSettingsOnCreate, customSettingsOnUpdate);
        }

        private ProfileSubtypeProperty CreateOrUpdatePeopleProperty(string name, string displayName,
       string description, string termStoreName,
       string groupName, string termSetName, bool required, Privacy privacy, bool isVisibleOnViewer,
       bool isVisibleOnEditor, bool isUserEditable, ProfileSubtypeProperty section)
        {
            TermSet termSet = null;
            if ((!String.IsNullOrEmpty(termStoreName))
                && (!String.IsNullOrEmpty(groupName))
                && (!String.IsNullOrEmpty(termSetName)))
            {
                TaxonomySession session = new TaxonomySession(_site);
                TermStore termStore = session.TermStores[termStoreName];
                Group group = termStore.Groups[groupName];
                termSet = group.TermSets[termSetName];
            }

            Action<CoreProperty> customSettingsOnCreate =
                coreProperty =>
                {

                    if (termSet != null)
                    {
                        coreProperty.TermSet = termSet;
                    }
                };

            Action<CoreProperty> customSettingsOnUpdate =
                coreProperty =>
                {
                    if (termSet != null)
                    {
                        coreProperty.TermSet = termSet;
                    }
                };

            return CreateOrUpdateProperty(name, PropertyDataType.Person,
                displayName, description, required, privacy, isVisibleOnViewer,
                isVisibleOnEditor, isUserEditable, section, customSettingsOnCreate, customSettingsOnUpdate);
        }

        private CoreProperty CreateOrUpdateCoreProperty(string name, string propertyType,
            string displayName, string description, Action<CoreProperty> customSettingsOnCreate, Action<CoreProperty> customSettingsOnUpdate)
        {
            if ((_corePropertyManager.GetSectionByName(name) != null)
                || (_profileTypePropertyManager.GetSectionByName(name) != null)
                || (_profileSubtypePropertyManager.GetSectionByName(name) != null))
            {
                throw new Exception(String.Format("WARNING! Section with the same name ‘{0}’ exists.",
                    name));
            }

            CoreProperty coreProperty = _corePropertyManager.GetPropertyByName(name);
            bool exist = (coreProperty != null);
            if (!exist)
            {
                coreProperty = _corePropertyManager.Create(false);
                coreProperty.Name = name;
                coreProperty.Type = propertyType;
            }

            if (coreProperty.Type.ToLower() == propertyType.ToLower())
            {
                coreProperty.DisplayName = displayName;
                coreProperty.Description = description;

                if (exist)
                {
                    // call custom settings method if specified 
                    if (customSettingsOnUpdate != null)
                    {
                        customSettingsOnUpdate.Invoke(coreProperty);
                    }
                    coreProperty.Commit();
                }
                else
                {
                    // call custom settings method if specified 
                    if (customSettingsOnCreate != null)
                    {
                        customSettingsOnCreate.Invoke(coreProperty);
                    }
                    _corePropertyManager.Add(coreProperty);
                }
            }
            else
            {
                throw new Exception(String.Format("The CoreProperty with the specified name ‘{0}’ exists, but is a type of ‘{1}’ not ‘{2}’ ",
                    name, coreProperty.Type, propertyType));
            }

            return coreProperty;
        }

        private ProfileSubtypeProperty CreateOrUpdateProperty(string name, string propertyType, string displayName,
           string description, bool required, Privacy privacy, bool isVisibleOnViewer,
           bool isVisibleOnEditor, bool isUserEditable, ProfileSubtypeProperty section,
           Action<CoreProperty> customSettingsOnCreate, Action<CoreProperty> customSettingsOnUpdate)
        {
            CoreProperty coreProperty = CreateOrUpdateCoreProperty(name, propertyType,
                displayName, description, customSettingsOnCreate, customSettingsOnUpdate);

            ProfileSubtypeProperty profileSubTypeProperty = CreateOrUpdateSubtypeProperty(
                coreProperty, required, privacy, isVisibleOnEditor, isVisibleOnViewer,
                isUserEditable, section);

            return profileSubTypeProperty;
        }

        private ProfileSubtypeProperty CreateOrUpdateSubtypeProperty(CoreProperty coreProperty,
           bool required, Privacy privacy, bool isVisibleOnEditor, bool isVisibleOnViewer,
           bool isUserEditable, ProfileSubtypeProperty section)
        {
            ProfileTypeProperty profileTypeProperty = CreateOrUpdateProfileTypeProperty(coreProperty,
                isVisibleOnEditor, isVisibleOnViewer);

            ProfileSubtypeProperty profileSubTypeProperty =
                _profileSubtypePropertyManager.GetPropertyByName(coreProperty.Name);

            if (profileSubTypeProperty == null)
            {
                profileSubTypeProperty = _profileSubtypePropertyManager.Create(profileTypeProperty);
                _profileSubtypePropertyManager.Add(profileSubTypeProperty);
                // set the section if specified and differs from the current one 
                if ((section != null)
                    && (section.Name != GetSectionForProperty(profileSubTypeProperty).Name))
                {
                    int displayOrder = GetLastDisplayOrderForSection(section);
                    _profileSubtypePropertyManager.SetDisplayOrderByPropertyName(coreProperty.Name,
                        displayOrder + 1);
                    _profileSubtypePropertyManager.CommitDisplayOrder();
                }
            }

            profileSubTypeProperty.IsUserEditable = isUserEditable;
            profileSubTypeProperty.DefaultPrivacy = privacy;
            profileSubTypeProperty.PrivacyPolicy = (required ? PrivacyPolicy.Mandatory :
                PrivacyPolicy.OptIn);
            profileSubTypeProperty.Commit();


            return profileSubTypeProperty;
        }

        private ProfileTypeProperty CreateOrUpdateProfileTypeProperty(CoreProperty coreProperty, bool isVisibleOnEditor, bool isVisibleOnViewer)
        {
            ProfileTypeProperty profileTypeProperty =
                _profileTypePropertyManager.GetPropertyByName(coreProperty.Name);
            if (profileTypeProperty == null)
            {
                profileTypeProperty = _profileTypePropertyManager.Create(coreProperty);
                _profileTypePropertyManager.Add(profileTypeProperty);
            }
            profileTypeProperty.IsVisibleOnViewer = isVisibleOnViewer;
            profileTypeProperty.IsVisibleOnEditor = isVisibleOnEditor;

            profileTypeProperty.Commit();

            return profileTypeProperty;
        }

        private ProfileSubtypeProperty GetSectionForProperty(ProfileSubtypeProperty property)
        {
            ProfileSubtypeProperty section = null;

            foreach (ProfileSubtypeProperty profileSubtypeProperty in _profileSubtypePropertyManager.PropertiesWithSection)
            {
                if (profileSubtypeProperty.IsSection)
                {
                    section = profileSubtypeProperty;
                }
                if (profileSubtypeProperty.Name == property.Name)
                {
                    break;
                }
            }
            return section;
        }

        private ProfileSubtypeProperty GetSectionForProperty(string propertyName)
        {
            ProfileSubtypeProperty property = _profileSubtypePropertyManager.GetPropertyByName(propertyName);
            return GetSectionForProperty(property.Name);
        }

        private int GetLastDisplayOrderForSection(ProfileSubtypeProperty section)
        {
            int displayOrder = 0;

            foreach (ProfileSubtypeProperty profileSubtypeProperty in _profileSubtypePropertyManager.PropertiesWithSection)
            {
                if ((section.DisplayOrder < displayOrder) && (profileSubtypeProperty.IsSection))
                {
                    break;
                }
                displayOrder = profileSubtypeProperty.DisplayOrder;
            }
            return displayOrder;
        }

        private int GetLastDisplayOrderForSection(string sectionName)
        {
            ProfileSubtypeProperty section = _profileSubtypePropertyManager.GetSectionByName(sectionName);
            return GetLastDisplayOrderForSection(section.Name);
        }

        private ProfileSubtypeProperty CreateOrUpdateIntProperty(string name, string displayName,
            string description, bool required, Privacy privacy, bool isVisibleOnViewer,
            bool isVisibleOnEditor, bool isUserEditable, ProfileSubtypeProperty section)
        {
            return CreateOrUpdateProperty(name, "integer", displayName,
            description, required, privacy, isVisibleOnViewer,
            isVisibleOnEditor, isUserEditable, section, null, null);
        }




        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
