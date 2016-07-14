using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Globalization;

namespace AcmeCorp.Engagements.Features.AlternativeLanguagesFeature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("71c54fee-0bfd-496a-ade0-435abda82ad6")]
    public class AlternativeLanguagesFeatureEventReceiver : SPFeatureReceiver
    {


        // Uncomment the method below to handle the event raised after a feature has been activated.
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;
            try
            {

                // Be sure the web template supports MUI. Some templates do not.
                if (web.Site.GetWebTemplates(web.Language)[web.WebTemplate].SupportsMultilingualUI)
                {
                    // Enable MUI.
                    web.IsMultilingual = true;
                    web.OverwriteTranslationsOnChange = false;

                    // Get the languages that are installed on the farm.
                    SPLanguageCollection installed = SPRegionalSettings.GlobalInstalledLanguages;

                    // Get the languages supported by this website.
                    IEnumerable<CultureInfo> supported = web.SupportedUICultures;

                    // Enable support for any installed language that is not already supported.
                    foreach (SPLanguage language in installed)
                    {
                        CultureInfo culture = new CultureInfo(language.LCID);

                        if (!supported.Contains(culture))
                        {

                            web.AddSupportedUICulture(culture);
                        }
                    }
                    web.Update();

                }

            }
            catch (Exception)
            {
                // TODO: Exception handler
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;
            try
            {

                // Get the languages that are installed on the farm.
                SPLanguageCollection installed = SPRegionalSettings.GlobalInstalledLanguages;

                // Get the languages supported by this website.
                IEnumerable<CultureInfo> supported = web.SupportedUICultures;

                // Enable support for any installed language that is not already supported.
                foreach (SPLanguage language in installed)
                {
                    if (language.LCID != (int)web.Language)
                    {
                        CultureInfo culture = new CultureInfo(language.LCID);
                        web.RemoveSupportedUICulture(culture);
                        web.Update();
                    }                    

                }

                web.IsMultilingual = false;
                web.OverwriteTranslationsOnChange = false;

                web.Update();

            }
            catch (Exception)
            {
                // TODO: Exception handler
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
