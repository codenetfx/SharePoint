using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Administration;
using System.Web.UI;

namespace AcmeCorp.Engagements.FarmConfiguration
{
    public partial class ModifyPropertyBag : PBSPageBase
    {
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
        const string EDIT_HEADER = "<H3 class='ms-standardheader'>Edit Property</H3>";
        const string DELETE_HEADER = "<H3 class='ms-standardheader'>Delete {0} Property</H3>";
        const string ADD_HEADER = "<H3 class='ms-standardheader'>Add Property</H3>";
        const string DEL_MSG = "Delete";
        const string DEL_IMG = "/_layouts/images/DELITEM.GIF";
        const string JS_MSGBOX = "return showMsg('{0}')";
        const string JS_CONFIRM = "return confirmModify('{0}','{1}')";
        const string MSG_DEL = "This will permanently delete the property! Please confirm that you want to delete the property ";
        const string MSG_ADD = "Please confirm that you want to add the property ";
        const string MSG_EDT = "Please confirm that you want to edit the property ";
        const string MSG_ADD_PRO = "The property was added ";
        const string MSG_DEL_PRO = "The property was deleted";
        const string MSG_EDT_PRO = "The property was modified ";

        protected void Page_Load(object sender, EventArgs e)
        {

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
                switch (mod)
                {
                    case "edt":
                        txtKey.Text = key;
                        lblHeader.Text = EDIT_HEADER;
                        txtKey.Enabled = false;

                        LoadProp();

                        lnkSave2.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_EDT));

