using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MusicXml.Domain;

namespace PianoApp.Models
{
    public class GreatStaffModel
    {
        public List<StaffModel> StaffList { get; set; } = new List<StaffModel>();
        public List<Measure> MeasureList { get; set; } = new List<Measure>();
        private StackPanel GreatStaff = new StackPanel();
        private Grid GreatStaffGrid = new Grid();

        public GreatStaffModel()
        {
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
