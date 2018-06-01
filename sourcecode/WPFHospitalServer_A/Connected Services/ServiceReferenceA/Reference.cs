﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WPFHospitalServer_A.ServiceReferenceA {
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
        private int AgeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ulong IDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
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
        public int Age {
            get {
                return this.AgeField;
            }
            set {
                if ((this.AgeField.Equals(value) != true)) {
                    this.AgeField = value;
                    this.RaisePropertyChanged("Age");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ulong ID {
            get {
                return this.IDField;
            }
            set {
                if ((this.IDField.Equals(value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
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
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/UpdatePatientFromB", ReplyAction="http://tempuri.org/IServiceA/UpdatePatientFromBResponse")]
        bool UpdatePatientFromB(WPFHospitalServer_A.ServiceReferenceA.Patient patient);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/UpdatePatientFromB", ReplyAction="http://tempuri.org/IServiceA/UpdatePatientFromBResponse")]
        System.Threading.Tasks.Task<bool> UpdatePatientFromBAsync(WPFHospitalServer_A.ServiceReferenceA.Patient patient);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/UpdatePatient", ReplyAction="http://tempuri.org/IServiceA/UpdatePatientResponse")]
        bool UpdatePatient(WPFHospitalServer_A.ServiceReferenceA.Patient patient);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/UpdatePatient", ReplyAction="http://tempuri.org/IServiceA/UpdatePatientResponse")]
        System.Threading.Tasks.Task<bool> UpdatePatientAsync(WPFHospitalServer_A.ServiceReferenceA.Patient patient);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/DeletePatient", ReplyAction="http://tempuri.org/IServiceA/DeletePatientResponse")]
        bool DeletePatient(WPFHospitalServer_A.ServiceReferenceA.Patient patient);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/DeletePatient", ReplyAction="http://tempuri.org/IServiceA/DeletePatientResponse")]
        System.Threading.Tasks.Task<bool> DeletePatientAsync(WPFHospitalServer_A.ServiceReferenceA.Patient patient);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/DeletePatientFromB", ReplyAction="http://tempuri.org/IServiceA/DeletePatientFromBResponse")]
        bool DeletePatientFromB(WPFHospitalServer_A.ServiceReferenceA.Patient patient);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/DeletePatientFromB", ReplyAction="http://tempuri.org/IServiceA/DeletePatientFromBResponse")]
        System.Threading.Tasks.Task<bool> DeletePatientFromBAsync(WPFHospitalServer_A.ServiceReferenceA.Patient patient);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/ReadPatient", ReplyAction="http://tempuri.org/IServiceA/ReadPatientResponse")]
        WPFHospitalServer_A.ServiceReferenceA.Patient ReadPatient(ulong ID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/ReadPatient", ReplyAction="http://tempuri.org/IServiceA/ReadPatientResponse")]
        System.Threading.Tasks.Task<WPFHospitalServer_A.ServiceReferenceA.Patient> ReadPatientAsync(ulong ID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/ReadPatientFromB", ReplyAction="http://tempuri.org/IServiceA/ReadPatientFromBResponse")]
        WPFHospitalServer_A.ServiceReferenceA.Patient ReadPatientFromB(ulong ID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/ReadPatientFromB", ReplyAction="http://tempuri.org/IServiceA/ReadPatientFromBResponse")]
        System.Threading.Tasks.Task<WPFHospitalServer_A.ServiceReferenceA.Patient> ReadPatientFromBAsync(ulong ID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/CreatePatient", ReplyAction="http://tempuri.org/IServiceA/CreatePatientResponse")]
        WPFHospitalServer_A.ServiceReferenceA.Patient CreatePatient();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/CreatePatient", ReplyAction="http://tempuri.org/IServiceA/CreatePatientResponse")]
        System.Threading.Tasks.Task<WPFHospitalServer_A.ServiceReferenceA.Patient> CreatePatientAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/CreatePatientFromB", ReplyAction="http://tempuri.org/IServiceA/CreatePatientFromBResponse")]
        WPFHospitalServer_A.ServiceReferenceA.Patient CreatePatientFromB();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/CreatePatientFromB", ReplyAction="http://tempuri.org/IServiceA/CreatePatientFromBResponse")]
        System.Threading.Tasks.Task<WPFHospitalServer_A.ServiceReferenceA.Patient> CreatePatientFromBAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/GetPatients", ReplyAction="http://tempuri.org/IServiceA/GetPatientsResponse")]
        WPFHospitalServer_A.ServiceReferenceA.Patient[] GetPatients();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/GetPatients", ReplyAction="http://tempuri.org/IServiceA/GetPatientsResponse")]
        System.Threading.Tasks.Task<WPFHospitalServer_A.ServiceReferenceA.Patient[]> GetPatientsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/GetPatientsFromB", ReplyAction="http://tempuri.org/IServiceA/GetPatientsFromBResponse")]
        WPFHospitalServer_A.ServiceReferenceA.Patient[] GetPatientsFromB();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/GetPatientsFromB", ReplyAction="http://tempuri.org/IServiceA/GetPatientsFromBResponse")]
        System.Threading.Tasks.Task<WPFHospitalServer_A.ServiceReferenceA.Patient[]> GetPatientsFromBAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/ConnectionOK", ReplyAction="http://tempuri.org/IServiceA/ConnectionOKResponse")]
        bool ConnectionOK();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceA/ConnectionOK", ReplyAction="http://tempuri.org/IServiceA/ConnectionOKResponse")]
        System.Threading.Tasks.Task<bool> ConnectionOKAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceAChannel : WPFHospitalServer_A.ServiceReferenceA.IServiceA, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceAClient : System.ServiceModel.ClientBase<WPFHospitalServer_A.ServiceReferenceA.IServiceA>, WPFHospitalServer_A.ServiceReferenceA.IServiceA {
        
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
        
        public bool UpdatePatientFromB(WPFHospitalServer_A.ServiceReferenceA.Patient patient) {
            return base.Channel.UpdatePatientFromB(patient);
        }
        
        public System.Threading.Tasks.Task<bool> UpdatePatientFromBAsync(WPFHospitalServer_A.ServiceReferenceA.Patient patient) {
            return base.Channel.UpdatePatientFromBAsync(patient);
        }
        
        public bool UpdatePatient(WPFHospitalServer_A.ServiceReferenceA.Patient patient) {
            return base.Channel.UpdatePatient(patient);
        }
        
        public System.Threading.Tasks.Task<bool> UpdatePatientAsync(WPFHospitalServer_A.ServiceReferenceA.Patient patient) {
            return base.Channel.UpdatePatientAsync(patient);
        }
        
        public bool DeletePatient(WPFHospitalServer_A.ServiceReferenceA.Patient patient) {
            return base.Channel.DeletePatient(patient);
        }
        
        public System.Threading.Tasks.Task<bool> DeletePatientAsync(WPFHospitalServer_A.ServiceReferenceA.Patient patient) {
            return base.Channel.DeletePatientAsync(patient);
        }
        
        public bool DeletePatientFromB(WPFHospitalServer_A.ServiceReferenceA.Patient patient) {
            return base.Channel.DeletePatientFromB(patient);
        }
        
        public System.Threading.Tasks.Task<bool> DeletePatientFromBAsync(WPFHospitalServer_A.ServiceReferenceA.Patient patient) {
            return base.Channel.DeletePatientFromBAsync(patient);
        }
        
        public WPFHospitalServer_A.ServiceReferenceA.Patient ReadPatient(ulong ID) {
            return base.Channel.ReadPatient(ID);
        }
        
        public System.Threading.Tasks.Task<WPFHospitalServer_A.ServiceReferenceA.Patient> ReadPatientAsync(ulong ID) {
            return base.Channel.ReadPatientAsync(ID);
        }
        
        public WPFHospitalServer_A.ServiceReferenceA.Patient ReadPatientFromB(ulong ID) {
            return base.Channel.ReadPatientFromB(ID);
        }
        
        public System.Threading.Tasks.Task<WPFHospitalServer_A.ServiceReferenceA.Patient> ReadPatientFromBAsync(ulong ID) {
            return base.Channel.ReadPatientFromBAsync(ID);
        }
        
        public WPFHospitalServer_A.ServiceReferenceA.Patient CreatePatient() {
            return base.Channel.CreatePatient();
        }
        
        public System.Threading.Tasks.Task<WPFHospitalServer_A.ServiceReferenceA.Patient> CreatePatientAsync() {
            return base.Channel.CreatePatientAsync();
        }
        
        public WPFHospitalServer_A.ServiceReferenceA.Patient CreatePatientFromB() {
            return base.Channel.CreatePatientFromB();
        }
        
        public System.Threading.Tasks.Task<WPFHospitalServer_A.ServiceReferenceA.Patient> CreatePatientFromBAsync() {
            return base.Channel.CreatePatientFromBAsync();
        }
        
        public WPFHospitalServer_A.ServiceReferenceA.Patient[] GetPatients() {
            return base.Channel.GetPatients();
        }
        
        public System.Threading.Tasks.Task<WPFHospitalServer_A.ServiceReferenceA.Patient[]> GetPatientsAsync() {
            return base.Channel.GetPatientsAsync();
        }
        
        public WPFHospitalServer_A.ServiceReferenceA.Patient[] GetPatientsFromB() {
            return base.Channel.GetPatientsFromB();
        }
        
        public System.Threading.Tasks.Task<WPFHospitalServer_A.ServiceReferenceA.Patient[]> GetPatientsFromBAsync() {
            return base.Channel.GetPatientsFromBAsync();
        }
        
        public bool ConnectionOK() {
            return base.Channel.ConnectionOK();
        }
        
        public System.Threading.Tasks.Task<bool> ConnectionOKAsync() {
            return base.Channel.ConnectionOKAsync();
        }
    }
}
