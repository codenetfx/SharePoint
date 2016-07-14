using System;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using KPMG.Engagements.SAPSalesOrderChargeableCodeRead;
using KPMG.Engagements.EngagementsDomain;
using SAPLookupTableService.EngagementsService;
using KPMG.Engagements.EngagementsApi.Utilities;

namespace KPMG.Engagements.SAPSalesOrderChargeableCodeRead
{

    public class SalesOrderChargeableCodeReadMappingObject
    {
        public string AuftragsNr = string.Empty;
        public string Bezeichnung = string.Empty;
        public string OpportunityNr = string.Empty;
        public string EngPartner = string.Empty;
        public string EngPartnerRoleCode = string.Empty;
        public string EngManager = string.Empty;
        public string EngManagerRoleCode = string.Empty;
        public string SentinelAppNummer = string.Empty;
        public string Function = string.Empty;
        public string BusinessArea = string.Empty;
        public string LoB = string.Empty;
        public string Segment = string.Empty;
        public string ProfitCenterEng = string.Empty;
        public string StartDatum = string.Empty;
        public string Jahr = string.Empty;
        public string SAPFirstCloseDate = string.Empty;
        public string WBAuftragStatus = string.Empty;
        public string WBAuftragStatusDatum = string.Empty;
        public string RetentionPolicy = string.Empty;
        public string IndefiniteHold = string.Empty;
        public string ExpiryDate = string.Empty;
        public string OpportunitySiteCollection = string.Empty;
        public string Account = string.Empty;
        public string AccountRoleCode = string.Empty;
        public string Beneficiary = string.Empty;
        public string BeneficiaryRoleCode = string.Empty;
    }

