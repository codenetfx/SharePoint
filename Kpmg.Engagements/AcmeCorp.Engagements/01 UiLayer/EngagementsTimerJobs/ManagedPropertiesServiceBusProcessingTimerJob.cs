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
    public class ManagedPropertiesServiceBusProcessingTimerJob : SPJobDefinition
    {
        /// <summary>
        /// The job name
        /// </summary>
        public const string JobName = "AcmeCorp Managed Properties Service Bus Processing Timer Job";

        /// <summary>
        /// SharePoint farm in use
        /// </summary>
        private SPFarm farm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedPropertiesServiceBusProcessingTimerJob"/> class.
        /// </summary>
        public ManagedPropertiesServiceBusProcessingTimerJob()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedPropertiesServiceBusProcessingTimerJob"/> class.
        /// </summary>
        /// <param name="webApp">The web app.</param>
        public ManagedPropertiesServiceBusProcessingTimerJob(SPWebApplication webApp)
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
                case "EDITTERM":
                    this.EditTerm(item.Value);
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Updates/Edits an existing term in the engagement site collection properties.
        /// </summary>
        /// <param name="tableName">Content of the message.</param>
        /// <param name="term">Content of the message.</param>
        /// <param name="language">Content of the message.</param>
        private void EditTerm(EngagementData messageContent)
        {
            try
            {
                string newSiteUrl = string.Empty;
                AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(this.farm, string.Empty).Api;

                api.EditTerm((string)messageContent.tableName, (string)messageContent.term, (string)messageContent.language);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in editing term - unable to update properties of engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        
    }
}
