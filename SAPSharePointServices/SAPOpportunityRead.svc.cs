using System;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using KPMG.Engagements.SAPOpportunityRead;
using KPMG.Engagements.EngagementsDomain;
using SAPLookupTableService.EngagementsService;
using KPMG.Engagements.EngagementsApi;
using KPMG.Engagements.EngagementsApi.Utilities;
using KPMGSapOpportunityCrmOutbound;




namespace KPMG.Engagements.SAPOpportunityRead
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


    public class NewBusinessTransactionDocumentID
    {

        private string valueField;

        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
    

    public class SAPOpportunityReadService : ISAPOpportunityReadService
    {

        public Dictionary<string, object> OpportunityCRMByIDReadQuery(string opportunityId, string typeCode)
        {
            
            try {

                EventLog.WriteEntry("SharePoint-KPMG", "Outbound:SAPOpportunityRead:OpportunityId=" + opportunityId.ToString() + ", typeCode=" + typeCode.ToString(), EventLogEntryType.Information);

                Dictionary<string, object> engagementProperties = new Dictionary<string, object>();     
                KPMGSapOpportunityCrmOutbound.OpportunityCRMByIDReadQueryRequest request = new KPMGSapOpportunityCrmOutbound.OpportunityCRMByIDReadQueryRequest();
            //NewBusinessTransactionDocumentID IDObject = new NewBusinessTransactionDocumentID();


            OpportunityCRMByIDQueryMessage_sync queryIn = new OpportunityCRMByIDQueryMessage_sync();
            OpportunityCRMByIDQuery_sync OppRequest = new OpportunityCRMByIDQuery_sync();
            BusinessTransactionDocumentID IDObject = new BusinessTransactionDocumentID();

            IDObject.Value = opportunityId;
            OppRequest.ID = IDObject;
            queryIn.Opportunity = OppRequest;
           

            //request.OpportunityCRMByIDQuery_sync = OppRequest; // Opportunity.ID.Value = IDObject.Value;
            //request.OpportunityCRMByIDQuery_sync.Opportunity.TypeCode = "Z_OP";

            BasicHttpBinding binding = new BasicHttpBinding();
            binding.SendTimeout = TimeSpan.FromMinutes(1);
            binding.OpenTimeout = TimeSpan.FromMinutes(1);
            binding.CloseTimeout = TimeSpan.FromMinutes(1);
            binding.ReceiveTimeout = TimeSpan.FromMinutes(10);
            //binding.AllowCookies = false;
            //binding.BypassProxyOnLocal = false;
            //binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            //binding.MessageEncoding = WSMessageEncoding.Text;
            //binding.TextEncoding = System.Text.Encoding.UTF8;
            //binding.TransferMode = TransferMode.Buffered;
            //binding.UseDefaultWebProxy = true;
            //binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;

            //binding.Security.Mode = BasicHttpSecurityMode.Transport; // für https notwendig
            //binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;

            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            //Determine the Environment we need to connect to

            string environmentName = "Environment";
            System.Configuration.Configuration rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
            string environmentPrefix = ConfigurationManager.AppSettings[environmentName.ToString()];
            if (environmentPrefix != null)
            {
                EventLog.WriteEntry("SharePoint-KPMG", "Environment defined: " + environmentPrefix, EventLogEntryType.Warning);
            }
            else
            {
                EventLog.WriteEntry("SharePoint-KPMG", "Environment not defined: Please set the Environment value before executing the service", EventLogEntryType.Warning);
                return (engagementProperties);

            }



            ChannelFactory<OpportunityCRMByIDQueryResponse_Outb> channelFactory = new ChannelFactory<OpportunityCRMByIDQueryResponse_Outb>(environmentPrefix+"_OpportunityOutbound");

            string endPointURL = channelFactory.Endpoint.Address.ToString();
            if (endPointURL.ToUpper().Contains("HTTPS://"))
                binding.Security.Mode = BasicHttpSecurityMode.Transport; // für https notwendig
            else
                binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;


            string environmentUserName = environmentPrefix+"_Username";
            rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
            string environmentUserNameValue = ConfigurationManager.AppSettings[environmentUserName.ToString()];
            if (environmentUserNameValue != null)
            {
                EventLog.WriteEntry("SharePoint-KPMG", "Environment username defined for: " + environmentPrefix, EventLogEntryType.Warning);
            }
            else
            {
                EventLog.WriteEntry("SharePoint-KPMG", "Environment username not defined: Please set the Environment value before executing the service", EventLogEntryType.Warning);
                return (engagementProperties);

            }

            string environmentPassword = environmentPrefix + "_Password";
            rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
            string environmentPasswordValue = ConfigurationManager.AppSettings[environmentPassword.ToString()];
            if (environmentPasswordValue != null)
            {
                EventLog.WriteEntry("SharePoint-KPMG", "Environment password defined for: " + environmentPrefix, EventLogEntryType.Warning);
            }
            else
            {
                EventLog.WriteEntry("SharePoint-KPMG", "Environment password not defined: Please set the Environment value before executing the service", EventLogEntryType.Warning);
                return (engagementProperties);

            }


           

            //ChannelFactory<OpportunityCRMByIDQueryResponse_Outb> channelFactory = new ChannelFactory<OpportunityCRMByIDQueryResponse_Outb>("OpportunityOutbound");

            //channelFactory.Credentials.UserName.UserName = "PIAPPLSPDED";
            //channelFactory.Credentials.UserName.Password = @"kuesa657464S";
            //OpportunityCRMByIDQueryResponse_OutbClient client = channelFactory.CreateChannel();


            //TEST
            //OpportunityCRMByIDQueryResponse_OutbClient client = new OpportunityCRMByIDQueryResponse_OutbClient(binding, new EndpointAddress("http://cix11.de.kworld.kpmg.com:54500/XISOAPAdapter/MessageServlet?senderParty=KPMG_DE&senderService=BC_SharePoint_SOAP&receiverParty=&receiverService=BC_ProcessIntegration&interface=OpportunityCRMByIDQueryResponse_Outb&interfaceNamespace=urn:kpmg:de:crm:nonsap:opportunitymanagement"));

            //UAT
            //OpportunityCRMByIDQueryResponse_OutbClient client = new OpportunityCRMByIDQueryResponse_OutbClient(binding, new EndpointAddress("https://cix22.de.kworld.kpmg.com:57501/XISOAPAdapter/MessageServlet?senderParty=KPMG_DE&senderService=BC_SharePoint_SOAP&receiverParty=&receiverService=BC_ProcessIntegration&interface=OpportunityCRMByIDQueryResponse_Outb&interfaceNamespace=urn:kpmg:de:crm:nonsap:opportunitymanagement"));
            
            OpportunityCRMByIDQueryResponse_OutbClient client = new OpportunityCRMByIDQueryResponse_OutbClient(binding, new EndpointAddress(endPointURL));

            client.ClientCredentials.UserName.UserName = environmentUserNameValue;
            client.ClientCredentials.UserName.Password = environmentPasswordValue;
                                                                                                                   
            //OpportunityCRMByIDQueryResponse_OutbClient client = new OpportunityCRMByIDQueryResponse_OutbClient("OpportunityOutbound");
              
            //client.ClientCredentials.UserName.UserName = "PIAPPLSPDED";
            //client.ClientCredentials.UserName.Password = @"kuesa657464S";


            //string UATUsername = "PIAPPLSPDEQ";
            //string UATPassword = @"jX}#dp}4iTB=CKXiy@tvuU(]4dpq~X5BFg32ZJoa";

            

            //KPMGSapOpportunityCrmOutbound.OpportunityCRMByIDQueryResponse_OutbClient client = new KPMGSapOpportunityCrmOutbound.OpportunityCRMByIDQueryResponse_OutbClient("OpportunityOutbound");

            KPMGSapOpportunityCrmOutbound.OpportunityCRMByIDResponseMessage_sync response = client.OpportunityCRMByIDReadQuery(queryIn);

            OpportunityReadMappingObject mapping = new OpportunityReadMappingObject();
            mapping.OpportunityNr = response.Opportunity.OpportunityHeader.ID.Value.ToString();

            if (response.Opportunity.OpportunityHeader.DescriptionName != null)
                mapping.Bezeichnung = response.Opportunity.OpportunityHeader.DescriptionName.ToString();
            else
                mapping.Bezeichnung = string.Empty;


            
            foreach (KPMGSapOpportunityCrmOutbound.OpptCRMByIDAllopportunities opportunityItem in response.Opportunity.OpportunityHeader.Party.Allopportunities)
            {

                string refPartnertFctCode = opportunityItem.RefPartnerFctCode.ToString();

                switch (refPartnertFctCode)
                {
                    case PartnerCodes.Account:
                        mapping.Account = opportunityItem.PartnerNoID.Value.ToString();
                        break;
                    case PartnerCodes.EngManager:
                        mapping.EngManager = opportunityItem.PartnerNoID.Value.ToString();
                        break;
                    case PartnerCodes.ConcurringPartner:
                        mapping.ConcurringPartner = opportunityItem.PartnerNoID.Value.ToString();
                        break;
                    case PartnerCodes.EngPartner:
                        mapping.EngPartner = opportunityItem.PartnerNoID.Value.ToString();
                        break;
                    case PartnerCodes.SalesManager:
                        mapping.SalesManager = opportunityItem.PartnerNoID.Value.ToString();
                        break;
                    case PartnerCodes.Leistungsempfaenger:
                        mapping.Leistungsempfaenger = opportunityItem.PartnerNoID.Value.ToString();
                        break;
                }


                mapping.Function = response.Opportunity.OpportunityHeader.KPMG_Enhancement.Function.ToString();
                mapping.ServiceArea = response.Opportunity.OpportunityHeader.KPMG_Enhancement.ServiceArea.ToString();
                mapping.LoB = response.Opportunity.OpportunityHeader.KPMG_Enhancement.LineOfBusiness.ToString();
                mapping.Segment = response.Opportunity.OpportunityHeader.KPMG_Enhancement.Segment.ToString();
                mapping.KPMGGesellschaft = response.Opportunity.OpportunityHeader.SalesAndServiceBusinessArea.ToString();
                mapping.Geschaeftsbereich717273 = string.Empty;
                mapping.Niederlassung = response.Opportunity.OpportunityHeader.SalesAndServiceBusinessArea.ToString();
                mapping.StartDatum = response.Opportunity.OpportunityHeader.Btheaderopportunityext.StartDate.ToString();
                mapping.EndDatum = response.Opportunity.OpportunityHeader.Btheaderopportunityext.ClosingDate.ToString();
                mapping.Status = response.Opportunity.OpportunityHeader.Status.ToString();
                mapping.EAStatus = response.Opportunity.OpportunityHeader.KPMG_Enhancement.EAStatus[0].StatusCode.ToString();
                mapping.Wahrscheinlichkeit = response.Opportunity.OpportunityHeader.Btheaderopportunityext.ChanceOfSuccessNumberValue.ToString();
                mapping.Fee = response.Opportunity.OpportunityHeader.Btheaderopportunityext.ExpectedSalesVolAmount.ToString();
                mapping.Waehrung = response.Opportunity.OpportunityHeader.Btheaderopportunityext.ExpectedSalesVolAmount.ToString();
                mapping.GlobalFee = response.Opportunity.OpportunityHeader.KPMG_Enhancement.GlobalFee.ToString();
                mapping.WaehrungGlobalFee = response.Opportunity.OpportunityHeader.KPMG_Enhancement.GlobalFee.ToString();
                mapping.ExpectedRecoveryRate = response.Opportunity.OpportunityHeader.KPMG_Enhancement.ExpectedRecoveryRate.ToString();
                mapping.SuccessFee = response.Opportunity.OpportunityHeader.KPMG_Enhancement.SuccessFee.ToString();
                mapping.ProfitCentre = response.Opportunity.OpportunityHeader.KPMG_Enhancement.ProfitCenter.ToString();
                mapping.Branche = response.Opportunity.OpportunityHeader.KPMG_Enhancement.Branch.ToString();
                mapping.AbschlussDatum = response.Opportunity.OpportunityHeader.KPMG_Enhancement.ExpectedEndDate.ToString();
                mapping.ForecastStartDate = response.Opportunity.OpportunityHeader.KPMG_Enhancement.ForcastStartDate.ToString();
            }

            ////filtering
            
            //    string filterKeyName = "Function-" + mapping.Function.ToString() + "-" + mapping.ServiceArea.ToString();
            //    System.Configuration.Configuration rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
            //    string customSAPFilterElement = ConfigurationManager.AppSettings[filterKeyName.ToString()];
            //    if (customSAPFilterElement != null )
            //    {
            //        EventLog.WriteEntry("SharePoint-KPMG", "Filter defined: Request permitted for Function=" + mapping.Function.ToString() + " and Service Area=" + mapping.ServiceArea.ToString(), EventLogEntryType.Warning);

            //    }
            //    else
            //    {
            //        EventLog.WriteEntry("SharePoint-KPMG", "Filter not defined: Request rejected for Function=" + mapping.Function.ToString() + " and Service Area=" + mapping.ServiceArea.ToString(), EventLogEntryType.Warning);
            //        return (engagementProperties); 

            //    }
           


            // add mapped values to the dictionary
           

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
            engagementProperties.Add("Expiry Date", string.Empty);
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

            EngagementsServiceClient EngagementsServiceClient = new EngagementsServiceClient();

            KPMG.Engagements.EngagementsApi.Api api = new KPMG.Engagements.EngagementsApi.Api("");
            ActiveDirectoryHelpers adHelpers = new ActiveDirectoryHelpers(api.AdDeputyGroupsOu, api.AdDeputiesGroupPrefix, api.AdDeputiesOuContainer, api.AdUsersOuContainer);
            string oppManager = adHelpers.ResolveLoginName(mapping.EngManager);
            string oppPartner = adHelpers.ResolveLoginName(mapping.EngPartner);

            EngagementsServiceClient.CreateNewOpportunitySite(mapping.OpportunityNr, oppManager, oppPartner, engagementProperties);
            return (engagementProperties);

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("SharePoint-KPMG", "Outbound:SAPOpportunityReadService:"+ex.ToString(), EventLogEntryType.Error);

                throw new Exception(ex.ToString());
            }

            

        }
        

 
    }
       
}
