using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MusicXml.Domain;

namespace PianoApp.Models
{
    public class StaffModel
    {

        public int Number { get; set; }
        public List<Note> NoteList { get; set; } = new List<Note>();
        public Grid Stave { get; set; }

        public Grid DrawStaff()
        {
            //create the grid
            Stave = new Grid();
            Stave.Height = 100;
            Stave.Width = 1055;

            // Define all rows in a staff
            RowDefinition rowDef0 = new RowDefinition();
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();
            RowDefinition rowDef4 = new RowDefinition();
            RowDefinition rowDef5 = new RowDefinition();
            RowDefinition rowDef6 = new RowDefinition();
            RowDefinition rowDef7 = new RowDefinition();
            RowDefinition rowDef8 = new RowDefinition();
            RowDefinition rowDef9 = new RowDefinition();
            RowDefinition rowDef10 = new RowDefinition();
            RowDefinition rowDef11 = new RowDefinition();
            RowDefinition rowDef12 = new RowDefinition();
            RowDefinition rowDef13 = new RowDefinition();
            RowDefinition rowDef14 = new RowDefinition();
            rowDef0.Height = new GridLength(1, GridUnitType.Star);
            rowDef1.Height = new GridLength(1, GridUnitType.Star);
            rowDef2.Height = new GridLength(1, GridUnitType.Star);
            rowDef3.Height = new GridLength(1, GridUnitType.Star);
            rowDef4.Height = new GridLength(1, GridUnitType.Star);
            rowDef5.Height = new GridLength(1, GridUnitType.Star);
            rowDef6.Height = new GridLength(1, GridUnitType.Star);
            rowDef7.Height = new GridLength(1, GridUnitType.Star);
            rowDef8.Height = new GridLength(1, GridUnitType.Star);
            rowDef9.Height = new GridLength(1, GridUnitType.Star);
            rowDef10.Height = new GridLength(1, GridUnitType.Star);
            rowDef11.Height = new GridLength(1, GridUnitType.Star);
            rowDef12.Height = new GridLength(1, GridUnitType.Star);
            rowDef13.Height = new GridLength(1, GridUnitType.Star);
            rowDef14.Height = new GridLength(1, GridUnitType.Star);
            Stave.RowDefinitions.Add(rowDef0);
            Stave.RowDefinitions.Add(rowDef1);
            Stave.RowDefinitions.Add(rowDef2);
            Stave.RowDefinitions.Add(rowDef3);
            Stave.RowDefinitions.Add(rowDef4);
            Stave.RowDefinitions.Add(rowDef5);
            Stave.RowDefinitions.Add(rowDef6);
            Stave.RowDefinitions.Add(rowDef7);
            Stave.RowDefinitions.Add(rowDef8);
            Stave.RowDefinitions.Add(rowDef9);
            Stave.RowDefinitions.Add(rowDef10);
            Stave.RowDefinitions.Add(rowDef11);
            Stave.RowDefinitions.Add(rowDef12);
            Stave.RowDefinitions.Add(rowDef13);
            Stave.RowDefinitions.Add(rowDef14);

            //add lines to the grid on the correct places
            for (int i = 0; i < 13; i++)
            {
                if (i >=3 && i <= 11 && i % 2 == 1)
                {
                    Line line = new Line();
                    line.Stroke = Brushes.Black;
                    line.X1 = 0;
                    line.X2 = Stave.Width-5;
                    line.HorizontalAlignment = HorizontalAlignment.Left;
                    line.VerticalAlignment = VerticalAlignment.Center;
                    line.StrokeThickness = 1;
                    Grid.SetRow(line, i);
                    Stave.Children.Add(line);
                }

            }

            return Stave;

        }
    }
}
