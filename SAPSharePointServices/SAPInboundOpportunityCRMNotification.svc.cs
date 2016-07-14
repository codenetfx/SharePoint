using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using KPMG.Engagements.SAPInboundOpportunityCRMNotification;
using KPMG.Engagements.SAPOpportunityRead;
using KPMG.Engagements.EngagementsDomain;
using SAPLookupTableService.EngagementsService;
using KPMG.Engagements.EngagementsApi.Utilities;

namespace KPMG.Engagements.SAPInboundOpportunityCRMNotification
{



    public class OpportunityReadMappingObject
    {
        public string OpportunityNr;
        public string Bezeichnung;
        public string Account;
        public string EngPartner;
        public string EngManager;
        public string SalesManager;
        public string ConcurringPartner;
        public string Function;
        public string ServiceArea;
        public string LoB;
        public string Segment;
        public string KPMGGesellschaft;
        public string Niederlassung;
        public string Geschaeftsbereich717273;
        public string Leistungsempfaenger;
        public string StartDatum;
        public string EndDatum;
        public string Status;
        public string EAStatus;
        public string ExpiryDate;
        public string Wahrscheinlichkeit;
        public string Fee;
        public string Waehrung;
        public string GlobalFee;
        public string WaehrungGlobalFee;
        public string ExpectedRecoveryRate;
        public string SuccessFee;
        public string ProfitCentre;
        public string Branche;
        public string AbschlussDatum;
        public string ForecastStartDate;

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

    [DataContract]
    public class SAPInboundOpportunityCRMNotificationService : ISAPInboundOpportunityCRMNotificationService
    {


        private string opportunityNr;
        private string opportunityManager;
        private string opportunityPartner;
        private string bezeichnung;

        private string account;  //", mapping.Account.ToString());
        private string engPartner; //", mapping.EngPartner.ToString());
        private string engManager; //", mapping.EngManager.ToString());
        private string salesManager; // ", mapping.SalesManager.ToString());
        private string concurringPartner; //", mapping.ConcurringPartner.ToString());
        private string function; //", mapping.Function.ToString());
        private string serviceArea; //", mapping.ServiceArea.ToString());
        private string loB; //", mapping.LoB.ToString());
        private string segment; //", mapping.Segment.ToString());
        private string kPMGGesellschaft; //", mapping.KPMGGesellschaft.ToString());
        private string niederlassung; //", mapping.Niederlassung.ToString());
        private string geschäftsbereich717273; //", mapping.Geschaeftsbereich717273.ToString());
        private string leistungsempfänger; //", mapping.Leistungsempfaenger.ToString());
        private string startdatum; //", mapping.StartDatum.ToString());
        private string enddatum; //", mapping.EndDatum.ToString());
        private string status; //", mapping.Status.ToString());
        private string eAStatus; //", mapping.EAStatus.ToString());
        private string expiryDate; //", mapping.ExpiryDate.ToString());
        private string wahrscheinlichkeit; //", mapping.Wahrscheinlichkeit.ToString());

        private string fee; //", mapping.Fee.ToString());
        private string waehrung; //", mapping.Waehrung.ToString());
        private string globalFee; //", mapping.GlobalFee.ToString());
        private string waehrungGlobalFee; //", mapping.WaehrungGlobalFee.ToString());
        private string expectedRecoveryRate; //", mapping.ExpectedRecoveryRate.ToString());
        private string successFee; //", mapping.SuccessFee.ToString());

        private string profitCentre; //", mapping.ProfitCentre.ToString());
        private string branche; //", mapping.Branche.ToString());
        private string abschlussdatum; //", mapping.AbschlussDatum.ToString());
        private string forecastStartDate; //", mapping.ForecastStartDate.ToString());









        [DataMember(IsRequired = true)]
        public string OpportunityNr
        {
            get { return this.opportunityNr; }
            set { this.opportunityNr = value; }
        }

        [DataMember(IsRequired = true)]
        public string OpportunityManager
        {
            get { return this.opportunityManager; }
            set { this.opportunityManager = value; }
        }

        [DataMember(IsRequired = true)]
        public string OpportunityPartner
        {
            get { return this.opportunityPartner; }
            set { this.opportunityPartner = value; }
        }

        [DataMember(IsRequired = false)]
        public string Bezeichnung { get { return this.bezeichnung; } set { this.bezeichnung = value; } }

        [DataMember(IsRequired = false)]
        public string Account
        {
            get { return this.account; }
            set { this.account = value; }
        }

