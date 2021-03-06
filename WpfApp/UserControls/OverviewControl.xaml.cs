using AirportLib;
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

namespace WpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for OverviewControl.xaml
    /// </summary>
    public partial class OverviewControl : UserControl
    {
        public Simulator Simulator { get; private set; }
        public OverviewControl()
        {
            InitializeComponent();
        }

        public void StartUp(Simulator simulator)
        {
            Simulator = simulator;

            foreach (Counter counter in Simulator.CountersInAirport.Counters)
            {
                WP_Counters.Children.Add(new CounterTemplate(counter));
            }
        }
    }
}
