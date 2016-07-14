using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using KPMG.Engagements.SAPInboundCustomerCRMNotification;
using KPMG.Engagements.EngagementsDomain;

namespace KPMG.Engagements.SAPInboundCustomerCRMNotification
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

    public class SAPInboundCustomerCRMUpdateService : ISAPInboundCustomerCRMNotificationService
    {

        public void CustomerCRMUpdateInformation(KPMGSapCrmCustomerInbound_OLD.CustomerCRMUpdateInformation request)
        {
            string reqId = string.Empty;
            string invoiceId = string.Empty;

            Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

            //todo
            //reqId = request.CustomerInvoiceCRMbyIDResponse_sync.MessageHeader.ID.ToString();

            if (!EventLog.SourceExists("CustomerCRMUpdateInformation"))
                EventLog.CreateEventSource("CustomerCRMUpdateInformation", "Application");

            EventLog.WriteEntry("CustomerCRMUpdateInformation", request.ToString(), EventLogEntryType.Information);

            BusinessPartnerReadMappingObject mapping = new BusinessPartnerReadMappingObject();
            mapping.ClientNo = request.CustomerCRMByIDResponse.BusinessPartner.InternalID;
            mapping.ClientName = request.CustomerCRMByIDResponse.BusinessPartner.Common.Organisation.Name.FirstLineName;

            foreach (KPMGSapCrmCustomerInbound.BPCRMElmntRspBPID identitem in request.CustomerCRMByIDResponse.BusinessPartner.Identification)
            {

                string IdentificationCode = identitem.PartyIdentifierTypeCode.ToString();
                switch (IdentificationCode)
                {
                    case IdentificationCodes.PartyIdentifierTypeCode:
                        mapping.SentinelId = identitem.BusinessPartnerID;
                        break;
                }

            }

            foreach (KPMGSapCrmCustomerInbound.CustCRMByIDRspBPRel relitem in request.CustomerCRMByIDResponse.BusinessPartner.Relationship)
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

            foreach (KPMGSapCrmCustomerInbound.CustCRMByIDRspBPRel relitem in request.CustomerCRMByIDResponse.BusinessPartner.Relationship)
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

            engagementProperties.Add("Client No", mapping.ClientNo.ToString());
            engagementProperties.Add("Client Name", mapping.ClientName.ToString());
            engagementProperties.Add("Sentinel ID", mapping.SentinelId.ToString());
            engagementProperties.Add("Lead key No", mapping.LeadKeyNo.ToString());
            engagementProperties.Add("Shareholder No", mapping.ShareholderNo.ToString());
            engagementProperties.Add("Shareholder Name", mapping.ShareholderName.ToString());


            //return (engagementProperties);




            //SAPBusinessPartnerReadService client = new SAPBusinessPartnerReadService();
            //engagementProperties = client.CustomerCRMByIDReadQuery(reqId, internalId);
            
            //todo
            //call engagement service
        }

        
    }
}
