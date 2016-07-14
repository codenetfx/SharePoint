using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace AcmeCorp.Engagements.FarmConfiguration
{
    public class ModifyPropertyBagSite : PBSPageBase
    {
        protected Label lblKey;
        protected Label lblValue;
        protected Label lblHeader;
        protected Label lblMsg;
        protected TextBox txtKey;
        protected TextBox txtValue;
        protected LinkButton lnkSave1;
        protected LinkButton lnkSave2;
        protected ImageButton btnSave1;
        protected ImageButton btnSave2;
        protected CheckBox cbMask;
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
                        lnkSave1.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_EDT));
                        lnkSave2.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_EDT));
                        btnSave1.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_EDT));
                        btnSave2.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_EDT));
                        break;
                    case "del":
                        txtKey.Text = key;
                        lnkSave1.Text = DEL_MSG;
                        lnkSave1.ToolTip = DEL_MSG;
                        lnkSave2.Text = DEL_MSG;
                        lnkSave2.ToolTip = DEL_MSG;
                        btnSave1.ToolTip = DEL_MSG;
                        btnSave2.ToolTip = DEL_MSG;
                        btnSave1.ImageUrl = DEL_IMG;
                        btnSave2.ImageUrl = DEL_IMG;
                        lnkSave1.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_DEL));
                        lnkSave2.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_DEL));
                        btnSave1.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_DEL));
                        btnSave2.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_DEL));
                        txtKey.Visible = false;
                        cbMask.Visible = false;
                        txtValue.Visible = false;
                        lblKey.Visible = false;
                        lblValue.Visible = false;

                        lblHeader.Text = String.Format(DELETE_HEADER, key);
                        break;
                    default:
                        lnkSave1.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_ADD));
                        lnkSave2.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_ADD));
                        btnSave1.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_ADD));
                        btnSave2.Attributes.Add("onclick", String.Format(JS_CONFIRM, key, MSG_ADD));
                        lblHeader.Text = ADD_HEADER;
                        break;
                }

            }


        }

        private void LoadProp()
        {

            switch (scope)
            {

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

        }

        private void Save()
        {
            try
            {

                string pbsValue = txtValue.Text;
                if (cbMask.Checked)
                {
                    if (pbsValue != "" && txtKey.Text != "")
                    {
                        SPSecurity.RunWithElevatedPrivileges(delegate()
                        {
                            AcmeCorp.Engagements.FarmConfiguration.cTripleDES des = new AcmeCorp.Engagements.FarmConfiguration.cTripleDES();
                            pbsValue = des.Encrypt(pbsValue);
                        });
                    }
                }
                switch (scope)
                {

                    case "lst":
                        using (SPSite oSPSite = new SPSite(site))
                        {
                            using (SPWeb oSPWeb = oSPSite.OpenWeb(web))
                            {
                                SPFolder oSPFolder = oSPWeb.GetFolder(guid);

                                switch (mod)
                                {
                                    case "edt":
                                        oSPFolder.Properties[txtKey.Text] = pbsValue;
                                        SPFolderUpdate(oSPWeb, oSPFolder);
                                        lblMsg.Text = MSG_EDT_PRO;
                                        break;

                                    case "del":

                                        oSPFolder.Properties[txtKey.Text] = null;
                                        oSPFolder.Properties.Remove(txtKey.Text);
                                        SPFolderUpdate(oSPWeb, oSPFolder);
                                        lblMsg.Text = MSG_DEL_PRO;
                                        break;

                                    default:

                                        oSPFolder.Properties.Add(txtKey.Text, pbsValue);
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
                                        oSPWeb.AllProperties[txtKey.Text] = pbsValue;
                                        SPWebUpdate(oSPWeb);
                                        lblMsg.Text = MSG_EDT_PRO;
                                        break;
                                    case "del":

                                        oSPWeb.AllProperties[txtKey.Text] = null;
                                        oSPWeb.AllProperties.Remove(txtKey.Text);
                                        SPWebUpdate(oSPWeb);
                                        lblMsg.Text = MSG_DEL_PRO;
                                        break;
                                    default:
                                        oSPWeb.AllProperties.Add(txtKey.Text, pbsValue);
                                        SPWebUpdate(oSPWeb);
                                        lblMsg.Text = MSG_ADD_PRO;
                                        break;
                                }
                            }

                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
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