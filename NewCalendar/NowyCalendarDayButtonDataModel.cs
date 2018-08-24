using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NewCalendar
{
    public class NowyCalendarDayButtonDataModel : DependencyObject
    {
        public string Connect { get; set; }
        public List<string> Lista { get; set; }

        static NowyCalendarDayButtonDataModel()
        {
            NameAndSurNameDependencyProperty = DependencyProperty.Register
                ("NameAndSurName",
                typeof(string), typeof(NowyCalendarDayButtonDataModel));
        }

        public static readonly DependencyProperty NameAndSurNameDependencyProperty;
        public string NameAndSurName
        {
            get { return (string)GetValue(NameAndSurNameDependencyProperty); }
            set { SetValue(NameAndSurNameDependencyProperty, value); }
        }

        public NowyCalendarDayButtonDataModel()
        {
            Inicjalizacja();
            
        }

        private void Inicjalizacja()
        {
            
            NameAndSurName = "ala";
            var data = DateTime.Today.DayOfWeek.ToString();

            var zmienna = data.ToString();
            Connect = zmienna;
        }
    }
}
