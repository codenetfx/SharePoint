using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using SAPLookupTableService;
using AcmeCorp.Engagements.EngagementsDomain;

namespace AcmeCorp.Engagements.SAPLookupTableService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class SAPLookupService : ISAPLookupTableService
    {

        public AcmeCorpSapCrmOutbound.CustomerCRMByIDReadQueryResponse CustomerCRMByIDReadQuery(string reqId, string internalId)
        {
            AcmeCorpSapCrmOutbound.CustomerCRMByIDReadQueryRequest crmrequest = new AcmeCorpSapCrmOutbound.CustomerCRMByIDReadQueryRequest();
            
            AcmeCorpSapCrmOutbound.BusinessDocumentMessageID msgId = new AcmeCorpSapCrmOutbound.BusinessDocumentMessageID();
            msgId.Value = reqId;
            
            crmrequest.CustomerCRMByIDQuery.MessageHeader.ID = msgId;
            crmrequest.CustomerCRMByIDQuery.BusinessPartnerSelectionByBusinessPartner.InternalID = internalId;

            AcmeCorpSapCrmOutbound.CustomerCRMByIDReadQueryResponse dummy = new AcmeCorpSapCrmOutbound.CustomerCRMByIDReadQueryResponse();
            return (dummy);

        }



        public AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows[] GetRowsFromTable(string tableName, string language)
        {
            
            
            AcmeCorp.Engagements.EngagementsDomain.CustomTableReadQueryRequest request = new AcmeCorp.Engagements.EngagementsDomain.CustomTableReadQueryRequest();

            //TODO - add endpoint
            //AcmeCorp.Engagements.EngagementsDomain.CustomTableRead_OutbClient instance = new AcmeCorp.Engagements.EngagementsDomain.CustomTableRead_OutbClient();
           
            //CustomTableQueryResponseRows[] rows;

            List<AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows> rows = new List<AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows>();


            switch (tableName)
            {
                case "Advisory":
                    
                    break;
                case "Tax":
                   
                    break;
                case "Legal":
                    break;
                

                default:
                    break;
            }


            AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow1 = new AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow1.Key = "customProperty1" + tableName;
            newRow1.Language = "en-US";
            newRow1.Value = "Acme";
            rows.Add(newRow1);

            AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow2 = new AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow2.Key = "customProperty2" + tableName;
            newRow2.Language = "DE";
            newRow2.Value = "Acme";
            rows.Add(newRow2);

            AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow3 = new AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow3.Key = "customProperty3" + tableName;
            newRow3.Language = "NL";
            newRow3.Value = "Acme";
            rows.Add(newRow3);

            AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow4 = new AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow4.Key = "customProperty4" + tableName;
            newRow4.Language = "en-US";
            newRow4.Value = "Acme";
            rows.Add(newRow1);

            AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow5 = new AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow5.Key = "customProperty5" + tableName;
            newRow5.Language = "DE";
            newRow5.Value = "Acme";
            rows.Add(newRow5);

            AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow6 = new AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow6.Key = "customProperty6" + tableName;
            newRow6.Language = "DE";
            newRow6.Value = "Acme";
            rows.Add(newRow6);

            AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow7 = new AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow7.Key = "customProperty7" + tableName;
            newRow7.Language = "NL";
            newRow7.Value = "Acme";
            rows.Add(newRow7);

            AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow8 = new AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow4.Key = "customProperty8" + tableName;
            newRow4.Language = "en-US";
            newRow4.Value = "Acme";
            rows.Add(newRow8);

            AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow9 = new AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow5.Key = "customProperty9" + tableName;
            newRow5.Language = "DE";
            newRow5.Value = "Acme";
            rows.Add(newRow9);

            AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow10 = new AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow6.Key = "customProperty10" + tableName;
            newRow6.Language = "NL";
            newRow6.Value = "Acme";
            rows.Add(newRow10);

            request.CustomTableReadQueryResponse = new AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponse();

            request.CustomTableReadQueryResponse.Rows = rows.ToArray();

            request.CustomTableReadQueryResponse.TableName = tableName;

            //instance.CustomTableReadQuery(ref request.CustomTableReadQueryResponse);

            return(request.CustomTableReadQueryResponse.Rows);
            
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
