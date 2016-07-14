using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Web;
using Microsoft.SharePoint.Administration;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Web.UI;


namespace AcmeCorp.Engagements.FarmConfiguration
{
    public   class PropertyBag : PBSPageBase
    {
        protected DropDownList ddlSite;

        protected DropDownList ddlFolder;

        protected Label PropertyTable;
        protected Label Header;
        protected string lblSite;

        protected Button btnSite;
        protected Button btnFolder;
        const string NS = "No selection";
        const string LIST = "List";
        const string SITE = "Site";

        const string LIST_HEADER = "<H3 class='ms-standardheader'>List: {0} Properties <a href=\"{1}/{2}\" target=\"_blank\">{1}/{2}</a></H3>";
        const string SITE_HEADER = "<H3 class='ms-standardheader'>Site: {0} Properties <a href=\"{1}\" target=\"_blank\">{1}</a></H3>";

        const string NEW_PROP = "<a onclick=\"javascript:OpenDialog('ModifyPropertyBag.aspx?mod=new&scope={0}&guid={1}&web={2}&site={3}');\" class=\"ms-toolbar\"  href='#' >";
        const string MOD_PROP = "<a href='#'  onmouseover=\"this.style.cursor='hand';\" title='Edit {0} Property'  onclick=\"javascript:OpenDialog('ModifyPropertyBag.aspx?mod={1}&scope={2}&guid={3}&web={4}&site={5}&key={0}');\">";
        const string EXP_PROP = "<a href='#'  onclick=\"javascript:OpenDialog('ExportPropertyBag.aspx?mod=new&scope={0}&guid={1}&web={2}&site={3}','','dialogWidth=400px;dialogHeight=400px;center=yes;help=no;scroll=no;status=no;');\" class=\"ms-toolbar\"  onmouseover=\"this.style.cursor='hand';\" >";
        const string IMP_PROP = "<a href='#'  onclick=\"javascript:window.open('ImportPropertyBag.aspx?scope={0}&guid={1}&web={2}&site={3}','_self');\" class=\"ms-toolbar\"  onmouseover=\"this.style.cursor='hand';\" >";

