using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using KPMG.Engagements.SAPBusinessPartnerRead;
using KPMG.Engagements.EngagementsDomain;

namespace KPMG.Engagements.SAPInboundBusinessPartnerRead
{

    public class SalesOrderChargeableCodeReadMappingObject
    {
        public string AuftragsNr;
        public string Bezeichnung;
        public string OpportunityNr;
        public string EngPartner;
        public string EngPartnerRoleCode;
        public string EngManager;
        public string EngManagerRoleCode;
        public string SentinelAppNummer;
        public string Function;
        public string BusinessArea;
        public string LoB;
        public string Segment;
        public string ProfitCenterEng;
        public string StartDatum;
        public string Jahr;
        public string SAPFirstCloseDate;
        public string WBAuftragStatus;
        public string WBAuftragStatusDatum;
        public string RetentionPolicy;
        public string IndefiniteHold;
        public string ExpiryDate;
        public string OpportunitySiteCollection;
        public string Account;
        public string AccountRoleCode;
        public string Beneficiary;
        public string BeneficiaryRoleCode;
        public string LeadKey;
        public string LeadKeyRoleCode;
        public string ServiceCode;
        public string ServiceCodeDescription;
        public string ProcessCodeModuleKey;
        public string ProcessCodeModuleText;
        public string ProcessCodeValidFromDate;
        public string ProcessCodeValidToDate;
    }

    public static class RoleCodes
    {
        public const string Account = "AG";
        public const string Beneficiary = "WE";
        public const string LeadKey = "ZS";
        public const string EngPartner = "ZE";
        public const string EngManager = "ZM";

    };

    public class SAPInboundBusinessPartnerReadService : ISAPInboundBusinessPartnerReadService
    {

        public void ChargeableCodeChangeNotification(KPMGSapChargeableCodeCrmInbound_OLD.ChargeableCodeChangeNotification request)
        {
            
            Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

            if (!EventLog.SourceExists("ChargeableCodeChangeNotification"))
                EventLog.CreateEventSource("ChargeableCodeChangeNotification", "Application");

            EventLog.WriteEntry("ChargeableCodeChangeNotification", request.ToString(), EventLogEntryType.Information);

            SalesOrderChargeableCodeReadMappingObject mapping = new SalesOrderChargeableCodeReadMappingObject();
            
            mapping.AuftragsNr = request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.ID.ToString();
            mapping.Bezeichnung = request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.Description;
            mapping.OpportunityNr = request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.OpportunityID;



            foreach (KPMGSapChargeableCodeCrmInbound.SlsOrdERPByIDRsp_s_V3Pty salesOrderItem in request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.Party)
            {
                string roleCode = salesOrderItem.RoleCode.ToString();
                switch(roleCode)
                {
                    case RoleCodes.EngPartner:
                        mapping.EngPartnerRoleCode = roleCode.ToString();
                        mapping.EngPartner = salesOrderItem.InternalID.ToString();
                        break;

                    case RoleCodes.EngManager:
                        mapping.EngManagerRoleCode = roleCode.ToString();
                        mapping.EngManager = salesOrderItem.InternalID.ToString();
                        break;

                    case RoleCodes.Account:
                        mapping.AccountRoleCode = roleCode.ToString();
                        mapping.Account = salesOrderItem.InternalID.ToString();
                        break;

                    case RoleCodes.Beneficiary:
                        mapping.BeneficiaryRoleCode = roleCode.ToString();
                        mapping.Beneficiary = salesOrderItem.InternalID.ToString();
                        break;

                    case RoleCodes.LeadKey:
                        mapping.LeadKeyRoleCode = roleCode.ToString();
                        mapping.LeadKey = salesOrderItem.InternalID.ToString();
                        break;

                    default:
                        break;
                }
            }

            mapping.SentinelAppNummer = request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.SentinelID;
            mapping.Function = request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.Function;
            mapping.BusinessArea = request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.BusinessArea;
            mapping.LoB = request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.LineOfBusiness;
            mapping.Segment = request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.Segment;
            mapping.ProfitCenterEng = request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.ProfitCenter;
            mapping.StartDatum = request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.StartDate.ToString();
            mapping.Jahr = request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.BusinessYear;
            mapping.SAPFirstCloseDate = string.Empty; //not relevant
            mapping.WBAuftragStatus = request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.Status.ToString(); 
            mapping.WBAuftragStatusDatum = request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.StatusDate.ToString();


            //mapping.ServiceCode = response.SalesOrder.Item[0].ID.ToString();
            //mapping.ServiceCodeDescription = response.SalesOrder.Item[0].Description.ToString();
            //mapping.ProcessCodeModuleKey = response.SalesOrder.ProcessCodeList[0].ModuleID.ToString();
            //mapping.ProcessCodeModuleText = response.SalesOrder.ProcessCodeList[0].ModuleText.ToString();
            //mapping.ProcessCodeValidFromDate = response.SalesOrder.ProcessCodeList[0].ValidFrom.ToString();
            //mapping.ProcessCodeValidToDate = response.SalesOrder.ProcessCodeList[0].ValidTo.ToString();

           
            engagementProperties.Add("Auftrags-Nr", mapping.AuftragsNr.ToString());
            engagementProperties.Add("Bezeichnung", mapping.Bezeichnung.ToString());
            engagementProperties.Add("Opportunity Nr", mapping.OpportunityNr.ToString());
            engagementProperties.Add("Eng.Partner", mapping.EngPartner.ToString());
            engagementProperties.Add("Eng.Manager", mapping.EngManager.ToString());
            engagementProperties.Add("Sentinal App Nummer", mapping.SentinelAppNummer.ToString());

            engagementProperties.Add("Function", mapping.Function.ToString());
            engagementProperties.Add("LoB", mapping.LoB.ToString());
            engagementProperties.Add("Segment", mapping.Segment.ToString());
            engagementProperties.Add("Profitcenter Eng.", mapping.ProfitCenterEng.ToString());
            engagementProperties.Add("WB-Auftrag Status", mapping.WBAuftragStatus.ToString());
            engagementProperties.Add("WB-Auftrag Status Datum", mapping.WBAuftragStatusDatum.ToString());

            engagementProperties.Add("Account", mapping.Account.ToString());
            engagementProperties.Add("Beneficiary", mapping.Beneficiary.ToString());
            engagementProperties.Add("Lead Key", mapping.LeadKey.ToString());

            //engagementProperties.Add("Service Code", mapping.ServiceCode.ToString());
            //engagementProperties.Add("Service Code Description", mapping.ServiceCodeDescription.ToString());
            //engagementProperties.Add("Process Code Module Key", mapping.ProcessCodeModuleKey.ToString());
            //engagementProperties.Add("Process Code Module Text", mapping.ProcessCodeModuleText.ToString());
            //engagementProperties.Add("Process Code Valid From Date", mapping.ProcessCodeValidFromDate.ToString());
            //engagementProperties.Add("Process Code Valid To Date", mapping.ProcessCodeValidToDate.ToString());

            

            //request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.Party[0]

            //internalId = request.SalesOrderERPByIDResponse_sync_V3.SalesOrder.ID.ToString();

            //SAPBusinessPartnerReadService client = new SAPBusinessPartnerReadService();
            //engagementProperties = client.CustomerCRMByIDReadQuery(reqId, internalId);
            
            //todo
            //call engagement service
            //return (engagementProperties);
        }

        
    }
}
