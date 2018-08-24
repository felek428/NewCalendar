using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NewCalendar
{
    public class NowyCalendarButtonDataModel
    {
        public string Month { get; set; }

          

        public NowyCalendarButtonDataModel()
        {
            Inicjalizacja();
        }
        public void Inicjalizacja()
        {
            


            var dzisiaj = DateTime.Now.ToString();
            var zmienna = DateTime.Parse("2119/1/1").DayOfWeek.ToString(); //okreska jaki to dzien tygodnia z wpisanej daty
            int month = 2;
            int year = 2020;
            Month = DateTime.DaysInMonth(year,month).ToString(); // liczba dni kontretnego miesiaca w roku
            
        }
    }
}
