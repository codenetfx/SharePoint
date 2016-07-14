using System;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using KPMGSapCrmNewOpportunityInbound;
using KPMG.Engagements.SAPOpportunityRead;
using KPMG.Engagements.EngagementsDomain;
using SAPLookupTableService.EngagementsService;
using KPMG.Engagements.EngagementsApi.Utilities;

namespace SAPCustomerCRMNotificationWebService
{

    public class BusinessPartnerReadMappingObject
    {
        public string ClientNo = string.Empty;
        public string ClientName = string.Empty;
        public string SentinelId = string.Empty;
        public string LeadKeyNo = string.Empty;
        public string LeadName = string.Empty;
        public string ShareholderNo = string.Empty;
        public string ShareholderName = string.Empty;
    }

    public static class RoleCodes
    {
        public const string HasTheLeadKey = "ZZS001-1";
        public const string IsLeadOrganisationOf = "ZZS001-2";
        public const string IsShareholderOf = "BURC01-1";
        public const string HasTheShareHolder = "BURC01-2";
    };

    public static class IdentificationCodes
    {
        public const string PartyIdentifierTypeCode = "ZSENID";

    };

    
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://sap.com/xi/CRM/Global2")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CustomerCRMNotificationWebService : System.Web.Services.WebService
    {

        [WebMethod]
        //public void CustomerCRMUpdateInformation(KPMGSapCrmCustomerInbound.CustomerCRMUpdateInformation request)
        public void CustomerCRMByIDResponse(KPMGSapCrmCustomerInbound.CustCRMByIDRspBP BusinessPartner)
        {
            string reqId = string.Empty;
            string invoiceId = string.Empty;
            try {
            Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

            

            EventLog.WriteEntry("SharePoint-KPMG", ":Inbound:CustomerCRMNotificationWebService: BusinessPartner Request:"+BusinessPartner.ToString(), EventLogEntryType.Information);

            BusinessPartnerReadMappingObject mapping = new BusinessPartnerReadMappingObject();
            mapping.ClientNo = BusinessPartner.InternalID.ToString();
            mapping.ClientName = BusinessPartner.Common.BusinessPartnerFormattedName.ToString(); // Organisation.Name.FirstLineName;

            try
            {
                if (BusinessPartner.Identification.Length == 0) return;
            

            foreach (KPMGSapCrmCustomerInbound.BPCRMElmntRspBPID identitem in BusinessPartner.Identification)
            {

                string IdentificationCode = identitem.PartyIdentifierTypeCode.Value.ToString();
                if (IdentificationCode.Contains(IdentificationCodes.PartyIdentifierTypeCode.ToString()))
                {              
                        mapping.SentinelId = identitem.BusinessPartnerID.ToString();
                       
                }

            }
            }
            catch {
                EventLog.WriteEntry("SharePoint-KPMG", "Inbound:CustomerCRMNotificationWebService: Warning: Identification array missing from the message", EventLogEntryType.Warning);

            }

            try {
            foreach (KPMGSapCrmCustomerInbound.CustCRMByIDRspBPRel relitem in BusinessPartner.Relationship)
            {

                string RoleCode = relitem.RoleCode.Value.ToString();
                switch (RoleCode)
                {
                    case RoleCodes.HasTheLeadKey:
                        mapping.LeadKeyNo = relitem.RelationshipBusinessPartnerInternalID.ToString();
                        break;
                    case RoleCodes.IsLeadOrganisationOf:
                        mapping.LeadKeyNo = relitem.RelationshipBusinessPartnerInternalID.ToString();
                        break;
                }



            }
            }
            catch {
                EventLog.WriteEntry("SharePoint-KPMG", "Inbound:CustomerCRMNotificationWebService:Relationship array missing from the message", EventLogEntryType.Warning);

            }

            mapping.LeadName = string.Empty; // use client service to get this information
            mapping.ShareholderNo = string.Empty;
            mapping.ShareholderName = string.Empty;
            mapping.ShareholderNo = string.Empty;


            //if (BusinessPartner.Relationship == null)
            //{
            //    EventLog.WriteEntry("SharePoint-KPMG", "Inbound:CustomerCRMNotificationWebService:Business.Relationship array is missing", EventLogEntryType.Error);
            //    return;
            //}

            try
            {

                foreach (KPMGSapCrmCustomerInbound.CustCRMByIDRspBPRel relitem in BusinessPartner.Relationship)
                {

                    string RoleCode = relitem.RoleCode.Value.ToString();
                    switch (RoleCode)
                    {
                        case RoleCodes.IsShareholderOf:
                            mapping.ShareholderNo = relitem.RelationshipBusinessPartnerInternalID.ToString();
                            break;
                        case RoleCodes.HasTheShareHolder:
                            mapping.ShareholderNo = relitem.RelationshipBusinessPartnerInternalID.ToString();
                            break;
                    }

                }
            }
            catch
            {
                EventLog.WriteEntry("SharePoint-KPMG", "Inbound:CustomerCRMNotificationWebService:Business.Relationship array is missing", EventLogEntryType.Error);
                return;
            }



            ////filtering - not necessary, per Email from Thomas Fuhrmann (19.7.2013)

            //string filterKeyName = "Function-" + BusinessPartner.Function.ToString() + "-" + mapping.ServiceArea.ToString();
            //System.Configuration.Configuration rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
            //string customSAPFilterElement = ConfigurationManager.AppSettings[filterKeyName.ToString()];
            //if (customSAPFilterElement != null)
            //{
            //    EventLog.WriteEntry("SharePoint-KPMG", "Filter defined: Request permitted for Function=" + mapping.Function.ToString() + " and Service Area=" + SalesOrder.SalesAndServiceBusinessArea.ToString(), EventLogEntryType.Warning);

            //}
            //else
            //{
            //    EventLog.WriteEntry("SharePoint-KPMG", "Filter not defined: Request rejected for Function=" + mapping.Function.ToString() + " and Service Area=" + SalesOrder.SalesAndServiceBusinessArea.ToString(), EventLogEntryType.Warning);
            //    return;

            //}

            engagementProperties.Add("Client No", mapping.ClientNo.ToString());
            engagementProperties.Add("Client Name", mapping.ClientName.ToString());
            engagementProperties.Add("Sentinel ID", mapping.SentinelId.ToString());
            engagementProperties.Add("Lead key No", mapping.LeadKeyNo.ToString());
            engagementProperties.Add("Shareholder No", mapping.ShareholderNo.ToString());
            engagementProperties.Add("Shareholder Name", mapping.ShareholderName.ToString());


            EngagementsServiceClient EngagementsServiceClient = new EngagementsServiceClient("EngagementsServiceEndPoint");

            //KPMG.Engagements.EngagementsApi.Api api = new KPMG.Engagements.EngagementsApi.Api("");
            //ActiveDirectoryHelpers adHelpers = new ActiveDirectoryHelpers(api.AdDeputyGroupsOu, api.AdDeputiesGroupPrefix, api.AdDeputiesOuContainer, api.AdUsersOuContainer);
            //string oppManager = adHelpers.ResolveLoginName(mapping.EngManager);
            //string oppPartner = adHelpers.ResolveLoginName(mapping.EngPartner);

            string OpportunityNr = "test";
            string EngManager = "test";
            string EngPartner = "test";
            //EngagementsServiceClient.UpdateOpportunitySiteProperties(OpportunityNr, EngManager, EngPartner, engagementProperties);

            }

            catch (Exception ex)
            {
                EventLog.WriteEntry("SharePoint-KPMG", ":Inbound:SalesOrderWebService Error:"+ex.ToString(), EventLogEntryType.Error);
                throw new Exception(ex.ToString());
            }

        }
        
        

    }
}
