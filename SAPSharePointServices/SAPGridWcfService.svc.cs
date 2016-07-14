using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
//using Telerik.Web.UI;
//using Telerik.Web.Data;
using KPMG.Engagements.SAPBusinessPartnerRead;

namespace SAPLookupTableService
{

    [DataContract]
    public class SPCustomProperty
    {
        [DataMember]
        public Int32 Key { get; set; }
        [DataMember]
        public string CustomPropertyName { get; set; }
        [DataMember]
        public string Language { get; set; }
        [DataMember]
        public string Value { get; set; }
    }

    public class ResultData
    {
        public int Count { get; set; }
        public List<SPCustomProperty> Data { get; set; }
    }

    [ServiceKnownType(typeof(SPCustomProperty))]
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    public class SAPGridWcfService
    {

        [OperationContract]
        public Dictionary<string, object> OpportunityCRMByIDReadQuery(string opportunityId, string typeCode)
        {
            Dictionary<string, object> engagementProperties = new Dictionary<string, object>();
            KPMG.Engagements.SAPOpportunityRead.SAPOpportunityReadService instance = new KPMG.Engagements.SAPOpportunityRead.SAPOpportunityReadService();

            engagementProperties = instance.OpportunityCRMByIDReadQuery(opportunityId, typeCode);
            return (engagementProperties);
        }

            
        [OperationContract]
        public Dictionary<string, object> ChargeableCodeByIDUpdateQuery(string ChargeableCodeNo)
        {
            //ChargeableCodeByIDUpdateQueryResponse
            Dictionary<string, object> engagementProperties = new Dictionary<string, object>();
            KPMG.Engagements.SAPSalesOrderChargeableCodeRead.SAPSalesOrderChargeableCodeReadService instance = new KPMG.Engagements.SAPSalesOrderChargeableCodeRead.SAPSalesOrderChargeableCodeReadService();

            engagementProperties = instance.ChargeableCodeByIDUpdateQuery(ChargeableCodeNo);
            return (engagementProperties);
        }

        [OperationContract]
        public Dictionary<string, object> NonChargeableCodeByIDReadQuery(string orderNumber)
        {
            //NonChargeableCodeByIDReadQueryResponse
            Dictionary<string, object> engagementProperties = new Dictionary<string, object>();
            KPMG.Engagements.SAPInternalOrderNonChargeableCodeRead.SAPInternalOrderNonChargeableCodeReadService instance = new KPMG.Engagements.SAPInternalOrderNonChargeableCodeRead.SAPInternalOrderNonChargeableCodeReadService();

            engagementProperties = instance.NonChargeableCodeByIDReadQuery(orderNumber);
            return (engagementProperties);
        }

        [OperationContract]
        public  Dictionary<string, object> CustomerCRMReadQuery(string reqId, string internalId)
        {
            //KPMGSapCrmOutbound.CustomerCRMByIDResponseMessage
            Dictionary<string, object> engagementProperties = new Dictionary<string, object>();
            KPMG.Engagements.SAPBusinessPartnerRead.SAPBusinessPartnerReadService instance = new KPMG.Engagements.SAPBusinessPartnerRead.SAPBusinessPartnerReadService();

            engagementProperties = instance.CustomerCRMByIDReadQuery(reqId, internalId);
            //todo - invoke EngagementService and pass engagementProperties

            //KPMGSapCrmOutbound.CustomerCRMByIDResponseMessage response
            return(engagementProperties);
        }

        [OperationContract]
        public ResultData SAPGetDataAndCount(int startRowIndex, int maximumRows, string sortExpression, string filterExpression)
        {
            //GridBindingData data = RadGrid.GetBindingData("LinqToSql.DataContext", "SPCustomProperties", startRowIndex, maximumRows, sortExpression, filterExpression);
            ResultData result = new ResultData();

            KPMG.Engagements.SAPLookupTableService.SAPLookupService instance = new KPMG.Engagements.SAPLookupTableService.SAPLookupService();

            KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows[] rowsArray = instance.GetRowsFromTable("Advisory","DE");

            List<SPCustomProperty> SPCustomPropertyList = new List<SPCustomProperty>();
            int cnt = 1;
            foreach (KPMG.Engagements.EngagementsDomain.CustomTableQueryResponseRows rowItem in rowsArray)
            {
                SPCustomProperty sp = new SPCustomProperty();
                sp.Key = cnt++;
                sp.CustomPropertyName = rowItem.Key;
                sp.Language = rowItem.Language;
                sp.Value = rowItem.Value;

                SPCustomPropertyList.Add(sp);
            }

            //List<SPCustomProperty> SPCustomPropertyList = rowsArray.Cast<SPCustomProperty>().ToList();

            result.Data = SPCustomPropertyList;

            //result.Data = data.Data.OfType<LinqToSql.SPCustomProperty>().Select(p => new SPCustomProperty()
            //{
            //    Key = p.Key,
            //    Language = p.Language,
            //    Value = p.Value
            //}).ToList();
            result.Count = SPCustomPropertyList.Count;
            return result;
        }
    }
}