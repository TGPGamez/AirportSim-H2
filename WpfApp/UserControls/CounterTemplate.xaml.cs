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
using WpfApp.Utils;

namespace WpfApp.UserControls
{
    /// <summary>
    /// Interaction logic for CounterTemplate.xaml
    /// </summary>
    public partial class CounterTemplate : UserControl
    {
        public Counter Counter { get; private set; }
        public CounterTemplate(Counter counter)
        {
            Counter = counter;
            InitializeComponent();

            RT_flightColor.Fill = ColorByID.GetColor(counter.ID);
            Update();
        }

        public void Update()
        {
            if (Counter.IsOpen)
            {
                LB_flightTitle.Content = $"Skrænke {Counter.ID} - Åben";
                LB_flightDestination.Content = Counter.Flight.Destination;
                LB_flightName.Content = Counter.Flight.Name;
                LB_flightDeparture.Content = Counter.Flight.Departure.ToShortTimeString();
                LB_flightQueue.Content = $"Kø: {Counter.Flight.GetNoCheckedInAmount()}";
                LB_flightCheckedIn.Content = $"Checked Ind: {Counter.Flight.GetCheckedInAmount()}";
            } else
            {
                LB_flightTitle.Content = $"Skrænke {Counter.ID} - Lukket";
                LB_flightDestination.Content = "";
                LB_flightName.Content = "";
                LB_flightDeparture.Content = "";
                LB_flightQueue.Content = "";
                LB_flightCheckedIn.Content = "";
            }
        }
    }
}