                        btnSave2.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_EDT));
                        break;
                    case "del":
                        txtKey.Text = key;

                        lnkSave2.Text = DEL_MSG;
                        lnkSave2.ToolTip = DEL_MSG;

                        btnSave2.ToolTip = DEL_MSG;

                        btnSave2.ImageUrl = DEL_IMG;

                        lnkSave2.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_DEL));

                        btnSave2.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_DEL));
                        txtKey.Visible = false;
                        txtValue.Visible = false;
                        lblKey.Visible = false;
                        lblValue.Visible = false;

                        lblHeader.Text = String.Format(DELETE_HEADER, key);
                        break;
                    default:

                        lnkSave2.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_ADD));

                        btnSave2.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_ADD));
                        lblHeader.Text = ADD_HEADER;
                        break;
                }

            }
        }

        private void LoadProp()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
         {
             SPFarm farm;

             switch (scope)
             {
                 case "farm":
                     farm = SPFarm.Local;
                     if (farm.Properties.ContainsKey(key))
                     {
                         txtValue.Text = farm.Properties[key].ToString();
                     }
                     break;
                 case "ser":
                     farm = SPFarm.Local;
                     SPServer server = farm.Servers[guid];
                     if (server.Properties.ContainsKey(key))
                     {
                         txtValue.Text = server.Properties[key].ToString();
                     }
                     break;
                 case "app":
                     farm = SPFarm.Local;
                     SPWebService service = farm.Services.GetValue<SPWebService>("");
                     SPWebApplication oSPWebApplication = service.WebApplications[guid];
                     if (oSPWebApplication.Properties.ContainsKey(key))
                     {
                         txtValue.Text = oSPWebApplication.Properties[key].ToString();
                     }
                     break;
                 case "lst":
                     using (SPSite oSPSite = new SPSite(site))
                     {
                         using (SPWeb oSPWeb = oSPSite.OpenWeb(web))
                         {
                             SPFolder oSPFolder = oSPWeb.GetFolder(guid);
                             if (oSPFolder.Properties.ContainsKey(key))
                             {
                                 txtValue.Text = oSPFolder.Properties[key].ToString();
                             }
                         }
                     }
                     break;
                 default:

                     using (SPSite oSPSite = new SPSite(site))
                     {
                         using (SPWeb oSPWeb = oSPSite.OpenWeb(web))
                         {
                             if (oSPWeb.AllProperties.ContainsKey(key))
                             {
                                 txtValue.Text = oSPWeb.AllProperties[key].ToString();
                             }
                         }
                     }
                     break;
             }
         });
        }

        private void Save()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
         {
             SPFarm farm;

             switch (scope)
             {
                 case "farm":
                     SPSecurity.RunWithElevatedPrivileges(delegate()
                     {
                         farm = SPFarm.Local;
                         switch (mod)
                         {
                             case "edt":
                                 farm.Properties[txtKey.Text] = txtValue.Text;
                                 farm.Update();
                                 lblMsg.Text = MSG_EDT_PRO;
                                 break;
                             case "del":

                                 farm.Properties[txtKey.Text] = null;
                                 farm.Properties.Remove(txtKey.Text);
                                 farm.Update();
                                 lblMsg.Text = MSG_DEL_PRO;
                                 break;
                             default:
                                 farm.Properties.Add(txtKey.Text, txtValue.Text);
                                 farm.Update();
                                 lblMsg.Text = MSG_ADD_PRO;
                                 break;
                         }
                     });
                     break;
                 case "ser":
                     SPSecurity.RunWithElevatedPrivileges(delegate()
                     {
                         farm = SPFarm.Local;
                         SPServer server = farm.Servers[guid];
                         switch (mod)
                         {
                             case "edt":
                                 server.Properties[txtKey.Text] = txtValue.Text;
                                 server.Update();
                                 lblMsg.Text = MSG_EDT_PRO;
                                 break;
                             case "del":

                                 server.Properties[txtKey.Text] = null;
                                 server.Properties.Remove(txtKey.Text);
                                 server.Update();
                                 lblMsg.Text = MSG_DEL_PRO;
                                 break;
                             default:
                                 server.Properties.Add(txtKey.Text, txtValue.Text);
                                 server.Update();
                                 lblMsg.Text = MSG_ADD_PRO;
                                 break;
                         }
                     });
                     break;
                 case "app":
                     SPSecurity.RunWithElevatedPrivileges(delegate()
                     {

                         farm = SPFarm.Local;
                         SPWebService service = farm.Services.GetValue<SPWebService>("");
                         SPWebApplication oSPWebApplication = service.WebApplications[guid];

                         switch (mod)
                         {
                             case "edt":
                                 oSPWebApplication.Properties[txtKey.Text] = txtValue.Text;
                                 oSPWebApplication.Update();
                                 lblMsg.Text = MSG_EDT_PRO;
                                 break;
                             case "del":

                                 oSPWebApplication.Properties[txtKey.Text] = null;
                                 oSPWebApplication.Properties.Remove(txtKey.Text);
                                 oSPWebApplication.Update();
                                 lblMsg.Text = MSG_DEL_PRO;
                                 break;
                             default:
                                 oSPWebApplication.Properties.Add(txtKey.Text, txtValue.Text);
                                 oSPWebApplication.Update();
                                 lblMsg.Text = MSG_ADD_PRO;
                                 break;
                         }
                     });
                     break;
                 case "lst":
                     using (SPSite oSPSite = new SPSite(site))
                     {
                         using (SPWeb oSPWeb = oSPSite.OpenWeb(web))
                         {
                             SPFolder oSPFolder = oSPWeb.GetFolder(guid);

                             switch (mod)
                             {
                                 case "edt":
                                     oSPFolder.Properties[txtKey.Text] = txtValue.Text;
                                     SPFolderUpdate(oSPWeb, oSPFolder);
                                     //oSPFolder.Update();
                                     lblMsg.Text = MSG_EDT_PRO;
                                     break;

                                 case "del":

                                     oSPFolder.Properties[txtKey.Text] = null;
                                     oSPFolder.Properties.Remove(txtKey.Text);
                                     //oSPFolder.Update();
                                     SPFolderUpdate(oSPWeb, oSPFolder);
                                     lblMsg.Text = MSG_DEL_PRO;
                                     break;

                                 default:

                                     oSPFolder.Properties.Add(txtKey.Text, txtValue.Text);
                                     //oSPFolder.Update();
                                     SPFolderUpdate(oSPWeb, oSPFolder);
                                     lblMsg.Text = MSG_ADD_PRO;
                                     break;
                             }

                         }
                     }
                     break;
                 default:

                     using (SPSite oSPSite = new SPSite(site))
                     {
                         using (SPWeb oSPWeb = oSPSite.OpenWeb(web))
                         {
                             switch (mod)
                             {
                                 case "edt":
                                     oSPWeb.AllProperties[txtKey.Text] = txtValue.Text;
                                     SPWebUpdate(oSPWeb);
                                     //oSPWeb.Update();
                                     lblMsg.Text = MSG_EDT_PRO;
                                     break;
                                 case "del":

                                     oSPWeb.AllProperties[txtKey.Text] = null;
                                     oSPWeb.AllProperties.Remove(txtKey.Text);
                                     SPWebUpdate(oSPWeb);
                                     //oSPWeb.Update();
                                     lblMsg.Text = MSG_DEL_PRO;
                                     break;
                                 default:

                                     oSPWeb.AllProperties.Add(txtKey.Text, txtValue.Text);
                                     SPWebUpdate(oSPWeb);
                                     //oSPWeb.Update();
                                     lblMsg.Text = MSG_ADD_PRO;
                                     break;
                             }
                         }

                     }
                     break;
             }

         });
        }

        protected void Save_Click(object sender, ImageClickEventArgs e)
        {
            Save();
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            Save();
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
    }
}
