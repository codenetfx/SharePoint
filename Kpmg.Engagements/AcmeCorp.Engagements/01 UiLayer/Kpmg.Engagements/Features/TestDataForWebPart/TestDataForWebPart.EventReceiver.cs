using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;

namespace AcmeCorp.Engagements.Features.TestDataForWebPart
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("30e3b7a6-7825-4add-bb27-5e666aeae7c1")]
    public class TestDataForWebPartEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb targetWeb = properties.Feature.Parent as SPWeb;
            int intValueSmall = 123;
            targetWeb.AllProperties["status"] = "Status field";
            targetWeb.AllProperties["statusdate"] = new DateTime(1976, 11, 20, 19, 23, 45);
            targetWeb.AllProperties["Name des Mandanten"] = "Value - Name Des Mandaten";
            targetWeb.AllProperties["Opportunity Nr"] = intValueSmall;
            targetWeb.AllProperties["Account"] = "Value-Account";
            targetWeb.AllProperties["Concurring Partner"] = "Value-Concurring Partner";
            targetWeb.AllProperties["Niederlassung"] = "Value - Niederlassung";
            targetWeb.AllProperties["Bezeichnung"] = "Value - Bezeichnung";
            targetWeb.AllProperties["WB-Auftrags-Nr"] = 123456;
            targetWeb.AllProperties["Eng.Partner"] = "Value - Eng.Partner";
            targetWeb.AllProperties["Eng.Manager"] = "Value - Eng.Manager";
            targetWeb.AllProperties["WB-Auftrag Status"] = "Value - WB-Auftrag Status";
            targetWeb.AllProperties["WB-Auftrag Status Datum"] = new DateTime(2012, 11, 23);

            targetWeb.IndexedPropertyKeys.Add("status");
            targetWeb.IndexedPropertyKeys.Add("statusdate");
            targetWeb.IndexedPropertyKeys.Add("Name des Mandanten");
            targetWeb.IndexedPropertyKeys.Add("Opportunity Nr");
            targetWeb.IndexedPropertyKeys.Add("Account");
            targetWeb.IndexedPropertyKeys.Add("Concurring Partner");
            targetWeb.IndexedPropertyKeys.Add("Niederlassung");
            targetWeb.IndexedPropertyKeys.Add("Bezeichnung");
            targetWeb.IndexedPropertyKeys.Add("WB-Auftrags-Nr");
            targetWeb.IndexedPropertyKeys.Add("Eng.Partner");
            targetWeb.IndexedPropertyKeys.Add("Eng.Manager");
            targetWeb.IndexedPropertyKeys.Add("WB-Auftrag Status");
            targetWeb.IndexedPropertyKeys.Add("WB-Auftrag Status Datum");
            targetWeb.Update();
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


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
