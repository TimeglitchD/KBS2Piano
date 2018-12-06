using MusicXml.Domain;
using PianoApp.Controllers;
using PianoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PianoApp.Views
{
    public class NoteView
    {

        public MusicPieceController mPc;

        public NoteView(StaveView sv)
        {
            this.mPc = sv.MusicPieceController;
            mPc.SheetController.NoteView = this;
            DrawNotes();
        }

        public void DrawNotes()
        {
            decimal totalwidth = 0;
            int idx = 0;
            foreach (var greatStaff in mPc.Sheet.GreatStaffModelList)
            {
                totalwidth = 0;
                if(greatStaff.MeasureList.Count() == 0)
                {
                    DrawEnd(totalwidth, mPc.Sheet.GreatStaffModelList[idx]);
                }

                AddMeasureLine(greatStaff, totalwidth);


                foreach (var measure in greatStaff.MeasureList)
                {
                    
                    totalwidth += measure.Width;
                    if (measure == greatStaff.MeasureList.Last<Measure>() && greatStaff == mPc.Sheet.GreatStaffModelList.Last<GreatStaffModel>())
                    {
                        DrawEnd(totalwidth, greatStaff);
                    }
                    else
                    {
                        AddMeasureLine(greatStaff, totalwidth);
                    }


                    foreach (var measureElement in measure.MeasureElements)
                    {
                        if (measureElement.Type.Equals(MeasureElementType.Note))
                        {
                            var note = (Note)measureElement.Element;
                            StaffModel staff;
                            if (note.Staff.Equals(1))
                            {
                                staff = greatStaff.StaffList.First<StaffModel>();

                            }
                            else
                            {
                                staff = greatStaff.StaffList.Last<StaffModel>();
                
                            }
                            Grid staveGrid = staff.stave;
                            GNote(note, note.Pitch, staff, staveGrid, totalwidth, measure.Width);
                            Console.WriteLine("Note drawn");
                        }
                    }
                    Console.WriteLine("Measure finished");
                }

                Console.WriteLine("Great stave finished");
            }
            Console.WriteLine("Notes finished.");
        }

        private void DrawEnd(decimal totalwidth, GreatStaffModel greatStaff)
        {
            AddMeasureLine(greatStaff, 1055 - 10);

            Line endline = Measureline();
            Grid greatstaffgrid = greatStaff.GreatStaffGrid;
            Grid.SetColumn(endline, 1);
            endline.StrokeThickness = 5;
            endline.HorizontalAlignment = HorizontalAlignment.Left;
            Thickness margin = endline.Margin;
            margin.Left = 1055 - 5;
            endline.Margin = margin;

            endline.VerticalAlignment = VerticalAlignment.Stretch;
            greatstaffgrid.Children.Add(endline);
        }

        private void GNote(Note n, Pitch pitch, StaffModel staff, Grid staveGrid, decimal totalwidth, decimal width)
        {
            Ellipse note = n.GetNote();


            Thickness margin = note.Margin;
            margin.Left = n.XPos + (float)totalwidth - (float)width;
            note.Margin = margin;

            int row = CheckRow(pitch, staff.Number);

            //draws small line when needed
            if (row == 1 || row == 13)
            {
                Line line = new Line();
                line.X1 = 0;
                line.X2 = 15 + 7.5;
                line.Stroke = Brushes.Black;
                margin.Left = n.XPos + (float)totalwidth - (float)width - 7.5 / 2;
                line.Margin = margin;
                Grid.SetRow(line, row);
                Grid.SetColumn(line, 1);
                line.HorizontalAlignment = HorizontalAlignment.Left;
                line.VerticalAlignment = VerticalAlignment.Center;
                staveGrid.Children.Add(line);
            }
        
            Grid.SetRow(note, row - 1);
            Grid.SetRowSpan(note, 3);
            Grid.SetColumn(note, 1);
            note.HorizontalAlignment = HorizontalAlignment.Left;
            note.VerticalAlignment = VerticalAlignment.Center;
            staveGrid.Children.Add(note);
        }

        private int CheckRow(Pitch pitch, int staff)
        {
            int row = 1;
            if (staff == 1)
            {
                //if clef is G
                Console.WriteLine("clef is G");
                switch (pitch.Step)
                {
                    case 'A':
                        if (pitch.Octave == 5)
                        {
                            row = 1;
                        }else
                        if (pitch.Octave == 4)
                        {
                            row = 8;
                        }
                        else
                        {
                            row = 8;
                        }
                        break;
                    case 'G':
                        if (pitch.Octave == 5)
                        {
                            row = 2;
                        }else
                        if (pitch.Octave == 4)
                        {
                            row = 9;
                        }
                        else
                        {
                            row = 9;
                        }
                        break;
                    case 'F':
                        if (pitch.Octave == 5)
                        {
                            row = 3;
                        }else
                        if (pitch.Octave == 4)
                        {
                            row = 10;
                        }
                        else
                        {
                            row = 10;
                        }
                        break;
                    case 'E':
                        if (pitch.Octave == 5)
                        {
                            row = 4;
                        }else
                        if (pitch.Octave == 4)
                        {
                            row = 11;
                        }
                        else
                        {
                            row = 11;
                        }
                        break;
                    case 'D':
                        if (pitch.Octave == 5)
                        {
                            row = 5;
                        }else
                        if (pitch.Octave == 4)
                        {
                            row = 12;
                        }
                        else
                        {
                            row = 12;
                        }
                        break;
                    case 'C':
                        if (pitch.Octave == 5)
                        {
                            row = 6;
                        }else
                        if (pitch.Octave == 4)
                        {
                            row = 13;
                        }
                        else
                        {
                            row = 13;
                        }
                        break;
                    case 'B':
                        row = 7;
                        break;

                }
            }
            if (staff == 2)
            {
                //if clef is F
                Console.WriteLine("Clef is f");
                switch (pitch.Step)
                {
                    case 'C':
                        if (pitch.Octave == 4)
                        {
                            row = 1;
                        }else
                        if (pitch.Octave == 3)
                        {
                            row = 8;
                        }
                        else
                        {
                            row = 8;
                        }
                        break;
                    case 'B':
                        if (pitch.Octave == 4)
                        {
                            row = 2;
                        }else
                        if (pitch.Octave == 3)
                        {
                            row = 9;
                        }
                        else
                        {
                            row = 9;
                        }
                        break;
                    case 'A':
                        if (pitch.Octave == 4)
                        {
                            row = 3;
                        }else
                        if (pitch.Octave == 3)
                        {
                            row = 10;
                        }
                        else
                        {
                            row = 10;
                        }
                        break;
                    case 'G':
                        if (pitch.Octave == 3)
                        {
                            row = 4;
                        }else
                        if (pitch.Octave == 2)
                        {
                            row = 11;
                        }
                        else
                        {
                            row = 11;
                        }
                        break;
                    case 'F':
                        if (pitch.Octave == 3)
                        {
                            row = 5;
                        }else
                        if (pitch.Octave == 2)
                        {
                            row = 12;
                        }
                        else
                        {
                            row = 12;
                        }
                        break;
                    case 'E':
                        if (pitch.Octave == 3)
                        {
                            row = 6;
                        }else
                        if (pitch.Octave == 2)
                        {
                            row = 13;
                        }
                        else
                        {
                            row = 13;
                        }
                        break;
                    case 'D':
                        row = 7;
                        break;

                }
            }

            return row;
        }

//        public Ellipse GetNote()
//        {
//            Ellipse note = new Ellipse();
//            note.Fill = Brushes.Black;
//            note.Stroke = Brushes.Black;
//            note.Width = 15;
//            note.Height = note.Width;
//            return note;
//        }

        private void AddMeasureLine(GreatStaffModel greatStaff, decimal totalwidth)
        {
            Line measureline = Measureline();
            Grid greatstaffgrid = greatStaff.GreatStaffGrid;
            Grid.SetColumn(measureline, 1);
            measureline.HorizontalAlignment = HorizontalAlignment.Left;
            Thickness margin = measureline.Margin;
            if (totalwidth > 1055)
            {
                margin.Left = 1050;
            }
            else
            {
                margin.Left = (float)totalwidth;
            }
            measureline.Margin = margin;


            measureline.VerticalAlignment = VerticalAlignment.Stretch;
            greatstaffgrid.Children.Add(measureline);
        }
        
        private Line Measureline()
        {
            Line line = new Line();
            line.Y1 = 23;
            line.Y2 = 177;
            line.Stroke = Brushes.Black;
            return line;
        }
    }
}
