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

        public Grid DrawStaff()
        {
            //create the grid
            Grid stave = new Grid();
            stave.Height = 100;
            stave.Width = 1050;
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
            stave.RowDefinitions.Add(rowDef1);
            stave.RowDefinitions.Add(rowDef2);
            stave.RowDefinitions.Add(rowDef3);
            stave.RowDefinitions.Add(rowDef4);
            stave.RowDefinitions.Add(rowDef5);
            stave.RowDefinitions.Add(rowDef6);
            stave.RowDefinitions.Add(rowDef7);
            stave.RowDefinitions.Add(rowDef8);
            stave.RowDefinitions.Add(rowDef9);
            stave.RowDefinitions.Add(rowDef10);
            stave.RowDefinitions.Add(rowDef11);
            stave.RowDefinitions.Add(rowDef12);
            stave.RowDefinitions.Add(rowDef13);

            //add lines to the grid on the correct places
            for (int i = 0; i < 13; i++)
            {
                if (i > 0 && i < 12 && i % 2 == 0)
                {
                    Line line = new Line();
                    line.Stroke = Brushes.Black;
                    line.X1 = 0;
                    line.X2 = 1050;
                    line.HorizontalAlignment = HorizontalAlignment.Left;
                    line.VerticalAlignment = VerticalAlignment.Center;
                    line.StrokeThickness = 2;
                    Grid.SetRow(line, i);
                    stave.Children.Add(line);
                }

            }

            return stave;

        }
    }
}
