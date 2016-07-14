// -----------------------------------------------------------------------
// <copyright file="EngagementsAcmeCorpBranding.EventReceiver.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.Features.EngagementsAcmeCorpBranding
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;

    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>
    [Guid("40a02042-498e-4f97-a97f-35cb9e534767")]
    public class EngagementsAcmeCorpBrandingEventReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// Occurs after a Feature is activated. Handle the event raised after a feature has been activated.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties" /> object that represents the properties of the event.</param>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;
            SPWeb web = site.RootWeb;
            SPFile sharePointColorFile = web.GetFile(web.Url + "/_catalogs/theme/15/PaletteAcmeCorp.SPCOLOR");
            SPTheme sharePointTheme = SPTheme.Open("AcmeCorpProgrammaticalTheme", sharePointColorFile);
            sharePointTheme.ApplyTo(web, true);
        }

        // Uncomment the method below to handle the event raised before a feature is deactivated.

        ////public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        ////{

        ////}

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
