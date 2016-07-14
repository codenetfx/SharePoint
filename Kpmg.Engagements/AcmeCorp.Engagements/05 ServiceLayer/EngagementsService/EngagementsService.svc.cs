// -----------------------------------------------------------------------
// <copyright file="EngagementsService.svc.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Web.Configuration;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using AcmeCorp.Engagements;
    using AcmeCorp.Engagements.EngagementsDomain;
    using AcmeCorp.Engagements.EngagementsDomain.Entities;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.

    /// <summary>
    /// Project Space web service
    /// </summary>
    public class EngagementsService : IEngagementsService
    {
        // Method that is used for testing ServiceBus relaying capabilities

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string GetData(string value)
        {
            string retval = string.Empty;
            string sbConnectionString = WebConfigurationManager.AppSettings["ServiceBusConnectionString"].ToString();
            string sbQueueName = WebConfigurationManager.AppSettings["ServiceBusQueueName"].ToString();

            AcmeCorp.Engagements.EngagementsServiceBus.ServiceBus serviceBus = new EngagementsServiceBus.ServiceBus(sbConnectionString);
            QueueClient queueClient = serviceBus.CreateSbQueueClient(sbQueueName);
            queueClient.PrefetchCount = 200;
            Dictionary<string, EngagementData> serviceBusRetVal = serviceBus.ReceiveBrokeredMessageCollection(queueClient);
            foreach (var item in serviceBusRetVal)
            {
                string label = item.Key;
                EngagementData engagementData = item.Value;
                retval += label + engagementData.EngagementId + "\n";
                if (engagementData.Managers != null)
                {
                    retval += engagementData.Managers.ToString();
                }
            }

            return string.Format("You entered: {0}", retval);
        }

        /// <summary>
        /// Gets the data using data contract.
        /// </summary>
        /// <param name="composite">The composite.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">composite</exception>
        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }

            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }

            return composite;
        }



        /// <summary>
        /// Edit Term.
        /// </summary>
        /// <param name="tableName">tablename.</param>
        /// <param name="term">term</param>
        /// <param name="language">language</param>
        /// <returns></returns>
        public string EditTerm(string tablename, string term, string language)
        {
            string editTerm = string.Empty;
            string sbConnectionString = WebConfigurationManager.AppSettings["ServiceBusConnectionString"].ToString();
            string sbQueueName = WebConfigurationManager.AppSettings["ServiceBusQueueName"].ToString();

            try
            {
                AcmeCorp.Engagements.EngagementsServiceBus.ServiceBus serviceBus = new EngagementsServiceBus.ServiceBus(sbConnectionString);
                QueueClient queueClient = serviceBus.CreateSbQueueClient(sbQueueName);
                EngagementData engagementData = new EngagementData()
                {
                    tableName = tablename,
                    term = term,
                    language = language
                   
                };

                serviceBus.SendBrokeredMessage("EDITTERM#" + term.ToString(), engagementData, queueClient);
            }
            catch (Exception ex)
            {
                editTerm = "Error in Edit Term posting: " + ex.Message;
            }

            return editTerm;
        }

        /// <summary>
        /// Creates the new engagement site.
        /// </summary>
        /// <param name="engagementId">The engagement id.</param>
        /// <param name="engagementFoldersType">Type of the engagement folders.</param>
        /// <param name="managers">The managers.</param>
        /// <param name="partners">The partners.</param>
        /// <param name="staff">The staff.</param>
        /// <param name="engagementProperties">The engagement properties.</param>
        /// <returns></returns>
        public string CreateNewEngagementSite(long engagementId, int engagementFoldersType, List<string> managers, List<string> partners, List<string> staff, Dictionary<string, object> engagementProperties)
        {
            string newSiteUrl = string.Empty;
            string sbConnectionString = WebConfigurationManager.AppSettings["ServiceBusConnectionString"].ToString();
            string sbQueueName = WebConfigurationManager.AppSettings["ServiceBusQueueName"].ToString();

            try
            {               
                AcmeCorp.Engagements.EngagementsServiceBus.ServiceBus serviceBus = new EngagementsServiceBus.ServiceBus(sbConnectionString);
                QueueClient queueClient = serviceBus.CreateSbQueueClient(sbQueueName);
                EngagementData engagementData = new EngagementData()
                    {
                        EngagementId = engagementId,
                        EngagementFoldersType = engagementFoldersType,
                        Managers = managers,
                        Partners = partners,
                        Staff = staff,
                        EngagementProperties = engagementProperties
                    };

                serviceBus.SendBrokeredMessage("CREATEENGAGEMENT#" + engagementId.ToString(), engagementData, queueClient);                
            }
            catch (Exception ex)
            {
                newSiteUrl = "Error in engagement site creation: " + ex.Message;
            }

            return newSiteUrl;
        }

        /// <summary>
        /// Creates the new engagement site.
        /// </summary>
        /// <param name="opportunityId">The opportunity id.</param>
        /// <param name="managers">The managers.</param>
        /// <param name="partners">The partners.</param>
        /// <param name="staff">The staff.</param>
        /// <param name="opportunityProperties">The engagement properties.</param>
        /// <returns></returns>
        public string CreateNewOpportunitySite(long opportunityId, List<string> managers, List<string> partners, List<string> staff, Dictionary<string, object> opportunityProperties)
        {
            string newSiteUrl = string.Empty;
            string sbConnectionString = WebConfigurationManager.AppSettings["ServiceBusConnectionString"].ToString();
            string sbQueueName = WebConfigurationManager.AppSettings["ServiceBusQueueName"].ToString();

            try
            {               
                AcmeCorp.Engagements.EngagementsServiceBus.ServiceBus serviceBus = new EngagementsServiceBus.ServiceBus(sbConnectionString);
                QueueClient queueClient = serviceBus.CreateSbQueueClient(sbQueueName);
                EngagementData engagementData = new EngagementData()
                {
                    EngagementId = opportunityId,

                    Managers = managers,
                    Partners = partners,
                    Staff = staff,
                    EngagementProperties = opportunityProperties
                };

                serviceBus.SendBrokeredMessage("CREATEOPPORTUNITY#" + opportunityId.ToString(), engagementData, queueClient);                
            }
            catch (Exception ex)
            {
                newSiteUrl = "Error in opportunity site creation: " + ex.Message;
            }

            return newSiteUrl;
        }

        /// <summary>
        /// Updates properties of engagement site.
        /// </summary>
        /// <param name="engagementId">The engagement id.</param>
        /// <param name="engagementFoldersType">Type of the engagement folders.</param>
        /// <param name="managers">The managers.</param>
        /// <param name="partners">The partners.</param>
        /// <param name="staff">The staff.</param>
        /// <param name="engagementProperties">The engagement properties.</param>
        /// <returns></returns>
        public string UpdateEngagementSiteProperties(long engagementId, int engagementFoldersType, List<string> managers, List<string> partners, List<string> staff, Dictionary<string, object> engagementProperties)
        {
            string newSiteUrl = string.Empty;
            string sbConnectionString = WebConfigurationManager.AppSettings["ServiceBusConnectionString"].ToString();
            string sbQueueName = WebConfigurationManager.AppSettings["ServiceBusQueueName"].ToString();

            try
            {
                AcmeCorp.Engagements.EngagementsServiceBus.ServiceBus serviceBus = new EngagementsServiceBus.ServiceBus(sbConnectionString);
                QueueClient queueClient = serviceBus.CreateSbQueueClient(sbQueueName);
                EngagementData engagementData = new EngagementData()
                {
                    EngagementId = engagementId,
                    EngagementFoldersType = engagementFoldersType,
                    Managers = managers,
                    Partners = partners,
                    Staff = staff,
                    EngagementProperties = engagementProperties
                };

                serviceBus.SendBrokeredMessage("UPDATEENGAGEMENT#" + engagementId.ToString(), engagementData, queueClient);
            }
            catch (Exception ex)
            {
                newSiteUrl = "Error in engagement site update: " + ex.Message;
            }

            return newSiteUrl;
        }

        /// <summary>
        /// Updates properties of opportunity site.
        /// </summary>
        /// <param name="opportunityId">The opportunity id.</param>
        /// <param name="managers">The managers.</param>
        /// <param name="partners">The partners.</param>
        /// <param name="staff">The staff.</param>
        /// <param name="opportunityProperties">The opportunity properties.</param>
        /// <returns></returns>
        public string UpdateOpportunitySiteProperties(long opportunityId, List<string> managers, List<string> partners, List<string> staff, Dictionary<string, object> opportunityProperties)
        {
            string newSiteUrl = string.Empty;
            string sbConnectionString = WebConfigurationManager.AppSettings["ServiceBusConnectionString"].ToString();
            string sbQueueName = WebConfigurationManager.AppSettings["ServiceBusQueueName"].ToString();

            try
            {                
                AcmeCorp.Engagements.EngagementsServiceBus.ServiceBus serviceBus = new EngagementsServiceBus.ServiceBus(sbConnectionString);
                QueueClient queueClient = serviceBus.CreateSbQueueClient(sbQueueName);
                EngagementData engagementData = new EngagementData()
                {
                    EngagementId = opportunityId,

                    Managers = managers,
                    Partners = partners,
                    Staff = staff,
                    EngagementProperties = opportunityProperties
                };

                serviceBus.SendBrokeredMessage("UPDATEOPPORTUNITY#" + opportunityId.ToString(), engagementData, queueClient);                
            }
            catch (Exception ex)
            {
                newSiteUrl = "Error in engagement site update: " + ex.Message;
            }

            return newSiteUrl;
        }

        /// <summary>
        /// Gets the engagement status.
        /// </summary>
        /// <param name="engagementId">The engagement id.</param>
        /// <returns></returns>
        public string GetEngagementStatus(long engagementId)
        {
            string newSiteUrl = string.Empty;
            string sbConnectionString = WebConfigurationManager.AppSettings["ServiceBusConnectionString"].ToString();
            string sbQueueName = WebConfigurationManager.AppSettings["ServiceBusQueueName"].ToString();

            try
            {                
                AcmeCorp.Engagements.EngagementsServiceBus.ServiceBus serviceBus = new EngagementsServiceBus.ServiceBus(sbConnectionString);
                QueueClient queueClient = serviceBus.CreateSbQueueClient(sbQueueName);
                EngagementData engagementData = new EngagementData()
                {
                    EngagementId = engagementId,
                };

                serviceBus.SendBrokeredMessage("GETENGAGEMENTSTATUS#" + engagementId.ToString(), engagementData, queueClient);                
            }
            catch (Exception ex)
            {
                newSiteUrl += "Error while trying to get status of engagement with ID: " + engagementId.ToString();
                newSiteUrl += "\n" + ex.Message;
            }

            return newSiteUrl;
        }

        /// <summary>
        /// Closes the engagement.
        /// </summary>
        /// <param name="engagementId">The engagement id.</param>
        /// <returns></returns>
        public string CloseEngagement(long engagementId)
        {
            string operationStatus = string.Empty;
            string sbConnectionString = WebConfigurationManager.AppSettings["ServiceBusConnectionString"].ToString();
            string sbQueueName = WebConfigurationManager.AppSettings["ServiceBusQueueName"].ToString();
            
            try
            {               
                AcmeCorp.Engagements.EngagementsServiceBus.ServiceBus serviceBus = new EngagementsServiceBus.ServiceBus(sbConnectionString);
                QueueClient queueClient = serviceBus.CreateSbQueueClient(sbQueueName);
                EngagementData engagementData = new EngagementData()
                {
                    EngagementId = engagementId,
                };

                serviceBus.SendBrokeredMessage("CLOSEENGAGEMENT#" + engagementId.ToString(), engagementData, queueClient);                
            }
            catch (Exception ex)
            {
                operationStatus += "Error while trying to close engagement with ID: " + engagementId.ToString();
                operationStatus += "\n" + ex.Message;
            }

            return operationStatus;
        }

        /// <summary>
        /// Creates the engagement from opportunity.
        /// </summary>
        /// <param name="opportunityId">The opportunity id.</param>
        /// <returns></returns>
        public string CreateEngagementFromOpportunity(long opportunityId)
        {
            string operationStatus = string.Empty;
            string sbConnectionString = WebConfigurationManager.AppSettings["ServiceBusConnectionString"].ToString();
            string sbQueueName = WebConfigurationManager.AppSettings["ServiceBusQueueName"].ToString();
           
            try
            {                
                AcmeCorp.Engagements.EngagementsServiceBus.ServiceBus serviceBus = new EngagementsServiceBus.ServiceBus(sbConnectionString);
                QueueClient queueClient = serviceBus.CreateSbQueueClient(sbQueueName);
                EngagementData engagementData = new EngagementData()
                {
                    EngagementId = opportunityId,
                };

                serviceBus.SendBrokeredMessage("CONVERTTOENGAGEMENT#" + opportunityId.ToString(), engagementData, queueClient);                
            }
            catch (Exception ex)
            {
                operationStatus += "Error while trying to close engagement with ID: " + opportunityId.ToString();
                operationStatus += "\n" + ex.Message;
            }

            return operationStatus;
        }

        /// <summary>
        /// Reopens the engagement.
        /// </summary>
        /// <param name="engagementId">The engagement id.</param>
        /// <returns></returns>
        public string ReopenEngagement(long engagementId)
        {
            string operationStatus = string.Empty;
            string sbConnectionString = WebConfigurationManager.AppSettings["ServiceBusConnectionString"].ToString();
            string sbQueueName = WebConfigurationManager.AppSettings["ServiceBusQueueName"].ToString();

            try
            {
                AcmeCorp.Engagements.EngagementsServiceBus.ServiceBus serviceBus = new EngagementsServiceBus.ServiceBus(sbConnectionString);
                QueueClient queueClient = serviceBus.CreateSbQueueClient(sbQueueName);
                EngagementData engagementData = new EngagementData()
                {
                    EngagementId = engagementId,
                };

                serviceBus.SendBrokeredMessage("REOPENENGAGEMENT#" + engagementId.ToString(), engagementData, queueClient);
            }
            catch (Exception ex)
            {
                operationStatus += "Error while trying to reopen engagement with ID: " + engagementId.ToString();
                operationStatus += "\n" + ex.Message;
            }

            return operationStatus;
        }
    }
}
