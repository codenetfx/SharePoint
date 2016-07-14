// -----------------------------------------------------------------------
// <copyright file="EventReceiver.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsTimerJobs.Features.EngagementsServiceBusProcessingTimerJobFeature
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>
    [Guid("3234cbba-27a8-4056-ab29-4939f25eb78b")]
    public class EngagementsServiceBusProcessingTimerJobFeatureEventReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// Handles the event raised after a feature has been activated.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties" /> object that represents the properties of the event.</param>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
            this.DeleteJob(webApp.JobDefinitions);

            EngagementsServiceBusProcessingTimerJob serviceBusJob = new EngagementsServiceBusProcessingTimerJob(webApp);

            SPMinuteSchedule schedule = new SPMinuteSchedule();
            schedule.BeginSecond = 0;
            schedule.EndSecond = 59;
            schedule.Interval = 30;

            serviceBusJob.Schedule = schedule;
            serviceBusJob.Update();
        }

        /// <summary>
        /// Handles the event raised when a Feature is deactivated.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties" /> object that represents the properties of the event.</param>
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
            this.DeleteJob(webApp.JobDefinitions);
        }

        //// Uncomment the method below to handle the event raised after a feature has been installed.
        ////public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        ////{
        ////}

        //// Uncomment the method below to handle the event raised before a feature is uninstalled.
        ////public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        ////{
        ////}

        //// Uncomment the method below to handle the event raised when a feature is upgrading.
        ////public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        ////{
        ////}

        /// <summary>
        /// Deletes the job.
        /// </summary>
        /// <param name="jobs">The jobs.</param>
        private void DeleteJob(SPJobDefinitionCollection jobs)
        {
            foreach (SPJobDefinition job in jobs)
            {
                if (job.Name.Equals(EngagementsServiceBusProcessingTimerJob.JobName, StringComparison.OrdinalIgnoreCase))
                {
                    job.Delete();
                }
            }
        }
    }
}