    public static class RoleCodes
    {
        public const string Account = "AG";
        public const string Beneficiary = "WE";
        public const string LeadKey = "ZS";
        public const string EngPartner = "ZE";
        public const string EngManager = "ZM";

    };

    

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class SAPSalesOrderChargeableCodeReadService : ISAPSalesOrderChargeableCodeReadService
    {

        //KPMGSapChargeableCodeCrmOutbound.ChargeableCodeByIDUpdateQueryResponse ChargeableCodeByIDUpdateQuery(KPMGSapChargeableCodeCrmOutbound.ChargeableCodeByIDUpdateQueryRequest request);


        public Dictionary<string, object> ChargeableCodeByIDUpdateQuery(string ChargeableCodeNo)
        {

            try {
                EventLog.WriteEntry("SharePoint-KPMG", "Outbound:SAPSalesOrderChargeableCodeRead:ChargeableCodeNo=" + ChargeableCodeNo.ToString(), EventLogEntryType.Information);

            Dictionary<string, object> engagementProperties = new Dictionary<string, object>();
            KPMGSapChargeableCodeCrmOutbound.ChargeableCodeByIDUpdateQueryRequest request = new KPMGSapChargeableCodeCrmOutbound.ChargeableCodeByIDUpdateQueryRequest();


            KPMGSapChargeableCodeCrmOutbound.SalesOrderID myId = new KPMGSapChargeableCodeCrmOutbound.SalesOrderID();
            myId.Value = ChargeableCodeNo;
            
            request.SalesOrderERPByIDQuery_sync_V3.SalesOrderERPSelectionByID.ID = myId;


            KPMGSapChargeableCodeCrmOutbound.ChargeableCodeByIDQuery_OutbClient client = new KPMGSapChargeableCodeCrmOutbound.ChargeableCodeByIDQuery_OutbClient("ChargeableCodeOutbound");

            KPMGSapChargeableCodeCrmOutbound.SlsOrdERPByIDRspMsg_s_V3 response = client.ChargeableCodeByIDUpdateQuery(request.SalesOrderERPByIDQuery_sync_V3);
            
            SalesOrderChargeableCodeReadMappingObject mapping = new SalesOrderChargeableCodeReadMappingObject();
            mapping.AuftragsNr = response.SalesOrder.ID.Value.ToString();
            mapping.Bezeichnung = response.SalesOrder.Description;
            mapping.OpportunityNr = response.SalesOrder.OpportunityID.ToString();
           

            foreach (KPMGSapChargeableCodeCrmOutbound.SlsOrdERPByIDRsp_s_V3Pty salesOrderItem in response.SalesOrder.Party)
            {
                string roleCode = salesOrderItem.RoleCode.ToString();

                switch (roleCode)
                {

                    case RoleCodes.EngPartner:
                        mapping.EngPartnerRoleCode = salesOrderItem.RoleCode.ToString();
                        mapping.EngPartner = salesOrderItem.InternalID.Value.ToString();
                        break;

                    case RoleCodes.EngManager:
                        mapping.EngManagerRoleCode = salesOrderItem.RoleCode.ToString();
                        mapping.EngManager = salesOrderItem.InternalID.Value.ToString();
                        break;

                    case RoleCodes.Account:
                        mapping.Account = salesOrderItem.InternalID.Value.ToString();
                        break;

                    case RoleCodes.Beneficiary:
                        mapping.BeneficiaryRoleCode = salesOrderItem.RoleCode.ToString();
                        mapping.Beneficiary = salesOrderItem.InternalID.Value.ToString();
                        break;

                    default:
                        break;
                }
            }

            mapping.SentinelAppNummer = response.SalesOrder.SentinelID;
            mapping.Function = response.SalesOrder.Function;
            mapping.BusinessArea = response.SalesOrder.BusinessArea;
            mapping.LoB = response.SalesOrder.LineOfBusiness;
            mapping.Segment = response.SalesOrder.Segment;
            mapping.ProfitCenterEng = response.SalesOrder.ProfitCenter;
            mapping.StartDatum = response.SalesOrder.StartDate.ToString();
            mapping.Jahr = response.SalesOrder.BusinessYear.ToString();
            mapping.SAPFirstCloseDate = string.Empty; //not relevant
            mapping.WBAuftragStatus = response.SalesOrder.Status.ToString(); //verify //.Status.UserStatus.Code;
            mapping.WBAuftragStatusDatum = response.SalesOrder.StatusDate.ToString(); //verify


            ////filtering

            //string filterKeyName = "Function-" + mapping.Function.ToString() + "-" + response.SalesOrder.SalesAndServiceBusinessArea.ToString();
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
           

           

            engagementProperties.Add("Auftrags-Nr", mapping.AuftragsNr.ToString());
            engagementProperties.Add("Bezeichnung", mapping.Bezeichnung.ToString());
            engagementProperties.Add("Opportunity Nr", mapping.OpportunityNr.ToString());
            engagementProperties.Add("Eng.Partner", mapping.EngPartner.ToString());
            engagementProperties.Add("Eng.Manager", mapping.EngManager.ToString());
            engagementProperties.Add("Sentinel App Nummer", mapping.SentinelAppNummer.ToString());
            engagementProperties.Add("Function", mapping.Function.ToString());
            engagementProperties.Add("LoB", mapping.LoB.ToString());
            engagementProperties.Add("Segment", mapping.Segment.ToString());
            engagementProperties.Add("Profitcenter Eng.", mapping.ProfitCenterEng.ToString());
            engagementProperties.Add("StartDatum", mapping.StartDatum.ToString());
            engagementProperties.Add("Jahr", mapping.Jahr.ToString());
            engagementProperties.Add("SAP First Close date", mapping.SAPFirstCloseDate.ToString());
            engagementProperties.Add("WB-Auftrag Status", mapping.WBAuftragStatus.ToString());
            engagementProperties.Add("WB-Auftrag Status Datum", mapping.WBAuftragStatusDatum.ToString());
            engagementProperties.Add("Retention Policy", mapping.RetentionPolicy.ToString());
            engagementProperties.Add("Indefinite Hold", mapping.IndefiniteHold.ToString());
            engagementProperties.Add("Expiry Date", mapping.ExpiryDate.ToString());
            engagementProperties.Add("Opportunity Site Collection", mapping.OpportunitySiteCollection.ToString());
            engagementProperties.Add("Account", mapping.Account.ToString());
            engagementProperties.Add("Beneficiary", mapping.Beneficiary.ToString());


            EngagementsServiceClient EngagementsServiceClient = new EngagementsServiceClient();

            KPMG.Engagements.EngagementsApi.Api api = new KPMG.Engagements.EngagementsApi.Api("");
            ActiveDirectoryHelpers adHelpers = new ActiveDirectoryHelpers(api.AdDeputyGroupsOu, api.AdDeputiesGroupPrefix, api.AdDeputiesOuContainer, api.AdUsersOuContainer);
            string engManager = adHelpers.ResolveLoginName(mapping.EngManager);
            string engPartner = adHelpers.ResolveLoginName(mapping.EngPartner);

            EngagementsServiceClient.CreateNewEngagementSite(mapping.OpportunityNr, mapping.AuftragsNr, engManager, engPartner, engagementProperties);
           

            return (engagementProperties);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("SharePoint-KPMG", ":Outbound:SAPsalesOrderChargeableCodeRead:"+ ex.ToString(), EventLogEntryType.Error);

                throw new Exception(ex.ToString());
            }

        }



       
        public string GetData(int value)
        {
            

            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
