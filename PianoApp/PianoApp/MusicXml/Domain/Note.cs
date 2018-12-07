using System;
using PianoApp.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace MusicXml.Domain
{
    public class Note
    {
        internal Note()
        {
            Type = string.Empty;
            Duration = -1;
            Voice = -1;
            Staff = -1;
            IsChordTone = false;
        }

        public bool Duplicate { get; set; } = false;

        public int MeasureNumber { get; set; }

        public float XPos { get; internal set; }

        public string Type { get; internal set; }

        public int Voice { get; internal set; }

        public int Duration { get; internal set; }

        public Lyric Lyric { get; internal set; }

        public Pitch Pitch { get; internal set; }

        public int Staff { get; internal set; }

        public bool IsChordTone { get; internal set; }

        public bool Dot { get; internal set; }

        public bool IsRest { get; internal set; }

        public string Accidental { get; internal set; }

        public int FingerNum { get; set; }

        public bool Active { get; set; } = false;

        public NoteState State { get; set; } = NoteState.Idle;

        public string Stem { get; internal set; }

        public Ellipse ell = new Ellipse();

        public List<object> note = new List<object>();

        public List<object> GetNote()
        {

            if (!IsRest)
            {
                Label notebase = new Label();
                notebase.FontFamily = GreatStaffModel.Notehedz;
                notebase.FontSize = 67;
                notebase.Foreground = Brushes.Black;

                switch (Type)
                {
                    default:
                        //whole
                        notebase.Content = "ä";
                        break;
                    case "half":
                        if (Stem == "up")
                        {
                            notebase.Content = "ã";
                        }
                        else
                        {
                            notebase.Content = "ó";
                        }
                        break;
                    case "quarter":
                        if (Stem == "up")
                        {
                            notebase.Content = "â";
                        }
                        else
                        {
                            notebase.Content = "ò";
                        }
                        break;
                    case "eighth":
                        if (Stem == "up")
                        {
                            if (IsChordTone)
                            {
                                notebase.Content = "â";
                            }
                            else
                            {
                                notebase.Content = "á";
                            }
                        }
                        else
                        {
                            if (IsChordTone)
                            {
                                notebase.Content = "ò";
                            }
                            else
                            {
                                notebase.Content = "ñ";
                            }
                        }
                        break;
                    case "16th":
                        if (Stem == "up")
                        {
                            if (IsChordTone)
                            {
                                notebase.Content = "â";
                            }
                            else
                            {
                                notebase.Content = "à";
                            }
                        }
                        else
                        {
                            if (IsChordTone)
                            {
                                notebase.Content = "ò";
                            }
                            else
                            {
                                notebase.Content = "ð";
                            }
                        }
                        break;
                }
                note.Add(notebase);

            }
            else
            {
                note.Add(GetRest());
            }

            if (Dot)
            {
                Ellipse dot = new Ellipse();
                dot.Width = 5;
                dot.Height = 5;
                dot.Stroke = Brushes.Black;
                dot.Fill = Brushes.Black;
                dot.Name = "dot";
                note.Add(dot);
            }

            Color();

            return note;
        }

        public void setIdle()
        {
            State = NoteState.Idle;
        }

        public void Color()
        {
            foreach (object sh in note)
            {

                if (sh is Label)
                {
                    ColorLabel(sh);
                }
                else
                {
                    ColorShape(sh);
                }


            }
        }

        public Label GetRest()
        {
            Label rest = new Label();
            rest.FontFamily = GreatStaffModel.Metdemo;
            rest.FontSize = 54;
            rest.Foreground = Brushes.Black;

            switch (Type)
            {
                default:
                    //DRAW WHOLE
                    rest.Content = "-";
                    break;
                case "half":
                    //DRAW HALF
                    rest.Content = "-";
                    break;
                case "quarter":
                    //DRAW QUARTER
                    rest.Content = "A";
                    break;
                case "eighth":
                    //DRAW EIGHTH
                    rest.FontFamily = GreatStaffModel.Notehedz;
                    rest.FontSize = 65;
                    rest.Content = "è";
                    break;
                case "16th":
                    //DRAW SIXTEENTH
                    rest.FontFamily = GreatStaffModel.Notehedz;
                    rest.FontSize = 65;
                    rest.Content = "ç";
                    break;
            }

            return rest;
        }

        public void ColorShape(object sh)
        {
            Shape shp = (Shape)sh;

            if (State.Equals(NoteState.Active))
            {
                if (Staff == 1)
                {
                    shp.Fill = Brushes.Aquamarine;
                    shp.Stroke = Brushes.Aquamarine;
                }
                else if (Staff == 2)
                {
                    shp.Fill = Brushes.DarkOrchid;
                    shp.Stroke = Brushes.DarkOrchid;
                }

            }
            else if (State.Equals(NoteState.Wrong))
            {
                shp.Fill = Brushes.Red;
                shp.Stroke = Brushes.Red;
            }
            else if (State.Equals(NoteState.Good))
            {
                shp.Fill = Brushes.Green;
                shp.Stroke = Brushes.Green;
            }
            else
            {
                shp.Fill = Brushes.Black;
                shp.Stroke = Brushes.Black;
            }
        }

        public void ColorLabel(object sh)
        {
            Label shp = (Label)sh;

            if (State.Equals(NoteState.Active))
            {
                if (Staff == 1)
                {
                    shp.Foreground = Brushes.Aquamarine;
                }
                else if (Staff == 2)
                {
                    shp.Foreground = Brushes.DarkOrchid;
                }

            }
            else if (State.Equals(NoteState.Wrong))
            {
                shp.Foreground = Brushes.Red;
            }
            else if (State.Equals(NoteState.Good))
            {
                shp.Foreground = Brushes.Green;
            }
            else
            {
                shp.Foreground = Brushes.Black;
            }

        }


        public enum NoteState
        {
            Active,
            Idle,
            Wrong,
            Good
        }
    }
}
