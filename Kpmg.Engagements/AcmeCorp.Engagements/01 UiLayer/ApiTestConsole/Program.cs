// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.ApiTestConsole
{
    using AcmeCorp.Engagements.UiHelpers.ApiFactory;
    using Microsoft.SharePoint;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Test console
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "*", Justification = "Test application")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.SpacingRules", "*", Justification = "Test application")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Test application")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.LayoutRules", "*", Justification = "Test application")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "*", Justification = "Test application")]
    public class Program
    {
        /// <summary>
        /// Console entry method
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main(string[] args)
        {
            try
            {

                //DeleteAcmeCorpFarmProperties();

                string ouGroups = "";
                using (SPSite site = new SPSite("https://pspace"))
                {
                    AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(site.WebApplication.Farm, "https://pspace;").Api;

                    string[] owners = { "SHAREPOINTDEV\\Harroverty", "SHAREDOVE\\Apither" };
                    string[] deputies = { "SHAREPOINTDEV\\Magur1987", "SHAREDOVE\\Magur1987" };

                    api.CreateNewInternalSite("One internal site", "Description of a internal site", owners, deputies);

                    //ouGroups = api.AdDeputyGroupsOu;
                };


                //string url = CreateEngagement(api, 9900003);

                //AcmeCorp.Engagements.EngagementsApi.Utilities.ActiveDirectoryHelpers adHelpers = new EngagementsApi.Utilities.ActiveDirectoryHelpers();

                //adHelpers.CreateAdGroupInOu("MyTestGroup", ouGroups);


                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        /// <summary>
        /// Creates the engagement.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <param name="engagementId">The engagement id.</param>
        /// <returns>Engagement URL</returns>
        private static string CreateEngagement(AcmeCorp.Engagements.EngagementsApi.Api api, int engagementId)
        {
            string[] Managers = { "SHAREDOVE\\Ottly1937" };
            string[] Partners = { "SHAREDOVE\\Whory1990" };
            string[] Staff = { "SHAREDOVE\\Waragod", "SHAREDOVE\\Ropened" };

            Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

            engagementProperties.Add("Name des Mandanten", "Company");
            engagementProperties.Add("Opportunity Nr", engagementId.ToString());
            engagementProperties.Add("Account", "Company AG");
            engagementProperties.Add("Concurring Partner", Partners[0]);
            engagementProperties.Add("Niederlassung", "Berlin");
            engagementProperties.Add("Bezeichnung", "Some description");
            engagementProperties.Add("WB-Auftrags-Nr", engagementId.ToString());
            engagementProperties.Add("Eng.Partner", Partners[0]);
            engagementProperties.Add("Eng.Manager", Managers[0]);
            engagementProperties.Add("WB-Auftrag Status", "Open");
            engagementProperties.Add("WB-Auftrag Status Datum", DateTime.Now.Date);

            //TODO:
            //to be used later for partner and manager groups
            engagementProperties.Add("partnergroup", string.Empty);
            engagementProperties.Add("managergroup", string.Empty);

            string engagementUrl = api.CreateNewEngagementSite(engagementId, EngagementsDomain.Enums.EngagementFoldersType.StandardFolders, Managers, Partners, Staff, engagementProperties);

            return engagementUrl;
        }

        private static void DeleteAcmeCorpFarmProperties()
        {
            using (SPSite oSPsite = new SPSite("https://pspace"))
            {
                using (SPWeb oSPWeb = oSPsite.OpenWeb())
                {

                    Hashtable allprops = oSPsite.WebApplication.Farm.Properties;

                    List<string> keys = new List<string>();

                    foreach (string key in allprops.Keys)
                    {
                        keys.Add(key);
                    }

                    foreach (string key in keys)
                    {
                        if (key.StartsWith("AcmeCorp") || key.StartsWith("Acme"))
                        {
                            oSPsite.WebApplication.Farm.Properties.Remove(key);
                        }
                    }

                    oSPsite.WebApplication.Farm.Update();
                }
            }
        }
    }
}
