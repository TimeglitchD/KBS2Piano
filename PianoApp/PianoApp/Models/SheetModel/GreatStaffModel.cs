using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MusicXml.Domain;

namespace PianoApp.Models
{
    public class GreatStaffModel
    {
        public List<StaffModel> StaffList { get; set; } = new List<StaffModel>();
        public List<Measure> MeasureList { get; set; } = new List<Measure>();
        private StackPanel _greatStaff = new StackPanel();
        public Grid GreatStaffGrid { get; } = new Grid();
        public static FontFamily Metdemo { get; set; }
        public static FontFamily Notehedz { get; set; }

        // Create greatStaffModel 
        public GreatStaffModel()
        {

            // Adding the right fonts
            Uri uri = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"/MetDemo.ttf");
            Metdemo = new FontFamily(uri, "MetDemo");

            Uri uri2 = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"/NoteHedz170.ttf");
            Notehedz = new FontFamily(uri2, "NoteHedz");

            // Give the greatStaffs a number
            StaffList.Add(new StaffModel(){Number = 1});
            StaffList.Add(new StaffModel(){Number = 2});
        }

        // Draw the greatStaff
        public Grid DrawGreatStaff()
        {
            // Define 2 columns
            ColumnDefinition col1 = new ColumnDefinition();
            ColumnDefinition col2 = new ColumnDefinition();

            // Add columns to the grid
            GreatStaffGrid.ColumnDefinitions.Add(col1);
            GreatStaffGrid.ColumnDefinitions.Add(col2);

            // Set width of the grid
            col1.Width = new GridLength(100);
            col2.Width = new GridLength(20, GridUnitType.Star);

            foreach (var item in StaffList)
            {
                _greatStaff.Children.Add(item.DrawStaff());
            }

            // Add the greatstaff to the first column
            Grid.SetColumn(_greatStaff, 1);

            Thickness thickness = new Thickness();
            thickness.Top = -3;
            thickness.Bottom = -3;

            // Add the G key to the first staff in grid
            Label gkey = new Label();
            gkey.FontFamily = Metdemo;
            gkey.Content = ":   ";
            gkey.Margin = thickness;
            gkey.HorizontalAlignment = HorizontalAlignment.Left;
            gkey.VerticalAlignment = VerticalAlignment.Top;
            gkey.FontSize = 54;
            Grid.SetColumn(gkey, 1);
            GreatStaffGrid.Children.Add(gkey);

            // Add the F key to the second staff in grid
            Label fkey = new Label();
            fkey.FontFamily = Metdemo;
            fkey.Content = "$   ";
            fkey.Margin = thickness;
            fkey.HorizontalAlignment = HorizontalAlignment.Left;
            fkey.VerticalAlignment = VerticalAlignment.Bottom;
            fkey.FontSize = 54;
            Grid.SetColumn(fkey, 1);
            GreatStaffGrid.Children.Add(fkey);

            // Add the accolade to the beginning of the great staff
            Label accolade = new Label();
            accolade.Content = "{";
            accolade.FontSize = 100;
            accolade.VerticalAlignment = VerticalAlignment.Center;
            accolade.HorizontalAlignment = HorizontalAlignment.Right;
            Grid.SetColumn(accolade, 0);

            _greatStaff.HorizontalAlignment = HorizontalAlignment.Left;

            GreatStaffGrid.Children.Add(_greatStaff);
            GreatStaffGrid.Children.Add(accolade);

            return GreatStaffGrid;
        }
    }
}
