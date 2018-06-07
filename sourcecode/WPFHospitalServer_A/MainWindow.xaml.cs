using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFHospitalServer_A;
using WPFHospitalServer_A.ServiceReferenceA;

namespace WPFHospitalServer_A
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServiceReferenceA.ServiceAClient server = new ServiceReferenceA.ServiceAClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //   List<Patient> patients = new List<Patient>();
                //  patients.AddRange(server.GetPatients());
                List<PatientView> patients = new List<PatientView>();
                patients.Add(new PatientView() { Name = "Ana", ID = 1, Age = 21 });
                patients.Add(new PatientView() { Name = "Bruna", ID = 2, Age = 22 });
                patients.Add(new PatientView() { Name = "Carla", ID = 3, Age = 23 });
                patients.Add(new PatientView() { Name = "Diana", ID = 4, Age = 24 });
                patients.Add(new PatientView() { Name = "Eduarda", ID = 5, Age = 25 });

                lst_Patients.ItemsSource = patients;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void lst_Patients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(e.AddedItems.Count>0)
                {
                    PatientView patient = e.AddedItems[0] as PatientView;
                    if(patient!=null)
                    {
                        grp_Patient.DataContext = patient;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
