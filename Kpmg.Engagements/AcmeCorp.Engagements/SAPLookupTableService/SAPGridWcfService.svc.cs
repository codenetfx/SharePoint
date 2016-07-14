using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using Telerik.Web.UI;
using Telerik.Web.Data;
using AcmeCorp.Engagements.SAPLookupTableService;

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
        public ResultData SAPGetDataAndCount(int startRowIndex, int maximumRows, string sortExpression, string filterExpression)
        {
            //GridBindingData data = RadGrid.GetBindingData("LinqToSql.DataContext", "SPCustomProperties", startRowIndex, maximumRows, sortExpression, filterExpression);
            ResultData result = new ResultData();

            AcmeCorp.Engagements.SAPLookupTableService.SAPLookupService instance = new AcmeCorp.Engagements.SAPLookupTableService.SAPLookupService();

            AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows[] rowsArray = instance.GetRowsFromTable("Advisory","DE");

            List<SPCustomProperty> SPCustomPropertyList = new List<SPCustomProperty>();
            int cnt = 1;
            foreach (AcmeCorp.Engagements.EngagementsDomain.CustomTableQueryResponseRows rowItem in rowsArray)
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