using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp.Utils
{
    public static class ColorByID
    {
        public static Brush GetColor(int id)
        {
            var colors = typeof(Brushes).GetProperties().Select(x => new { Brush = x.GetValue(null) as Brush }).ToArray();

            if (id + 20 < colors.Length)
            {
                return colors[id + 20].Brush;
            }
            return colors[0].Brush;
        }
    }
}
