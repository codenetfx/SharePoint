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
using KPMGSapChargeableCodeCrmInbound;
using KPMG.Engagements.SAPBusinessPartnerRead;
using KPMG.Engagements.EngagementsDomain;
using SAPLookupTableService.EngagementsService;
using KPMG.Engagements.EngagementsApi.Utilities;

namespace SAPSalesOrderWebService
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

    
    
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://sap.com/xi/APPL/Global2")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SalesOrderWebService : System.Web.Services.WebService
    {

        [WebMethod]
        //public void ChargeableCodeChangeNotification(KPMGSapChargeableCodeCrmInbound.ChargeableCodeChangeNotification request)
        public void SalesOrderERPByIDResponse_sync_V3(KPMGSapChargeableCodeCrmInbound.SlsOrdERPByIDRsp_s_V3SlsOrd SalesOrder)
        {

            try {
            Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

            
            
      

            EventLog.WriteEntry("SharePoint-KPMG", ":Inbound:SalesOrderWebService:SalesOrder Request:"+SalesOrder.ToString(), EventLogEntryType.Information);

            SalesOrderChargeableCodeReadMappingObject mapping = new SalesOrderChargeableCodeReadMappingObject();

            mapping.AuftragsNr = SalesOrder.ID.Value.ToString();
            mapping.Bezeichnung = SalesOrder.Description;
            mapping.OpportunityNr = SalesOrder.OpportunityID.ToString();



            foreach (KPMGSapChargeableCodeCrmInbound.SlsOrdERPByIDRsp_s_V3Pty salesOrderItem in SalesOrder.Party)
            {
                string roleCode = salesOrderItem.RoleCode.ToString();
                switch (roleCode)
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

                    default:
                        break;
                }
            }

            mapping.SentinelAppNummer = SalesOrder.SentinelID;
            mapping.Function = SalesOrder.Function;
            mapping.BusinessArea = SalesOrder.BusinessArea;
            mapping.LoB = SalesOrder.LineOfBusiness;
            mapping.Segment = SalesOrder.Segment;
            mapping.ProfitCenterEng = SalesOrder.ProfitCenter;
            mapping.StartDatum = SalesOrder.StartDate.ToString();
            mapping.Jahr = SalesOrder.BusinessYear;
            mapping.SAPFirstCloseDate = string.Empty; //not relevant
            mapping.WBAuftragStatus = SalesOrder.Status.ToString();
            mapping.WBAuftragStatusDatum = SalesOrder.StatusDate.ToString();

            //filtering

            string filterKeyName = "Function-*";
            System.Configuration.Configuration rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
            string WildcardSAPFilterElement = ConfigurationManager.AppSettings[filterKeyName.ToString()];
            if (WildcardSAPFilterElement != null)
            {
                EventLog.WriteEntry("SharePoint-KPMG", "Wildcard Filter defined: Request permitted for Function=" + mapping.Function.ToString(), EventLogEntryType.Warning);

            }
            else
            {
                EventLog.WriteEntry("SharePoint-KPMG", "Wildcard Filter not defined: Request rejected for Function=" + mapping.Function.ToString(), EventLogEntryType.Warning);


            }


            filterKeyName = "Function-" + mapping.Function.ToString() + "-" + SalesOrder.SalesAndServiceBusinessArea.ToString();
            rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
            string customSAPFilterElement = ConfigurationManager.AppSettings[filterKeyName.ToString()];

            if (WildcardSAPFilterElement == null)
            if (customSAPFilterElement != null)
            {
                EventLog.WriteEntry("SharePoint-KPMG", "Filter defined: Request permitted for Function=" + mapping.Function.ToString() + " and Service Area=" + SalesOrder.SalesAndServiceBusinessArea.ToString(), EventLogEntryType.Warning);

            }
            else
            {
                EventLog.WriteEntry("SharePoint-KPMG", "Filter not defined: Request rejected for Function=" + mapping.Function.ToString() + " and Service Area=" + SalesOrder.SalesAndServiceBusinessArea.ToString(), EventLogEntryType.Warning);
                return;

            }

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

            EngagementsServiceClient EngagementsServiceClient = new EngagementsServiceClient("EngagementsServiceEndPoint");

            //KPMG.Engagements.EngagementsApi.Api api = new KPMG.Engagements.EngagementsApi.Api("");
            //ActiveDirectoryHelpers adHelpers = new ActiveDirectoryHelpers(api.AdDeputyGroupsOu, api.AdDeputiesGroupPrefix, api.AdDeputiesOuContainer, api.AdUsersOuContainer);
            //string oppManager = adHelpers.ResolveLoginName(mapping.EngManager);
            //string oppPartner = adHelpers.ResolveLoginName(mapping.EngPartner);

            EngagementsServiceClient.CreateNewOpportunitySite(mapping.OpportunityNr, mapping.EngManager, mapping.EngPartner, engagementProperties);
        }

            catch (Exception ex)
            {
                EventLog.WriteEntry("SharePoint-KPMG", ":Inbound:SalesOrderWebService Error:" + ex.ToString(), EventLogEntryType.Error);
                throw new Exception(ex.ToString());
            }
            
}
            

    }
        
}
