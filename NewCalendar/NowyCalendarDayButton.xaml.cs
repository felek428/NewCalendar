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

        public int Nrdnia { get; set; }

        public NowyCalendarDayButton()
        {
            
            InitializeComponent();

            LinearGradientBrush gradient = new LinearGradientBrush();

            gradient.StartPoint = new Point(0.5, 0);
            gradient.EndPoint = new Point(0.5, 1);

            gradient.GradientStops.Add(new GradientStop(Colors.AliceBlue, 0.0));
            gradient.GradientStops.Add(new GradientStop(Colors.LightSteelBlue, 1.0));

            Tekst.Background = gradient;
            // NowyCalendarDayButtonDataModel dm = new NowyCalendarDayButtonDataModel();

            // DataContext = dm;



            DataContext = this;

        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Label labelek = new Label();
            labelek.Name = "Ala";
            labelek.Content = "Ruja";
            
            labelek.ToolTip = labelek.Content;
            
            labelek.MouseLeftButtonDown += new MouseButtonEventHandler(LabelClick);



            Border ramka = new Border();
            ramka.BorderBrush = new SolidColorBrush(Colors.SkyBlue);
            ramka.BorderThickness = new Thickness(1, 1, 1, 1);
            ramka.CornerRadius = new CornerRadius(20, 20, 20, 20);
            ramka.Background = new SolidColorBrush(Colors.AliceBlue);
            ramka.Child = labelek;


            Label test = new Label();
            test.Content = "ala";

           

            Dok.Children.Add(ramka);
            DockPanel.SetDock(ramka, Dock.Top);
        }
        private void LabelClick(object sender, MouseEventArgs et)
        {
            var zmienna = (sender as Label).Content;
            MessageBox.Show("PL132143141413431\n" + zmienna.ToString());
        }

        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenu contextMenu = new ContextMenu();



            MenuItem nowyLabel = new MenuItem();
            nowyLabel.Header = "Dodaj notke";
            nowyLabel.Click += new RoutedEventHandler(MenuItem_Click);
            contextMenu.Items.Add(nowyLabel);
            (sender as UserControl).ContextMenu = contextMenu;
        }
    }
}
