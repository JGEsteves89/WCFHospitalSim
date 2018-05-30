using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibray
{
    /// <summary>
    /// This class contains the information about te patient
    /// </summary>
    [DataContract]
    public class Patient
    {
        /// <summary>
        /// Identification Number of Patient
        /// </summary>
        public ulong ID
        {
            get;
            set;
        }

        /// <summary>
        /// Patient's Name
        /// </summary>
        [DataMember]
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// Patient Age
        /// </summary>
        [DataMember]
        public int age;

        /// <summary>
        /// 
        /// </summary>
        public Patient()
        {

        }

        /// <summary>
        /// Initializes an instance of patient
        /// </summary>
        /// <param name="name">Name of patient</param>
        /// <param name="age">Age of Patient</param>
        public Patient(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        /// <summary>
        /// Initializes an instance of patient
        /// </summary>
        /// <param name="name">Name of patient</param>
        /// <param name="age">Age of Patient</param>
        /// <param name="id">ID of Patient</param>
        public Patient(string name, int age, ulong id)
        {
            this.name = name;
            this.age = age;
            this.ID = id;
        }
    }
}
