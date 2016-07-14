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
//using KPMG.Engagements.SAPInboundNewOpportunityCRMNotification;
using KPMG.Engagements.SAPOpportunityRead;
using KPMG.Engagements.EngagementsDomain;
using SAPLookupTableService.EngagementsService;
using KPMG.Engagements.EngagementsApi.Utilities;

namespace SAPOpportunityWebService
{
    
    public class OpportunityReadMappingObject
    {
        public string OpportunityNr = string.Empty;
        public string Bezeichnung = string.Empty;
        public string Account = string.Empty;
        public string EngPartner = string.Empty;
        public string EngManager = string.Empty;
        public string SalesManager = string.Empty;
        public string ConcurringPartner = string.Empty;
        public string Function = string.Empty;
        public string ServiceArea = string.Empty;
        public string LoB = string.Empty;
        public string Segment = string.Empty;
        public string KPMGGesellschaft = string.Empty;
        public string Niederlassung = string.Empty;
        public string Geschaeftsbereich717273 = string.Empty;
        public string Leistungsempfaenger = string.Empty;
        public string StartDatum = string.Empty;
        public string EndDatum = string.Empty;
        public string Status = string.Empty;
        public string EAStatus = string.Empty;
        public string ExpiryDate = string.Empty;
        public string Wahrscheinlichkeit = string.Empty;
        public string Fee = string.Empty;
        public string Waehrung = string.Empty;
        public string GlobalFee = string.Empty;
        public string WaehrungGlobalFee = string.Empty;
        public string ExpectedRecoveryRate = string.Empty;
        public string SuccessFee = string.Empty;
        public string ProfitCentre = string.Empty;
        public string Branche = string.Empty;
        public string AbschlussDatum = string.Empty;
        public string ForecastStartDate = string.Empty;

    }

