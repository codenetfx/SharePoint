// -----------------------------------------------------------------------
// <copyright file="ServiceBus.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    using AcmeCorp.Engagements.EngagementsDomain;
    using AcmeCorp.Engagements.EngagementsDomain.Entities;
    using Acme.Core;
    using Acme.Core.DiagnosticSystem.Enums;
    using Acme.Core.DiagnosticSystem.ExceptionEntities;
    using Acme.Core.DiagnosticSystem.ExceptionManager;

    /// <summary>
    /// Service Bus processing logic
    /// </summary>
    public partial class ServiceBus
    {
        /// <summary>
        /// Service Bus message factory
        /// </summary>
        MessagingFactory messageFactory;

        /// <summary>
        /// Service Bus namespace manager
        /// </summary>
        NamespaceManager namespaceManager;

        /// <summary>
        /// Service Bus queue client
        /// </summary>
        QueueClient myQueueClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBus" /> class.
        /// </summary>
        /// <param name="serviceBusConnectionString">Service Bus connection string.</param>
        public ServiceBus(string serviceBusConnectionString)
        {
            if (serviceBusConnectionString == null || serviceBusConnectionString == string.Empty)
            {
                return;
            }

            this.messageFactory = MessagingFactory.CreateFromConnectionString(serviceBusConnectionString);
            this.namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);

            if (this.namespaceManager == null)
            {
                return;
            }
        }

        /// <summary>
        /// Creates the Service Bus queue client.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns>Queue Client</returns>
        public QueueClient CreateSbQueueClient(string queueName)
        {
            try
            {
                if (!this.namespaceManager.QueueExists(queueName))
                {
                    this.namespaceManager.CreateQueue(new QueueDescription(queueName)
                    {
                        // Minimum value
                        DuplicateDetectionHistoryTimeWindow = new TimeSpan(0, 0, 20),
                        RequiresDuplicateDetection = false
                    });
                }

                this.myQueueClient = this.messageFactory.CreateQueueClient(queueName);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in creating ServiceBus queue client.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }

            return this.myQueueClient;
        }

        /// <summary>
        /// Sends the brokered message.
        /// </summary>
        /// <param name="messageLabel">The message label.</param>
        /// <param name="messageToSend">The message to send.</param>
        /// <param name="queueClient">The queue client.</param>
        public void SendBrokeredMessage(string messageLabel, EngagementData messageToSend, QueueClient queueClient)
        {
            try
            {
                BrokeredMessage sendMessage = new BrokeredMessage(messageToSend);
                sendMessage.Label = messageLabel;
                this.myQueueClient.Send(sendMessage);
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in sending ServiceBus brokered message to queue client.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Receives the brokered message.
        /// </summary>
        /// <param name="queueClient">The queue client.</param>
        /// <returns>string message</returns>
        public string ReceiveBrokeredMessage(QueueClient queueClient)
        {
            string returnMessage = string.Empty;
            try
            {
                // Receive the message from the queue
                BrokeredMessage receivedMessage = this.myQueueClient.Receive(TimeSpan.FromSeconds(5));
                if (receivedMessage != null)
                {
                    returnMessage = receivedMessage.GetBody<string>();
                    receivedMessage.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in receiving brokered message from ServiceBus.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }

            return returnMessage;
        }

        /// <summary>
        /// Receives the brokered message collection.
        /// </summary>
        /// <param name="queueClient">The queue client.</param>
        /// <returns>Collection of brokered messages</returns>
        public Dictionary<string, EngagementData> ReceiveBrokeredMessageCollection(QueueClient queueClient)
        {
            Dictionary<string, EngagementData> returnMessages = new Dictionary<string, EngagementData>();
            try
            {
                // Receive the message from the queue
                IEnumerable<BrokeredMessage> receivedMessages = this.myQueueClient.ReceiveBatch(20000);
                foreach (var item in receivedMessages)
                {
                    if (item != null)
                    {
                        returnMessages.Add(item.Label, item.GetBody<EngagementData>());
                        item.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in receiving collection of brokered messages from ServiceBus queue.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }

            return returnMessages;
        }

        /// <summary>
        /// Closes the message factory.
        /// </summary>
        /// <param name="messageFactory">The message factory.</param>
        public void CloseMessageFactory(MessagingFactory messageFactory)
        {
            try
            {
                if (messageFactory != null)
                {
                    messageFactory.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeApplicationException(0, "Error in closing ServiceBus message factory.", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }
    }
}
