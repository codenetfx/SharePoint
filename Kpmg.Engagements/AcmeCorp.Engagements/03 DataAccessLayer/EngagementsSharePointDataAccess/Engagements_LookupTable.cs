// -----------------------------------------------------------------------
// <copyright file="EngagementsSharePointDal_Engagements.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsSharePointDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.Taxonomy;
    using Acme.SharePointCore.Helpers;
    using AcmeCorp.Engagements.SAPLookupTableService;
    
    
    /// <summary>
    /// SharePoint DataAccess Layer for AcmeCorp Engagements
    /// </summary>
    public partial class EngagementsSharePointDal : AcmeCorp.Engagements.EngagementsDomain.IAcmeCorpEngagementsDalInterface
    {


        public AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows[] GetDataFromLookupTable(string tableName, string language)
        {
            AcmeCorp.Engagements.SAPLookupTableService.SAPLookupService instance = new SAPLookupTableService.SAPLookupService();
            return(instance.GetRowsFromTable(tableName, language));
        }





        /// <summary>
        /// Processes terms by comparing SAP Service response with custom properties stored in each SC term store 
        /// </summary>
        /// <param name="termset">Engagement folders type.</param>
        /// <param name="term">Engagement owners.</param>
        /// <param name="language">Engagement partners.</param>
        /// <returns>
        /// nothing
        /// </returns>
        public void ProcessTermsAsync(string termset, string language)
        {
            bool needsUpdate = false;
            Engagements.EngagementsDomain.CustomTableQueryResponseRows[] rows;
            //string newSiteUrl = string.Empty;
            //string sbConnectionString = WebConfigurationManager.AppSettings["ServiceBusConnectionString"].ToString();
            //string sbQueueName = WebConfigurationManager.AppSettings["ServiceBusQueueName"].ToString();

            rows = GetDataFromLookupTable(termset, language);

            List<Engagements.EngagementsDomain.CustomTableQueryResponseRows> rowList = rows.ToList(); //<Engagements.EngagementsDomain.CustomTableQueryResponseRows>;

            try
            {

                // Get all Site Collections
                SPWebApplication webApplication = SPContext.Current.Site.WebApplication;
                SPSiteCollection siteCollections = webApplication.Sites;


                foreach (SPSite siteCollection in siteCollections)
                {
                    //in each site collection, search through term store for a given term


                    TaxonomySession session = new TaxonomySession(siteCollection);

                    //TODO - Read from Configuration
                    TermStore termStore = session.TermStores["AcmeCorp"];


                    TermCollection terms = session.GetTerms(language, true);

                    // Search all Terms that start with the provided term from
                    // all TermStores associated with the provided site.
                    //TermCollection terms = session.GetTerms(term,
                    //    true, // Only search in default labels
                    //    StringMatchOption.StartsWith,
                    //    5,  // The maximum number of terms returned from each TermStore
                    //    true); // The results should not contain unavailable terms

                    //update each found term

                    foreach (Engagements.EngagementsDomain.CustomTableQueryResponseRows rowItem in rowList)
                    {
                        string name = rowItem.Key;

                        Term settingsTerm = terms.Where(x => x.Name == name).FirstOrDefault();
                        if (settingsTerm == null) //term does not exist - add term
                        {
                            AddTerm(termset, name, language, this.rootSiteCollection.ToString());
                            //settingsTerm = terms.CreateTerm(name, terms.TermStore.DefaultLanguage);
                        }

                        if (settingsTerm != null) // term does exist - edit
                        {
                            //EditTerm(termset, name, language, SPSiteURL);
                            
                        

                        }

                    }


                    // To determine which terms to deprecate, we have to look for terms which exist in term store but not in SAP response
                    foreach (Term termItem in terms)
                    {

                        string key = rowList.Where(x => x.Key == termItem.Name).FirstOrDefault().Key;
                        if (key == null) // term exists in term store but was removed in SAP - deprecate
                        {
                            DeprecateTerm(termset, termItem.Name, this.rootSiteCollection.ToString());
                        }


                    }

                    //commit changes
                    termStore.CommitAll();

                } // foreach SC

            }

            catch (Exception ex)
            {
                //newSiteUrl = "Error in opportunity site creation: " + ex.Message;
            }

        } //ProcessTermsAsync


        /// <summary>
        /// Processes terms by comparing SAP Service response with custom properties stored in each SC term store 
        /// </summary>
        /// <param name="termset">Engagement folders type.</param>
        /// <param name="term">Engagement owners.</param>
        /// <param name="language">Engagement partners.</param>
        /// <returns>
        /// nothing
        /// </returns>
        public void ProcessTermsSync(string termset, string language)
        {
            Engagements.EngagementsDomain.CustomTableQueryResponseRows[] rows;
            rows = GetDataFromLookupTable(termset, language);
            List<Engagements.EngagementsDomain.CustomTableQueryResponseRows> rowList = rows.ToList(); //<Engagements.EngagementsDomain.CustomTableQueryResponseRows>;
            try
            {
                    TaxonomySession session = new TaxonomySession(new SPSite(this.rootSiteCollection));

                    //TODO - Read from Configuration
                    TermStore termStore = session.TermStores["AcmeCorp"];


                    TermCollection terms = session.GetTerms(language, true);

                    // Search all Terms that start with the provided term from
                    // all TermStores associated with the provided site.
                    //TermCollection terms = session.GetTerms(term,
                    //    true, // Only search in default labels
                    //    StringMatchOption.StartsWith,
                    //    5,  // The maximum number of terms returned from each TermStore
                    //    true); // The results should not contain unavailable terms

                    //update each found term

                    foreach (Engagements.EngagementsDomain.CustomTableQueryResponseRows rowItem in rowList)
                    {
                        string name = rowItem.Key;

                        Term settingsTerm = terms.Where(x => x.Name == name).FirstOrDefault();
                        if (settingsTerm == null) //term does not exist - add term
                        {
                            AddTerm(termset, name, language, this.rootSiteCollection.ToString());             
                        }

                    }
                        

                    // To determine which terms to deprecate, we have to look for terms which exist in term store but not in SAP response
                    foreach (Term termItem in terms)
                    {
                          
                        string key = rowList.Where(x => x.Key == termItem.Name).FirstOrDefault().Key;
                        if (key == null) // term exists in term store but was removed in SAP - deprecate
                        {
                            DeprecateTerm(termset, termItem.Name, this.rootSiteCollection.ToString());
                        }

                        
                    }

                    //commit changes
                    termStore.CommitAll();


                   

                }
           
            catch (Exception ex)
            {
                //newSiteUrl = "Error in opportunity site creation: " + ex.Message;
            }

        } //ProcessTermsSync

        /// <summary>
        /// Adds a term to the term store 
        /// </summary>
        /// <param name="termstore">Engagement folders type.</param>
        /// <param name="term">Engagement owners.</param>
        /// <param name="language">Engagement partners.</param>
        /// <returns>
        /// nothing
        /// </returns>
        public void AddTerm(string termSetName, string term, string language, string SPSiteURL)
        {
            bool needsUpdate = false;

            using (SPSite site = new SPSite(SPSiteURL))
            {
                TaxonomySession _TaxonomySession = new TaxonomySession(site);


                //Get instance of the Term Store 
                //TODO: Provision term store with a script or programatically

                TermStore store = _TaxonomySession.TermStores["AcmeCorp"];

                string groupName = "AcmeCorp SAP Lookup tables";


                //Now create a new Term Group

                //bool canWrite = store.DoesUserHavePermissions(TaxonomyRights.Contributor);

                GroupCollection groups = store.Groups;

                // if taxonomy group does not exist, create one...
                Group settingsGroup = store.Groups.Where(x => x.Name == groupName).FirstOrDefault();
                if (settingsGroup == null)
                {
                    settingsGroup = store.CreateGroup(groupName);
                    needsUpdate = true;
                }

                TermSet settingsTermSet = settingsGroup.TermSets.Where(x => x.Name == termSetName).FirstOrDefault();
                if (settingsTermSet == null)
                {
                    settingsTermSet = settingsGroup.CreateTermSet(termSetName);
                    settingsTermSet.IsAvailableForTagging = false;
                    needsUpdate = true;
                }

                TermCollection settingsTerm = settingsGroup.TermStore.GetTerms(term, true);
                if (settingsTerm == null)
                {
                    Term newTerm = settingsTermSet.CreateTerm(term, 1033);
                    newTerm.IsAvailableForTagging = false;
                    needsUpdate = true;
                }


                if (needsUpdate)
                {
                    store.CommitAll();
                }


            }

        }

        /// <summary>
        /// Edit a term to the term store 
        /// </summary>
        /// <param name="termstore">Engagement folders type.</param>
        /// <param name="term">Engagement owners.</param>
        /// <param name="language">Engagement partners.</param>
        /// <returns>
        /// nothing
        /// </returns>
        public void EditTerm(string termstore, string term, string language, string SPSiteURL)
        {
            bool needsUpdate = false;

            try
            {

                // Get all Site Collections
                SPWebApplication webApplication = SPContext.Current.Site.WebApplication;
                SPSiteCollection siteCollections = webApplication.Sites;

                foreach (SPSite siteCollection in siteCollections)
                {
                    //in each site collection, search through term store for a given term


                    TaxonomySession session = new TaxonomySession(siteCollection);

                    TermStore termStore = session.TermStores[0];

                    // Search all Terms that start with the provided term from
                    // all TermStores associated with the provided site.
                    TermCollection terms = session.GetTerms(term,
                        true, // Only search in default labels
                        StringMatchOption.StartsWith,
                        5,  // The maximum number of terms returned from each TermStore
                        true); // The results should not contain unavailable terms

                    //update each found term
                    foreach (Term termItem in terms)
                    {
                        termItem.Name = term;
                    }

                    //commit changes
                    termStore.CommitAll();

                }
            }
            catch (Exception ex)
            {
                //newSiteUrl = "Error in opportunity site creation: " + ex.Message;
            }
        }

        /// <summary>
        /// Deprecate a term from the term store 
        /// </summary>
        /// <param name="termstore">Engagement folders type.</param>
        /// <param name="term">Engagement owners.</param>
        /// <param name="language">Engagement partners.</param>
        /// <returns>
        /// nothing
        /// </returns>
        public void DeprecateTerm(string termSetName, string term, string SPSiteURL)
        {
            bool needsUpdate = false;

            using (SPSite site = new SPSite(SPSiteURL))
            {
                TaxonomySession _TaxonomySession = new TaxonomySession(site);


                //Get instance of the Term Store 
                //TODO: Provision term store with a script or programatically

                TermStore store = _TaxonomySession.TermStores["AcmeCorp"];

                string groupName = "AcmeCorp SAP Lookup tables";


                //Now create a new Term Group

                //bool canWrite = store.DoesUserHavePermissions(TaxonomyRights.Contributor);


                GroupCollection groups = store.Groups;

                Group settingsGroup = store.Groups.Where(x => x.Name == groupName).FirstOrDefault();
                if (settingsGroup != null)
                {
                    needsUpdate = true;
                }


             
                TermCollection settingsTerm = settingsGroup.TermStore.GetTerms(term, true);
                if (settingsTerm == null)
                {
                    foreach (Term item in settingsTerm)
                    {
                        item.Deprecate(true);
                        needsUpdate = true;
                    }
                }


                if (needsUpdate)
                {
                    store.CommitAll();
                }

               
            }

        }



        
    }
}
