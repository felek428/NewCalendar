using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Logika interakcji dla klasy NowyCalendar.xaml
    /// </summary>
    public partial class NowyCalendar : UserControl
    {
        /// <summary>
        /// limit do tworzenia dni poprzedniego miesiaca oznaczony przez indeks kolumny w ktorym zostal oznaczony 1 dzien obecnego miesiaca
        /// </summary>
        private int previousDaysLimit; 
        /// <summary>
        /// Indeks rzedu z ktorego ma zaczac dodawac dni kolejnego miesiaca;
        /// </summary>
        private int rowStart; 
        /// <summary>
        /// Przechowuje numer stanu w jakim znajduje sie kalendarz
        /// </summary>
        private int states = 1;
        public NowyCalendar()
        {
            InitializeComponent();
            CreateDaysOfWeek();
            //CreateCalendarDayButton();
            CreateCalendarDayButtonTest(GetCurrentMonthDaysNumber());
            CreatePreviousMonthDays();
            CreateNextMonthDays();
            CreateCalendarButton();

            
            NowyCalendarDayButton dm = new NowyCalendarDayButton();
            DataContext = dm;
        }

        private void MonthYear_Click(object sender, RoutedEventArgs e)
        {
            if(MonthView.Visibility == Visibility.Visible)
            {
                MonthView.Visibility = Visibility.Hidden;
                YearView.Visibility = Visibility.Visible;
            }
            else
            {
                MonthView.Visibility = Visibility.Visible;
                YearView.Visibility = Visibility.Hidden;
            }
            
        }

        private void CreateCalendarButton() //tworzy userontrola dla miesiecy
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    NowyCalendarButton miesiac = new NowyCalendarButton();
                    NowyCalendarButtonDataModel he = new NowyCalendarButtonDataModel();

                    YearView.Children.Add(miesiac);
                    Grid.SetColumn(miesiac, i);
                    Grid.SetRow(miesiac, j);
                }
            }
        }
        private void CreateCalendarDayButton() //tworzy usercontrol dla dni miesiaca
        {
            int pom = 0;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    pom++;
                    
                    NowyCalendarDayButton guzik = new NowyCalendarDayButton();
                    guzik.Content = pom;
                    MonthView.Children.Add(guzik);
                    Grid.SetColumn(guzik, j);
                    Grid.SetRow(guzik, i);
                }
            }
        }
        /// <summary>
        /// Tworzy dni obecnego miesiaca w widoku
        /// </summary>
        /// <param name="daysNumber"></param>
        private void CreateCalendarDayButtonTest(int daysNumber)
        {
            var indexRow = 0;
            for(int i = 0; i < daysNumber; i++)
            {
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();

                var dayOfWeek = DateTime.Parse((year + "/" + month + "/" + (i+1).ToString()).ToString()).DayOfWeek.ToString(); //sprawdza jaki to dzien tygodnia
                
                var lista = DaysOfWeek.ColumnDefinitions.ToList();
                var indexCol = lista.IndexOf(DaysOfWeek.ColumnDefinitions.Where(c => c.Name == dayOfWeek).SingleOrDefault()); //pobieram index kolumny podanego dnia
                
                if(i == 0)
                {
                    previousDaysLimit = indexCol;
                }
                NowyCalendarDayButton dzien = new NowyCalendarDayButton();
                dzien.Content = i+1;

                MonthView.Children.Add(dzien);
                Grid.SetColumn(dzien, indexCol);
                Grid.SetRow(dzien, indexRow);

                if (dayOfWeek.Equals("Sunday"))
                {
                    indexRow++;
                }

                rowStart = indexRow;
                var xd = "ala";
            }
        }
        /// <summary>
        /// tworzy nagłówek dla kolumn z  nazwami dni tygodnia
        /// </summary>
        private void CreateDaysOfWeek() 
        {
            for(int i = 0; i < 7; i++)
            {
                Thickness margines = new Thickness();
                margines.Left = 1;
                margines.Right = 1;
                margines.Top = 1;
                margines.Bottom = 1;

                TextBlock tbDay = new TextBlock();
                tbDay.VerticalAlignment = VerticalAlignment.Center;
                tbDay.FontSize = 16;
                tbDay.Foreground = new SolidColorBrush(Colors.Black);  
                tbDay.Text = DaysNameTranslate(DaysOfWeek.ColumnDefinitions[i].Name);
                tbDay.TextAlignment = TextAlignment.Center;
                DaysOfWeek.Children.Add(tbDay);
                Grid.SetColumn(tbDay, i);
            }
        }
        /// <summary>
        /// Tlumaczy dni tygodnia na polski jezyk
        /// </summary>
        /// <param name="dzienEng"> String z dniem tygodnia po ang</param>
        /// <returns></returns>
        private string DaysNameTranslate(string dzienEng) //tlumaczy angielskie nazwy dni tygodnia na polski
        {
            switch(dzienEng)
            {
                case "Monday":
                    return "Poniedziałek";
                case "Tuesday":
                    return "Wtorek";
                case "Wednesday":
                    return "Środa";
                case "Thursday":
                    return "Czwartek";
                case "Friday":
                    return "Piątek";
                case "Saturday":
                    return "Sobota";
                case "Sunday":
                    return "Niedziela";
                default:
                    return "0";

            }
            
        }
        /// <summary>
        /// Tworzy dni poprzedniego miesiaca w widoku obecnego
        /// </summary>
        private void CreatePreviousMonthDays()
        {
            var indexRow = 0;
            var MonthDays = Convert.ToInt32(DateTime.DaysInMonth(DateTime.Now.Year, Convert.ToInt32(DateTime.Now.Month)-1).ToString()); //pobiera liczbe dni wybranego miesiaca w roku
            for (int i = previousDaysLimit; i > 0; i--)
            {
                string year = DateTime.Now.Year.ToString();
                string month = (Convert.ToInt32(DateTime.Now.Month.ToString())-1).ToString();
                
                //var dayOfWeek = DateTime.Parse((year + "/" + month + "/" + (i + 1).ToString()).ToString()).DayOfWeek.ToString(); //sprawdza jaki to dzien tygodnia

                //var lista = DaysOfWeek.ColumnDefinitions.ToList();
                //var indexCol = lista.IndexOf(DaysOfWeek.ColumnDefinitions.Where(c => c.Name == dayOfWeek).SingleOrDefault()); //pobieram index kolumny podanego dnia

                
                NowyCalendarDayButton dzien = new NowyCalendarDayButton();
                dzien.Content = MonthDays - (i-1);
                dzien.Opacity = 0.8;
                dzien.IsEnabled = false;
                MonthView.Children.Add(dzien);
                Grid.SetColumn(dzien, (previousDaysLimit-i));
                Grid.SetRow(dzien, indexRow);

                
                var xd = "ala";
            }
        }
        /// <summary>
        /// Tworzy dni kolejnego miesiaca w widoku obecnego
        /// </summary>
        private void CreateNextMonthDays()
        {
            var indexRow = rowStart;
            var MonthDays = Convert.ToInt32(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString()); //pobiera liczbe dni wybranego miesiaca w roku
            for (int i = 0; i < 9; i++)
            {
                string year = DateTime.Now.Year.ToString();
                string month = (DateTime.Now.Month+1).ToString();

                var dayOfWeek = DateTime.Parse((year + "/" + month + "/" + (i + 1).ToString()).ToString()).DayOfWeek.ToString(); //sprawdza jaki to dzien tygodnia

                var lista = DaysOfWeek.ColumnDefinitions.ToList();
                var indexCol = lista.IndexOf(DaysOfWeek.ColumnDefinitions.Where(c => c.Name == dayOfWeek).SingleOrDefault()); //pobieram index kolumny podanego dnia

                
                NowyCalendarDayButton dzien = new NowyCalendarDayButton();
                dzien.Content = i + 1;
                dzien.Opacity = 0.8;
                dzien.IsEnabled = false;

                MonthView.Children.Add(dzien);
                Grid.SetColumn(dzien, indexCol);
                Grid.SetRow(dzien, indexRow);

                if (dayOfWeek.Equals("Sunday"))
                {
                    indexRow++;
                }


                var xd = "ala";
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            

            
        }
        /// <summary>
        /// Pobiera liczbe dni obecnego miesiaca
        /// </summary>
        /// <returns></returns>
        private int GetCurrentMonthDaysNumber()
        {
            var date = Convert.ToInt32(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString()); ;
            return date;
        }
        /// <summary>
        /// Pobiera liczbe dni poprzedniego miesiaca
        /// </summary>
        /// <returns></returns>
        private int GetLastMonthDaysNumber()
        {
            return GetCurrentMonthDaysNumber() - 1;
        }
        /// <summary>
        /// Pobiera liczbe dni kolejnego miesiaca
        /// </summary>
        /// <returns></returns>
        private int GetNextMonthDaysNumber()
        {
            return GetCurrentMonthDaysNumber() + 1;
        }
    }
}
