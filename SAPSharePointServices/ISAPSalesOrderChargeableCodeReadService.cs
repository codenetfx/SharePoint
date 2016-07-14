using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using KPMG.EngagementsDomain.KPMGSapCrmOutbound;
//using KPMG.EngagementsDomain.KPMGSapCrmInbound;

namespace KPMG.Engagements.SAPSalesOrderChargeableCodeRead
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ISAPSalesOrderChargeableCodeReadService
    {
        [OperationContract]
        //KPMGSapChargeableCodeCrmOutbound.ChargeableCodeByIDUpdateQueryResponse ChargeableCodeByIDUpdateQuery(KPMGSapChargeableCodeCrmOutbound.ChargeableCodeByIDUpdateQueryRequest request);
        Dictionary<string, object> ChargeableCodeByIDUpdateQuery(string ChargeableCodeNo);

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
