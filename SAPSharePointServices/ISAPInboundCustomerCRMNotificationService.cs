using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using KPMG.EngagementsDomain.KPMGSapCrmOutbound;
using KPMG.EngagementsDomain.KPMGSapCrmInbound;

namespace KPMG.Engagements.SAPInboundCustomerCRMNotification
{
     [ServiceContract]
    public interface ISAPInboundCustomerCRMNotificationService
    {
        [OperationContract]
        void CustomerCRMUpdateInformation(KPMGSapCrmCustomerInbound_OLD.CustomerCRMUpdateInformation request);
    }


    
}
