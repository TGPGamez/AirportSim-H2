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
using AirportLib;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Simulator Simulator { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeAirport();
        }

        private void InitializeAirport()
        {
            Simulator = new Simulator(15, 20, 25);
            Simulator.IsAutoGenereatedReservationsEnabled = true;
            Simulator.Start();
        }

        private void Sim_Speed_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int newSpeed = (int)button.Tag;
            Simulator.Time.Speed = newSpeed;
        }

        private void Sim_Activity_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int newActivity = (int)button.Tag;
            Simulator.ActivityLevel = newActivity;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Simulator.Stop();
        }
    }
}
