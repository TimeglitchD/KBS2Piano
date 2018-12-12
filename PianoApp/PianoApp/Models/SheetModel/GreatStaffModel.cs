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
        private StackPanel GreatStaff = new StackPanel();
        public Grid GreatStaffGrid { get; } = new Grid();
        public static FontFamily Metdemo { get; set; }
        public static FontFamily Notehedz { get; set; }


        public GreatStaffModel()
        {
            //initialize the fonts
            Uri uri = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"/fonts/MetDemo.ttf");
            Metdemo = new FontFamily(uri, "MetDemo");

            Uri uri2 = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"/fonts/NoteHedz170.ttf");
            Notehedz = new FontFamily(uri2, "NoteHedz");

            StaffList.Add(new StaffModel(){Number = 1});
            StaffList.Add(new StaffModel(){Number = 2});
        }

        public Grid DrawGreatStaff()
        {
            ColumnDefinition col1 = new ColumnDefinition();
            ColumnDefinition col2 = new ColumnDefinition();
            GreatStaffGrid.ColumnDefinitions.Add(col1);
            GreatStaffGrid.ColumnDefinitions.Add(col2);
            col1.Width = new GridLength(100);
            col2.Width = new GridLength(20, GridUnitType.Star);

            

            foreach (var item in StaffList)
            {
                GreatStaff.Children.Add(item.DrawStaff());
                Console.WriteLine("Stave added");
            }

            Grid.SetColumn(GreatStaff, 1);

            Thickness thickness = new Thickness();
            thickness.Top = -3;
            thickness.Bottom = -3;

            Label gkey = new Label();
            gkey.FontFamily = Metdemo;
            gkey.Content = ":   ";
            gkey.Margin = thickness;
            gkey.HorizontalAlignment = HorizontalAlignment.Left;
            gkey.VerticalAlignment = VerticalAlignment.Top;
            gkey.FontSize = 54;
            Grid.SetColumn(gkey, 1);
            GreatStaffGrid.Children.Add(gkey);

            Label fkey = new Label();
            fkey.FontFamily = Metdemo;
            fkey.Content = "$   ";
            fkey.Margin = thickness;
            fkey.HorizontalAlignment = HorizontalAlignment.Left;
            fkey.VerticalAlignment = VerticalAlignment.Bottom;
            fkey.FontSize = 54;
            Grid.SetColumn(fkey, 1);
            GreatStaffGrid.Children.Add(fkey);

            Label accolade = new Label();
            accolade.Content = "{";
            accolade.FontSize = 100;
            accolade.VerticalAlignment = VerticalAlignment.Center;
            accolade.HorizontalAlignment = HorizontalAlignment.Right;
            Grid.SetColumn(accolade, 0);

            GreatStaff.HorizontalAlignment = HorizontalAlignment.Left;

            GreatStaffGrid.Children.Add(GreatStaff);
            GreatStaffGrid.Children.Add(accolade);

            return GreatStaffGrid;
        }
    }
}