        private string scope = String.Empty;
        private string site = String.Empty;
        private string web = String.Empty;
        private string guid = String.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            lblSite = SPContext.Current.Web.Title;
            if (!Page.IsPostBack)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {


                    if (!Page.IsPostBack)
                    {
                        scope = "web";
                        ViewPropertyBag(SITE);
                        GetLists();
                    }


                });
            }

        }



        protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetLists();
            EnabledControls();
        }

        protected void ddlFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnabledControls();
        }



        private void GetLists()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                ddlFolder.Items.Clear();
                ListItem item = new ListItem(NS, NS);
                ddlFolder.Items.Add(item);

                using (SPSite oSPSite = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb oSPWeb = oSPSite.AllWebs[SPContext.Current.Web.ID])
                    {

                        SPListCollection collLists = oSPWeb.Lists;
                        foreach (SPList oList in collLists)
                        {
                            ListItem folder = new ListItem(oList.Title, oList.RootFolder.ToString());
                            ddlFolder.Items.Add(folder);
                        }
                    }
                }

            });
        }

        private void ViewPropertyBag(string scope)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPFarm farm;
                PropertyTable.Text = "";
                string siteUrl = "";
                switch (scope)
                {
                    case LIST:


                        siteUrl = SPContext.Current.Site.Url;


                        using (SPSite oSPSite = new SPSite(siteUrl))
                        {
                            site = oSPSite.ID.ToString();
                            SPWeb oSPWeb = null;
                            if (ddlSite == null)
                            {
                                oSPWeb = oSPSite.OpenWeb(SPContext.Current.Web.ServerRelativeUrl);
                            }
                            else if (ddlSite.SelectedValue == NS)
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

                        string webUrl = "";

                        siteUrl = SPContext.Current.Site.Url;
                        webUrl = SPContext.Current.Web.ServerRelativeUrl;

                        using (SPSite oSPSite = new SPSite(siteUrl))
                        {
                            site = oSPSite.ID.ToString();
                            using (SPWeb oSPWeb = oSPSite.OpenWeb(webUrl))
                            {
                                web = oSPWeb.ID.ToString();
                                guid = oSPWeb.ID.ToString();
                                Header.Text = String.Format(SITE_HEADER, oSPWeb.Title, oSPWeb.Url);
                                PropertyTable.Text = BuildPropertyTable(oSPWeb.AllProperties);
                            }
                        }
                        break;


                }
            });
        }


        protected void btnSite_Click(object sender, EventArgs e)
        {
            scope = "web";
            ViewPropertyBag(SITE);
        }

        protected void btnFolder_Click(object sender, EventArgs e)
        {
            scope = "lst";
            if (ddlFolder.SelectedValue != NS)
            {
                ViewPropertyBag(LIST);
            }
            else
            {
                PropertyTable.Text = "";
                Header.Text = "";
            }
        }

        private void EnabledControls()
        {
            PropertyTable.Text = "";
            Header.Text = "";



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
            sb.Append("<td class=\"\" nowrap>");
            sb.Append(String.Format(NEW_PROP, scope, guid, web, site));
            sb.Append("New Property</a>");
            sb.Append("</td>");
            sb.Append("<td class=\"ms-toolbar\" style=\"width: 10px;\">&nbsp;</td>");
            sb.Append("<td class=\"ms-toolbar\">");
            sb.Append(String.Format(EXP_PROP, scope, guid, web, site));
            sb.Append("<img alt=\"Export Properties<\" src=\"/_layouts/images/EXPTITEM.GIF\" border=\"0\" hspace=\"2\" /></a>");
            sb.Append("</td>");
            sb.Append("<td class=\"\"  nowrap>");
            sb.Append(String.Format(EXP_PROP, scope, guid, web, site));
            sb.Append("Export Properties</a>");
            sb.Append("</td>");
            sb.Append("<td class=\"ms-toolbar\" style=\"width: 10px;\">&nbsp;</td>");
            sb.Append("<td class=\"ms-toolbar\">");
            sb.Append(String.Format(IMP_PROP, scope, guid, web, site));
            sb.Append("<img alt=\"Import Properties<\" src=\"/_layouts/images/IMPITEM.GIF\" border=\"0\" hspace=\"2\" /></a>");
            sb.Append("</td>");
            sb.Append("<td class=\"\"  nowrap>");
            sb.Append(String.Format(IMP_PROP, scope, guid, web, site));
            sb.Append("Import Properties</a>");
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
                if (property.Value != null)
                {
                    sp.Add(new SortedProperties(property.Key.ToString(), property.Value.ToString()));
                }
            }

            sp.Sort(new GenericComparer<SortedProperties>("Key", GenericComparer<SortedProperties>.SortOrder.Ascending));

            foreach (SortedProperties property in sp)
            {
                sb.Append("<tr" + classStyle + ">");
                sb.Append("<td class='ms-vb2'>" + property.Key + "</td>");
                sb.Append("<td class='ms-vb2'>" + property.Value + "</td>");
                sb.Append("<td class='ms-vb2' style=\"text-align:center\">");
                sb.Append(String.Format(MOD_PROP, property.Key.ToString(), "edt", scope, guid, web, site));
                sb.Append("<img src='/_layouts/images/EDITITEM.GIF' border='0' alt='Edit " + property.Key + " Property' ></a></td>");
                sb.Append("<td   class='ms-vb2' style=\"text-align:center\">");
                sb.Append(String.Format(MOD_PROP, property.Key.ToString(), "del", scope, guid, web, site));
                sb.Append("<img src='/_layouts/images/DELITEM.GIF' border='0' alt='Delete " + property.Key + " Property'></a></td>");
                sb.Append("</tr>");

                if (classStyle == "")
                {
                    classStyle = " class='ms-alternating'";
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





    }
}