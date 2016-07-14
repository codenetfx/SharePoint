// -----------------------------------------------------------------------
// <copyright file="EngagementsServiceBusProcessingTimerJob.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsTimerJobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;
    using AcmeCorp.Engagements.EngagementsDomain.Entities;
    using AcmeCorp.Engagements.UiHelpers.ApiFactory;

    /// <summary>
    /// Timer job that connects to Service  Bus and performs processing based on items in SB Queue
    /// </summary>
    public class EngagementsServiceBusProcessingTimerJob : SPJobDefinition
    {
        /// <summary>
        /// The job name
        /// </summary>
        public const string JobName = "AcmeCorp Engagements Service Bus Processing Timer Job";

        /// <summary>
        /// SharePoint farm in use
        /// </summary>
        private SPFarm farm;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngagementsServiceBusProcessingTimerJob"/> class.
        /// </summary>
        public EngagementsServiceBusProcessingTimerJob()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngagementsServiceBusProcessingTimerJob"/> class.
        /// </summary>
        /// <param name="webApp">The web app.</param>
        public EngagementsServiceBusProcessingTimerJob(SPWebApplication webApp)
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
            SPWebApplication webApp = this.Parent as SPWebApplication;
            this.farm = webApp.Farm;
            string sbConnectionString = this.farm.Properties["PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_ServiceBusConnectionString"].ToString();
            string sbQueueName = this.farm.Properties["PS_CONFIG_AcmeCorp_DataAccessLayer_ImplementationParameters_ServiceBusQueueName"].ToString();
            if ((sbConnectionString != null && sbConnectionString != string.Empty) && (sbQueueName != null && sbQueueName != string.Empty))
            {
                this.ProcessServiceBusQueue(sbConnectionString, sbQueueName);
            }
        }

        /// <summary>
        /// Processes the service bus queue.
        /// </summary>
        /// <param name="sbConnectionString">Service Bus connection string.</param>
        /// <param name="sbQueueName">Name of the Service Bus queue.</param>
        /// <exception cref="Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException">0;Error in processing ServiceBus queue.</exception>
        private void ProcessServiceBusQueue(string sbConnectionString, string sbQueueName)
        {
            try
            {
                AcmeCorp.Engagements.EngagementsServiceBus.ServiceBus serviceBus = new EngagementsServiceBus.ServiceBus(sbConnectionString);
                QueueClient queueClient = serviceBus.CreateSbQueueClient(sbQueueName);
                Dictionary<string, EngagementData> serviceBusRetVal = serviceBus.ReceiveBrokeredMessageCollection(queueClient);

                foreach (KeyValuePair<string, EngagementData> item in serviceBusRetVal)
                {
                    this.ExecuteApiCalls(item);
                }
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in processing ServiceBus queue.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Executes the API calls.
        /// </summary>
        /// <param name="item">The item.</param>
        private void ExecuteApiCalls(KeyValuePair<string, EngagementData> item)
        {
            // We are using BrokeredMessage label to identify type of operation
            // and as label has to be unique in order to overcome duplicate detection system of Service Bus
            // our logic has to extract operation from label.
            string command = item.Key.Split('#')[0];
            switch (command)
            {
                
                case "CREATEENGAGEMENT":
                    this.CreateEngagement(item.Value);
                    break;
                case "UPDATEENGAGEMENT":
                    this.UpdateEngagementSite(item.Value);
                    break;
                case "GETENGAGEMENTSTATUS":
                    this.GetEngagementStatus(item.Value);
                    break;
                case "CLOSEENGAGEMENT":
                    this.CloseEngagement(item.Value);
                    break;
                case "REOPENENGAGEMENT":
                    this.ReopenEngagement(item.Value);
                    break;
                case "CREATEOPPORTUNITY":
                    this.CreateOpportunity(item.Value);
                    break;
                case "UPDATEOPPORTUNITY":
                    this.UpdateOpportunitySite(item.Value);
                    break;

                case "CONVERTTOENGAGEMENT":
                    this.CreateEngagementFromOpportunity(item.Value);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Creates the engagement.
        /// </summary>
        /// <param name="messageContent">Content of the message.</param>
        private void CreateEngagement(EngagementData messageContent)
        {
            try
            {
                string newSiteUrl = string.Empty;
                AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(this.farm, string.Empty).Api;

                newSiteUrl = api.CreateNewEngagementSite((int)messageContent.EngagementId, (EngagementsDomain.Enums.EngagementFoldersType)messageContent.EngagementFoldersType, messageContent.Managers.ToArray(), messageContent.Partners.ToArray(), messageContent.Staff.ToArray(), messageContent.EngagementProperties);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in processing ServiceBus queue - unable to create engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Creates the opportunity.
        /// </summary>
        /// <param name="messageContent">Content of the message.</param>
        private void CreateOpportunity(EngagementData messageContent)
        {
            try
            {
                string newSiteUrl = string.Empty;
                AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(this.farm, string.Empty).Api;

                newSiteUrl = api.CreateNewOpportunitySite((int)messageContent.EngagementId, messageContent.Managers.ToArray(), messageContent.Partners.ToArray(), messageContent.Staff.ToArray(), messageContent.EngagementProperties);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in processing ServiceBus queue - unable to create engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Updates the engagement site collection properties.
        /// </summary>
        /// <param name="messageContent">Content of the message.</param>
        private void UpdateEngagementSite(EngagementData messageContent)
        {
            try
            {
                string newSiteUrl = string.Empty;
                AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(this.farm, string.Empty).Api;

                newSiteUrl = api.UpdateEngagementSiteProperties((int)messageContent.EngagementId, (EngagementsDomain.Enums.EngagementFoldersType)messageContent.EngagementFoldersType, messageContent.Managers.ToArray(), messageContent.Partners.ToArray(), messageContent.Staff.ToArray(), messageContent.EngagementProperties);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in processing ServiceBus queue - unable to update properties of engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Updates the opportunity site collection properties.
        /// </summary>
        /// <param name="messageContent">Content of the message.</param>
        private void CreateEngagementFromOpportunity(EngagementData messageContent)
        {
            try
            {
                string newSiteUrl = string.Empty;
                AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(this.farm, string.Empty).Api;

                newSiteUrl = api.CreateEngagementFromOpportunity((int)messageContent.EngagementId, (int)messageContent.EngagementId);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in processing ServiceBus queue - unable to update properties of engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Updates the opportunity site collection properties.
        /// </summary>
        /// <param name="messageContent">Content of the message.</param>
        private void UpdateOpportunitySite(EngagementData messageContent)
        {
            try
            {
                string newSiteUrl = string.Empty;
                AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(this.farm, string.Empty).Api;

                newSiteUrl = api.UpdateOpportunitySiteProperties((int)messageContent.EngagementId, messageContent.Managers.ToArray(), messageContent.Partners.ToArray(), messageContent.Staff.ToArray(), messageContent.EngagementProperties);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in processing ServiceBus queue - unable to update properties of engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Reopens the engagement.
        /// </summary>
        /// <param name="statusString">The status string.</param>
        private void ReopenEngagement(EngagementData statusString)
        {
            try
            {
                long engagementId = statusString.EngagementId;
                string result = string.Empty;
                AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(this.farm, string.Empty).Api;

                api.ReopenEngagement(engagementId);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in processing ServiceBus queue - unable to reopen engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Closes the engagement.
        /// </summary>
        /// <param name="statusString">The status string.</param>
        private void CloseEngagement(EngagementData statusString)
        {
            try
            {
                long engagementId = statusString.EngagementId;
                string result = string.Empty;
                AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(this.farm, string.Empty).Api;

                api.CloseEngagement(engagementId);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in processing ServiceBus queue - unable to close engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Gets the engagement status.
        /// </summary>
        /// <param name="statusString">The status string.</param>
        private void GetEngagementStatus(EngagementData statusString)
        {
            try
            {
                long engagementId = statusString.EngagementId;
                string result = string.Empty;
                AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(this.farm, string.Empty).Api;

                result = api.GetEngagementStatus(engagementId);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in processing ServiceBus queue - unable to get status of engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }
    }
}
