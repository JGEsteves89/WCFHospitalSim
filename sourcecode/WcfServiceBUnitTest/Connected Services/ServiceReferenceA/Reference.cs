﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WcfServiceBUnitTest.ServiceReferenceA {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Patient", Namespace="http://schemas.datacontract.org/2004/07/SharedLibray")]
    [System.SerializableAttribute()]
    public partial class Patient : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string nameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int age {
            get {
                return this.ageField;
            }
            set {
                if ((this.ageField.Equals(value) != true)) {
                    this.ageField = value;
                    this.RaisePropertyChanged("age");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                if ((object.ReferenceEquals(this.nameField, value) != true)) {
                    this.nameField = value;
                    this.RaisePropertyChanged("name");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReferenceA.IServiceA")]
    public interface IServiceA {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/getPatients", ReplyAction="http://tempuri.org/IServiceA/getPatientsResponse")]
        WcfServiceBUnitTest.ServiceReferenceA.Patient[] getPatients();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/getPatients", ReplyAction="http://tempuri.org/IServiceA/getPatientsResponse")]
        System.Threading.Tasks.Task<WcfServiceBUnitTest.ServiceReferenceA.Patient[]> getPatientsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/getPatientsFromB", ReplyAction="http://tempuri.org/IServiceA/getPatientsFromBResponse")]
        WcfServiceBUnitTest.ServiceReferenceA.Patient[] getPatientsFromB();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/getPatientsFromB", ReplyAction="http://tempuri.org/IServiceA/getPatientsFromBResponse")]
        System.Threading.Tasks.Task<WcfServiceBUnitTest.ServiceReferenceA.Patient[]> getPatientsFromBAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/ConnectionOK", ReplyAction="http://tempuri.org/IServiceA/ConnectionOKResponse")]
        bool ConnectionOK();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/ConnectionOK", ReplyAction="http://tempuri.org/IServiceA/ConnectionOKResponse")]
        System.Threading.Tasks.Task<bool> ConnectionOKAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceAChannel : WcfServiceBUnitTest.ServiceReferenceA.IServiceA, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceAClient : System.ServiceModel.ClientBase<WcfServiceBUnitTest.ServiceReferenceA.IServiceA>, WcfServiceBUnitTest.ServiceReferenceA.IServiceA {
        
        public ServiceAClient() {
        }
        
        public ServiceAClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceAClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceAClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceAClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public WcfServiceBUnitTest.ServiceReferenceA.Patient[] getPatients() {
            return base.Channel.getPatients();
        }
        
        public System.Threading.Tasks.Task<WcfServiceBUnitTest.ServiceReferenceA.Patient[]> getPatientsAsync() {
            return base.Channel.getPatientsAsync();
        }
        
        public WcfServiceBUnitTest.ServiceReferenceA.Patient[] getPatientsFromB() {
            return base.Channel.getPatientsFromB();
        }
        
        public System.Threading.Tasks.Task<WcfServiceBUnitTest.ServiceReferenceA.Patient[]> getPatientsFromBAsync() {
            return base.Channel.getPatientsFromBAsync();
        }
        
        public bool ConnectionOK() {
            return base.Channel.ConnectionOK();
        }
        
        public System.Threading.Tasks.Task<bool> ConnectionOKAsync() {
            return base.Channel.ConnectionOKAsync();
        }
    }
}
