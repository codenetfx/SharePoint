using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using KPMG.EngagementsDomain.KPMGSapCrmOutbound;
//using KPMG.EngagementsDomain.KPMGSapCrmInbound;

namespace KPMG.Engagements.SAPBusinessPartnerRead
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ISAPBusinessPartnerReadService
    {
        [OperationContract]
        //KPMGSapCrmOutbound.CustomerCRMByIDResponseMessage CustomerCRMByIDReadQuery(string reqId, string internalId);
        Dictionary<string, object> CustomerCRMByIDReadQuery(string reqId, string internalId);

     
    }


    
}
