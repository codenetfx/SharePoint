using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using SAPLookupTableService;
using KPMG.Engagements.EngagementsDomain;

namespace KPMG.Engagements.SAPLookupTableService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class SAPLookupService : ISAPLookupTableService
    {

        



        public KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows[] GetRowsFromTable(string tableName, string language)
        {
            
            
            KPMG.Engagements.EngagementsDomain.CustomTableReadQueryRequest request = new KPMG.Engagements.EngagementsDomain.CustomTableReadQueryRequest();

            //TODO - add endpoint
            //KPMG.Engagements.EngagementsDomain.CustomTableRead_OutbClient instance = new KPMG.Engagements.EngagementsDomain.CustomTableRead_OutbClient();
           
            //CustomTableQueryResponseRows[] rows;

            List<KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows> rows = new List<KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows>();


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


            KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow1 = new KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow1.Key = "customProperty1" + tableName;
            newRow1.Language = "en-US";
            newRow1.Value = "PlanB";
            rows.Add(newRow1);

            KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow2 = new KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow2.Key = "customProperty2" + tableName;
            newRow2.Language = "DE";
            newRow2.Value = "PlanB";
            rows.Add(newRow2);

            KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow3 = new KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow3.Key = "customProperty3" + tableName;
            newRow3.Language = "NL";
            newRow3.Value = "PlanB";
            rows.Add(newRow3);

            KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow4 = new KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow4.Key = "customProperty4" + tableName;
            newRow4.Language = "en-US";
            newRow4.Value = "PlanB";
            rows.Add(newRow1);

            KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow5 = new KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow5.Key = "customProperty5" + tableName;
            newRow5.Language = "DE";
            newRow5.Value = "PlanB";
            rows.Add(newRow5);

            KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow6 = new KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow6.Key = "customProperty6" + tableName;
            newRow6.Language = "DE";
            newRow6.Value = "PlanB";
            rows.Add(newRow6);

            KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow7 = new KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow7.Key = "customProperty7" + tableName;
            newRow7.Language = "NL";
            newRow7.Value = "PlanB";
            rows.Add(newRow7);

            KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow8 = new KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow4.Key = "customProperty8" + tableName;
            newRow4.Language = "en-US";
            newRow4.Value = "PlanB";
            rows.Add(newRow8);

            KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow9 = new KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow5.Key = "customProperty9" + tableName;
            newRow5.Language = "DE";
            newRow5.Value = "PlanB";
            rows.Add(newRow9);

            KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows newRow10 = new KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows();
            newRow6.Key = "customProperty10" + tableName;
            newRow6.Language = "NL";
            newRow6.Value = "PlanB";
            rows.Add(newRow10);

            request.CustomTableReadQueryResponse = new KPMG.Engagements.EngagementsDomain.CustomTableQueryResponse();

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
