using System;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using KPMG.Engagements.SAPBusinessPartnerRead;
using KPMG.Engagements.EngagementsDomain;
using SAPLookupTableService.EngagementsService;


namespace KPMG.Engagements.SAPBusinessPartnerRead
{

    public class BusinessPartnerReadMappingObject
    {
        public string ClientNo;
        public string ClientName;
        public string SentinelId;
        public string LeadKeyNo;
        public string LeadName;
        public string ShareholderNo;
        public string ShareholderName;
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

    public class SAPBusinessPartnerReadService : ISAPBusinessPartnerReadService
    {

        public Dictionary<string, object> CustomerCRMByIDReadQuery(string reqId, string internalId)
        {
            try {
            KPMGSapCrmOutbound.CustomerCRMByIDReadQueryRequest crmrequest = new KPMGSapCrmOutbound.CustomerCRMByIDReadQueryRequest();
            Dictionary<string, object> engagementProperties = new Dictionary<string, object>();


            KPMGSapCrmOutbound.BusinessDocumentMessageID msgId = new KPMGSapCrmOutbound.BusinessDocumentMessageID();
            msgId.Value = reqId;
            
            crmrequest.CustomerCRMByIDQuery.MessageHeader.ID = msgId;
            crmrequest.CustomerCRMByIDQuery.BusinessPartnerSelectionByBusinessPartner.InternalID = internalId;

            KPMGSapCrmOutbound.CustomerCRMByIDQueryResponse_OutbClient client = new KPMGSapCrmOutbound.CustomerCRMByIDQueryResponse_OutbClient("CustomersOutbound");

            KPMGSapCrmOutbound.CustomerCRMByIDResponseMessage response =  client.CustomerCRMByIDReadQuery(crmrequest.CustomerCRMByIDQuery);
            
            BusinessPartnerReadMappingObject mapping = new BusinessPartnerReadMappingObject();
            mapping.ClientNo = response.BusinessPartner.InternalID;
            mapping.ClientName = response.BusinessPartner.Common.Organisation.Name.FirstLineName;
            
            foreach (KPMGSapCrmOutbound.BPCRMElmntRspBPID identitem in response.BusinessPartner.Identification)
            {
                
                string IdentificationCode = identitem.PartyIdentifierTypeCode.ToString();
                switch (IdentificationCode)
                {
                    case IdentificationCodes.PartyIdentifierTypeCode:
                        mapping.SentinelId = identitem.BusinessPartnerID;
                        break;           
                }

            }

            foreach (KPMGSapCrmOutbound.CustCRMByIDRspBPRel relitem in response.BusinessPartner.Relationship)
            {
                
                string RoleCode = relitem.RoleCode.ToString();
                switch (RoleCode)
                {                       
                    case RoleCodes.HasTheLeadKey:
                        mapping.LeadKeyNo = relitem.RelationshipBusinessPartnerInternalID;
                        break;
                    case RoleCodes.IsLeadOrganisationOf:
                        mapping.LeadKeyNo = relitem.RelationshipBusinessPartnerInternalID;
                        break;
                }

                    

            }
            mapping.LeadName = string.Empty; // use client service to get this information
            
            foreach (KPMGSapCrmOutbound.CustCRMByIDRspBPRel relitem in response.BusinessPartner.Relationship)
            {
               
                string RoleCode = relitem.RoleCode.ToString();
                switch (RoleCode)
                {
                    case RoleCodes.IsShareholderOf:
                        mapping.ShareholderNo = relitem.RelationshipBusinessPartnerInternalID;
                        break;
                    case RoleCodes.HasTheShareHolder:
                        mapping.ShareholderNo = relitem.RelationshipBusinessPartnerInternalID;
                        break;
                }

            }

            ////filtering

            //string filterKeyName = "Function-" + response.BusinessPartner.Function.ToString() + "-" + response.ServiceArea.ToString();
            //System.Configuration.Configuration rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
            //string customSAPFilterElement = ConfigurationManager.AppSettings[filterKeyName.ToString()];
            //if (customSAPFilterElement != null)
            //{
            //    EventLog.WriteEntry("SharePoint-KPMG", "Filter defined: Request permitted for Function=" + mapping.Function.ToString() + " and Service Area=" + mapping.ServiceArea.ToString(), EventLogEntryType.Warning);

            //}
            //else
            //{
            //    EventLog.WriteEntry("SharePoint-KPMG", "Filter not defined: Request rejected for Function=" + mapping.Function.ToString() + " and Service Area=" + mapping.ServiceArea.ToString(), EventLogEntryType.Warning);
            //    return (engagementProperties);

            //}
           

            

            engagementProperties.Add("Client No", mapping.ClientNo.ToString());
            engagementProperties.Add("Client Name", mapping.ClientName.ToString());
            engagementProperties.Add("Sentinel ID", mapping.SentinelId.ToString());
            engagementProperties.Add("Lead key No", mapping.LeadKeyNo.ToString());
            engagementProperties.Add("Shareholder No", mapping.ShareholderNo.ToString());
            engagementProperties.Add("Shareholder Name", mapping.ShareholderName.ToString());

            EngagementsServiceClient EngagementsServiceClient = new EngagementsServiceClient();
            //EngagementsServiceClient.UpdateEngagementSiteProperties(
            return (engagementProperties);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("SharePoint-KPMG", "Outbound:SAPOpportunityReadService:" + ex.ToString(), EventLogEntryType.Error);

                throw new Exception(ex.ToString());
            }


        }



       
       
    }
}
