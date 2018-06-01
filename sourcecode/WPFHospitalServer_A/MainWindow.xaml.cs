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

namespace WPFHospitalServer_A
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            lb_Patients.Items.Add("Ola 1");
            lb_Patients.Items.Add("Ola 2");
            lb_Patients.Items.Add("Ola 3");
            lb_Patients.Items.Add("Ola 4");
            lb_Patients.Items.Add("Ola 5");
            lb_Patients.Items.Add("Ola 6");
            lb_Patients.Items.Add("Ola 7");

            lb_Patients.Items.Add("Ola 1");
        }
    }
}
