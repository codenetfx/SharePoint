        using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AcmeCorp.Engagements.EngagementsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IEngagementsService
    {

        [OperationContract]
        string GetData(string value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        string EditTerm(string tablename, string term, string language);

        [OperationContract]
        string CreateNewEngagementSite(long engagementId, int engagementFoldersType, List<string> managers, List<string> partners, List<string> staff, Dictionary<string, object> engagementProperties);

        [OperationContract]
        string UpdateEngagementSiteProperties(long engagementId, int engagementFoldersType, List<string> managers, List<string> partners, List<string> staff, Dictionary<string, object> engagementProperties);

        [OperationContract]
        string GetEngagementStatus(long engagementId);

        [OperationContract]
        string CloseEngagement(long engagementId);

        [OperationContract]
        string ReopenEngagement(long engagementId);

        [OperationContract]
        string CreateNewOpportunitySite(long opportunityId, List<string> managers, List<string> partners, List<string> staff, Dictionary<string, object> opportunityProperties);

        [OperationContract]
        string UpdateOpportunitySiteProperties(long opportunityId, List<string> managers, List<string> partners, List<string> staff, Dictionary<string, object> opportunityProperties);

        [OperationContract]
        string CreateEngagementFromOpportunity(long opportunityId);

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
