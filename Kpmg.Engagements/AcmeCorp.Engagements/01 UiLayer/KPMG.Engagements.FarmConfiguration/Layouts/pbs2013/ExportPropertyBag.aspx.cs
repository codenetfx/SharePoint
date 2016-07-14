using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint;
using System.Xml;
using Microsoft.SharePoint.Administration;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;

namespace AcmeCorp.Engagements.FarmConfiguration
{
    public class ExportPropertyBag : PBSPageBase
    {
        protected DropDownList ddlLists;
        protected Label lblMsg;
        private string scope = String.Empty;
        private Guid site = Guid.Empty;
        private Guid web = Guid.Empty;
        private Guid guid = Guid.Empty;
        private string key = String.Empty;
        private string mod = String.Empty;

        const string QS_SCOPE = "scope";
        const string QS_SITE = "site";
        const string QS_WEB = "web";
        const string QS_MODE = "mod";
        const string QS_KEY = "key";
        const string QS_GUID = "guid";
        const string OPN_FILE = "<a class=\"ms-toolbar\" onclick=\"javascript:window.open('{0}');\"   onmouseover=\"this.style.cursor='hand';\" >{0}</a>";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Request.QueryString[QS_SITE] != null && Request.QueryString[QS_SITE] != String.Empty)
            {
                site = new Guid(Request.QueryString[QS_SITE]);
            }
            else
            {
                site = SPContext.Current.Site.ID;
            }
            if (Request.QueryString[QS_WEB] != null && Request.QueryString[QS_WEB] != String.Empty)
            {
                web = new Guid(Request.QueryString[QS_WEB]);
            }
            else
            {
                web = SPContext.Current.Web.ID;
            }
            if (Request.QueryString[QS_GUID] != null && Request.QueryString[QS_GUID] != String.Empty)
            {
                guid = new Guid(Request.QueryString[QS_GUID]);
            }
            if (Request.QueryString[QS_MODE] != null)
            {
                mod = Request.QueryString[QS_MODE];
            }
            if (Request.QueryString[QS_KEY] != null)
            {
                key = Request.QueryString[QS_KEY];
            }
            if (Request.QueryString[QS_SCOPE] != null)
            {
                scope = Request.QueryString[QS_SCOPE];
            }


