using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SharePoint.Administration;
using System.Web.UI.WebControls;
using System.Text;
using System.Reflection;

namespace AcmeCorp.Engagements.FarmConfiguration
{
    public partial class PropertyBagSettings : LayoutsPageBase
    {
        const string NS = "No selection";
        const string LIST = "List";
        const string SITE = "Site";
        const string SITE_COLL = "SiteCollection";
        const string WEB_APP = "WebApplication";
        const string FARM = "Farm";
        const string SERVER = "Server";
        const string FARM_HEADER = "<H3 class='ms-standardheader'>Farm: {0} Properties</H3>";
        const string SERVER_HEADER = "<H3 class='ms-standardheader'>Server: {0} Properties</H3>";
        const string LIST_HEADER = "<H3 class='ms-standardheader'>List: {0} Properties <a href=\"{1}/{2}\" target=\"_blank\">{1}/{2}</a></H3>";
        const string SITE_HEADER = "<H3 class='ms-standardheader'>Site: {0} Properties <a href=\"{1}\" target=\"_blank\">{1}</a></H3>";
        const string WEB_APP_HEADER = "<H3 class='ms-standardheader'>Web Application: {0} Properties <a href=\"{1}\" target=\"_blank\">{1}</a></H3>";
        const string NEW_PROP = "<H3 class='ms-standardheader'><a onclick=\"javascript:OpenDialog('ModifyPropertyBag.aspx?mod=new&scope={0}&guid={1}&web={2}&site={3}');\" href='#'  ></H3>";
        const string MOD_PROP = "<a href='#' title='Edit {0} Property'  onclick=\"javascript:OpenDialog('ModifyPropertyBag.aspx?mod={1}&scope={2}&guid={3}&web={4}&site={5}&key={0}');\">";

        private string scope = String.Empty;
        private string site = String.Empty;
        private string web = String.Empty;
        private string guid = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                ddlWebApplications.Items.Clear();
                ddlSiteCollection.Items.Clear();
                ddlSite.Items.Clear();
                ListItem item = new ListItem(NS, NS);
                ddlWebApplications.Items.Add(item);
                ddlSiteCollection.Items.Add(item);
                ddlSite.Items.Add(item);

                SPFarm farm = SPFarm.Local;
                SPWebService service = farm.Services.GetValue<SPWebService>("");
                lblFarm.Text = farm.Name;

                foreach (SPServer server in farm.Servers)
                {
                    ListItem ser = new ListItem(server.Name + " (" + server.Address + ")", server.Name);
                    ddlServer.Items.Add(ser);
                }

