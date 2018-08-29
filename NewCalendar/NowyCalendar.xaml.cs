using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// Przechowuje nazwe miesiaca
        /// </summary>
        public string MonthName { get; set; }
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
        /// <summary>
        /// Obecnie wyswietlany miesiac w widoku
        /// </summary>
        public int actualMonth = DateTime.Now.Month;
        /// <summary>
        /// Obecnie wyswietlany rok w widkou
        /// </summary>
        private int actualYear = DateTime.Now.Year;
        /// <summary>
        /// Przechowuje miesiac poprzedzajacy obecnie wyswietlany
        /// </summary>
        private int previousMonth = (DateTime.Now.Month)-1;
        /// <summary>
        /// Liczba dni poprzedniego miesiaca wyswietlonego w widoku obecnego. Potrzebne do wyliczenia liczby pozostalego miejsca dla dni kolejnego miesiaca
        /// </summary>
        private int previusDaysNumber;
        public  int GetMonth
        {
            get { return actualMonth; }
        }

        public NowyCalendar()
        {
            
            InitializeComponent();
            MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth);
            MonthYear.Content = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth);
            CreateDaysOfWeek();
            //CreateCalendarDayButton();
            CreateCalendarDayButtonTest(GetCurrentMonthDaysNumber(actualYear,actualMonth),GetCurrentMonth());
            CreatePreviousMonthDays(GetLastMonthDaysNumber());
            CreateNextMonthDays();
            CreateCalendarButton();
            
            
            NowyCalendarDayButton dm = new NowyCalendarDayButton();
            DataContext = dm;


            DataContext = this;
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
        private void CreateCalendarDayButtonTest(int daysNumber, int month)
        {
            MonthView.Children.Clear();
            var indexRow = 0;
            for(int i = 0; i < daysNumber; i++)
            {
                
                string year = DateTime.Now.Year.ToString();
                

                var dayOfWeek = DateTime.Parse((actualYear + "/" + month + "/" + (i+1).ToString()).ToString()).DayOfWeek.ToString(); //sprawdza jaki to dzien tygodnia
                
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
        private void CreatePreviousMonthDays(int monthDays)
        {
            var indexRow = 0;
            for (int i = previousDaysLimit; i > 0; i--)
            {
                string year = DateTime.Now.Year.ToString();
                string month = (Convert.ToInt32(DateTime.Now.Month.ToString())-1).ToString();
                
                //var dayOfWeek = DateTime.Parse((year + "/" + month + "/" + (i + 1).ToString()).ToString()).DayOfWeek.ToString(); //sprawdza jaki to dzien tygodnia

                //var lista = DaysOfWeek.ColumnDefinitions.ToList();
                //var indexCol = lista.IndexOf(DaysOfWeek.ColumnDefinitions.Where(c => c.Name == dayOfWeek).SingleOrDefault()); //pobieram index kolumny podanego dnia

                
                NowyCalendarDayButton dzien = new NowyCalendarDayButton();
                dzien.Content = monthDays - (i-1);
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
            var buffor = 0;
            var indexRow = rowStart;
            var MonthDays = Convert.ToInt32(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString()); //pobiera liczbe dni wybranego miesiaca w roku
            for (int i = 0; i < 42-(previousDaysLimit+GetCurrentMonthDaysNumber(actualYear,actualMonth)); i++)
            {
                if (actualMonth == 12)
                {
                    buffor = 1;
                }
                else
                {
                    buffor = actualMonth+1;
                }


                var dayOfWeek = DateTime.Parse((actualYear + "/" + buffor + "/" + (i + 1).ToString()).ToString()).DayOfWeek.ToString(); //sprawdza jaki to dzien tygodnia

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
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            
            if( actualMonth == 1)
            {
                actualMonth = 12;
                actualYear -= 1;
            }
            else
            {
                actualMonth -= 1;
            }

            if(previousMonth == 1)
            {
                previousMonth = 12;
            }
            else
            {
                previousMonth -= 1;
            }
            MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth);
            MonthYear.Content = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth);
            rok.Text = actualYear.ToString();
            CreateCalendarDayButtonTest(GetCurrentMonthDaysNumber(actualYear,actualMonth), GetCurrentMonth());


            Console.WriteLine(actualYear);
            Console.WriteLine(GetMonth);
            CreatePreviousMonthDays(GetCurrentMonthDaysNumber(actualYear, previousMonth));
            CreateNextMonthDays();
            //var miesiac = GetPreviousMonth();
            //var liczbadni= GetLastMonthDaysNumber();
            var ala = "asa";

        }
        /// <summary>
        /// Pobiera liczbe dni obecnego miesiaca
        /// </summary>
        /// <returns></returns>
        private int GetCurrentMonthDaysNumber(int year, int month)
        {
            var date = Convert.ToInt32(DateTime.DaysInMonth(year, month).ToString());
            
            return date;
        }
        /// <summary>
        /// Pobiera liczbe dni poprzedniego miesiaca
        /// </summary>
        /// <returns></returns>
        private int GetLastMonthDaysNumber()
        {

            var buffor = actualMonth-1;

            
                
            
                          
            return GetCurrentMonthDaysNumber(actualYear,buffor);
        }
        /// <summary>
        /// Pobiera liczbe dni kolejnego miesiaca
        /// </summary>
        /// <returns></returns>
        private int GetNextMonthDaysNumber()
        {
            return GetCurrentMonthDaysNumber(actualYear,actualMonth+1);
        }
        private int GetCurrentMonth()
        {

            return actualMonth;
        }
        private int GetPreviousMonth()
        {
            var Buffor = actualMonth-1;
            return Buffor;
        }
        private int GetNextMonth()
        {
            return GetCurrentMonth() + 1;
        }
    }
}
