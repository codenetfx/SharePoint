using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.Office.Server.Search.ContentProcessingEnrichment;
using Microsoft.Office.Server.Search.ContentProcessingEnrichment.PropertyTypes;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.SharePoint;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading;


namespace ContentEnrichment
{

            //$ssa = Get-SPEnterpriseSearchServiceApplication
            //$config = New-SPEnterpriseSearchContentEnrichmentConfiguration
            //$config.DebugMode = $false
            //$config.Endpoint = "http://localhost:64401/EnrichmentService.svc"
            //$config.FailureMode = "WARNING"
            //$config.InputProperties = "Path"
            //$config.OutputProperties = "ProductCategory"
            //$config.SendRawData = $false
            //$config.Trigger = 'StartsWith(Path,"http://intranet.contoso.com/adventureworks/models/")'
            //Set-SPEnterpriseSearchContentEnrichmentConfiguration –SearchApplication $ssa –ContentEnrichmentConfiguration $config



    public class EnrichmentService : IContentProcessingEnrichmentService
    {
        private Dictionary<string, string> metadataList = new Dictionary<string, string>()
    {
        {"Beschreibung",""}, 
        {"Opportunity-Nr","KE-Opportunity-Nr"}, 
        {"WB-Auftrags-Nr","KE-WB-Auftrags-Nr"}, 
        {"Account","KE-Account"}, 
        {"Function","KE-Function"}, 
        {"Sales Manager","KE-SalesManager"}, 
        {"Eng.Manager","KE-Eng-Manager"}, 
        {"Eng.Partner","KE-Eng-Partner"}, 
        {"Niederlassung","KE-Niederlassung"}, 
        {"PS-Status","KE-status"}, 
        {"Bezeichnung","KE-Bezeichnung"}, 
        {"LoB","KE-LoB"}, 
        {"Service Area","KE-Service-Area"}, 
        {"KPMG Gesellschaft","KE-Gesellschaft"}, 
    };

        public ProcessedItem ProcessItem(Item item)
        {
            ProcessedItem processedItem = new ProcessedItem();
            processedItem.ItemProperties = new List<AbstractProperty>();

            AbstractProperty pathProperty = item.ItemProperties.Where(p => p.Name == "Path").FirstOrDefault();
            if (pathProperty != null)
            {
                Property<string> pathProp = pathProperty as Property<string>;
                if (pathProp != null)
                {

                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPSite currentSiteCollection = new SPSite("https://projectspace-test.de.kworld.kpmg.com/ "))
                        {
                            using (SPWeb currentWeb = currentSiteCollection.OpenWeb())
                            {
                                // unsafe updates are required to be able to write to the property bag
                                currentWeb.AllowUnsafeUpdates = true;

                                try
                                {
                                    foreach (var managedProperty in metadataList)
                                    {
                                        //if (pathProp.Value.Contains(managedProperty.Key))
                                        //{
                                            Property<string> modelProp = new Property<string>()
                                            {
                                                Name = managedProperty.Value,
                                                Value = currentWeb.AllProperties[managedProperty.Value].ToString()
                                            };
                                            processedItem.ItemProperties.Add(modelProp);
                                        //}
                                    }
                                }
                                catch { //todo
                                }
 
                            }
                        }
                    });

                    
                }
            }

            return processedItem;
        }
    }
}
