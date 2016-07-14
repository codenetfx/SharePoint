using System;
using System.Web;
using System.Web.Caching;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Administration;
using System.Xml;
using System.Net;

namespace AcmeCorp.Engagements.FarmConfiguration
{
    public class ImportPropertyBag : PBSPageBase
    {
        private string scope = String.Empty;
        private Guid site = SPContext.Current.Site.ID;
        private Guid web = SPContext.Current.Web.ID;
        private Guid guid = Guid.Empty;
        private string key = String.Empty;
        private string mod = String.Empty;
        protected Label lblMsg;
        protected Label lblDesc;
        protected Label txtImportHeader;
        protected Button BtnCancel;
        protected Button btnLoadProperties;
        protected SPGridView grdProperties;
        protected CheckBox cbOverwrite;
        protected HtmlInputHidden HiddenPropertiesSelections;
        protected TextBox txtUrl;
        const string LIST_HEADER = "<H3 class='ms-standardheader'>Import properties to {0} List <a href=\"{1}/{2}\" target=\"_blank\">{1}/{2}</a></H3>";
        const string SITE_HEADER = "<H3 class='ms-standardheader'>Import properties to {0} Site <a href=\"{1}\" target=\"_blank\">{1}</a></H3>";
        const string WEBAPP_HEADER = "<H3 class='ms-standardheader'>Import properties to {0} Web Application <a href=\"{1}\" target=\"_blank\">{1}</a></H3>";
        const string FARM_HEADER = "<H3 class='ms-standardheader'>Import properties to {0} Farm </H3>";
        const string SERVER_HEADER = "<H3 class='ms-standardheader'>Import properties to {0} Server </H3>";
        const string IMPSUC = "The property {0} was successfully imported <br />";
        const string IMPEXI = "Import failed: The property {0} is already exists <br />";
        const string IMPMOD = "The property {0} was successfully modified (old value: {1} | new value: {2})<br />";
        const string QS_SCOPE = "scope";
        const string QS_SITE = "site";
        const string QS_WEB = "web";
        const string QS_GUID = "guid";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            if (Request.QueryString[QS_SITE] != null && Request.QueryString[QS_SITE] != String.Empty)
            {
                site = new Guid(Request.QueryString[QS_SITE]);
            }
            if (Request.QueryString[QS_WEB] != null && Request.QueryString[QS_WEB] != String.Empty)
            {
                web = new Guid(Request.QueryString[QS_WEB]);
            }
            if (Request.QueryString[QS_GUID] != null && Request.QueryString[QS_GUID] != String.Empty)
            {
                guid = new Guid(Request.QueryString[QS_GUID]);
            }

            if (Request.QueryString[QS_SCOPE] != null)
            {
                scope = Request.QueryString[QS_SCOPE];
            }

            txtImportHeader.Text = SetHeader(scope);

        }

