using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using AirportLib;
using WpfApp.ViewModel;

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
            this.DataContext = new NavigationViewModel();
            InitializeAirport();
        }

        private void InitializeAirport()
        {
            Simulator = new Simulator(15, 20, 25);
            Simulator.IsAutoGenereatedReservationsEnabled = true;
            Simulator.Start();
            new DispatcherTimer(TimeSpan.FromSeconds(0.01),
                DispatcherPriority.Normal,
                OnTick,
                Dispatcher.CurrentDispatcher);
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (Monitor.TryEnter(Simulator))
            {
                GL_Time_HHmm.Text = $" {Simulator.Time.DateTime.ToString("HH:mm")}";
                GL_Time_ddMM.Text = $" {Simulator.Time.DateTime.ToString("dd-MM")}";
                GL_Speed.Text = $"| Hastighed {Simulator.Time.Speed}x |";
                GL_Activity.Text = $"Travlhed level {Simulator.ActivityLevel} |";

                Monitor.PulseAll(Simulator);
                Monitor.Exit(Simulator);
            }
        }

        private void Sim_Speed_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mI = (MenuItem)sender;
            string newSpeed = mI.Tag.ToString();
            Simulator.Time.Speed = int.Parse(newSpeed);
        }

        private void Sim_Activity_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mI = (MenuItem)sender;
            string newActivity = mI.Tag.ToString();
            Simulator.ActivityLevel = int.Parse(newActivity);
        }

        private void Window_Closed(object sender, EventArgs e) => Simulator.Stop();

        private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}
