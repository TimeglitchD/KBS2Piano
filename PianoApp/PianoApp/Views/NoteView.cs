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

        private MusicPieceController mPc;
        private decimal scale;
        private GreatStaffModel stave;

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
                stave = greatStaff;
                totalwidth = 0;
                if(greatStaff.MeasureList.Count() == 0)
                {
                    DrawEnd(mPc.Sheet.GreatStaffModelList[idx]);
                }

                AddMeasureLine(greatStaff, totalwidth);

                foreach (var measure in greatStaff.MeasureList)
                {
                    scale += measure.Width;
                }
                if (scale != 0)
                {
                    scale = 1050 / scale;
                }

                foreach (var measure in greatStaff.MeasureList)
                {


                    totalwidth += measure.Width * scale;

                    //make a list with all the notes
                    List<Note> notelist = new List<Note>();
                    foreach (var measureel in measure.MeasureElements)
                    {
                        if (measureel.Type.Equals(MeasureElementType.Note))
                        {
                            Note noot = (Note)measureel.Element;
                            notelist.Add(noot);
                        }
                    }
                    //get the first note
                    Note prevnote = notelist.First();

                    foreach (var note in notelist)
                    {
                        StaffModel staff;
                        //check whether the note belongs to the upper staff or not
                        if (note.Staff.Equals(1))
                        {
                            staff = greatStaff.StaffList.First<StaffModel>();
                        }
                        else
                        {
                            staff = greatStaff.StaffList.Last<StaffModel>();
                        }
                        Grid staveGrid = staff.stave;
                        GNote(note, note.Pitch, staff, staveGrid, totalwidth, measure.Width * scale, prevnote);
                        prevnote = note;

                    }



                    //check if this is the last measure of the piece
                    if (measure == greatStaff.MeasureList.Last<Measure>() && greatStaff == mPc.Sheet.GreatStaffModelList.Last<GreatStaffModel>())
                    {
                        //draw the end if so
                        DrawEnd(greatStaff);
                    }
                    else
                    {
                        //draw a measureline if not
                        if (measure != greatStaff.MeasureList.Last<Measure>())
                        {
                            AddMeasureLine(greatStaff, totalwidth);
                        }
                        else
                        {
                            AddMeasureLine(greatStaff, 1050);
                        }
                    }
                    Console.WriteLine("Measure finished");
                }

                Console.WriteLine("Great stave finished");
            }
            Console.WriteLine("Notes finished.");
        }


        private void DrawEnd(GreatStaffModel greatStaff)
        {
            AddMeasureLine(greatStaff, 1055 - 10);

            Line endline = GetSymbolMeasureline();
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

        //decide if it's a rest or a note and draw it
        private void GNote(Note n, Pitch pitch, StaffModel staff, Grid staveGrid, decimal totalwidth, decimal width, Note prevnote)
        {

            float pos = prevnote.XPos;
            if (n.IsRest)
            {
                List<object> restlist = n.GetNote();

                if (prevnote != n)
                {
                    if (!prevnote.IsRest)
                    {
                        pos = pos * (float)scale + 20;
                    }
                    else
                    {
                        pos += 20;
                    }

                    if (prevnote.Dot)
                    {
                        pos += 10;
                    }
                }
                else
                {
                    pos = 0;
                }


                //if we've entered a new staff, set the position back to the beginning
                if (prevnote.Staff != n.Staff)
                {
                    if (n.MeasureNumber == stave.MeasureList.First<Measure>().Number)
                    {
                        pos = 70;
                    }
                    else
                    {
                        pos = 0;
                    }
                }


                foreach (var item in restlist)
                {
                    try
                    {
                        Label rest = (Label)item;
                        Thickness margin = rest.Margin;
                        switch (n.Type)
                        {
                            //default is whole
                            default:
                                Grid.SetRow(rest, 1);
                                margin.Left = (float)totalwidth - (float)width / 2;
                                margin.Top = -5;
                                break;
                            case "half":
                                Grid.SetRow(rest, 1);
                                margin.Left = pos + (float)totalwidth - (float)width;
                                margin.Top = -15;
                                break;
                            case "quarter":
                                Grid.SetRow(rest, 1);
                                margin.Left = pos + (float)totalwidth - (float)width;
                                break;
                            case "eighth":
                                Grid.SetRow(rest, 1);
                                margin.Left = pos + (float)totalwidth - (float)width;
                                margin.Top = 5;
                                break;
                            case "16th":
                                Grid.SetRow(rest, 1);
                                margin.Left = pos + (float)totalwidth - (float)width;
                                margin.Top = 5;
                                break;
                        }
                        n.XPos = pos;
                        Console.WriteLine("POSITION: " + n.XPos);
                        margin.Top -= 15;
                        rest.Margin = margin;

                        Grid.SetRowSpan(rest, 20);
                        Grid.SetColumn(rest, 1);
                        rest.HorizontalAlignment = HorizontalAlignment.Left;
                        rest.VerticalAlignment = VerticalAlignment.Center;
                        staveGrid.Children.Add(rest);
                    }
                    catch
                    {
                        Shape rest = (Shape)item;
                        Thickness margin = rest.Margin;
                        //draw the dot in the correct position
                        margin.Left = pos + (float)totalwidth - (float)width + 30;
                        rest.Margin = margin;
                        Grid.SetRow(rest, 6);
                        Grid.SetColumn(rest, 1);
                        rest.HorizontalAlignment = HorizontalAlignment.Left;
                        rest.VerticalAlignment = VerticalAlignment.Center;
                        staveGrid.Children.Add(rest);
                    }

                }
            }
            else
            {
                List<object> note = n.GetNote();
                foreach (object obj in note)
                {
                    int row = CheckRow(pitch, staff.Number);

                    try
                    {
                        Label lb = (Label)obj;

                        Thickness margin = lb.Margin;
                        margin.Left = n.XPos * (float)scale + (float)totalwidth - (float)width - 5;
                        margin.Top -= 22;

                        switch (n.Stem)
                        {
                            default:
                                margin.Top -= 0;
                                break;
                            case "up":
                                margin.Top -= 43;
                                break;
                            case "down":
                                //STARTINGPOINT
                                break;
                        }

                        switch (n.Type)
                        {
                            default:
                                margin.Top -= 16;
                                break;
                            case "half":
                                break;
                            case "quarter":
                                break;
                            case "eighth":
                                //STARTINGPOINT
                                break;
                            case "16th":
                                break;
                        }
                        lb.Margin = margin;
                        margin.Top = 0;


                        if (row != 0)
                        {

                            //draws small line when needed
                            if (row == 1 || row == 13)
                            {
                                Line line = new Line();
                                line.X1 = 0;
                                line.X2 = 15 + 7.5;
                                line.Stroke = Brushes.Black;
                                margin.Left = n.XPos * (float)scale + (float)totalwidth - (float)width - 7.5 / 2;
                                line.Margin = margin;
                                Grid.SetRow(line, row);
                                Grid.SetColumn(line, 1);
                                line.HorizontalAlignment = HorizontalAlignment.Left;
                                line.VerticalAlignment = VerticalAlignment.Center;
                                staveGrid.Children.Add(line);
                            }

                            Grid.SetRowSpan(lb, 20);
                            Grid.SetRow(lb, row);

                            Grid.SetColumn(lb, 1);
                            lb.HorizontalAlignment = HorizontalAlignment.Left;
                            lb.VerticalAlignment = VerticalAlignment.Top;
                            staveGrid.Children.Add(lb);
                        }
                    }
                    catch (System.InvalidCastException)
                    {
                        Shape sh = (Shape)obj;

                        Thickness margin = sh.Margin;
                        margin.Left = n.XPos * (float)scale + (float)totalwidth - (float)width;
                        sh.Margin = margin;

                        //draw the dot in the correct position
                        margin.Left += 20;
                        sh.Margin = margin;
                        if (row > 2)
                        {
                            if (row % 2 == 0)
                            {
                                Grid.SetRow(sh, row);
                            }
                            else
                            {
                                Grid.SetRow(sh, row - 1);
                            }
                        }
                        else
                        {
                            Grid.SetRow(sh, row);
                        }
                        Grid.SetColumn(sh, 1);
                        sh.HorizontalAlignment = HorizontalAlignment.Left;
                        sh.VerticalAlignment = VerticalAlignment.Center;
                        staveGrid.Children.Add(sh);
                    }
                }
            }
        }

        private int CheckRow(Pitch pitch, int staff)
        {
            int row = 1;
            if (staff == 1)
            {
                //if clef is G
                switch (pitch.Step)
                {
                    default:
                        row = 0;
                        break;
                    case 'B':
                        if (pitch.Octave == 4)
                        {
                            row = 7;
                        }
                        else
                        {
                            if (pitch.Octave > 4)
                            {
                                row = 7;
                                //Draw 8va
                            }
                            else
                            {
                                row = 7;
                                //Draw 8vb
                            }
                        }
                        break;
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
                            if (pitch.Octave > 5)
                            {
                                row = 1;
                                //Draw 8va
                            }
                            else
                            {
                                row = 8;
                                //Draw 8vb
                            }
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
                            if (pitch.Octave > 5)
                            {
                                row = 2;
                                //Draw 8va
                            }
                            else
                            {
                                row = 9;
                                //Draw 8vb
                            }
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
                            if (pitch.Octave > 5)
                            {
                                row = 3;
                                //Draw 8va
                            }
                            else
                            {
                                row = 10;
                                //Draw 8vb
                            }
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
                            if (pitch.Octave > 5)
                            {
                                row = 4;
                                //Draw 8va
                            }
                            else
                            {
                                row = 11;
                                //Draw 8vb
                            }
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
                            if (pitch.Octave > 5)
                            {
                                row = 5;
                                //Draw 8va
                            }
                            else
                            {
                                row = 12;
                                //Draw 8vb
                            }
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
                            if (pitch.Octave > 5)
                            {
                                row = 6;
                                //Draw 8va
                            }
                            else
                            {
                                row = 13;
                                //Draw 8vb
                            }
                        }
                        break;


                }
            }
            if (staff == 2)
            {
                //if clef is F
                switch (pitch.Step)
                {
                    default:
                        row = 0;
                        break;
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
                            if (pitch.Octave > 4)
                            {
                                row = 1;
                                //Draw 8va
                            }
                            else
                            {
                                row = 8;
                                //Draw 8vb
                            }
                        }
                        break;
                    case 'B':
                        if (pitch.Octave == 3)
                        {
                            row = 2;
                        }
                        else
                        if (pitch.Octave == 2)
                        {
                            row = 9;
                        }
                        else
                        {
                            if (pitch.Octave > 3)
                            {
                                row = 2;
                                //Draw 8va
                            }
                            else
                            {
                                row = 9;
                                //Draw 8vb
                            }
                        }
                        break;
                    case 'A':
                        if (pitch.Octave == 3)
                        {
                            row = 3;
                        }
                        else
                        if (pitch.Octave == 2)
                        {
                            row = 10;
                        }
                        else
                        {
                            if (pitch.Octave > 3)
                            {
                                row = 3;
                                //Draw 8va
                            }
                            else
                            {
                                row = 10;
                                //Draw 8vb
                            }
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
                            if (pitch.Octave > 3)
                            {
                                row = 4;
                                //Draw 8va
                            }
                            else
                            {
                                row = 11;
                                //Draw 8vb
                            }
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
                            if (pitch.Octave > 3)
                            {
                                row = 5;
                                //Draw 8va
                            }
                            else
                            {
                                row = 12;
                                //Draw 8vb
                            }
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
                            if (pitch.Octave > 3)
                            {
                                row = 6;
                                //Draw 8va
                            }
                            else
                            {
                                row = 13;
                                //Draw 8vb
                            }
                        }
                        break;
                    case 'D':
                        if (pitch.Octave == 3)
                        {
                            row = 7;
                        }
                        else
                        {
                            if (pitch.Octave > 4)
                            {
                                row = 7;
                                //Draw 8va
                            }
                            else
                            {
                                row = 7;
                                //Draw 8vb
                            }
                        }
                        break;

                }
            }

            return row;
        }

        private void AddMeasureLine(GreatStaffModel greatStaff, decimal totalwidth)
        {
            Line measureline = GetSymbolMeasureline();
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

        //every drawn symbol

        private Line GetSymbolMeasureline()
        {
            Line line = new Line();
            line.Y1 = 23;
            line.Y2 = 177;
            line.Stroke = Brushes.Black;
            return line;
        }


    }
}
