﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace wx_site_micc.GetUserByCodeService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Userinfo", Namespace="http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class Userinfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private int errcodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string errmsgField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UserIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DeviceIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string user_ticketField;
        
        private int expires_inField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int errcode {
            get {
                return this.errcodeField;
            }
            set {
                if ((this.errcodeField.Equals(value) != true)) {
                    this.errcodeField = value;
                    this.RaisePropertyChanged("errcode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string errmsg {
            get {
                return this.errmsgField;
            }
            set {
                if ((object.ReferenceEquals(this.errmsgField, value) != true)) {
                    this.errmsgField = value;
                    this.RaisePropertyChanged("errmsg");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string UserId {
            get {
                return this.UserIdField;
            }
            set {
                if ((object.ReferenceEquals(this.UserIdField, value) != true)) {
                    this.UserIdField = value;
                    this.RaisePropertyChanged("UserId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string DeviceId {
            get {
                return this.DeviceIdField;
            }
            set {
                if ((object.ReferenceEquals(this.DeviceIdField, value) != true)) {
                    this.DeviceIdField = value;
                    this.RaisePropertyChanged("DeviceId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string user_ticket {
            get {
                return this.user_ticketField;
            }
            set {
                if ((object.ReferenceEquals(this.user_ticketField, value) != true)) {
                    this.user_ticketField = value;
                    this.RaisePropertyChanged("user_ticket");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=5)]
        public int expires_in {
            get {
                return this.expires_inField;
            }
            set {
                if ((this.expires_inField.Equals(value) != true)) {
                    this.expires_inField = value;
                    this.RaisePropertyChanged("expires_in");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GetUserByCodeService.GetUserByCodeServiceSoap")]
    public interface GetUserByCodeServiceSoap {
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 accessToken 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetUserinfo", ReplyAction="*")]
        wx_site_micc.GetUserByCodeService.GetUserinfoResponse GetUserinfo(wx_site_micc.GetUserByCodeService.GetUserinfoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetUserinfo", ReplyAction="*")]
        System.Threading.Tasks.Task<wx_site_micc.GetUserByCodeService.GetUserinfoResponse> GetUserinfoAsync(wx_site_micc.GetUserByCodeService.GetUserinfoRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetUserinfoRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetUserinfo", Namespace="http://tempuri.org/", Order=0)]
        public wx_site_micc.GetUserByCodeService.GetUserinfoRequestBody Body;
        
        public GetUserinfoRequest() {
        }
        
        public GetUserinfoRequest(wx_site_micc.GetUserByCodeService.GetUserinfoRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetUserinfoRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string accessToken;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string code;
        
        public GetUserinfoRequestBody() {
        }
        
        public GetUserinfoRequestBody(string accessToken, string code) {
            this.accessToken = accessToken;
            this.code = code;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetUserinfoResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetUserinfoResponse", Namespace="http://tempuri.org/", Order=0)]
        public wx_site_micc.GetUserByCodeService.GetUserinfoResponseBody Body;
        
        public GetUserinfoResponse() {
        }
        
        public GetUserinfoResponse(wx_site_micc.GetUserByCodeService.GetUserinfoResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetUserinfoResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public wx_site_micc.GetUserByCodeService.Userinfo GetUserinfoResult;
        
        public GetUserinfoResponseBody() {
        }
        
        public GetUserinfoResponseBody(wx_site_micc.GetUserByCodeService.Userinfo GetUserinfoResult) {
            this.GetUserinfoResult = GetUserinfoResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface GetUserByCodeServiceSoapChannel : wx_site_micc.GetUserByCodeService.GetUserByCodeServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetUserByCodeServiceSoapClient : System.ServiceModel.ClientBase<wx_site_micc.GetUserByCodeService.GetUserByCodeServiceSoap>, wx_site_micc.GetUserByCodeService.GetUserByCodeServiceSoap {
        
        public GetUserByCodeServiceSoapClient() {
        }
        
        public GetUserByCodeServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GetUserByCodeServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GetUserByCodeServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GetUserByCodeServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        wx_site_micc.GetUserByCodeService.GetUserinfoResponse wx_site_micc.GetUserByCodeService.GetUserByCodeServiceSoap.GetUserinfo(wx_site_micc.GetUserByCodeService.GetUserinfoRequest request) {
            return base.Channel.GetUserinfo(request);
        }
        
        public wx_site_micc.GetUserByCodeService.Userinfo GetUserinfo(string accessToken, string code) {
            wx_site_micc.GetUserByCodeService.GetUserinfoRequest inValue = new wx_site_micc.GetUserByCodeService.GetUserinfoRequest();
            inValue.Body = new wx_site_micc.GetUserByCodeService.GetUserinfoRequestBody();
            inValue.Body.accessToken = accessToken;
            inValue.Body.code = code;
            wx_site_micc.GetUserByCodeService.GetUserinfoResponse retVal = ((wx_site_micc.GetUserByCodeService.GetUserByCodeServiceSoap)(this)).GetUserinfo(inValue);
            return retVal.Body.GetUserinfoResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<wx_site_micc.GetUserByCodeService.GetUserinfoResponse> wx_site_micc.GetUserByCodeService.GetUserByCodeServiceSoap.GetUserinfoAsync(wx_site_micc.GetUserByCodeService.GetUserinfoRequest request) {
            return base.Channel.GetUserinfoAsync(request);
        }
        
        public System.Threading.Tasks.Task<wx_site_micc.GetUserByCodeService.GetUserinfoResponse> GetUserinfoAsync(string accessToken, string code) {
            wx_site_micc.GetUserByCodeService.GetUserinfoRequest inValue = new wx_site_micc.GetUserByCodeService.GetUserinfoRequest();
            inValue.Body = new wx_site_micc.GetUserByCodeService.GetUserinfoRequestBody();
            inValue.Body.accessToken = accessToken;
            inValue.Body.code = code;
            return ((wx_site_micc.GetUserByCodeService.GetUserByCodeServiceSoap)(this)).GetUserinfoAsync(inValue);
        }
    }
}
