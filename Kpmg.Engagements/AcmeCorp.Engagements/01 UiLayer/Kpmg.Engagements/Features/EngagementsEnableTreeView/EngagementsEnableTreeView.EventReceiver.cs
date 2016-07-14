// -----------------------------------------------------------------------
// <copyright file="EngagementsEnableTreeView.EventReceiver.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.Features.EngagementsEnableTreeView
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using Microsoft.SharePoint;

    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>
    [Guid("4e00e575-e111-48a1-97b0-4bb3c9454f32")]
    public class EngagementsEnableTreeViewEventReceiver : SPFeatureReceiver
    {        
        /// <summary>
        /// Occurs after a Feature is activated. Handles the event raised after a feature has been activated.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties" /> object that represents the properties of the event.</param>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            // Enable TreeView and save old path to site logo
            SPWeb web = properties.Feature.Parent as SPWeb;
            try
            {
                web.Properties["OldTreeViewEnabledValue"] = web.TreeViewEnabled.ToString();
                web.Properties["OldSiteLogoUrl"] = web.SiteLogoUrl;
                web.TreeViewEnabled = true;
                web.Lists.EnsureSiteAssetsLibrary();
                web.SiteLogoUrl = web.Url + "/SiteAssets/200px-AcmeCorp_svg.png";
                web.Update();
            }
            catch (Exception)
            {
                // TODO: Exception handler
            }
        }

        /// <summary>
        /// Occurs when a Feature is deactivated. Handles the event raised before a feature is deactivated.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties" /> object that represents the properties of the event.</param>
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;
            bool convResult = false;

            try
            {
                if (web.Properties["OldTreeViewEnabledValue"] != null && bool.TryParse(web.Properties["OldTreeViewEnabledValue"], out convResult))
                {
                    web.TreeViewEnabled = convResult;
                    web.Update();
                }

                if (web.Properties["OldSiteLogoUrl"] != null)
                {
                    web.SiteLogoUrl = web.Properties["OldSiteLogoUrl"];
                    web.Update();
                }
            }
            catch (Exception)
            {
                // TODO: Exception handler
            }
        }

        // Uncomment the method below to handle the event raised after a feature has been installed.

        ////public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        ////{
        ////}

        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        ////public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        ////{
        ////}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        ////public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        ////{
        ////}
    }
}
