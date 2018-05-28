using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PatientDLL
{
    [DataContract]
    public class Patient
    {
        [DataMember]
        public string name;

        [DataMember]
        public int age;

        public Patient(string name, int age)
        {
            this.name = name;
            this.age = age;
        }
    }
}
