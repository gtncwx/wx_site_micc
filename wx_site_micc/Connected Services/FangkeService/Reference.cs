﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace wx_site_micc.FangkeService {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="FangkeService.FangkeServiceSoap")]
    public interface FangkeServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/AddHVS010TB", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable AddHVS010TB(wx_site_micc.FangkeService.HVS010TB hvs010tb);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/AddHVS010TB", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Data.DataTable> AddHVS010TBAsync(wx_site_micc.FangkeService.HVS010TB hvs010tb);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SearchHVS010TB", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable SearchHVS010TB(wx_site_micc.FangkeService.HVS010TB hvs010tb);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SearchHVS010TB", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Data.DataTable> SearchHVS010TBAsync(wx_site_micc.FangkeService.HVS010TB hvs010tb);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CheckHVS010TB", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable CheckHVS010TB(wx_site_micc.FangkeService.HVS010TB hvs010tb);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CheckHVS010TB", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Data.DataTable> CheckHVS010TBAsync(wx_site_micc.FangkeService.HVS010TB hvs010tb);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class HVS010TB : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string emp_noField;
        
        private string emp_nmField;
        
        private string emp_zwField;
        
        private string dept_nmField;
        
        private string vs_nmField;
        
        private string vs_companyField;
        
        private int vs_countField;
        
        private string url_idField;
        
        private string vs_dateField;
        
        private string vs_btimeField;
        
        private string vs_etimeField;
        
        private string vs_noteField;
        
        private string use_noField;
        
        private string use_ipField;
        
        private string car_noField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string emp_no {
            get {
                return this.emp_noField;
            }
            set {
                this.emp_noField = value;
                this.RaisePropertyChanged("emp_no");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string emp_nm {
            get {
                return this.emp_nmField;
            }
            set {
                this.emp_nmField = value;
                this.RaisePropertyChanged("emp_nm");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string emp_zw {
            get {
                return this.emp_zwField;
            }
            set {
                this.emp_zwField = value;
                this.RaisePropertyChanged("emp_zw");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string dept_nm {
            get {
                return this.dept_nmField;
            }
            set {
                this.dept_nmField = value;
                this.RaisePropertyChanged("dept_nm");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string vs_nm {
            get {
                return this.vs_nmField;
            }
            set {
                this.vs_nmField = value;
                this.RaisePropertyChanged("vs_nm");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string vs_company {
            get {
                return this.vs_companyField;
            }
            set {
                this.vs_companyField = value;
                this.RaisePropertyChanged("vs_company");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public int vs_count {
            get {
                return this.vs_countField;
            }
            set {
                this.vs_countField = value;
                this.RaisePropertyChanged("vs_count");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string url_id {
            get {
                return this.url_idField;
            }
            set {
                this.url_idField = value;
                this.RaisePropertyChanged("url_id");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public string vs_date {
            get {
                return this.vs_dateField;
            }
            set {
                this.vs_dateField = value;
                this.RaisePropertyChanged("vs_date");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string vs_btime {
            get {
                return this.vs_btimeField;
            }
            set {
                this.vs_btimeField = value;
                this.RaisePropertyChanged("vs_btime");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string vs_etime {
            get {
                return this.vs_etimeField;
            }
            set {
                this.vs_etimeField = value;
                this.RaisePropertyChanged("vs_etime");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public string vs_note {
            get {
                return this.vs_noteField;
            }
            set {
                this.vs_noteField = value;
                this.RaisePropertyChanged("vs_note");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=12)]
        public string use_no {
            get {
                return this.use_noField;
            }
            set {
                this.use_noField = value;
                this.RaisePropertyChanged("use_no");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=13)]
        public string use_ip {
            get {
                return this.use_ipField;
            }
            set {
                this.use_ipField = value;
                this.RaisePropertyChanged("use_ip");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=14)]
        public string car_no {
            get {
                return this.car_noField;
            }
            set {
                this.car_noField = value;
                this.RaisePropertyChanged("car_no");
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
    public interface FangkeServiceSoapChannel : wx_site_micc.FangkeService.FangkeServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FangkeServiceSoapClient : System.ServiceModel.ClientBase<wx_site_micc.FangkeService.FangkeServiceSoap>, wx_site_micc.FangkeService.FangkeServiceSoap {
        
        public FangkeServiceSoapClient() {
        }
        
        public FangkeServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FangkeServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FangkeServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FangkeServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Data.DataTable AddHVS010TB(wx_site_micc.FangkeService.HVS010TB hvs010tb) {
            return base.Channel.AddHVS010TB(hvs010tb);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> AddHVS010TBAsync(wx_site_micc.FangkeService.HVS010TB hvs010tb) {
            return base.Channel.AddHVS010TBAsync(hvs010tb);
        }
        
        public System.Data.DataTable SearchHVS010TB(wx_site_micc.FangkeService.HVS010TB hvs010tb) {
            return base.Channel.SearchHVS010TB(hvs010tb);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> SearchHVS010TBAsync(wx_site_micc.FangkeService.HVS010TB hvs010tb) {
            return base.Channel.SearchHVS010TBAsync(hvs010tb);
        }
        
        public System.Data.DataTable CheckHVS010TB(wx_site_micc.FangkeService.HVS010TB hvs010tb) {
            return base.Channel.CheckHVS010TB(hvs010tb);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataTable> CheckHVS010TBAsync(wx_site_micc.FangkeService.HVS010TB hvs010tb) {
            return base.Channel.CheckHVS010TBAsync(hvs010tb);
        }
    }
}