        [DataMember(IsRequired = false)]
        public string EngPartner
        {
            get { return this.engPartner; }
            set { this.engPartner = value; }
        }

        [DataMember(IsRequired = false)]
        public string EngManager
        {
            get { return this.engManager; }
            set { this.engManager = value; }
        }

        [DataMember(IsRequired = false)]
        public string SalesManager
        {
            get { return this.salesManager; }
            set { this.salesManager = value; }
        }

        [DataMember(IsRequired = false)]
        public string ConcurringPartner
        {
            get { return this.concurringPartner; }
            set { this.concurringPartner = value; }
        }

        [DataMember(IsRequired = false)]
        public string Function
        {
            get { return this.function; }
            set { this.function = value; }
        }

        [DataMember(IsRequired = false)]
        public string ServiceArea
        {
            get { return this.serviceArea; }
            set { this.serviceArea = value; }
        }

        [DataMember(IsRequired = false)]
        public string LoB
        {
            get { return this.loB; }
            set { this.loB = value; }
        }

        [DataMember(IsRequired = false)]
        public string Segment
        {
            get { return this.segment; }
            set { this.segment = value; }
        }

        [DataMember(IsRequired = false)]
        public string KpmgGesellschaft
        {
            get { return this.kPMGGesellschaft; }
            set { this.kPMGGesellschaft = value; }
        }

        [DataMember(IsRequired = false)]
        public string Niederlassung
        {
            get { return this.niederlassung; }
            set { this.niederlassung = value; }
        }

        [DataMember(IsRequired = false)]
        public string Geschaeftsbereich
        {
            get { return this.geschäftsbereich717273; }
            set { this.geschäftsbereich717273 = value; }
        }

        [DataMember(IsRequired = false)]
        public string Leistungsempfaenger
        {
            get { return this.leistungsempfänger; }
            set { this.leistungsempfänger = value; }
        }

        [DataMember(IsRequired = false)]
        public string StartDatum
        {
            get { return this.startdatum; }
            set { this.startdatum = value; }
        }

        [DataMember(IsRequired = false)]
        public string EndDatum
        {
            get { return this.enddatum; }
            set { this.enddatum = value; }
        }

        [DataMember(IsRequired = false)]
        public string Status
        {
            get { return this.status; }
            set { this.status = value; }
        }


        [DataMember(IsRequired = false)]
        public string EAStatus
        {
            get { return this.eAStatus; }
            set { this.eAStatus = value; }
        }

        [DataMember(IsRequired = false)]
        public string ExpiryDate
        {
            get { return this.expiryDate; }
            set { this.expiryDate = value; }
        }

        [DataMember(IsRequired = false)]
        public string Wahrscheinlichkeit
        {
            get { return this.wahrscheinlichkeit; }
            set { this.wahrscheinlichkeit = value; }
        }

        [DataMember(IsRequired = false)]
        public string Fee
        {
            get { return this.fee; }
            set { this.fee = value; }
        }

        [DataMember(IsRequired = false)]
        public string Waehrung
        {
            get { return this.waehrung; }
            set { this.waehrung = value; }
        }

        [DataMember(IsRequired = false)]
        public string GlobalFee
        {
            get { return this.globalFee; }
            set { this.globalFee = value; }
        }

        [DataMember(IsRequired = false)]
        public string WaehrungGlobalFee
        {
            get { return this.waehrungGlobalFee; }
            set { this.waehrungGlobalFee = value; }
        }

        [DataMember(IsRequired = false)]
        public string ExpectedRecoveryRate
        {
            get { return this.expectedRecoveryRate; }
            set { this.expectedRecoveryRate = value; }
        }

        [DataMember(IsRequired = false)]
        public string SuccessFee
        {
            get { return this.successFee; }
            set { this.successFee = value; }
        }

        [DataMember(IsRequired = false)]
        public string ProfitCentre
        {
            get { return this.profitCentre; }
            set { this.profitCentre = value; }
        }

        [DataMember(IsRequired = false)]
        public string Branche
        {
            get { return this.branche; }
            set { this.branche = value; }
        }

        [DataMember(IsRequired = false)]
        public string Abschlussdatum
        {
            get { return this.abschlussdatum; }
            set { this.abschlussdatum = value; }
        }

        [DataMember(IsRequired = false)]
        public string ForecastStartDate
        {
            get { return this.forecastStartDate; }
            set { this.forecastStartDate = value; }
        }


