using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Services
{
    [DataContract]
    public class Patient
    {
        int age = 0;
        string name = "Hello ";
        ulong id = 0;

        [DataMember]
        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public ulong ID
        {
            get { return id; }
            set { id = value; }
        }
    }
}