        private string SetHeader(string scope)
        {
            string header = "";
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPFarm farm;
                switch (scope)
                {
                    case "farm":
                        farm = SPFarm.Local;
                        header = String.Format(FARM_HEADER, farm.Name);
                        break;
                    case "ser":
                        farm = SPFarm.Local;
                        SPServer server = farm.Servers[guid];
                        header = String.Format(SERVER_HEADER, server.Name);
                        break;
                    case "app":
                        farm = SPFarm.Local;
                        SPWebService service = farm.Services.GetValue<SPWebService>("");
                        SPWebApplication oSPWebApplication = service.WebApplications[guid];
                        header = String.Format(WEBAPP_HEADER, oSPWebApplication.Name, oSPWebApplication.AlternateUrls.GetResponseUrl(SPUrlZone.Default).Uri.AbsoluteUri.TrimEnd('/'));
                        break;
                    case "lst":
                        using (SPSite oSPSite = new SPSite(site))
                        {
                            using (SPWeb oSPWeb = oSPSite.OpenWeb(web))
                            {
                                SPFolder oSPFolder = oSPWeb.GetFolder(guid);
                                header = String.Format(LIST_HEADER, oSPFolder.Name, oSPWeb.Url, oSPFolder.Url);
                            }
                        }
                        break;

                    default:
                        using (SPSite oSPSite = new SPSite(site))
                        {
                            using (SPWeb oSPWeb = oSPSite.OpenWeb(web))
                            {
                                SPFolder oSPFolder = oSPWeb.GetFolder(guid);
                                header = String.Format(SITE_HEADER, oSPWeb.Title, oSPWeb.Url);
                            }
                        }
                        break;

                }

            });
            return header;
        }

        protected void btnCancel_Click(object Sender, EventArgs e)
        {

            SPUtility.Redirect(SPContext.Current.Web.Url + "/_layouts/15/pbs2013/PropertyBag.aspx", SPRedirectFlags.Static, HttpContext.Current);


        }

        protected void btnLoadProperties_Click(object Sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
        {
            if (btnLoadProperties.Text == "Cancel")
            {
                btnLoadProperties.Text = "Load Properties";
                txtUrl.Enabled = true;
                grdProperties.DataSource = new DataTable(); ;
                grdProperties.DataBind();
                lblMsg.Text = "";
            }
            else
            {
                grdProperties.DataSource = GetDatasource(txtUrl.Text);
                grdProperties.DataBind();
                txtUrl.Enabled = false;
                btnLoadProperties.Text = "Cancel";
            }
        });
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {

                StringBuilder sb = new StringBuilder();
                lblMsg.Text = "";
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPFarm farm;
                    string logLine = "";
                    string key = "";
                    string[] propertiesToImport = HiddenPropertiesSelections.Value.Split('#');
                    switch (scope)
                    {
                        case "farm":
                            farm = SPFarm.Local;
                            foreach (string property in propertiesToImport)
                            {
                                try
                                {
                                    if (property != string.Empty)
                                    {
                                        logLine = "";

                                        string value = findNode(property);
                                        key = property.Replace("pbs_number_sign_", "#");
                                        if (cbOverwrite.Checked)
                                        {
                                            if (!farm.Properties.ContainsKey(key))
                                            {

                                                farm.Properties.Add(key, value);
                                                farm.Update();
                                                logLine += String.Format(IMPSUC, key);

                                            }
                                            else
                                            {
                                                string oldValue = farm.Properties[key].ToString();
                                                farm.Properties[key] = value;
                                                farm.Update();

                                                logLine += String.Format(IMPMOD, key, oldValue, value);
                                            }
                                        }
                                        else
                                        {
                                            if (!farm.Properties.ContainsKey(key))
                                            {
                                                farm.Properties.Add(key, value);
                                                farm.Update();
                                                logLine += String.Format(IMPSUC, key);
                                            }
                                            else
                                            {
                                                logLine += String.Format(IMPEXI, key);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logLine += ex.Message;
                                }

                                sb.Append(logLine);
                            }
                            break;
                        case "ser":
                            farm = SPFarm.Local;
                            SPServer server = farm.Servers[guid];
                            foreach (string property in propertiesToImport)
                            {
                                try
                                {
                                    if (property != string.Empty)
                                    {
                                        logLine = "";

                                        string value = findNode(property);
                                        key = property.Replace("pbs_number_sign_", "#");
                                        if (cbOverwrite.Checked)
                                        {
                                            if (!server.Properties.ContainsKey(key))
                                            {

                                                server.Properties.Add(key, value);
                                                server.Update();
                                                logLine += String.Format(IMPSUC, key);
                                            }
                                            else
                                            {
                                                string oldValue = server.Properties[key].ToString();
                                                server.Properties[key] = value;
                                                server.Update();
                                                logLine += String.Format(IMPMOD, key, oldValue, value);

                                            }
                                        }
                                        else
                                        {
                                            if (!server.Properties.ContainsKey(key))
                                            {
                                                server.Properties.Add(key, value);
                                                server.Update();
                                                logLine += String.Format(IMPSUC, key);
                                            }
                                            else
                                            {
                                                logLine += String.Format(IMPEXI, key);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logLine += ex.Message;
                                }

                                sb.Append(logLine);
                                logLine = "";
                            }
                            break;
                        case "app":
                            farm = SPFarm.Local;
                            SPWebService service = farm.Services.GetValue<SPWebService>("");
                            SPWebApplication oSPWebApplication = service.WebApplications[guid];

                            foreach (string property in propertiesToImport)
                            {
                                try
                                {
                                    if (property != string.Empty)
                                    {
                                        logLine = "";

                                        string value = findNode(property);
                                        key = property.Replace("pbs_number_sign_", "#");
                                        if (cbOverwrite.Checked)
                                        {
                                            if (!oSPWebApplication.Properties.ContainsKey(key))
                                            {
                                                oSPWebApplication.Properties.Add(key, value);
                                                oSPWebApplication.Update();
                                                logLine += String.Format(IMPSUC, key);
                                            }
                                            else
                                            {
                                                string oldValue = oSPWebApplication.Properties[key].ToString();
                                                oSPWebApplication.Properties[key] = value;
                                                oSPWebApplication.Update();
                                                logLine += String.Format(IMPMOD, key, oldValue, value);
                                            }
                                        }
                                        else
                                        {
                                            if (!oSPWebApplication.Properties.ContainsKey(key))
                                            {
                                                oSPWebApplication.Properties.Add(key, value);
                                                oSPWebApplication.Update();
                                                logLine += String.Format(IMPSUC, key);
                                            }
                                            else
                                            {
                                                logLine += String.Format(IMPEXI, key);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logLine += ex.Message;
                                }

                                sb.Append(logLine);
                                logLine = "";
                            }
                            break;
                        case "lst":
                            using (SPSite oSPSite = new SPSite(site))
                            {
                                using (SPWeb oSPWeb = oSPSite.OpenWeb(web))
                                {
                                    SPFolder oSPFolder = oSPWeb.GetFolder(guid);

                                    foreach (string property in propertiesToImport)
                                    {
                                        try
                                        {
                                            if (property != string.Empty)
                                            {
                                                logLine = "";
                                                string value = findNode(property);
                                                key = property.Replace("pbs_number_sign_", "#");

                                                if (cbOverwrite.Checked)
                                                {
                                                    if (!oSPFolder.Properties.ContainsKey(key))
                                                    {
                                                        oSPFolder.Properties.Add(key, value);
                                                        SPFolderUpdate(oSPWeb, oSPFolder);
                                                        logLine += String.Format(IMPSUC, key);
                                                    }
                                                    else
                                                    {
                                                        string oldValue = oSPFolder.Properties[key].ToString();
                                                        oSPFolder.Properties[key] = value;
                                                        SPFolderUpdate(oSPWeb, oSPFolder);
                                                        logLine += String.Format(IMPMOD, key, oldValue, value);
                                                    }
                                                }
                                                else
                                                {
                                                    if (!oSPFolder.Properties.ContainsKey(key))
                                                    {
                                                        oSPFolder.Properties.Add(key, value);
                                                        SPFolderUpdate(oSPWeb, oSPFolder);
                                                        logLine += String.Format(IMPSUC, key);
                                                    }
                                                    else
                                                    {
                                                        logLine += String.Format(IMPEXI, key);
                                                    }
                                                }
                                            }
                                        }

                                        catch (Exception ex)
                                        {
                                            logLine += ex.Message;
                                        }

                                        sb.Append(logLine);
                                        logLine = "";
                                    }
                                }
                            }
                            break;

                        default:
                            using (SPSite oSPSite = new SPSite(site))
                            {
                                using (SPWeb oSPWeb = oSPSite.OpenWeb(web))
                                {
                                    foreach (string property in propertiesToImport)
                                    {
                                        try
                                        {
                                            if (property != string.Empty)
                                            {
                                                logLine = "";

                                                string value = findNode(property);
                                                key = property.Replace("pbs_number_sign_", "#");
                                                if (cbOverwrite.Checked)
                                                {
                                                    if (!oSPWeb.AllProperties.ContainsKey(property))
                                                    {
                                                        oSPWeb.AllProperties.Add(property, value);
                                                        SPWebUpdate(oSPWeb);
                                                        logLine += String.Format(IMPSUC, property);
                                                    }
                                                    else
                                                    {
                                                        string oldValue = oSPWeb.AllProperties[property].ToString();
                                                        oSPWeb.AllProperties[property] = value;
                                                        SPWebUpdate(oSPWeb);
                                                        logLine += String.Format(IMPMOD, property, oldValue, value);
                                                    }
                                                }
                                                else
                                                {
                                                    if (!oSPWeb.AllProperties.ContainsKey(property))
                                                    {
                                                        oSPWeb.AllProperties.Add(property, value);
                                                        SPWebUpdate(oSPWeb);
                                                        logLine += String.Format(IMPSUC, property);
                                                    }
                                                    else
                                                    {
                                                        logLine += String.Format(IMPEXI, property);
                                                    }
                                                }
                                            }
                                        }

                                        catch (Exception ex)
                                        {
                                            logLine += ex.Message;
                                        }

                                        if (property != "")
                                        {
                                            sb.Append(logLine);
                                            logLine = "";
                                        }
                                    }

                                }
                            }
                            break;

                    }

                });
                lblMsg.Text = sb.ToString();

            }
            catch (Exception ex)
            {

                lblMsg.Text = ex.Message;

            }
        }

        private void SPFolderUpdate(SPWeb oSPWeb, SPFolder oSPFolder)
        {
            if (!oSPWeb.AllowUnsafeUpdates)
            {
                oSPWeb.AllowUnsafeUpdates = true;
                oSPFolder.Update();
                oSPWeb.AllowUnsafeUpdates = false;
            }
            else
            {
                oSPFolder.Update();
            }

        }

        private void SPWebUpdate(SPWeb oSPWeb)
        {
            if (!oSPWeb.AllowUnsafeUpdates)
            {
                oSPWeb.AllowUnsafeUpdates = true;
                oSPWeb.Update();
                oSPWeb.AllowUnsafeUpdates = false;
            }
            else
            {
                oSPWeb.Update();
            }

        }

        private string findNode(string node)
        {
            string value = string.Empty;
            XmlUrlResolver resolver = new XmlUrlResolver();
            resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;

            XmlTextReader textReader = new XmlTextReader(txtUrl.Text);
            textReader.XmlResolver = resolver;
            textReader.WhitespaceHandling = WhitespaceHandling.None;

            while (textReader.Read())
            {
                if (Char.IsLetter(node, 0))
                {
                    if (textReader.Name.Equals(node) && textReader.IsStartElement())
                    {
                        textReader.Read();
                        value = textReader.Value.Replace("pbs_number_sign_", "#");

                    }
                }
                else
                {
                    if (textReader.Name.Equals("pbs_data_" + node) && textReader.IsStartElement())
                    {
                        textReader.Read();
                        value = textReader.Value.Replace("pbs_number_sign_", "#");

                    }
                }
            }

            return value;
        }

        private DataTable GetDatasource(string url)
        {

            try
            {

                lblMsg.Text = "";
                DataTable properties = new DataTable();
                properties.Columns.Add(new DataColumn("ID"));
                properties.Columns.Add(new DataColumn("Key"));
                properties.Columns.Add(new DataColumn("Value"));


                XmlUrlResolver resolver = new XmlUrlResolver();
                resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;

                XmlTextReader textReader = new XmlTextReader(url);
                textReader.XmlResolver = resolver;


                DataRow row = null;
                while (textReader.Read())
                {
                    if (textReader.NodeType == XmlNodeType.Element)
                    {
                        row = properties.NewRow();
                        key = textReader.Name;


                        row["ID"] = key.Replace("pbs_data_", "");
                        row["Key"] = key.Replace("pbs_data_", "");
                    }
                    if (textReader.NodeType == XmlNodeType.Text && row != null)
                    {
                        row["Value"] = textReader.Value;
                        properties.Rows.Add(row);
                        row = null;
                    }

                }

                return properties;


            }
            catch (Exception ex)
            {

                lblMsg.Text = ex.Message;
                return null;
            }
        }


    }
}