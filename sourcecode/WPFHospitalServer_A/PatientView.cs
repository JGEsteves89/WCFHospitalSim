using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPFHospitalServer_A
{
    public class PatientView:INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public PatientView()
        {

        }

        /// <summary>
        /// Initializes an instance of patient
        /// </summary>
        /// <param name="name">Name of patient</param>
        /// <param name="age">Age of Patient</param>
        public PatientView(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }

        /// <summary>
        /// Initializes an instance of patient
        /// </summary>
        /// <param name="name">Name of patient</param>
        /// <param name="age">Age of Patient</param>
        /// <param name="id">ID of Patient</param>
        public PatientView(string name, int age, ulong id)
        {
            this.Name = name;
            this.Age = age;
            this.ID = id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        private ulong id;
        /// <summary>
        /// Identification Number of Patient
        /// </summary>
        public ulong ID
        {
            get { return id; }
            set { id = value; NotifyPropertyChanged(); }
        }

        private string name;
        /// <summary>
        /// Patient's Name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; NotifyPropertyChanged(); }
        }

        private int age;
        /// <summary>
        /// Patient Age
        /// </summary>
        public int Age
        {
            get { return age; }
            set { age = value; NotifyPropertyChanged(); }
        }

       
    }
}