                foreach (SPWebApplication webApp in service.WebApplications)
                {
                    ListItem webApplication = new ListItem(webApp.Name + " (" + webApp.AlternateUrls.GetResponseUrl(SPUrlZone.Default).Uri.AbsoluteUri.TrimEnd('/') + ")", webApp.AlternateUrls.GetResponseUrl(SPUrlZone.Default).Uri.AbsoluteUri.TrimEnd('/'));
                    ddlWebApplications.Items.Add(webApplication);

                }
            }
            //  EnabledControls();
        }

        protected void ddlWebApplications_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSiteCollection.Items.Clear();
            ddlSite.Items.Clear();
            ddlFolder.Items.Clear();
            ListItem item = new ListItem(NS, NS);
            ddlSiteCollection.Items.Add(item);
            ddlSite.Items.Add(item);
            ddlFolder.Items.Add(item);
            SPSecurity.RunWithElevatedPrivileges(delegate()
           {
               if (ddlWebApplications.SelectedValue != NS)
               {
                   SPWebApplication webApp = SPWebApplication.Lookup(new Uri(ddlWebApplications.SelectedValue));
                   SPSiteCollection collSiteCollection = webApp.Sites;
                   foreach (SPSite oSPSite in collSiteCollection)
                   {
                       using (SPWeb oSPWeb = oSPSite.OpenWeb())
                       {
                           ListItem siteCollection = new ListItem(oSPWeb.Title + " (" + oSPWeb.Url + ")", oSPWeb.Url);
                           ddlSiteCollection.Items.Add(siteCollection);
                       }
                       oSPSite.Dispose();
                   }
               }
           });
            EnabledControls();

        }

        protected void ddlSiteCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSite.Items.Clear();
            ListItem item = new ListItem(NS, NS);
            ddlSite.Items.Add(item);
            SPSecurity.RunWithElevatedPrivileges(delegate()
           {
               if (ddlSiteCollection.SelectedValue != NS)
               {
                   using (SPSite oSPSite = new SPSite(ddlSiteCollection.SelectedValue))
                   {

                       foreach (SPWeb oSPWeb in oSPSite.AllWebs)
                       {
                           if (oSPWeb.Url != oSPSite.Url)
                           {
                               ListItem site = new ListItem(oSPWeb.Title + " (" + oSPWeb.Url + ")", oSPWeb.ServerRelativeUrl);
                               ddlSite.Items.Add(site);
                           }
                           oSPWeb.Dispose();
                       }
                   }

               }
           });
            GetLists(false);
            EnabledControls();

        }

        protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetLists(true);
            EnabledControls();
        }

        protected void ddlFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnabledControls();
        }

        private void GetLists(bool SubSite)
        {
            ddlFolder.Items.Clear();
            ListItem item = new ListItem(NS, NS);
            ddlFolder.Items.Add(item);
            SPSecurity.RunWithElevatedPrivileges(delegate()
           {
               if (ddlSiteCollection.SelectedValue != NS)
               {
                   using (SPSite oSPSite = new SPSite(ddlSiteCollection.SelectedValue))
                   {
                       SPWeb oSPWeb = null;
                       if (SubSite && ddlSite.SelectedValue != NS)
                       {
                           oSPWeb = oSPSite.OpenWeb(ddlSite.SelectedValue);
                       }
                       else
                       {
                           oSPWeb = oSPSite.OpenWeb();
                       }
                       SPListCollection collLists = oSPWeb.Lists;
                       foreach (SPList oList in collLists)
                       {
                           ListItem folder = new ListItem(oList.Title, oList.RootFolder.ToString());
                           ddlFolder.Items.Add(folder);
                       }
                       oSPWeb.Dispose();
                   }
               }
           });
        }

        private void ViewPropertyBag(string scope)
        {
            SPFarm farm;
            PropertyTable.Text = "";
            SPSecurity.RunWithElevatedPrivileges(delegate()
           {
               switch (scope)
               {
                   case LIST:

                       using (SPSite oSPSite = new SPSite(ddlSiteCollection.SelectedValue))
                       {
                           site = oSPSite.ID.ToString();
                           SPWeb oSPWeb = null;
                           if (ddlSite.SelectedValue == NS)
                           {
                               oSPWeb = oSPSite.OpenWeb();
                           }
                           else
                           {
                               oSPWeb = oSPSite.OpenWeb(ddlSite.SelectedValue);
                           }
                           web = oSPWeb.ID.ToString();

                           string[] folders = ddlFolder.SelectedValue.Split('/');

                           SPFolder oSPFolder = null;

                           if (folders.Length > 1)
                           {
                               oSPFolder = oSPWeb.Folders[folders[0]].SubFolders[folders[1]];
                           }
                           else
                           {
                               oSPFolder = oSPWeb.Folders[folders[0]];

                           }
                           guid = oSPFolder.UniqueId.ToString();
                           Header.Text = String.Format(LIST_HEADER, oSPFolder.Name, oSPWeb.Url, oSPFolder.Url);
                           PropertyTable.Text = BuildPropertyTable(oSPFolder.Properties);
                           oSPWeb.Dispose();
                       }
                       break;
                   case SITE:

                       using (SPSite oSPSite = new SPSite(ddlSiteCollection.SelectedValue))
                       {
                           site = oSPSite.ID.ToString();
                           using (SPWeb oSPWeb = oSPSite.OpenWeb(ddlSite.SelectedValue))
                           {
                               web = oSPWeb.ID.ToString();
                               guid = oSPWeb.ID.ToString();
                               Header.Text = String.Format(SITE_HEADER, oSPWeb.Title, oSPWeb.Url);
                               PropertyTable.Text = BuildPropertyTable(oSPWeb.AllProperties);
                           }
                       }
                       break;
                   case SITE_COLL:
                       using (SPSite oSPSite = new SPSite(ddlSiteCollection.SelectedValue))
                       {
                           site = oSPSite.ID.ToString();
                           using (SPWeb oSPWeb = oSPSite.OpenWeb())
                           {
                               web = oSPWeb.ID.ToString();
                               guid = oSPWeb.ID.ToString();
                               Header.Text = String.Format(SITE_HEADER, oSPWeb.Title, oSPWeb.Url);
                               PropertyTable.Text = BuildPropertyTable(oSPWeb.AllProperties);

                           }
                       }
                       break;
                   case FARM:
                       farm = SPFarm.Local;
                       PropertyTable.Text = BuildPropertyTable(farm.Properties);
                       Header.Text = String.Format(FARM_HEADER, farm.Name);
                       break;
                   case SERVER:
                       farm = SPFarm.Local;
                       SPServer server = farm.Servers[ddlServer.SelectedValue];
                       guid = server.Id.ToString();
                       PropertyTable.Text = BuildPropertyTable(server.Properties);
                       Header.Text = String.Format(SERVER_HEADER, server.Name);
                       break;
                   default:
                       SPWebApplication webApp = SPWebApplication.Lookup(new Uri(ddlWebApplications.SelectedValue));
                       guid = webApp.Id.ToString();
                       Header.Text = String.Format(WEB_APP_HEADER, webApp.Name, webApp.AlternateUrls.GetResponseUrl(SPUrlZone.Default).Uri.AbsoluteUri.TrimEnd('/'));
                       PropertyTable.Text = BuildPropertyTable(webApp.Properties);
                       break;

               }
           });
        }

        protected void btnWebApplication_Click(object sender, EventArgs e)
        {
            scope = "app";
            ViewPropertyBag(WEB_APP);
        }

        protected void btnSiteCollection_Click(object sender, EventArgs e)
        {
            scope = "web";
            ViewPropertyBag(SITE_COLL);
        }

        protected void btnSite_Click(object sender, EventArgs e)
        {
            scope = "web";
            ViewPropertyBag(SITE);
        }

        protected void btnFolder_Click(object sender, EventArgs e)
        {
            scope = "lst";
            ViewPropertyBag(LIST);
        }

        private void EnabledControls()
        {
            PropertyTable.Text = "";
            Header.Text = "";

            if (ddlWebApplications.SelectedValue != NS)
            {
                btnWebApplication.Enabled = true;
            }
            else
            {
                btnWebApplication.Enabled = false;
            }

            if (ddlWebApplications.Items.Count > 1)
            {
                ddlWebApplications.Enabled = true;
            }
            else
            {
                ddlWebApplications.Enabled = false;
            }

            if (ddlSiteCollection.SelectedValue != NS)
            {
                btnSiteCollection.Enabled = true;
            }
            else
            {
                btnSiteCollection.Enabled = false;
            }

            if (ddlSiteCollection.Items.Count > 1)
            {
                ddlSiteCollection.Enabled = true;
            }
            else
            {
                ddlSiteCollection.Enabled = false;
            }

            if (ddlSite.SelectedValue != NS)
            {
                btnSite.Enabled = true;
            }
            else
            {
                btnSite.Enabled = false;
            }

            if (ddlSite.Items.Count > 1)
            {
                ddlSite.Enabled = true;
            }
            else
            {
                ddlSite.Enabled = false;
            }

            if (ddlFolder.SelectedValue != NS)
            {
                btnFolder.Enabled = true;
            }
            else
            {
                btnFolder.Enabled = false;
            }

            if (ddlFolder.Items.Count > 1)
            {
                ddlFolder.Enabled = true;
            }
            else
            {
                ddlFolder.Enabled = false;
            }
        }

        private string BuildPropertyTable(Hashtable properties)
        {
            string classStyle = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("<br />");
            sb.Append("<table class=\"ms-toolbar\" width=\"100%\" style=\"padding-left: 3px; padding-right: 5px;\" cellspacing=\"0\" cellpadding=\"2\">");
            sb.Append("<tr>");
            sb.Append("<td>");
            sb.Append("<table cellspacing=\"1\" cellpadding=\"0\" border=\"0\">");
            sb.Append("<tr>");
            sb.Append("<td width=\"0\" class=\"ms-toolbar\">");
            sb.Append(String.Format(NEW_PROP, scope, guid, web, site));
            sb.Append("<img alt=\"New Property\" src=\"/_layouts/images/newitem.gif\" border=\"0\" hspace=\"2\" /></a>");
            sb.Append("</td>");
            sb.Append("<td class=\"\" width=\"100%\" nowrap>");
            sb.Append(String.Format(NEW_PROP, scope, guid, web, site));
            sb.Append("New Property</a>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("<br />");
            sb.Append("<table class=\"ms-listviewtable\" width=\"100%\" style=\"padding-left: 3px; padding-right: 5px;\" cellspacing=\"0\" cellpadding=\"2\">");
            sb.Append("<tr class='ms-viewheadertr'>");
            sb.Append("<td class='ms-vh2-nofilter ms-vh2-gridview'>Property</td>");
            sb.Append("<td class='ms-vh2-nofilter ms-vh2-gridview'>Value</td>");
            sb.Append("<td class='ms-vh2-nofilter ms-vh2-gridview' style=\"text-align:center\">Edit</td>");
            sb.Append("<td class='ms-vh2-nofilter ms-vh2-gridview' style=\"text-align:center\">Delete</td>");
            sb.Append("</tr>");



            List<SortedProperties> sp = new List<SortedProperties>();

            foreach (DictionaryEntry property in properties)
            {
                sp.Add(new SortedProperties(property.Key.ToString(), property.Value.ToString()));
            }

            sp.Sort(new GenericComparer<SortedProperties>("Key", GenericComparer<SortedProperties>.SortOrder.Ascending));

            foreach (SortedProperties property in sp)
            {
                sb.Append("<tr" + classStyle + ">");
                sb.Append("<td class='ms-vb2'>" + property.Key + "</td>");
                sb.Append("<td class='ms-vb2'>" + property.Value + "</td>");
                sb.Append("<td class='ms-vb2' style=\"text-align:center\">");
                sb.Append(String.Format(MOD_PROP, property.Key.ToString(), "edt", scope, guid, web, site));
                sb.Append("<img src='../_layouts/images/EDITITEM.GIF' border='0' alt='Edit " + property.Key + " Property' ></a></td>");
                sb.Append("<td   class='ms-vb2' style=\"text-align:center\">");
                sb.Append(String.Format(MOD_PROP, property.Key.ToString(), "del", scope, guid, web, site));
                sb.Append("<img src='../_layouts/images/DELITEM.GIF' border='0' alt='Delete " + property.Key + " Property'></a></td>");
                sb.Append("</tr>");

                if (classStyle == "")
                {
                    classStyle = " style=\"background-color: #ededed \"";
                }
                else
                {
                    classStyle = "";
                }
            }
            sb.Append("</table>");

            return sb.ToString();
        }

        public ICollection SortedHashTable(Hashtable ht)
        {
            ArrayList sorter = new ArrayList();
            sorter.AddRange(ht);
            sorter.Sort();
            return sorter;
        }

        protected void btnFarm_Click(object sender, EventArgs e)
        {
            scope = "farm";
            ViewPropertyBag(FARM);
        }

        protected void btnServer_Click(object sender, EventArgs e)
        {
            scope = "ser";
            ViewPropertyBag(SERVER);
        }

        public class SortedProperties
        {
            private string _key;
            private string _value;

            public string Key
            {
                get { return _key; }
                set { _key = value; }
            }

            public string Value
            {
                get { return _value; }
                set { _value = value; }
            }

            public SortedProperties(string key, string value)
            {
                _key = key;
                _value = value;
            }

        }

        public sealed class GenericComparer<T> : IComparer<T>
        {

            public enum SortOrder { Ascending, Descending };
            private string sortColumn;
            private SortOrder sortingOrder;

            public GenericComparer(string sortColumn, SortOrder sortingOrder)
            {
                this.sortColumn = sortColumn;
                this.sortingOrder = sortingOrder;
            }

            public string SortColumn
            {
                get { return sortColumn; }
            }

            public SortOrder SortingOrder
            {
                get { return sortingOrder; }
            }


            public int Compare(T x, T y)
            {

                PropertyInfo propertyInfo = typeof(T).GetProperty(sortColumn);
                IComparable obj1 = (IComparable)propertyInfo.GetValue(x, null);
                IComparable obj2 = (IComparable)propertyInfo.GetValue(y, null);
                if (sortingOrder == SortOrder.Ascending)
                {
                    return (obj1.CompareTo(obj2));
                }
                else
                {
                    return (obj2.CompareTo(obj1));
                }
            }

        }
    }
}