            if (!Page.IsPostBack)
            {
                LoadLists();
            }
        }


        private void LoadLists()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                ddlLists.Items.Clear();

                using (SPSite oSite = new SPSite(site))
                {
                    using (SPWeb oWeb = oSite.AllWebs[web])
                    {

                        SPListCollection collLists = oWeb.Lists;
                        foreach (SPList oList in collLists)
                        {
                            if (oList.BaseType == SPBaseType.DocumentLibrary)
                            {
                                ListItem folder = new ListItem(oList.Title, oWeb.Url + "/" + oList.RootFolder.ToString());
                                ddlLists.Items.Add(folder);
                                if (oList.Title == "Shared Documents")
                                {
                                    ddlLists.SelectedValue = oWeb.Url + "/" + oList.RootFolder.ToString();
                                }
                            }
                        }
                    }
                }

            });

        }



        private void Export(string folder)
        {

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {

                using (SPSite oSite = new SPSite(site))
                {
                    using (SPWeb oWeb = oSite.AllWebs[web])
                    {
                        try
                        {

                            string fileName = "";
                            oWeb.AllowUnsafeUpdates = true;
                            SPFolder myLibrary = oWeb.Folders[folder];
                            XmlWriterSettings wSettings = new XmlWriterSettings();
                            wSettings.Indent = true;
                            System.IO.MemoryStream ms = new System.IO.MemoryStream();
                            using (XmlWriter writer = XmlWriter.Create(ms, wSettings))
                            {
                                if (writer != null)
                                {
                                    writer.WriteComment("Property Bag Settings by Alon Havivi http://havivi.blogspot.com");
                                    writer.WriteComment(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
                                    writer.WriteStartElement("PropertyBags");
                                    if (scope == "lst")
                                    {
                                        SPFolder oSPFolder = oWeb.GetFolder(guid);
                                        List<SortedProperties> sp = SortProperties(oSPFolder.Properties);
                                        WriteElement(writer, sp);
                                        fileName = oSPFolder.Name.Trim();
                                    }
                                    else if (scope == "app")
                                    {
                                        SPFarm farm = SPFarm.Local;
                                        SPWebService service = farm.Services.GetValue<SPWebService>("");
                                        SPWebApplication oSPWebApplication = service.WebApplications[guid];
                                        SPServer server = farm.Servers[guid];
                                        List<SortedProperties> sp = SortProperties(oSPWebApplication.Properties);
                                        WriteElement(writer, sp);
                                        fileName = oSite.WebApplication.Name.Trim();
                                    }
                                    else if (scope == "ser")
                                    {
                                        SPFarm farm = SPFarm.Local;
                                        SPServer server = farm.Servers[guid];
                                        List<SortedProperties> sp = SortProperties(server.Properties);
                                        WriteElement(writer, sp);
                                        fileName = farm.Name.Trim();
                                        fileName = server.Name.Trim();
                                    }
                                    else if (scope == "farm")
                                    {
                                        SPFarm farm = SPFarm.Local;
                                        List<SortedProperties> sp = SortProperties(farm.Properties);
                                        WriteElement(writer, sp);
                                        fileName = farm.Name.Trim();
                                    }
                                    else
                                    {

                                        List<SortedProperties> sp = SortProperties(oWeb.AllProperties);
                                        WriteElement(writer, sp);
                                        fileName = oWeb.Title.Trim();

                                    }
                                    writer.WriteEndElement();
                                    writer.Flush();
                                }
                                byte[] buffer = ms.ToArray();
                                fileName += "_PropertyBag_" + Guid.NewGuid().ToString().Substring(1, 3) + ".pbs";
                                myLibrary.Files.Add(fileName, buffer, true);
                                myLibrary.Update();

                            }

                            lblMsg.Text = "Browse to the location of the saved file: " + String.Format(OPN_FILE, folder + "/" + fileName);
                        }
                        catch (Exception ex)
                        {

                            lblMsg.Text = ex.Message;
                        }
                    }
                }
            });


        }

        private static void WriteElement(XmlWriter writer, List<SortedProperties> sp)
        {
            string key = string.Empty;
            string value = string.Empty;
            foreach (SortedProperties property in sp)
            {
                key = property.Key;
                value = property.Value;

                if (key.Contains("#"))
                {
                    key = key.Replace("#", "pbs_number_sign_");
                }
                if (value.Contains("#"))
                {
                    value = value.Replace("#", "pbs_number_sign_");
                }

                if (Char.IsLetter(key, 0))
                {
                    writer.WriteElementString(key, value);
                }
                else
                {

                    writer.WriteElementString("pbs_data_" + key, value);

                }

            }
        }

        private static List<SortedProperties> SortProperties(Hashtable properties)
        {
            List<SortedProperties> sp = new List<SortedProperties>();
            foreach (DictionaryEntry property in properties)
            {
                if (property.Value != null)
                {
                    sp.Add(new SortedProperties(property.Key.ToString(), property.Value.ToString()));
                }
            }

            sp.Sort(new GenericComparer<SortedProperties>("Key", GenericComparer<SortedProperties>.SortOrder.Ascending));
            return sp;
        }

        public static string RemoveTroublesomeCharacters(string value)
        {
            string newString = value;
            for (int i = 0; i < value.Length; ++i)
            {
                if (value[i] > 0xFFFD)
                {
                    newString = "alon";
                }
                else if (value[i] < 0x20 && value[i] != '\t' & value[i] != '\n' & value[i] != '\r')
                {
                    newString = "alon";
                }

            }
            return newString.ToString();
        }
        protected void Export_Click(object sender, ImageClickEventArgs e)
        {
            Export(ddlLists.SelectedValue);
        }

        protected void Export_Click(object sender, EventArgs e)
        {
            Export(ddlLists.SelectedValue);
        }
    }
}