        public void OpportunityCRMUpdateInformation(OpportunityReadMappingObject request)
        {
            string opportunityId = string.Empty;
            string typeCode = string.Empty;

            Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

            //if (!EventLog.SourceExists("OpportunityCRMUpdateInformation"))
            //    EventLog.CreateEventSource("OpportunityCRMUpdateInformation", "Application");

            //EventLog.WriteEntry("OpportunityCRMUpdateInformation", request.ToString(), EventLogEntryType.Information);

            OpportunityReadMappingObject mapping = new OpportunityReadMappingObject();
            mapping.OpportunityNr = request.OpportunityNr.ToString();
            mapping.EngPartner = request.EngPartner.ToString();
            mapping.EngManager = request.EngManager.ToString();
            mapping.SalesManager = request.SalesManager.ToString();
            mapping.ConcurringPartner = request.ConcurringPartner.ToString();



            //foreach (KPMGSapOpportunityCrmInbound.OpptCRMByIDAllopportunities opportunityItem in request.Opportunity.OpportunityHeader.Party.Allopportunities) 
            //{

            //    string refPartnerFctCode = opportunityItem.RefPartnerFctCode.ToString();

            //    switch (refPartnerFctCode)
            //    {
            //        case PartnerCodes.Account:
            //            mapping.Account = opportunityItem.PartnerNoID.ToString();
            //            break;
            //        case PartnerCodes.EngManager:
            //            mapping.EngManager = opportunityItem.PartnerNoID.ToString();
            //            break;
            //        case PartnerCodes.ConcurringPartner:
            //            mapping.ConcurringPartner = opportunityItem.PartnerNoID.ToString();
            //            break;
            //        case PartnerCodes.EngPartner:
            //            mapping.EngPartner = opportunityItem.PartnerNoID.ToString();
            //            break;
            //        case PartnerCodes.SalesManager:
            //            mapping.SalesManager = opportunityItem.PartnerNoID.ToString();
            //            break;
            //        case PartnerCodes.Leistungsempfaenger:
            //            mapping.Leistungsempfaenger = opportunityItem.PartnerNoID.ToString();
            //            break;
            //    }

            mapping.OpportunityNr = request.OpportunityNr.ToString();
            mapping.Bezeichnung = request.Bezeichnung.ToString();
            mapping.Account = request.Account.ToString();
            mapping.Function = request.Function.ToString();
            mapping.ServiceArea = request.ServiceArea.ToString();
            mapping.LoB = request.LoB.ToString();
            mapping.Segment = request.Segment.ToString();
            mapping.KPMGGesellschaft = request.KPMGGesellschaft.ToString();
            mapping.Geschaeftsbereich717273 = string.Empty;
            mapping.Leistungsempfaenger = request.Leistungsempfaenger.ToString();
            mapping.Niederlassung = request.Niederlassung.ToString();
            mapping.StartDatum = request.StartDatum.ToString();
            mapping.EndDatum = request.EndDatum.ToString();
            mapping.Status = request.Status.ToString();
            mapping.EAStatus = request.EAStatus.ToString();
            mapping.Wahrscheinlichkeit = request.Wahrscheinlichkeit.ToString();
            mapping.ExpiryDate = request.ExpiryDate.ToString();
            mapping.Fee = request.Fee.ToString();
            mapping.Waehrung = request.Waehrung.ToString();
            mapping.GlobalFee = request.GlobalFee.ToString();
            mapping.WaehrungGlobalFee = request.WaehrungGlobalFee.ToString();
            mapping.ExpectedRecoveryRate = request.ExpectedRecoveryRate.ToString();
            mapping.SuccessFee = request.SuccessFee.ToString();
            mapping.ProfitCentre = request.ProfitCentre.ToString();
            mapping.Branche = request.Branche.ToString();
            
            mapping.AbschlussDatum = request.AbschlussDatum.ToString();
            
            mapping.ForecastStartDate = request.ForecastStartDate.ToString();





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




            EngagementsServiceClient EngagementsServiceClient = new EngagementsServiceClient("fudo");

            //KPMG.Engagements.EngagementsApi.Api api = new KPMG.Engagements.EngagementsApi.Api("");
            //ActiveDirectoryHelpers adHelpers = new ActiveDirectoryHelpers(api.AdDeputyGroupsOu, api.AdDeputiesGroupPrefix, api.AdDeputiesOuContainer, api.AdUsersOuContainer);
            //string oppManager = adHelpers.ResolveLoginName(mapping.EngManager);
            //string oppPartner = adHelpers.ResolveLoginName(mapping.EngPartner);

            EngagementsServiceClient.CreateNewOpportunitySite(mapping.OpportunityNr, mapping.EngManager, mapping.EngPartner, engagementProperties);

        }


    }
}
