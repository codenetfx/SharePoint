// -----------------------------------------------------------------------
// <copyright file="Form1.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace TestEngagementsService_WinForms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Microsoft.Office.Server.UserProfiles;
    using Microsoft.SharePoint;


    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "*", Justification = "Test application")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.SpacingRules", "*", Justification = "Test application")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Test application")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.LayoutRules", "*", Justification = "Test application")]
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "*", Justification = "Test application")]
    public partial class Form1 : Form
    {
                private TestEngagementsService_WinForms.EngagementsService.EngagementsServiceClient client;
 
        public Form1()
        {
            this.InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new TestEngagementsService_WinForms.EngagementsService.EngagementsServiceClient();
            client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            client.ChannelFactory.Credentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
        }

        private void LoadFields()
        {
            try
            {
                Helpers.DummyUsers dummyUsers = new Helpers.DummyUsers();

                this.toolStripStatusLabel1.Text = "Getting initial values... (users)";
                this.Cursor = Cursors.WaitCursor;

                this.txtStaff.Clear();

                using (SPSite spSite = new SPSite(ConfigurationManager.AppSettings["RootSite"]))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {

                        SPUserCollection allUsers = spWeb.SiteUsers;
                        SPUser mUser = allUsers[1];

                        this.txtWbNumber.Text = new Random().Next(10000, 99999).ToString();

                        //spWeb.EnsureUser("devsp\\johnsmith");

                        SPUser managerUser = spWeb.SiteUsers[this.GetFromSiteUsers(dummyUsers)];
                        this.txtEngagementManager.Text = managerUser.LoginName.Split('\\')[1];

                        SPUser partnerUser = spWeb.SiteUsers[this.GetFromSiteUsers(dummyUsers)];
                        this.txtEngagementPartner.Text = partnerUser.LoginName.Split('\\')[1];

                        SPUser concurentPartner = spWeb.SiteUsers[this.GetFromSiteUsers(dummyUsers)];
                        this.txtConcurringPartner.Text = concurentPartner.LoginName.Split('\\')[1];

                        this.datAuftragStatusDatum.Value = DateTime.Now.Date;

                        int staffNum = new Random().Next(5, 9);

                        List<string> staffList = new List<string>();

                        for (int i = 0; i < staffNum; i++)
                        {
                            SPUser staffUser = spWeb.SiteUsers[this.GetFromSiteUsers(dummyUsers)];
                            string staffLogin = staffUser.LoginName.Split('\\')[1];
                            staffList.Add(staffLogin);
                            this.txtStaff.AppendText(staffLogin);
                            if (i < staffNum - 1)
                            {
                                this.txtStaff.AppendText(";");
                            }
                        }

                    }
                }
                this.toolStripStatusLabel1.Text = string.Empty;
                this.Cursor = Cursors.Default;

                string bla = string.Empty;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        private string GetFromSiteUsers(Helpers.DummyUsers dummyUsers)
        {
            string username = dummyUsers.GetNextUser();
            string domain = ConfigurationManager.AppSettings["DefaultDomain"];
            return domain + username;

        }

        private void EnsureAllUsers()
        {
            try
            {
                Helpers.DummyUsers dummyUsers = new Helpers.DummyUsers();

                this.toolStripStatusLabel1.Text = "Ensuring all users";
                this.Cursor = Cursors.WaitCursor;

                this.txtStaff.Clear();

                using (SPSite spSite = new SPSite(ConfigurationManager.AppSettings["RootSite"]))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        foreach (string username in dummyUsers.DummyUserNames)
                        {
                            try
                            {
                                spWeb.EnsureUser(username);
                                Debug.Print("Ensured: " + username);

                            }
                            catch (Exception ex)
                            {
                                this.txtStaff.AppendText(username + ", ");
                                Debug.Print("Could not ensure: " + username);
                            }
                        }

                    }
                }
                this.toolStripStatusLabel1.Text = string.Empty;
                this.Cursor = Cursors.Default;

                string bla = string.Empty;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            this.LoadFields();
        }

        private void btnCreateEngagement_Click(object sender, EventArgs e)
        {
            string newSiteUrl = string.Empty;

            if (this.ValidateChildren())
            {
                try
                {


                    //AcmeCorp.Engagements.EngagementsApi.Api api = new AcmeCorp.Engagements.EngagementsApi.Api("");
                    string domain = ConfigurationManager.AppSettings["DefaultDomain"];

                    List<string> partners = new List<string>();
                    partners.Add(domain + this.txtEngagementPartner.Text);

                    List<string> managers = new List<string>();
                    managers.Add(domain + this.txtEngagementManager.Text);

                    List<string> staff = new List<string>();
                    foreach (string staffName in this.txtStaff.Text.Split(';'))
                    {
                        staff.Add(domain + staffName);
                    }

                    Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

                    engagementProperties.Add("Name des Mandanten", this.txtCompany.Text);
                    engagementProperties.Add("Opportunity Nr", this.txtOpportunityNumber.Text);
                    engagementProperties.Add("Account", this.txtCompany.Text);
                    engagementProperties.Add("Concurring Partner", domain + this.txtConcurringPartner.Text);
                    engagementProperties.Add("Niederlassung", this.txtNiederlasdsung.Text);
                    engagementProperties.Add("Bezeichnung", this.txtBezeichnung.Text);
                    engagementProperties.Add("WB-Auftrags-Nr", this.txtWbNumber.Text);
                    engagementProperties.Add("Eng.Partner", domain + this.txtEngagementPartner.Text);
                    engagementProperties.Add("Eng.Manager", domain + this.txtEngagementManager.Text);
                    engagementProperties.Add("WB-Auftrag Status", this.txtAuftragStatus.Text);
                    engagementProperties.Add("WB-Auftrag Status Datum", this.datAuftragStatusDatum.Value.Date);

                    //TODO:
                    //to be used later for partner and manager groups
                    engagementProperties.Add("partnergroup", string.Empty);
                    engagementProperties.Add("managergroup", string.Empty);

                    newSiteUrl = client.CreateNewEngagementSite(Convert.ToInt64(txtWbNumber.Text), 1, managers.ToArray(), partners.ToArray(), staff.ToArray(), engagementProperties);
                    this.txtOutput.Text = "Engagement site is created with WB ID: " + txtWbNumber.Text;




                }
                catch (Exception ex)
                {
                    this.txtOutput.Text = "Error in engagement site creation: " + ex.Message;

                    Debug.Print(ex.Message);
                }
            }
        }

        private void btnGetStatus_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.txtProcessSite.Text != null && this.txtProcessSite.Text != string.Empty)
                {

                long engagementId = Convert.ToInt64(this.txtProcessSite.Text);



                string retVal = client.GetEngagementStatus(engagementId);

                this.txtEngagementStatus.Text = retVal;
                }
                else
                {
                    MessageBox.Show("Please enter WB Nummer of site to process");
                    this.txtProcessSite.Focus();
                }

            }
            catch (Exception ex)
            {
                this.txtOutput.Text = "Error in engagement site creation: " + ex.Message;

                Debug.Print(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtProcessSite.Text != null && this.txtProcessSite.Text != string.Empty)
                {
                long engagementId = Convert.ToInt64(this.txtProcessSite.Text);
                
                string retVal = client.CloseEngagement(engagementId);

                this.txtEngagementStatus.Text = "Engagement " + engagementId.ToString() + " has been closed.";
                }
                else
                {
                    MessageBox.Show("Please enter WB Nummer of site to process");
                    this.txtProcessSite.Focus();
                }

            }
            catch (Exception ex)
            {
                this.txtOutput.Text = "Error by engagement closing: " + ex.Message;

                Debug.Print(ex.Message);
            }
        }

        private void btnReopen_Click(object sender, EventArgs e)
        {
            try
            {
                
if (this.txtProcessSite.Text != null && this.txtProcessSite.Text != string.Empty)
                {
                long engagementId = Convert.ToInt64(this.txtProcessSite.Text);

                string retVal = client.ReopenEngagement(engagementId);

                this.txtEngagementStatus.Text = "Engagement " + engagementId.ToString() + " has been reopened.";
}
                else
                {
                    MessageBox.Show("Please enter WB Nummer of site to process");
                    this.txtProcessSite.Focus();
                }

            }
            catch (Exception ex)
            {
                this.txtOutput.Text = "Error by reopening closing: " + ex.Message;

                Debug.Print(ex.Message);
            }
        }

        private void btnCreateOpportunity_Click(object sender, EventArgs e)
        {
            string newSiteUrl = string.Empty;

            if (this.ValidateChildren())
            {
                try
                {


                    //AcmeCorp.Engagements.EngagementsApi.Api api = new AcmeCorp.Engagements.EngagementsApi.Api("");
                    string domain = ConfigurationManager.AppSettings["DefaultDomain"];

                    List<string> partners = new List<string>();
                    partners.Add(domain + this.txtEngagementPartner.Text);

                    List<string> managers = new List<string>();
                    managers.Add(domain + this.txtEngagementManager.Text);

                    List<string> staff = new List<string>();
                    foreach (string staffName in this.txtStaff.Text.Split(';'))
                    {
                        staff.Add(domain + staffName);
                    }

                    Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

                    engagementProperties.Add("Name des Mandanten", this.txtCompany.Text);
                    engagementProperties.Add("Opportunity Nr", this.txtOpportunityNumber.Text);
                    engagementProperties.Add("Account", this.txtCompany.Text);
                    engagementProperties.Add("Concurring Partner", domain + this.txtConcurringPartner.Text);
                    engagementProperties.Add("Niederlassung", this.txtNiederlasdsung.Text);
                    engagementProperties.Add("Bezeichnung", this.txtBezeichnung.Text);
                    engagementProperties.Add("WB-Auftrags-Nr", this.txtWbNumber.Text);
                    engagementProperties.Add("Eng.Partner", domain + this.txtEngagementPartner.Text);
                    engagementProperties.Add("Eng.Manager", domain + this.txtEngagementManager.Text);
                    engagementProperties.Add("WB-Auftrag Status", this.txtAuftragStatus.Text);
                    engagementProperties.Add("WB-Auftrag Status Datum", this.datAuftragStatusDatum.Value.Date);

                    //TODO:
                    //to be used later for partner and manager groups
                    engagementProperties.Add("partnergroup", string.Empty);
                    engagementProperties.Add("managergroup", string.Empty);

                    newSiteUrl = client.CreateNewOpportunitySite(Convert.ToInt64(txtWbNumber.Text), managers.ToArray(), partners.ToArray(), staff.ToArray(), engagementProperties);
                    this.txtOutput.Text = "Opportunity site is created with WB ID: " + txtWbNumber.Text;

                }
                catch (Exception ex)
                {
                    this.txtOutput.Text = "Error in engagement site creation: " + ex.Message;

                    Debug.Print(ex.Message);
                }
            }
        }

        private void btnUpdateEngagement_Click(object sender, EventArgs e)
        {
            string newSiteUrl = string.Empty;

            if (this.ValidateChildren())
            {
                try
                {


                    //AcmeCorp.Engagements.EngagementsApi.Api api = new AcmeCorp.Engagements.EngagementsApi.Api("");
                    string domain = ConfigurationManager.AppSettings["DefaultDomain"];

                    List<string> partners = new List<string>();
                    partners.Add(domain + this.txtEngagementPartner.Text);

                    List<string> managers = new List<string>();
                    managers.Add(domain + this.txtEngagementManager.Text);

                    List<string> staff = new List<string>();
                    foreach (string staffName in this.txtStaff.Text.Split(';'))
                    {
                        staff.Add(domain + staffName);
                    }

                    Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

                    engagementProperties.Add("Name des Mandanten", this.txtCompany.Text);
                    engagementProperties.Add("Opportunity Nr", this.txtOpportunityNumber.Text);
                    engagementProperties.Add("Account", this.txtCompany.Text);
                    engagementProperties.Add("Concurring Partner", domain + this.txtConcurringPartner.Text);
                    engagementProperties.Add("Niederlassung", this.txtNiederlasdsung.Text);
                    engagementProperties.Add("Bezeichnung", this.txtBezeichnung.Text);
                    engagementProperties.Add("WB-Auftrags-Nr", this.txtWbNumber.Text);
                    engagementProperties.Add("Eng.Partner", domain + this.txtEngagementPartner.Text);
                    engagementProperties.Add("Eng.Manager", domain + this.txtEngagementManager.Text);
                    engagementProperties.Add("WB-Auftrag Status", this.txtAuftragStatus.Text);
                    engagementProperties.Add("WB-Auftrag Status Datum", this.datAuftragStatusDatum.Value.Date);

                    //TODO:
                    //to be used later for partner and manager groups
                    engagementProperties.Add("partnergroup", string.Empty);
                    engagementProperties.Add("managergroup", string.Empty);

                    newSiteUrl = client.UpdateEngagementSiteProperties(Convert.ToInt64(txtWbNumber.Text), 1, managers.ToArray(), partners.ToArray(), staff.ToArray(), engagementProperties);
                    this.txtOutput.Text = "Engagement site is Updated with WB ID: " + txtWbNumber.Text;

                }
                catch (Exception ex)
                {
                    this.txtOutput.Text = "Error in engagement site update: " + ex.Message;

                    Debug.Print(ex.Message);
                }
            }
        }

        private void btnUpdateOpportunity_Click(object sender, EventArgs e)
        {
            string newSiteUrl = string.Empty;

            if (this.ValidateChildren())
            {
                try
                {


                    //AcmeCorp.Engagements.EngagementsApi.Api api = new AcmeCorp.Engagements.EngagementsApi.Api("");
                    string domain = ConfigurationManager.AppSettings["DefaultDomain"];

                    List<string> partners = new List<string>();
                    partners.Add(domain + this.txtEngagementPartner.Text);

                    List<string> managers = new List<string>();
                    managers.Add(domain + this.txtEngagementManager.Text);

                    List<string> staff = new List<string>();
                    foreach (string staffName in this.txtStaff.Text.Split(';'))
                    {
                        staff.Add(domain + staffName);
                    }

                    Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

                    engagementProperties.Add("Name des Mandanten", this.txtCompany.Text);
                    engagementProperties.Add("Opportunity Nr", this.txtOpportunityNumber.Text);
                    engagementProperties.Add("Account", this.txtCompany.Text);
                    engagementProperties.Add("Concurring Partner", domain + this.txtConcurringPartner.Text);
                    engagementProperties.Add("Niederlassung", this.txtNiederlasdsung.Text);
                    engagementProperties.Add("Bezeichnung", this.txtBezeichnung.Text);
                    engagementProperties.Add("WB-Auftrags-Nr", this.txtWbNumber.Text);
                    engagementProperties.Add("Eng.Partner", domain + this.txtEngagementPartner.Text);
                    engagementProperties.Add("Eng.Manager", domain + this.txtEngagementManager.Text);
                    engagementProperties.Add("WB-Auftrag Status", this.txtAuftragStatus.Text);
                    engagementProperties.Add("WB-Auftrag Status Datum", this.datAuftragStatusDatum.Value.Date);

                    //TODO:
                    //to be used later for partner and manager groups
                    engagementProperties.Add("partnergroup", string.Empty);
                    engagementProperties.Add("managergroup", string.Empty);

                    newSiteUrl = client.UpdateOpportunitySiteProperties(Convert.ToInt64(txtWbNumber.Text), managers.ToArray(), partners.ToArray(), staff.ToArray(), engagementProperties);
                    this.txtOutput.Text = "Opportunity site is updated with WB ID: " + txtWbNumber.Text;

                }
                catch (Exception ex)
                {
                    this.txtOutput.Text = "Error in opportunity site update: " + ex.Message;

                    Debug.Print(ex.Message);
                }
            }
        }

        private void txtEngagementPartner_Validating(object sender, CancelEventArgs e)
        {
            string error = null;
            if (txtEngagementPartner.Text.Length == 0)
            {
                error = "Please enter valid username of Engagement Partner.";
                e.Cancel = true;
            }
            epEngagementPartner.SetError((Control)sender, error);

        }

        private void txtEngagementManager_Validating(object sender, CancelEventArgs e)
        {
            string error = null;
            if (txtEngagementManager.Text.Length == 0)
            {
                error = "Please enter valid username of Engagement Manager.";
                e.Cancel = true;
            }
            epEngagementManager.SetError((Control)sender, error);

        }

        private void txtStaff_Validating(object sender, CancelEventArgs e)
        {
            string error = null;
            if (txtStaff.Text.Length == 0)
            {
                error = "Please enter valid usernames for staff, delimited by semicollon (;).";
                e.Cancel = true;
            }
            epStaff.SetError((Control)sender, error);
        }

        private void txtConcurringPartner_Validating(object sender, CancelEventArgs e)
        {
            string error = null;
            if (txtConcurringPartner.Text.Length == 0)
            {
                error = "Please enter valid username of Concurring Partner.";
                e.Cancel = true;
            }
            epConcurringPartner.SetError((Control)sender, error);

        }

        private void txtWbNumber_Validating(object sender, CancelEventArgs e)
        {
            string error = null;
            if (txtWbNumber.Text.Length == 0)
            {
                error = "Please enter valid WB Number.";
                e.Cancel = true;
            }
            epWbNumber.SetError((Control)sender, error);
        }
    }
}
