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
    using Microsoft.SharePoint.Taxonomy;
    using Microsoft.SharePoint.Administration;
    using AcmeCorp.Engagements.EngagementsDomain.Entities;
    using AcmeCorp.Engagements.UiHelpers.ApiFactory;
    using AcmeCorp.Engagements.EngagementsDomain;
    
    /// <summary>
    /// Timer job that connects to Service  Bus and performs processing based on items in SB Queue
    /// </summary>
    public class EngagementsSAPSyncTimerJob : SPJobDefinition
    {
        /// <summary>
        /// The job name
        /// </summary>
        public const string JobName = "AcmeCorp Engagements SAP Sync Timer Job";

        /// <summary>
        /// SharePoint farm in use
        /// </summary>
        private SPFarm farm;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngagementsSAPSyncTimerJob"/> class.
        /// </summary>
        public EngagementsSAPSyncTimerJob()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngagementsSAPSyncTimerJob"/> class.
        /// </summary>
        /// <param name="webApp">The web app.</param>
        public EngagementsSAPSyncTimerJob(SPWebApplication webApp)
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

            
            //TODO - add all 'table names'?
            string tableName="Advisory";
            string language = string.Empty;

            //calls the SAP Service and adds/deprecates terms as needed
            ProcessTermsSync(tableName, language);
            
        }

        /// <summary>
        /// Process Terms - Add and Deprecate.
        /// </summary>
        /// <param name="tableName">tableName</param>
        /// <param name="language">language</param>
        private void ProcessTermsSync(string tableName, string language)
        {
            try
            {
                AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(this.farm, string.Empty).Api;

                api.ProcessTermsSync(tableName, language);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in processing ServiceBus queue - ProcessTermsSync", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }


        /// <summary>
        /// Deprecates an existing term from the engagement site collection properties.
        /// </summary>
        /// <param name="tableName">Content of the message.</param>
        /// <param name="term">Content of the message.</param>
        /// <param name="language">Content of the message.</param>
        private void DeprecateTerm(string tableName, string term, string language)
        {
            try
            {
                string newSiteUrl = string.Empty;
                AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(this.farm, string.Empty).Api;

                api.DeprecateTerm(tableName, term); 
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in deprecating term - unable to update properties of engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }



       


        /// <summary>
        /// Adds a new term to engagement site collection properties.
        /// </summary>
        /// <param name="tableName">Content of the message.</param>
        /// <param name="term">Content of the message.</param>
        /// <param name="language">Content of the message.</param>
        private void AddTerm(string tableName, string term, string language)
        {
            try
            {
                string newSiteUrl = string.Empty;
                AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(this.farm, string.Empty).Api;

                api.AddTerm(tableName, term, language); 
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in adding term - unable to update properties of engagement site", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
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
                case "ADDTERM":
                    //this.AddTerm;
                    break;
                case "EDITTERM":
                    //this.EditTerm
                    break;
                case "DEPRECATETERM":
                    //this.DeprecateTerm;
                    break;
                
                default:
                    break;
            }
        }

       
    }
}
