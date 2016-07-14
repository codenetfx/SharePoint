// -----------------------------------------------------------------------
// <copyright file="EngagementsDeputiesUpdateTimerJob.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsTimerJobs
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Office.Server.UserProfiles;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;
    using AcmeCorp.Engagements.UiHelpers.ApiFactory;

    /// <summary>
    /// Timer job that handles all operations with Deputies and Deputy Groups
    /// </summary>
    class EngagementsDeputiesUpdateTimerJob : SPJobDefinition
    {
        /// <summary>
        /// The job name
        /// </summary>
        public const string JobName = "AcmeCorp Engagements Deputies Update Timer Job";

        /// <summary>
        /// The time interval for which the job will be run
        /// </summary>
        public static DateTime timeInterval = DateTime.Now.AddDays(-2);

        /// <summary>
        /// SharePoint farm in use
        /// </summary>
        private SPFarm farm;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngagementsDeputiesUpdateTimerJob"/> class.
        /// </summary>
        public EngagementsDeputiesUpdateTimerJob()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngagementsDeputiesUpdateTimerJob"/> class.
        /// </summary>
        /// <param name="webApp">The web app.</param>
        public EngagementsDeputiesUpdateTimerJob(SPWebApplication webApp)
            : base(JobName, webApp, null, SPJobLockType.ContentDatabase)
        {
            this.Title = JobName;
        }

        /// <summary>
        /// Executes the job definition.
        /// </summary>
        /// <param name="targetInstanceId">For target types of <see cref="T:Microsoft.SharePoint.Administration.SPContentDatabase" /> this is the database ID of the content database being processed by the running job. This value is GUID.Empty for all other target types.</param>
        public override void Execute(Guid targetInstanceId)
        {
            // Run the job picking up all changes since last time this job is run
            if (this.LastRunTime != null)
            {
                timeInterval = this.LastRunTime;
            }

            SPWebApplication webApp = this.Parent as SPWebApplication;
            this.farm = webApp.Farm;

            this.ProcessAuditEntries(webApp);
        }

        /// <summary>
        /// Load and process appropriate entries in audit log.
        /// </summary>
        /// <param name="webApp">Web application where Timer Job has executed.</param>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeUiException">0;Could not process audit entries in timer job.</exception>
        private void ProcessAuditEntries(SPWebApplication webApp)
        {
            SPServiceContext serviceContext = SPServiceContext.GetContext(webApp.ServiceApplicationProxyGroup, SPSiteSubscriptionIdentifier.Default);
            UserProfileManager profileConfigManager = new UserProfileManager(serviceContext);
            try
            {
                UserProfileChangeQuery query = new UserProfileChangeQuery(true, true);
                query.ChangeTokenStart = new UserProfileChangeToken(timeInterval);
                query.Add = true;
                query.Custom = true;
                query.Delete = true;
                query.MultiValueProperty = false;
                query.SingleValueProperty = true;
                query.Update = true;
                query.UpdateMetadata = false;
                UserProfileChangeCollection collection = profileConfigManager.GetChanges(query);
                List<string> affectedUserAccounts = new List<string>();
                foreach (UserProfileChange change in collection)
                {
                    if (change is UserProfileSingleValueChange)
                    {
                        UserProfileSingleValueChange propertyChange = (UserProfileSingleValueChange)change;
                        if (propertyChange.ProfileProperty.Name.StartsWith("AcmeCorpDep"))
                        {
                            if (!affectedUserAccounts.Contains(change.AccountName))
                            {
                                affectedUserAccounts.Add(change.AccountName);
                            }
                        }
                    }
                }

                this.ProcessAffectedAccounts(affectedUserAccounts, serviceContext);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeUiException(0, "Could not process audit entries in timer job.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Processes the affected accounts.
        /// </summary>
        /// <param name="affectedUserAccounts">The affected user accounts.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeUiException">0;Could not process affected accounts in timer job.</exception>
        private void ProcessAffectedAccounts(List<string> affectedUserAccounts, SPServiceContext serviceContext)
        {
            try
            {
                foreach (string userAccount in affectedUserAccounts)
                {
                    this.LoadPropertiesAndSetupAdGroup(userAccount, serviceContext);
                }
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeUiException(0, "Could not process affected accounts in timer job.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Loads the Deputies and Deputy Group ID from User Profile Properties and calls operations in AD
        /// </summary>
        /// <param name="userAccount">The user account.</param>
        /// <param name="serviceContext">The service context.</param>
        private void LoadPropertiesAndSetupAdGroup(string userAccount, SPServiceContext serviceContext)
        {
            try
            {
                UserProfileManager profileManager = new UserProfileManager(serviceContext);
                UserProfile userProfile = profileManager.GetUserProfile(userAccount);
                List<string> deputyAccounts = new List<string>();
                string deputyGroupId = string.Empty;

                deputyAccounts = this.ReadDeputyAccountsFromProfile(userProfile);
                deputyGroupId = this.ReadOrCreateDeputyGroupId(userAccount, userProfile);

                this.UpdateDeputyGroupInAd(deputyGroupId, deputyAccounts);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not set up Deputy group for user:" + userAccount, ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Reads Native GUID of Deputies group in AD for specified user profile or creates one if it does not exist 
        /// </summary>
        /// <param name="userAccount">The user account.</param>
        /// <param name="userProfile">The user profile.</param>
        /// <returns>Native GUID of the deputies group from AD</returns>
        private string ReadOrCreateDeputyGroupId(string userAccount, UserProfile userProfile)
        {
            string deputyGroupId = string.Empty;
            try
            {
                AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(this.farm, string.Empty).Api;
                AcmeCorp.Engagements.EngagementsApi.Utilities.ActiveDirectoryHelpers adHelpers = new EngagementsApi.Utilities.ActiveDirectoryHelpers(api);

                if (userProfile["AcmeCorpDeputiesGroupId"].Value != null && userProfile["AcmeCorpDeputiesGroupId"].Value.ToString() != string.Empty)
                {
                    deputyGroupId = userProfile["AcmeCorpDeputiesGroupId"].Value.ToString();
                }
                else
                {
                    deputyGroupId = adHelpers.CreateDeputyGroupInAdForUser(userAccount);
                    userProfile["AcmeCorpDeputiesGroupId"].Value = deputyGroupId;
                    userProfile.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not create Deputy group for user:" + userAccount, ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }

            return deputyGroupId;
        }

        /// <summary>
        /// Reads the deputy accounts from profile and adds their usernames to the list
        /// </summary>
        /// <param name="userProfile">List of all usernames which are deputies for specified user profile.</param>
        /// <returns>List of usernames for deputies</returns>
        private List<string> ReadDeputyAccountsFromProfile(UserProfile userProfile)
        {
            List<string> deputyAccounts = new List<string>();
            if (userProfile["AcmeCorpDeputy1"].Value != null && userProfile["AcmeCorpDeputy1"].Value.ToString() != string.Empty)
            {
                deputyAccounts.Add(userProfile["AcmeCorpDeputy1"].Value.ToString());
            }

            if (userProfile["AcmeCorpDeputy2"].Value != null && userProfile["AcmeCorpDeputy2"].Value.ToString() != string.Empty)
            {
                deputyAccounts.Add(userProfile["AcmeCorpDeputy2"].Value.ToString());
            }

            if (userProfile["AcmeCorpDeputy3"].Value != null && userProfile["AcmeCorpDeputy3"].Value.ToString() != string.Empty)
            {
                deputyAccounts.Add(userProfile["AcmeCorpDeputy3"].Value.ToString());
            }

            if (userProfile["AcmeCorpDeputy4"].Value != null && userProfile["AcmeCorpDeputy4"].Value.ToString() != string.Empty)
            {
                deputyAccounts.Add(userProfile["AcmeCorpDeputy4"].Value.ToString());
            }

            if (userProfile["AcmeCorpDeputy5"].Value != null && userProfile["AcmeCorpDeputy5"].Value.ToString() != string.Empty)
            {
                deputyAccounts.Add(userProfile["AcmeCorpDeputy5"].Value.ToString());
            }

            return deputyAccounts;
        }

        /// <summary>
        /// Updates the deputy group in ad.
        /// </summary>
        /// <param name="deputyGroupId">The deputy group Native GUID.</param>
        /// <param name="deputyAccounts">List of the deputy accounts.</param>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException">0;Could not update Deputy Group in AD.</exception>
        private void UpdateDeputyGroupInAd(string deputyGroupId, List<string> deputyAccounts)
        {
            AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(this.farm, string.Empty).Api;
            AcmeCorp.Engagements.EngagementsApi.Utilities.ActiveDirectoryHelpers adHelpers = new EngagementsApi.Utilities.ActiveDirectoryHelpers(api);

            try
            {
                adHelpers.RemoveAllUsersFromGroup(deputyGroupId);
                foreach (string userAccount in deputyAccounts)
                {
                    string userDn = adHelpers.CreateDnForUser(userAccount);
                    adHelpers.AddUserToGroup(userDn, deputyGroupId);
                }
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApiException(0, "Could not update Deputy Group in AD.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }
    }
}
