﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AcmeCorp.Engagements.EngagementsDomain
{

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "urn:AcmeCorp:eu:generic:basis:services", ConfigurationName = "CustomTableRead_Outb")]
    public interface CustomTableRead_Outb
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://sap.com/xi/WebService/soap1.1", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        CustomTableReadQueryRequest CustomTableReadQuery(CustomTableReadQueryRequest request);

        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action = "http://sap.com/xi/WebService/soap1.1", ReplyAction = "*")]
        System.Threading.Tasks.Task<CustomTableReadQueryRequest> CustomTableReadQueryAsync(CustomTableReadQueryRequest request);
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:AcmeCorp:eu:generic:basis:services")]
    public partial class CustomTableQueryResponse
    {

        private string tableNameField;

        private CustomTableQueryResponseRows[] rowsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string TableName
        {
            get
            {
                return this.tableNameField;
            }
            set
            {
                this.tableNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Rows", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public CustomTableQueryResponseRows[] Rows
        {
            get
            {
                return this.rowsField;
            }
            set
            {
                this.rowsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:AcmeCorp:eu:generic:basis:services")]
    public partial class CustomTableQueryResponseRows
    {

        private string keyField;

        private string languageField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string Language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
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

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CustomTableReadQueryRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "urn:AcmeCorp:eu:generic:basis:services", Order = 0)]
        public CustomTableQueryResponse CustomTableReadQueryResponse;

        public CustomTableReadQueryRequest()
        {
        }

        public CustomTableReadQueryRequest(CustomTableQueryResponse CustomTableReadQueryResponse)
        {
            this.CustomTableReadQueryResponse = CustomTableReadQueryResponse;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CustomTableRead_OutbChannel : CustomTableRead_Outb, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CustomTableRead_OutbClient : System.ServiceModel.ClientBase<CustomTableRead_Outb>, CustomTableRead_Outb
    {

        public CustomTableRead_OutbClient()
        {
        }

        public CustomTableRead_OutbClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public CustomTableRead_OutbClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public CustomTableRead_OutbClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public CustomTableRead_OutbClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CustomTableReadQueryRequest CustomTableRead_Outb.CustomTableReadQuery(CustomTableReadQueryRequest request)
        {
            return base.Channel.CustomTableReadQuery(request);
        }

        public void CustomTableReadQuery(ref CustomTableQueryResponse CustomTableReadQueryResponse)
        {
            CustomTableReadQueryRequest inValue = new CustomTableReadQueryRequest();
            inValue.CustomTableReadQueryResponse = CustomTableReadQueryResponse;
            CustomTableReadQueryRequest retVal = ((CustomTableRead_Outb)(this)).CustomTableReadQuery(inValue);
            CustomTableReadQueryResponse = retVal.CustomTableReadQueryResponse;
        }

        public System.Threading.Tasks.Task<CustomTableReadQueryRequest> CustomTableReadQueryAsync(CustomTableReadQueryRequest request)
        {
            return base.Channel.CustomTableReadQueryAsync(request);
        }
    }

}