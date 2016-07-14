using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using AcmeCorp.Engagements.UiHelpers.ApiFactory;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SharePoint.WebControls;

namespace AcmeCorp.ProjectSpace.Administration.CreateInternalContainer
{
    [ToolboxItemAttribute(false)]
    public partial class CreateInternalContainer : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public CreateInternalContainer()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
             {
                        AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(SPContext.Current.Site.WebApplication.Farm, SPContext.Current.Web).Api;
                        string lang = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpper();

                        lblTitle.Text = api.Localization.GetValue(lblTitle.ID, lang);
                        lblContainerOwners.Text = api.Localization.GetValue(lblContainerOwners.ID, lang);
                        lblDeputies.Text = api.Localization.GetValue(lblDeputies.ID, lang);
                        lblReason.Text = api.Localization.GetValue(lblReason.ID, lang);
                        btnSubmit.Text = api.Localization.GetValue(btnSubmit.ID, lang);
             }
            catch (Exception ex)
            {
                litErrorMsg.Text = "<!-- Message: " + ex.ToString() + " Trace: " + ex.StackTrace.ToString() + "-->";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {        
            if (SPContext.Current.Web.Url.Contains("dev-sp"))
            {
                SPUtility.SendEmail(SPContext.Current.Web, false, false, "administrator@devsp.com", "Site Creation request", "Engagement Creation Request");
            }


            AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(SPContext.Current.Site.WebApplication.Farm, SPContext.Current.Web).Api;

            List<string> owners = new List<string>();
            foreach (PickerEntity user in sppOwners.Entities)
            {
                owners.Add(user.Description.ToString());
            }

            List<string> deputies = new List<string>();
            foreach (PickerEntity user in sppOwners.Entities)
            {

                deputies.Add(user.Description.ToString());
            }

            string siteUrl = api.CreateNewInternalSite(tbTitle.Text, tbReason.Text, owners.ToArray(), deputies.ToArray());
            //Page.Response.AddHeader("REFRESH", "2;URL=" + siteUrl);

            litResult.Text = "Create creation proccess started for site: " + siteUrl;


            // Call the SB methods
            //////AcmeCorp.Engagements.EngagementsServiceBus.ServiceBus serviceBus = new EngagementsServiceBus.ServiceBus();
            //////QueueClient queueClient = serviceBus.CreateSbQueueClient("EngagementsServiceCalls");
            //////EngagementData engagementData = new EngagementData()
            //////{
            //////    EngagementId = engagementId,
            //////    EngagementFoldersType = engagementFoldersType,
            //////    Managers = managers,
            //////    Partners = partners,
            //////    Staff = staff,
            //////    EngagementProperties = engagementProperties
            //////};

            //////serviceBus.SendBrokeredMessage("CREATEENGAGEMENT#" + engagementId.ToString(), engagementData, queueClient);

            //var wsm = new Microsoft.SharePoint.WorkflowServices.WorkflowServicesManager(SPContext.Current.Web);

            //var subscription = wsm.GetWorkflowSubscriptionService().GetSubscription(new Guid("{2DC2893B-DEC0-433A-BB34-8DC0CD9CC8FC}"));
            //var wfi = wsm.GetWorkflowInstanceService();
            //var payload = new Dictionary<string, object>();

            //wfi.StartWorkflow(subscription, 

            //SPSecurity.RunWithElevatedPrivileges(delegate()
            //{
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        var wsm = new Microsoft.SharePoint.WorkflowServices.WorkflowServicesManager(web);

                        if (wsm.IsConnected)
                        {
                            //Microsoft.SharePoint.WorkflowServices.WorkflowDeploymentService wfDepSvc = wsm.GetWorkflowDeploymentService();
                            //Microsoft.SharePoint.WorkflowServices.WorkflowDefinitionCollection wfDefinitions = wfDepSvc.EnumerateDefinitions(true /*published definitions only*/);

                            //foreach (Microsoft.SharePoint.WorkflowServices.WorkflowDefinition definition in wfDefinitions)
                            //{
                            //    var subscription = wsm.GetWorkflowSubscriptionService().GetSubscription(definition.Id);

                            //    //retrieve the xaml for the definition
                            //    string xaml = definition.Xaml;

                            //    //retrieve the xaml for the definition
                            //    string xaml = definition.Xaml;
                            //}

                            //}
                            //SPList historyList = web.Lists["Workflow History"];  // Workflow history list
                            //SPList taskList = web.Lists["Workflow Tasks"];

                            //Microsoft.SharePoint.Workflow.SPWorkflowTemplate workflowTemplate =  web.WorkflowTemplates.GetTemplateByName("test10", System.Globalization.CultureInfo.CurrentCulture);

                            //web.AllowUnsafeUpdates = true;
                            //try
                            //{
                            //    string workflowAssocName = "test10";

                            //    // Create workflow association
                            //    Microsoft.SharePoint.Workflow.SPWorkflowAssociation workflowAssociation = Microsoft.SharePoint.Workflow.SPWorkflowAssociation.CreateWebAssociation(workflowTemplate, workflowAssocName, taskList, historyList);

                            //    // Set workflow parameters 
                            //    workflowAssociation.AllowManual = true;
                            //    workflowAssociation.AutoStartCreate = true;
                            //    workflowAssociation.AutoStartChange = false;

                            //    // Add workflow association to my list
                            //    web.WorkflowAssociations.Add(workflowAssociation);

                            //    // Enable workflow
                            //    workflowAssociation.Enabled = true;
                            //}
                            //finally
                            //{
                            //    web.AllowUnsafeUpdates = false;
                            //}

                            var subscription = wsm.GetWorkflowSubscriptionService().GetSubscription(new Guid("{ff4ea6a2-6088-488e-b7c2-cb6d2683306d}")); // {ff4ea6a2-6088-488e-b7c2-cb6d2683306d}"));
                            var wfi = wsm.GetWorkflowInstanceService();

                            var payload = new Dictionary<string, object>();
                            //payload.Add("WorkflowStart", "StartWorkflow");

                            //var payload = new Object();
				string formData = subscription.GetProperty("FormData");
				if(formData != null && formData != "undefined" && formData != "")
				{
					string[] assocParams = formData.Split(';');
					for(int i = 0; i < assocParams.Length; i++)
					{
                        payload[assocParams[i]] = subscription.PropertyDefinitions[assocParams[i]];
					}
				}
                //if(itemId)
                //{
                //    wfManager.getWorkflowInstanceService().startWorkflowOnListItem(subscription, itemId, params);
                //}
                //else
                //{
                //    wfManager.getWorkflowInstanceService().startWorkflow(subscription, params);
                //}

                            wfi.StartWorkflow(subscription, payload);

                            //SPList list = web.Lists[0].WorkflowAssociations.Add

                            ////Microsoft.SharePoint.Workflow.SPWorkflowAssociation wfAssoc = site.RootWeb.WorkflowAssociations.GetAssociationByBaseID.GetAssociationByName("Test11", System.Globalization.CultureInfo.CurrentCulture);
                            //Microsoft.SharePoint.Workflow.SPWorkflowAssociation wfAssoc = list.WorkflowAssociations.GetAssociationByName("Test10", System.Globalization.CultureInfo.CurrentCulture);
                            //site.WorkflowManager.StartWorkflow(site, wfAssoc, "", Microsoft.SharePoint.Workflow.SPWorkflowRunOptions.Asynchronous);
                        }
                    }
                }
            //});

            //SPListItem listItem = properties.ListItem;
            //SPWorkflowAssociation wfAssoc = listItem.ParentList.WorkflowAssociations.GetAssociationByName("Workflow Name", System.Globalization.CultureInfo.CurrentCulture);
            //listItem.Web.Site.WorkflowManager.StartWorkflow(listItem, wfAssoc, wfAssoc.AssociationData, true);
            }
            catch (Exception ex)
            {
                litErrorMsg.Text = "<!-- Message: " + ex.ToString() + " Trace: " + ex.StackTrace.ToString() + "-->";
            }
        }
    }
}
