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

namespace NewCalendar
{
    /// <summary>
    /// Logika interakcji dla klasy NowyCalendarDayButton.xaml
    /// </summary>
    public partial class NowyCalendarDayButton : UserControl
    {
        public string ContentConnect { get; set; }
        public NowyCalendarDayButton()
        {
            ContentConnect = "ala";
            InitializeComponent();
            
            // NowyCalendarDayButtonDataModel dm = new NowyCalendarDayButtonDataModel();

            // DataContext = dm;





        }
        

        private void UserControl_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Background = new SolidColorBrush(Colors.DeepSkyBlue);
            Background.Opacity = 100;
        }
    }
}