    public static class PartnerCodes
    {
        public const string Account = "0000021";
        public const string EngPartner = "Z0000002";
        public const string EngManager = "Z0000006";
        public const string SalesManager = "00000010";
        public const string ConcurringPartner = "ZCONCURP";
        public const string Leistungsempfaenger = "SRV_LSTG";
    };

    
    
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://sap.com/xi/CRM/SE")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class OpportunityWebService : System.Web.Services.WebService
    {

        

        [WebMethod]
        public void OpportunityCRMByIDResponse_sync(KPMGSapCrmNewOpportunityInbound.OpptCRMByIDResponse_sync Opportunity)
        {
            string opportunityId = string.Empty;
            string typeCode = string.Empty;

            try
            {
                Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

                //if (!EventLog.SourceExists("OpportunityCRMUpdateInformation"))
                //    EventLog.CreateEventSource("OpportunityCRMUpdateInformation", "Application");

                //EventLog.WriteEntry("OpportunityCRMUpdateInformation", OpportunityCRMByIDResponse_sync.ToString(), EventLogEntryType.Information);

                EventLog.WriteEntry("SharePoint-KPMG", " Opportunity request ID:" + Opportunity.ID.ToString(), EventLogEntryType.Information);

                OpportunityReadMappingObject mapping = new OpportunityReadMappingObject();
                mapping.OpportunityNr = Opportunity.OpportunityHeader.ID.Value.ToString();
                if (Opportunity.OpportunityHeader.DescriptionName != null)
                    mapping.Bezeichnung = Opportunity.OpportunityHeader.DescriptionName.Value.ToString();

                if (Opportunity.OpportunityHeader.Party.Allopportunities == null)
                {
                    EventLog.WriteEntry("SharePoint-KPMG", " Opportunity array empty. Exiting" + Opportunity.ToString(), EventLogEntryType.Error);
                    return;
                }



                foreach (KPMGSapCrmNewOpportunityInbound.OpptCRMByIDAllopportunities opportunityItem in Opportunity.OpportunityHeader.Party.Allopportunities)
                {

                    string refPartnerFctCode = opportunityItem.RefPartnerFctCode.ToString();

                    if (refPartnerFctCode.Contains(PartnerCodes.Account))
                         mapping.Account = opportunityItem.PartnerNoID.Value.ToString();

                    if (refPartnerFctCode.Contains(PartnerCodes.EngManager))
                        mapping.EngManager = opportunityItem.PartnerNoID.Value.ToString();

                    if (refPartnerFctCode.Contains(PartnerCodes.ConcurringPartner))
                        mapping.ConcurringPartner = opportunityItem.PartnerNoID.Value.ToString();

                    if (refPartnerFctCode.Contains(PartnerCodes.EngPartner))
                        mapping.EngPartner = opportunityItem.PartnerNoID.Value.ToString();

                    if (refPartnerFctCode.Contains(PartnerCodes.SalesManager))
                        mapping.SalesManager = opportunityItem.PartnerNoID.Value.ToString();

                    if (refPartnerFctCode.Contains(PartnerCodes.Leistungsempfaenger))
                        mapping.Leistungsempfaenger = opportunityItem.PartnerNoID.Value.ToString();

                    //switch (refPartnerFctCode)
                    //{
                    //    case PartnerCodes.Account:
                    //        mapping.Account = opportunityItem.PartnerNoID.Value.ToString();
                    //        break;
                    //    case PartnerCodes.EngManager:
                    //        mapping.EngManager = opportunityItem.PartnerNoID.Value.ToString();
                    //        break;
                    //    case PartnerCodes.ConcurringPartner:
                    //        mapping.ConcurringPartner = opportunityItem.PartnerNoID.Value.ToString();
                    //        break;
                    //    case PartnerCodes.EngPartner:
                    //        mapping.EngPartner = opportunityItem.PartnerNoID.Value.ToString();
                    //        break;
                    //    case PartnerCodes.SalesManager:
                    //        mapping.SalesManager = opportunityItem.PartnerNoID.Value.ToString();
                    //        break;
                    //    case PartnerCodes.Leistungsempfaenger:
                    //        mapping.Leistungsempfaenger = opportunityItem.PartnerNoID.Value.ToString();
                    //        break;
                    //}

                    
                    if (Opportunity.OpportunityHeader.KPMG_Enhancement.Function != null)
                        mapping.Function = Opportunity.OpportunityHeader.KPMG_Enhancement.Function.ToString();
                    else
                    {
                        mapping.Function = string.Empty;
                        EventLog.WriteEntry("SharePoint-KPMG", "Function is mandatory and not defined in " + Opportunity.ToString(), EventLogEntryType.Error);
                        return;
                    }

                    EventLog.WriteEntry("SharePoint-KPMG", "Function: " + mapping.Function.ToString(), EventLogEntryType.Information);

                    if (Opportunity.OpportunityHeader.KPMG_Enhancement.ServiceArea != null)
                        mapping.ServiceArea = Opportunity.OpportunityHeader.KPMG_Enhancement.ServiceArea.ToString();


                    EventLog.WriteEntry("SharePoint-KPMG", "Service Area: " + mapping.ServiceArea.ToString(), EventLogEntryType.Information);

                    if (Opportunity.OpportunityHeader.KPMG_Enhancement.LineOfBusiness != null)
                        mapping.LoB = Opportunity.OpportunityHeader.KPMG_Enhancement.LineOfBusiness.ToString();
                    
                    if (Opportunity.OpportunityHeader.KPMG_Enhancement.Segment != null)
                        mapping.Segment = Opportunity.OpportunityHeader.KPMG_Enhancement.Segment.ToString();
                    
                    if (Opportunity.OpportunityHeader.SalesAndServiceBusinessArea != null)
                        mapping.KPMGGesellschaft = Opportunity.OpportunityHeader.SalesAndServiceBusinessArea.ToString();
                    
                    mapping.Geschaeftsbereich717273 = string.Empty;

                    if (Opportunity.OpportunityHeader.SalesAndServiceBusinessArea != null)
                        mapping.Niederlassung = Opportunity.OpportunityHeader.SalesAndServiceBusinessArea.SalesOfficeID.ToString();
                    
                    if (Opportunity.OpportunityHeader.Btheaderopportunityext.StartDate != null)
                        mapping.StartDatum = Opportunity.OpportunityHeader.Btheaderopportunityext.StartDate.ToString();

                    if (Opportunity.OpportunityHeader.Btheaderopportunityext.ClosingDate != null)
                        mapping.EndDatum = Opportunity.OpportunityHeader.Btheaderopportunityext.ClosingDate.ToString();

                    if (Opportunity.OpportunityHeader.Status != null)
                        mapping.Status = Opportunity.OpportunityHeader.Status.Currentstatusofheader.StatusCode.Value.ToString();
                    
                    if (Opportunity.OpportunityHeader.KPMG_Enhancement.EAStatus != null)
                        mapping.EAStatus = Opportunity.OpportunityHeader.KPMG_Enhancement.EAStatus.ToString();


                    if (Opportunity.OpportunityHeader.Btheaderopportunityext.ChanceOfSuccessNumberValue != null)
                        mapping.Wahrscheinlichkeit = Opportunity.OpportunityHeader.Btheaderopportunityext.ChanceOfSuccessNumberValue.ToString();

                    if (Opportunity.OpportunityHeader.Btheaderopportunityext.ExpectedSalesVolAmount != null)
                        mapping.Fee = Opportunity.OpportunityHeader.Btheaderopportunityext.ExpectedSalesVolAmount.Value.ToString();

                    if (Opportunity.OpportunityHeader.Btheaderopportunityext.ExpectedSalesVolAmount != null)
                        mapping.Waehrung = Opportunity.OpportunityHeader.Btheaderopportunityext.ExpectedSalesVolAmount.Value.ToString();

                    if (Opportunity.OpportunityHeader.KPMG_Enhancement.GlobalFee != null)
                        mapping.GlobalFee = Opportunity.OpportunityHeader.KPMG_Enhancement.GlobalFee.Value.ToString();

                    if (Opportunity.OpportunityHeader.KPMG_Enhancement.GlobalFee != null)
                        mapping.WaehrungGlobalFee = Opportunity.OpportunityHeader.KPMG_Enhancement.GlobalFee.Value.ToString();

                    if (Opportunity.OpportunityHeader.KPMG_Enhancement.ExpectedRecoveryRate != null)
                        mapping.ExpectedRecoveryRate = Opportunity.OpportunityHeader.KPMG_Enhancement.ExpectedRecoveryRate.ToString();

                    if (Opportunity.OpportunityHeader.KPMG_Enhancement.SuccessFee !=null)
                        mapping.SuccessFee = Opportunity.OpportunityHeader.KPMG_Enhancement.SuccessFee.Value.ToString();

                    if (Opportunity.OpportunityHeader.KPMG_Enhancement.ProfitCenter != null)
                        mapping.ProfitCentre = Opportunity.OpportunityHeader.KPMG_Enhancement.ProfitCenter.ToString();

                    if (Opportunity.OpportunityHeader.KPMG_Enhancement.Branch != null)
                        mapping.Branche = Opportunity.OpportunityHeader.KPMG_Enhancement.Branch.ToString();

                    if (Opportunity.OpportunityHeader.KPMG_Enhancement.ExpectedEndDate != null)
                        mapping.AbschlussDatum = Opportunity.OpportunityHeader.KPMG_Enhancement.ExpectedEndDate.ToString();

                    if (Opportunity.OpportunityHeader.KPMG_Enhancement.ForcastStartDate != null)
                        mapping.ForecastStartDate = Opportunity.OpportunityHeader.KPMG_Enhancement.ForcastStartDate.ToString();

                    mapping.ExpiryDate = string.Empty;



                }

                //filtering

                string filterKeyName = "Function-*";
                System.Configuration.Configuration rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
                string WildcardSAPFilterElement = ConfigurationManager.AppSettings[filterKeyName.ToString()];
                if (WildcardSAPFilterElement != null)
                {
                    EventLog.WriteEntry("SharePoint-KPMG", "Wildcard Filter defined: Request permitted for Function=" + mapping.Function.ToString() + " and Service Area=" + mapping.ServiceArea.ToString(), EventLogEntryType.Warning);

                }
                else
                {
                    EventLog.WriteEntry("SharePoint-KPMG", "Wildcard Filter not defined: Request rejected for Function=" + mapping.Function.ToString() + " and Service Area=" + mapping.ServiceArea.ToString(), EventLogEntryType.Warning);
                    

                }


                filterKeyName = "Function-" + mapping.Function.ToString() + "-" + mapping.ServiceArea.ToString();
                rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
                string customSAPFilterElement = ConfigurationManager.AppSettings[filterKeyName.ToString()];

                if (WildcardSAPFilterElement == null)
                if (customSAPFilterElement != null)
                {
                    EventLog.WriteEntry("SharePoint-KPMG", "Filter defined: Request permitted for Function=" + mapping.Function.ToString() + " and Service Area=" + mapping.ServiceArea.ToString(), EventLogEntryType.Warning);

                }
                else
                {
                    EventLog.WriteEntry("SharePoint-KPMG", "Filter not defined: Request rejected for Function=" + mapping.Function.ToString() + " and Service Area=" + mapping.ServiceArea.ToString(), EventLogEntryType.Warning);
                    return;

                }


                


                engagementProperties.Add("Opportunity Nr", mapping.OpportunityNr.ToString());
                engagementProperties.Add("Bezeichnung", mapping.Bezeichnung.ToString());
                engagementProperties.Add("Account", mapping.Account.ToString());
                engagementProperties.Add("Eng.Partner", mapping.EngPartner.ToString());
                engagementProperties.Add("Eng.Manager", mapping.EngManager.ToString());
                engagementProperties.Add("Sales Manager", mapping.SalesManager.ToString());
                engagementProperties.Add("Concurring Partner", mapping.ConcurringPartner.ToString());
                engagementProperties.Add("Function", mapping.Function.ToString());
                engagementProperties.Add("Service Area", mapping.ServiceArea.ToString());
                engagementProperties.Add("LoB", mapping.LoB.ToString());
                engagementProperties.Add("Segment", mapping.Segment.ToString());
                engagementProperties.Add("KPMG Gesellschaft", mapping.KPMGGesellschaft.ToString());
                engagementProperties.Add("Niederlassung", mapping.Niederlassung.ToString());
                engagementProperties.Add("Geschäftsbereich 71/72/73", mapping.Geschaeftsbereich717273.ToString());
                engagementProperties.Add("Leistungsempfänger", mapping.Leistungsempfaenger.ToString());
                engagementProperties.Add("Startdatum", mapping.StartDatum.ToString());
                engagementProperties.Add("Enddatum", mapping.EndDatum.ToString());
                engagementProperties.Add("Status", mapping.Status.ToString());
                engagementProperties.Add("EA Status", mapping.EAStatus.ToString());
                engagementProperties.Add("Expiry Date", mapping.ExpiryDate.ToString());
                engagementProperties.Add("Wahrscheinlichkeit", mapping.Wahrscheinlichkeit.ToString());

                engagementProperties.Add("Fee", mapping.Fee.ToString());
                engagementProperties.Add("Währung", mapping.Waehrung.ToString());
                engagementProperties.Add("Global Fee", mapping.GlobalFee.ToString());
                engagementProperties.Add("Währung Global Fee", mapping.WaehrungGlobalFee.ToString());
                engagementProperties.Add("Expected Recovery Rate", mapping.ExpectedRecoveryRate.ToString());
                engagementProperties.Add("Success Fee", mapping.SuccessFee.ToString());

                engagementProperties.Add("Profit centre", mapping.ProfitCentre.ToString());
                engagementProperties.Add("Branche", mapping.Branche.ToString());
                engagementProperties.Add("Abschlussdatum", mapping.AbschlussDatum.ToString());
                engagementProperties.Add("Forecast Start Date", mapping.ForecastStartDate.ToString());




                EngagementsServiceClient EngagementsServiceClient = new EngagementsServiceClient("EngagementsServiceEndPoint");

                //KPMG.Engagements.EngagementsApi.Api api = new KPMG.Engagements.EngagementsApi.Api("");
                //ActiveDirectoryHelpers adHelpers = new ActiveDirectoryHelpers(api.AdDeputyGroupsOu, api.AdDeputiesGroupPrefix, api.AdDeputiesOuContainer, api.AdUsersOuContainer);
                //string oppManager = adHelpers.ResolveLoginName(mapping.EngManager);
                //string oppPartner = adHelpers.ResolveLoginName(mapping.EngPartner);

                EngagementsServiceClient.CreateNewOpportunitySite(mapping.OpportunityNr, mapping.EngManager, mapping.EngPartner, engagementProperties);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("SharePoint-KPMG", ":Inbound:OpportunityWebService" + ex.ToString(), EventLogEntryType.Error);

                throw new Exception(ex.ToString());
            }

        }
    }
}
