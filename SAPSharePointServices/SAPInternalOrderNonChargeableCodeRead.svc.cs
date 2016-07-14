using System;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using KPMG.Engagements.SAPInternalOrderNonChargeableCodeRead;
using KMPGSapNonChargeableCodeCrmOutbound;
using KPMG.Engagements.EngagementsDomain;
using SAPLookupTableService.EngagementsService;
using KPMG.Engagements.EngagementsApi;
using KPMG.Engagements.EngagementsApi.Utilities;

namespace KPMG.Engagements.SAPInternalOrderNonChargeableCodeRead
{

    public class InternalOrderNonChargeableCodeReadMappingObject
    {
        public string NonChargeCode;
        public string NonChargeCodeDescription;
        public string NonChargeCodeStatus;   
    }

  

    

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class SAPInternalOrderNonChargeableCodeReadService : ISAPInternalOrderNonChargeableCodeReadService
    {

        public  Dictionary<string, object> NonChargeableCodeByIDReadQuery(string orderNumber)
        {
            try {

                EventLog.WriteEntry("SharePoint-KPMG:Outbound:SAPInternalOrderNonChargeableCodeRead", "orderNumber=" + orderNumber.ToString(), EventLogEntryType.Information);
            
            KMPGSapNonChargeableCodeCrmOutbound.NonChargeableCodeByIDReadQueryRequest request = new KMPGSapNonChargeableCodeCrmOutbound.NonChargeableCodeByIDReadQueryRequest();


            KMPGSapNonChargeableCodeCrmOutbound.InternalOrderQueryByID myOrder = new KMPGSapNonChargeableCodeCrmOutbound.InternalOrderQueryByID();
            myOrder.OrderNumber = orderNumber;

            request.NonChargeableCodeReadQuery = myOrder;

            KMPGSapNonChargeableCodeCrmOutbound.NonChargeableCodeByIDQuery_OutbClient client = new KMPGSapNonChargeableCodeCrmOutbound.NonChargeableCodeByIDQuery_OutbClient("NonChargeableCodeOutbound");

            KMPGSapNonChargeableCodeCrmOutbound.NonChargeableCodeByIDReadQueryResponse response = new KMPGSapNonChargeableCodeCrmOutbound.NonChargeableCodeByIDReadQueryResponse(); 
            InternalOrderResponseDetails internalOrderResponseDetails = client.NonChargeableCodeByIDReadQuery(request.NonChargeableCodeReadQuery);
            InternalOrderNonChargeableCodeReadMappingObject mapping = new InternalOrderNonChargeableCodeReadMappingObject();
            mapping.NonChargeCode = response.NonChargeableCodeReadResponse.InternalOrder.OrderNumber.ToString();
            mapping.NonChargeCodeDescription = response.NonChargeableCodeReadResponse.InternalOrder.Description;

            if (response.NonChargeableCodeReadResponse.SystemStatusForOrder.Count() > 0)
                mapping.NonChargeCodeStatus = response.NonChargeableCodeReadResponse.SystemStatusForOrder[0].ToString();
            else
                mapping.NonChargeCodeStatus = string.Empty;


            response.NonChargeableCodeReadResponse.InternalOrder = internalOrderResponseDetails.InternalOrder;

            ////filtering - not necessary, per Email from Thomas Fuhrmann (19.7.2013)

            //string filterKeyName = "Function-" + mapping.Function.ToString() + "-" + mapping.ServiceArea.ToString();
            //System.Configuration.Configuration rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
            //string customSAPFilterElement = ConfigurationManager.AppSettings[filterKeyName.ToString()];
            //if (customSAPFilterElement != null)
            //{
            //    EventLog.WriteEntry("SharePoint-KPMG", "Filter defined: Request permitted for Function=" + mapping.Function.ToString() + " and Service Area=" + mapping.ServiceArea.ToString(), EventLogEntryType.Warning);

            //}
            //else
            //{
            //    EventLog.WriteEntry("SharePoint-KPMG", "Filter not defined: Request rejected for Function=" + mapping.Function.ToString() + " and Service Area=" + mapping.ServiceArea.ToString(), EventLogEntryType.Warning);
            //    return (engagementProperties);

            //}
           

            // add mapped values to the dictionary
            Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

            engagementProperties.Add("Non Charge Code", mapping.NonChargeCode.ToString());
            engagementProperties.Add("Non Charge Code Description", mapping.NonChargeCodeDescription.ToString());
            engagementProperties.Add("Non Charge Code Status", mapping.NonChargeCodeStatus.ToString());


            EngagementsServiceClient EngagementsServiceClient = new EngagementsServiceClient();

            KPMG.Engagements.EngagementsApi.Api api = new KPMG.Engagements.EngagementsApi.Api("");
            ActiveDirectoryHelpers adHelpers = new ActiveDirectoryHelpers(api.AdDeputyGroupsOu, api.AdDeputiesGroupPrefix, api.AdDeputiesOuContainer, api.AdUsersOuContainer);
            //string oppManager = adHelpers.ResolveLoginName(mapping.EngManager);
            //string oppPartner = adHelpers.ResolveLoginName(mapping.EngPartner);

            string opportunityNr = string.Empty;
            string oppPartner = string.Empty;
            string oppManager = string.Empty;
            string engagementId = string.Empty;

            EngagementsServiceClient.UpdateEngagementSiteProperties(opportunityNr, engagementId, oppManager, oppPartner, engagementProperties);
            return (engagementProperties);

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("SharePoint-KPMG", ":Outbound:SAPInternalOrderNonChargeableCodeRead:"+ex.ToString(), EventLogEntryType.Error);

                throw new Exception(ex.ToString());
            }

        }



       
        
        
    }
}
