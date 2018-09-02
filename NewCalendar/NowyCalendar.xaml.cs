﻿using System;
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
        private int states = 0;
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
        /// Przechowuje nastepny miesiac wzgledem obecnie wyswietanego
        /// </summary>
        private int nextMonth = (DateTime.Now.Month + 1);
        /// <summary>
        /// Stala liczba "dzieci" w gridzie od wyswietlania dni
        /// </summary>
        private static int gridChildren = 42;
        /// <summary>
        /// Lewy kraniec przedzialu przy wyswietlaniu lat
        /// </summary>
        private int fullYearLeft;
        /// <summary>
        /// Prawy kraniec przedzialu przy wyswietlaniu lat
        /// </summary>
        private int fullYearRight;
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
            
            
            
            NowyCalendarDayButton dm = new NowyCalendarDayButton();
            DataContext = dm;


            DataContext = this;
        }
        /// <summary>
        /// Akcje po nacisnieciu glownego buttona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthYear_Click(object sender, RoutedEventArgs e)
        {


            if(states == 0)
            {
                states = 1;
                YearView.Children.Clear();
                MonthView.Visibility = Visibility.Hidden;
                YearView.Visibility = Visibility.Visible;
                CreateCalendarButton();
                MonthYear.Content = actualYear;
            }
            else if(states == 1)
            {
                YearView.Children.Clear();
                CreateYearSection();
                MonthYear.Content = (fullYearLeft.ToString() + "-" + fullYearRight.ToString());
                states = 2;
                CreateCalendarButton();
            }
            Console.WriteLine(states);
        }
        /// <summary>
        /// Tworzy objekt typu NowyCalendarButton na widoku miesiecy i lat
        /// </summary>
        private void CreateCalendarButton()
        {
            var yearBuffor = fullYearLeft;
            var monthNumber = 1; //Wykorzystywana przy uzupelnianiu contentu buttona poprzez nazwy miesiecy
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    
                    NowyCalendarButton miesiac = new NowyCalendarButton();
                    
                    if (states == 1)
                    {
                        var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber);
                        var monthNameBuffor = monthName.Substring(1); // przechowuje nazwe miesiaca z pominieciem 1 znaku
                        monthName = monthName.Substring(0, 1).ToUpper(); // powieksza 1 litere miesiacca
                        miesiac.Content = monthName + monthNameBuffor; // skleja 1 litere z reszta nazwy miesiaca i przypisuje do Contentu guzika
                        miesiac.VerticalContentAlignment = VerticalAlignment.Center;
                        miesiac.HorizontalContentAlignment = HorizontalAlignment.Center;
                        monthNumber += 1;
                        var zmienna = DateTime.ParseExact(miesiac.Content.ToString().ToLower(), "MMMM", CultureInfo.CurrentCulture).Month;

                        NowyCalendarButton now = new NowyCalendarButton();
                        
                        
                    } else if (states == 2)
                    {
                        miesiac.VerticalContentAlignment = VerticalAlignment.Center;
                        miesiac.HorizontalContentAlignment = HorizontalAlignment.Center;
                        //miesiac.Content = 2017;
                        CreateYearSection();
                        if (i == 0 && j == 0)
                        {
                            miesiac.Content = fullYearLeft - 1;
                            miesiac.Opacity = 0.5;
                            miesiac.IsEnabled = false;
                        }
                        else if (i == 2 && j == 3)
                        {
                            miesiac.Content = fullYearRight + 1;
                            miesiac.Opacity = 0.5;
                            miesiac.IsEnabled = false;
                        }
                        else if(i == 0 && j == 1)
                        {
                            miesiac.Content = fullYearLeft;
                        }
                        else
                        {
                            yearBuffor += 1;
                            miesiac.Content = yearBuffor;
                        }

                        Console.WriteLine(fullYearLeft);
                        Console.WriteLine(fullYearRight);
                    }
                    // miesiac.Click += new RoutedEventHandler(CalendarDayButtonClick);
                    miesiac.MouseLeftButtonDown += new MouseButtonEventHandler(ClickDayButton); //Przypisuje do nowego objektu metode
                    YearView.Children.Add(miesiac);
                    Grid.SetColumn(miesiac, j);
                    Grid.SetRow(miesiac, i);
                }
            }
        }

        private void CreateYearSection()
        {
            var first2Numbers = actualYear.ToString().Substring(0, 2);
            var last2Numbers = actualYear.ToString().Substring(2, 1);
            last2Numbers = last2Numbers.Substring(0, 1) + 0;
            fullYearLeft = Convert.ToInt32(first2Numbers + last2Numbers); // lewy kraniec przedzialu
            last2Numbers = last2Numbers.Substring(0, 1) + 9;
            fullYearRight = Convert.ToInt32(first2Numbers + last2Numbers);
        }
        /// <summary>
        /// Akcje po nacisnieciu na guzik NowyCallendarButton. Akcja zależy od  stanu w jakim jest kalendarz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickDayButton(object sender, MouseButtonEventArgs e)
        {
            if(sender is NowyCalendarButton && states == 1 )
            {
                YearView.Children.Clear();
                YearView.Visibility = Visibility.Hidden;
                MonthView.Visibility = Visibility.Visible;

                actualMonth = DateTime.ParseExact((sender as NowyCalendarButton).Content.ToString().ToLower(), "MMMM", CultureInfo.CurrentCulture).Month; // pobiera miesiac po nacisnieciu guzika i nastepnie zamienia go na int
                previousMonth = actualMonth - 1;
                nextMonth = actualMonth + 1;
                CreateCalendarDayButtonTest(GetCurrentMonthDaysNumber(actualYear, actualMonth), actualMonth);
                
                if(actualMonth == 1)
                {
                    previousMonth = 12;
                }
                if(actualMonth == 12)
                {
                    nextMonth = 1;
                }
                CreatePreviousMonthDays(GetCurrentMonthDaysNumber(actualYear, previousMonth));
                CreateNextMonthDays();
                MonthYear.Content = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth); // przypisanie na nowo nazwy obecnego miesiaca i wyswietlenie na buttonie
                states = 0;
            }
            else if (sender is NowyCalendarButton && states == 2)
            {
                YearView.Children.Clear();
                actualYear = Convert.ToInt32((sender as NowyCalendarButton).Content);
                MonthYear.Content = actualYear;
                states = 1;
                CreateCalendarButton();

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
                
                var lista = DaysOfWeek.ColumnDefinitions.ToList(); // pobieram kolumny do listy 
                var indexCol = lista.IndexOf(DaysOfWeek.ColumnDefinitions.Where(c => c.Name == dayOfWeek).SingleOrDefault()); //pobieram index kolumny podanego dnia
                
                if(i == 0) //pobieram numer kolumny w ktorej zostal wpisany 1 dzien podczas 1 iteracji
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
            var yearBuffor = actualYear;
            var indexRow = rowStart;
            var MonthDays = Convert.ToInt32(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString()); //pobiera liczbe dni wybranego miesiaca w roku
            for (int i = 0; i < gridChildren-(previousDaysLimit+GetCurrentMonthDaysNumber(actualYear,actualMonth)); i++) // 42 stala liczba "dzieci" w gridzie, dlatego wpisana statycznie
            {
                if (actualMonth == 12)
                {
                    buffor = 1;
                    yearBuffor = actualYear + 1;
                }
                else
                {
                    buffor = actualMonth+1;
                }


                var dayOfWeek = DateTime.Parse((yearBuffor + "/" + buffor + "/" + (i + 1).ToString()).ToString()).DayOfWeek.ToString(); //sprawdza jaki to dzien tygodnia

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
        /// <summary>
        /// Akcje wykonywane po nacisnieciu guzika poprzedni
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if(states == 0)
            {
                if (actualMonth == 1)
                {
                    actualMonth = 12;
                    actualYear -= 1;
                }
                else
                {
                    actualMonth -= 1;
                }

                if (previousMonth == 1)
                {
                    previousMonth = 12;
                    nextMonth -= 1;
                }else if(previousMonth == 11)
                {
                    previousMonth -= 1;
                    nextMonth = 12;
                }
                else
                {
                    previousMonth -= 1;
                    nextMonth -= 1;
                }
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth);
                MonthYear.Content = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth); //pobiera nazwe miesiaca w jezyku jaki jest ustawiony na komputerze na podstawie int'a
                rok.Text = actualYear.ToString();
                CreateCalendarDayButtonTest(GetCurrentMonthDaysNumber(actualYear, actualMonth), GetCurrentMonth());


                Console.WriteLine(" previous "+previousMonth);
                Console.WriteLine("next"+nextMonth);
                CreatePreviousMonthDays(GetCurrentMonthDaysNumber(actualYear, previousMonth));
                CreateNextMonthDays();
            }
            else if (states == 1)
            {
                actualYear -= 1;
                MonthYear.Content = actualYear;
            }
            else if (states == 2)
            {
                actualYear -= 10;
                YearView.Children.Clear();
                CreateYearSection();
                CreateCalendarButton();
                if((DateTime.Now.Year - actualYear)> 99)
                {
                    PreviousButton.IsEnabled = false;
                }
            }

            //var miesiac = GetPreviousMonth();
            //var liczbadni= GetLastMonthDaysNumber();
            var ala = "asa";

        }
        /// <summary>
        /// Akcje po nacisnieciu guzika nastepny
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if(states == 0)
            {
                if (actualMonth == 12)
                {
                    actualMonth = 1;
                    actualYear += 1;
                }
                else
                {
                    actualMonth += 1;
                }

                if (nextMonth == 12)
                {
                    nextMonth = 1;
                    previousMonth += 1;
                }
                else if (nextMonth == 2)
                {
                    previousMonth = 1;
                    nextMonth += 1;
                }
                else
                {
                    previousMonth += 1;
                    nextMonth += 1;
                }
                MonthYear.Content = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth);
                rok.Text = actualYear.ToString();
                CreateCalendarDayButtonTest(GetCurrentMonthDaysNumber(actualYear, actualMonth), GetCurrentMonth());
                CreatePreviousMonthDays(GetCurrentMonthDaysNumber(actualYear, previousMonth));
                CreateNextMonthDays();
                Console.WriteLine("previous" + previousMonth);
            }else if(states == 1)
            {
                actualYear += 1;
                MonthYear.Content = actualYear;
            }
            else if(states == 2)
            {
                actualYear += 10;
                YearView.Children.Clear();
                CreateYearSection();
                CreateCalendarButton();
                if ((DateTime.Now.Year - actualYear) < -98)
                {
                    NextButton.IsEnabled = false;
                }
            }
            

        }
        /// <summary>
        /// akcja po nacisnieciu przycisku z rokiem lub miesiacem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalendarDayButtonClick(object sender, RoutedEventArgs e)
        {
            if(sender is Button && states == 1)
            {
                YearView.Children.Clear();
                YearView.Visibility = Visibility.Hidden;
                MonthView.Visibility = Visibility.Visible;
                states = 0;
            }else if (sender is Button && states == 2)
            {
                states = 1;
                CreateCalendarButton();
                
            }
        }
        /// <summary>
        /// Pobiera liczbe dni podanego miesiaca
        /// </summary>
        /// <param name="year">Rok</param>
        /// <param name="month">Miesiac w ktorym pobieram liczbe dni</param>
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
        private int GetCurrentMonth()
        {

            return actualMonth;
        }

    }
}
