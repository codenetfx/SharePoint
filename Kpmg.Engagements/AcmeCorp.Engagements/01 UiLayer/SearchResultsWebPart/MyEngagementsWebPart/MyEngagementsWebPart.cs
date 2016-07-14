// -----------------------------------------------------------------------
// <copyright file="MyEngagementsWebPart.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace SearchResultsWebPart.MyEngagementsWebPart
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Xml;
    using System.Xml.Linq;

    using Microsoft.Office.Server.Search;
    using Microsoft.Office.Server.Search.Administration;
    using Microsoft.Office.Server.Search.Query;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;

    /// <summary>
    /// My Engagements web part. Displays a list of engagements for which current user has access rights.
    /// </summary>
    [ToolboxItemAttribute(false)]
    public class MyEngagementsWebPart : WebPart
    {
        /// <summary>
        /// The property items
        /// </summary>
        private string propertyItems = string.Empty;

        /// <summary>
        /// My engagements grid
        /// </summary>
        private SPGridView myEngagementsGrid;

        /// <summary>
        /// The property item collection
        /// </summary>
        private string[] propertyItemCollection;

        /// <summary>
        /// The error message
        /// </summary>
        private Label errorMessage = new Label();

        /// <summary>
        /// The search results
        /// </summary>
        private List<EngagementSearchResult> searchResults = new List<EngagementSearchResult>();

        /// <summary>
        /// The engagement view
        /// </summary>
        private DataView engagementView;

        /// <summary>
        /// Gets or sets the property items.
        /// </summary>
        /// <value>
        /// Semicollon delimited list of custom web properties to be included in search result
        /// </value>
        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
WebDisplayName("Web Properties"),
WebDescription("Semicollon delimited list of custom web properties to be included in search result")]
        public string PropertyItems
        {
            get
            {
                return this.propertyItems;
            }

            set
            {
                this.propertyItems = value;
            }
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();
            this.LoadPropertyKeys();
            this.FindMyEngagements();
        }

        /// <summary>
        /// Called when [view page changing].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void OnViewPageChanging(object sender, GridViewPageEventArgs e)
        {
            // Recreate current sort if needed
            if (ViewState["SortDirection"] != null && ViewState["SortExpression"] != null)
            {
                // We have an active sorting, so this need to be preserved
                this.engagementView.Sort = ViewState["SortExpression"].ToString()
                    + " " + ViewState["SortDirection"].ToString();
            }

            this.myEngagementsGrid.PageIndex = e.NewPageIndex;
            this.myEngagementsGrid.DataBind();
        }

        /// <summary>
        /// Parse value of custom property and return string array
        /// </summary>
        private void LoadPropertyKeys()
        {
            if (this.propertyItems != string.Empty)
            {
                this.propertyItemCollection = this.propertyItems.Trim().Split(';');
            }
            if (this.propertyItemCollection != null)
            {
                foreach (string item in this.propertyItemCollection)
                {
                    item.Trim();
                }
            }

        }

        /// <summary>
        /// Performs search for all relevant engagement sites based on specified web template.
        /// </summary>
        private void FindMyEngagements()
        {
            ResultTableCollection engagementsResults = new ResultTableCollection();
            DataTable myEngagementsTable = new DataTable();

            SPSite site = SPContext.Current.Site;
            using (KeywordQuery engagementsQuery = new KeywordQuery(site))
            {
                engagementsQuery.ResultsProvider = SearchProvider.Default;
                engagementsQuery.SelectProperties.Clear();
                engagementsQuery.SelectProperties.Add("Title");
                engagementsQuery.SelectProperties.Add("Path");
                engagementsQuery.SelectProperties.Add("KE-status");
                engagementsQuery.SelectProperties.Add("KE-statusdate");
                engagementsQuery.SelectProperties.Add("KE-Name-des-Mandanten");
                engagementsQuery.SelectProperties.Add("KE-Opportunity-Nr");
                engagementsQuery.SelectProperties.Add("KE-Account");
                engagementsQuery.SelectProperties.Add("KE-Concurring-Partner");
                engagementsQuery.SelectProperties.Add("KE-Niederlassung");
                engagementsQuery.SelectProperties.Add("KE-Bezeichnung");
                engagementsQuery.SelectProperties.Add("KE-WB-Auftrags-Nr");
                engagementsQuery.SelectProperties.Add("KE-Eng-Partner");
                engagementsQuery.SelectProperties.Add("KE-Eng-Manager");
                engagementsQuery.SelectProperties.Add("KE-WB-Auftrag-Status");
                engagementsQuery.SelectProperties.Add("KE-WB-Auftrag-Status-Datum");
                engagementsQuery.RowLimit = 500;
                engagementsQuery.TrimDuplicates = false;

                // engagementsQuery.QueryText = "contentclass:STS_Web OR contentclass:STS_Site";
                // Show only websites created with Engagements web template
                engagementsQuery.QueryText = "WebTemplate:ENGAGEMENTSSITEDEF";

                try
                {
                    // Execute search on default search provider.
                    SearchExecutor searchExecutor = new SearchExecutor();
                    engagementsResults = searchExecutor.ExecuteQuery(engagementsQuery);
                }
                catch (Exception ex)
                {

                    // TODO Exception Handling;
                }

                // Load DataTable with search results and call function that will create GridView with results
                if (engagementsResults.Count > 0)
                {
                    var myEngagements = engagementsResults.Filter("TableType", KnownTableTypes.RelevantResults);
                    if (myEngagements != null && myEngagements.Count() == 1)
                    {
                        myEngagementsTable.Load(myEngagements.First(), LoadOption.OverwriteChanges);
                    }

                    this.FillResultsGrid(myEngagementsTable);

                    // searchResults = LoadSearchResults(myEngagementsTable);
                    // FillResultsGrid(searchResults);
                }
            }
        }

        /// <summary>
        /// Fills the results grid.
        /// </summary>
        /// <param name="searchResults">The search results.</param>
        private void FillResultsGrid(List<EngagementSearchResult> searchResults)
        {
            // SPGridView setup
            this.myEngagementsGrid = new SPGridView();
            this.myEngagementsGrid.DataSource = searchResults;
            this.myEngagementsGrid.AutoGenerateColumns = false;
            this.myEngagementsGrid.PageSize = 20;

            // Create and display HyperLink field for all relevant engagements
            HyperLinkField hc = new HyperLinkField();
            hc.DataTextField = "Title";
            hc.DataNavigateUrlFields = new string[] { "Url" };
            this.myEngagementsGrid.Columns.Add(hc);

            // Bind Grid and add to Web Part
            this.myEngagementsGrid.DataBind();
            Controls.Add(this.myEngagementsGrid);
        }

        /// <summary>
        /// Loads the search results.
        /// </summary>
        /// <param name="myEngagementsTable">My engagements table.</param>
        /// <returns>List of Engagements retrieved from search.</returns>
        private List<EngagementSearchResult> LoadSearchResults(DataTable myEngagementsTable)
        {
            IEnumerable<EngagementSearchResult> filteredResults = from engResult in myEngagementsTable.AsEnumerable()
                                                                  where engResult.Field<string>("Path").Contains("/engagements")
                                                                  select new EngagementSearchResult
                                                                  {
                                                                      Title = engResult.Field<string>("Title"),
                                                                      Url = engResult.Field<string>("Path"),
                                                                      KEstatus = engResult.Field<string>("KE-status"),
                                                                      KEstatusdate = engResult.Field<DateTime?>("KE-statusdate"),
                                                                      KENamedesMandanten = engResult.Field<string>("KE-Name-des-Mandanten"),
                                                                      KEOpportunityNr = engResult.Field<int?>("KE-Opportunity-Nr"),
                                                                      KEAccount = engResult.Field<string>("KE-Account"),
                                                                      KEConcurringPartner = engResult.Field<string>("KE-Concurring-Partner"),
                                                                      KENiederlassung = engResult.Field<string>("KE-Niederlassung"),
                                                                      KEBezeichnung = engResult.Field<string>("KE-Bezeichnung"),
                                                                      KEWBAuftragsNr = engResult.Field<int?>("KE-WB-Auftrags-Nr"),
                                                                      KEEngPartner = engResult.Field<string>("KE-Eng-Partner"),
                                                                      KEEngManager = engResult.Field<string>("KE-Eng-Manager"),
                                                                      KEWBAuftragStatus = engResult.Field<string>("KE-WB-Auftrag-Status"),
                                                                      KEWBAuftragStatusDatum = engResult.Field<DateTime?>("KE-WB-Auftrag-Status-Datum")
                                                                  };

            return filteredResults.ToList();
        }

        /// <summary>
        /// Fills the results grid.
        /// </summary>
        /// <param name="myEngagementsTable">My engagements table.</param>
        private void FillResultsGrid(DataTable myEngagementsTable)
        {
            // Create the data view from data table with search results
            this.engagementView = myEngagementsTable.AsDataView();

            // Initialize the SharePoint Grid View 
            this.myEngagementsGrid = new SPGridView();
            this.myEngagementsGrid.AutoGenerateColumns = false;
            this.myEngagementsGrid.AllowSorting = true;
            this.myEngagementsGrid.DataSource = this.engagementView;
            this.myEngagementsGrid.Sorting += this.myEngagementsGrid_Sorting;

            // Create and display HyperLink field for all relevant entries
            HyperLinkField hc = new HyperLinkField();
            hc.HeaderText = "Engagement";
            hc.DataTextField = "Title";
            hc.DataNavigateUrlFields = new string[] { "Path" };
            hc.SortExpression = "Title";
            this.myEngagementsGrid.Columns.Add(hc);

            this.AddCustomColumnsToGrid(this.myEngagementsGrid);

            // Grouping in web part
            // myEngagementsGrid.AllowGrouping = true;
            // myEngagementsGrid.AllowGroupCollapse = true;
            // myEngagementsGrid.GroupField = "KE-Account";
            // myEngagementsGrid.GroupDescriptionField = "KE-Account";
            // myEngagementsGrid.GroupFieldDisplayName = "KE-Account"; 
            Controls.Add(this.myEngagementsGrid);

            // Setup paging in web part
            this.myEngagementsGrid.PageSize = 5;
            this.myEngagementsGrid.AllowPaging = true;

            this.myEngagementsGrid.PageIndexChanging += new GridViewPageEventHandler(this.OnViewPageChanging);
            this.myEngagementsGrid.PagerTemplate = null;

            this.myEngagementsGrid.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the myEngagementsGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs"/> instance containing the event data.</param>
        private void myEngagementsGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            string lastExpression = string.Empty;
            if (ViewState["SortExpression"] != null)
            {
                lastExpression = ViewState["SortExpression"].ToString();
            }

            string lastDirection = "asc";
            if (ViewState["SortDirection"] != null)
            {
                lastDirection = ViewState["SortDirection"].ToString();
            }

            string newDirection = "asc";
            if (e.SortExpression == lastExpression)
            {
                newDirection = (lastDirection == "asc") ? "desc" : "asc";
            }

            ViewState["SortExpression"] = e.SortExpression;
            ViewState["SortDirection"] = newDirection;

            this.engagementView.Sort = e.SortExpression + " " + newDirection;
            this.myEngagementsGrid.DataBind();
        }

        /// <summary>
        /// Adds the custom columns to grid for all custom properties in Web part config.
        /// </summary>
        /// <param name="myEngagementsGrid">My engagements grid.</param>
        private void AddCustomColumnsToGrid(GridView myEngagementsGrid)
        {
            if (propertyItemCollection != null)
            {
                for (int i = 0; i < this.propertyItemCollection.Length - 1; i++)
                {
                    BoundField bf = new BoundField();
                    bf.DataField = this.propertyItemCollection[i];
                    bf.HeaderText = this.propertyItemCollection[i].Trim().Remove(0, 3).Replace('-', ' ');
                    bf.SortExpression = this.propertyItemCollection[i];
                    myEngagementsGrid.Columns.Add(bf);
                    bf = null;
                }
            }

        }
    }
}
