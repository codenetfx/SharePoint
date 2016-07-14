// -----------------------------------------------------------------------
// <copyright file="ActiveDirectoryHelpers.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsApi.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class that contains helper functions for AD operations
    /// </summary>
    public class ActiveDirectoryHelpers
    {
        #region Private members
        /// <summary>
        /// Variable that contains AD compatible path of the OU where all deputy groups will be stored
        /// </summary>
        private string ouPath = string.Empty;

        /// <summary>
        /// Variable that contains value of OU in which engagement-specific OU is created
        /// </summary>
        private string deputiesOuContainer;

        /// <summary>
        /// Full identifier of AD container where users are stored
        /// </summary>
        private string usersOuContainer;

        /// <summary>
        /// Prefix that will be prepended when creating group name in the AD
        /// </summary>
        private string deputiesGroupPrefix;

        /// <summary>
        /// The OU inside AD used to store deputies
        /// </summary>
        private string deputiesAdOu;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveDirectoryHelpers" /> class.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException">0;Could not initialize Active Directory Helpers class</exception>
        public ActiveDirectoryHelpers(AcmeCorp.Engagements.EngagementsApi.Api api)
        {
            try
            {
                this.deputiesAdOu = api.AdDeputyGroupsOu; // "EngagementDeputies";
                this.deputiesGroupPrefix = api.AdDeputiesGroupPrefix; // "Deputies";
                this.deputiesOuContainer = api.AdDeputiesOuContainer; // "OU=AcmeCorp,DC=spfarm,DC=local";
                this.usersOuContainer = api.AdUsersOuContainer; // "CN=Users,DC=spfarm,DC=local";
                this.ouPath = "OU=" + this.deputiesAdOu + "," + this.deputiesOuContainer;
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not initialize Active Directory Helpers class", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }
        
        #region Public properties

        /// <summary>
        /// Gets or sets variable that contains value of OU in which engagement-specific OU is created
        /// </summary>
        /// <value>
        /// variable that contains value of OU in which engagement-specific OU is created
        /// </value>
        public string DeputiesOuContainer
        {
            get { return this.deputiesOuContainer; }
            set { this.deputiesOuContainer = value; }
        }
        
        /// <summary>
        /// Gets or sets the full identifier of AD container where users are stored
        /// </summary>
        /// <value>
        /// Full identifier of AD container where users are stored
        /// </value>
        public string UsersOuContainer
        {
            get { return this.usersOuContainer; }
            set { this.usersOuContainer = value; }
        }

        /// <summary>
        /// Gets or sets the OU in AD for storing deputies group
        /// </summary>
        /// <value>
        /// The deputies OU in AD.
        /// </value>
        public string DeputiesAdOu
        {
            get
            {
                return this.deputiesAdOu;
            }

            set
            {
                this.deputiesAdOu = value;
            }
        }

        /// <summary>
        /// Gets or sets the deputies group prefix.
        /// </summary>
        /// <value>
        /// The deputies group prefix.
        /// </value>
        public string DeputiesGroupPrefix
        {
            get { return this.deputiesGroupPrefix; }
            set { this.deputiesGroupPrefix = value; }
        }

        #endregion
             
        /// <summary>
        /// Creates the AD group in specified OU.
        /// </summary>
        /// <param name="groupName">Name of new group.</param>
        /// <param name="adOu">The OU in AD where we want to create the group.</param>
        public void CreateAdGroupInOu(string groupName, string adOu)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not create AD group in OU.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Adds the user to group.
        /// </summary>
        /// <param name="userDn">The user full qualified identifier in the domain.</param>
        /// <param name="groupGuid">The native GUID for the group where use should be added.</param>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException">0;Could not add users to AD group.</exception>
        public void AddUserToGroup(string userDn, string groupGuid)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry("LDAP://<" + "GUID=" + groupGuid + ">");
                dirEntry.Properties["member"].Add(userDn);
                dirEntry.CommitChanges();
                dirEntry.Close();
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not add users to AD group.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Removes all users from group.
        /// </summary>
        /// <param name="groupGuid">The native GUID for the group to process.</param>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException">0;Could not remove all users from AD group.</exception>
        public void RemoveAllUsersFromGroup(string groupGuid)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry("LDAP://<" + "GUID=" + groupGuid + ">");
                dirEntry.Properties["member"].Clear();
                dirEntry.CommitChanges();
                dirEntry.Close();
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not remove all users from AD group.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Creates the deputy group in ad for user. Returns newly created group or already existing group in AD
        /// </summary>
        /// <param name="userAccount">The user account.</param>
        /// <returns>native GUID of an AD group</returns>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException">
        /// 0;Could not create group in AD and read its native GUID.
        /// or
        /// 0;Could not read native GUID for a group in AD.
        /// </exception>
        public string CreateDeputyGroupInAdForUser(string userAccount)
        {
            string userName = userAccount.Split('\\')[1];
            string groupName = this.deputiesGroupPrefix + "-" + userName;
            string groupNativeGuid = string.Empty;

            if (!DirectoryEntry.Exists("LDAP://CN=" + groupName + "," + this.ouPath))
            {
                try
                {
                    DirectoryEntry entry = new DirectoryEntry("LDAP://" + this.ouPath);
                    DirectoryEntry group = entry.Children.Add("CN=" + groupName, "group");
                    group.Properties["sAmAccountName"].Value = groupName;
                    group.CommitChanges();
                    groupNativeGuid = group.NativeGuid;
                }
                catch (Exception ex)
                {
                    throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not create group in AD and read its native GUID.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
                }
            }
            else
            {
                try
                {
                    DirectoryEntry existingAdGroup = new DirectoryEntry("LDAP://CN=" + groupName + "," + this.ouPath);
                    groupNativeGuid = existingAdGroup.NativeGuid;
                }
                catch (Exception ex)
                {
                    throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not read native GUID for a group in AD.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
                }
            }

            return groupNativeGuid;
        }

        /// <summary>
        /// Returns name of deputies group for user. If Deputies group does not exist, creates a new deputies group
        /// </summary>
        /// <param name="userAccount">The user account.</param>
        /// <returns>
        /// native GUID of an AD group
        /// </returns>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException">0;Could not create group in AD and read its native GUID.
        /// or
        /// 0;Could not read native GUID for a group in AD.</exception>
        public string GetDeputiesGroup(string userAccount)
        {
            string userName = userAccount.Split('\\')[1];
            string groupName = this.deputiesGroupPrefix + "-" + userName;
            string returnedName = string.Empty;

            if (!DirectoryEntry.Exists("LDAP://CN=" + groupName + "," + this.ouPath))
            {
                try
                {
                    DirectoryEntry entry = new DirectoryEntry("LDAP://" + this.ouPath);
                    DirectoryEntry group = entry.Children.Add("CN=" + groupName, "group");
                    group.Properties["sAmAccountName"].Value = groupName;
                    group.CommitChanges();
                    returnedName = group.Name;
                }
                catch (Exception ex)
                {
                    throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not create group in AD and read its name.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
                }
            }
            else
            {
                try
                {
                    DirectoryEntry existingAdGroup = new DirectoryEntry("LDAP://CN=" + groupName + "," + this.ouPath);
                    returnedName = existingAdGroup.Name;
                }
                catch (Exception ex)
                {
                    throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not read Name for a group in AD.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
                }
            }

            return returnedName;
        }

        /// <summary>
        /// Creates the AD compatible string for user based on his username.
        /// </summary>
        /// <param name="userAccount">The user account.</param>
        /// <returns>DN string for user based on his account name</returns>
        public string CreateDnForUser(string userAccount)
        {
            string userDn = string.Empty;
            userDn = "CN=" + userAccount.Split('\\')[1] + "," + this.usersOuContainer;
            return userDn;
        }

        /// <summary>
        /// Gets the string array of deputies usernames for specified users.
        /// </summary>
        /// <param name="usersArray">The users array.</param>
        /// <returns>string array of deputies usernames</returns>
        internal string[] GetDeputiesForUsers(string[] usersArray)
        {
            List<string> deputiesList = new List<string>();
            try
            {
                foreach (var item in usersArray)
                {
                    string[] deputies = this.GetDeputiesForUser(item);
                    if (deputies != null && deputies.Length != 0)
                    {
                        foreach (var deputyUsername in deputies)
                        {
                            deputiesList.Add(deputyUsername);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not read deputies for specified users.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
            
            return deputiesList.ToArray();
        }

        /// <summary>
        /// Gets the deputies for user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>string array of deputies usernames</returns>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException">0;Could not read Name for a group in AD.</exception>
        private string[] GetDeputiesForUser(string userName)
        {
            List<string> deputiesUsernamesList = new List<string>();

            string groupName = this.deputiesGroupPrefix + "-" + userName.Split('\\')[1];

            try
            {
                DirectoryEntry existingAdGroup = new DirectoryEntry("LDAP://CN=" + groupName + "," + this.ouPath);
                if (existingAdGroup.Username != null)
                {
                    object groupMembers = null;
                    groupMembers = existingAdGroup.Invoke("Members", null);
                    if (groupMembers != null)
                    {
                        foreach (object memberUser in (IEnumerable)groupMembers)
                        {
                            DirectoryEntry adUser = new DirectoryEntry(memberUser);
                            deputiesUsernamesList.Add(adUser.Properties["name"].Value.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not read deputies for specified user.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }

            return deputiesUsernamesList.ToArray();
        }
    }
}
