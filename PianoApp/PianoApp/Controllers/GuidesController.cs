using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MusicXml.Domain;

namespace PianoApp.Controllers
{
    public class GuidesController
    {
        public int _bpm;

        public int Bpm { get; set; }
        public NoteType Note { get; set; }

        public PianoController Piano;
        public Score Score;

        private void CheckNoteIntersect()
        {
            foreach (var scorePart in Score.Parts)
            {
                //Access all measures inside the music piece
                foreach (var scorePartMeasure in scorePart.Measures)
                {
                    //Access te elements inside a measure
                    foreach (var measureElement in scorePartMeasure.MeasureElements)
                    {
                        //Access the element if it is a note
                        if (measureElement.Type.Equals(MeasureElementType.Note))
                        {
                            var note = (Note)measureElement.Element;
                            if (note.Pitch != null)
                            {
                                foreach (var keyModel in Piano.PianoModel.OctaveModelList[note.Pitch.Octave].KeyModelList)
                                {
                                    if (note.Pitch.Step.ToString() == keyModel.Step.ToString() && note.Pitch.Alter == keyModel.Alter)
                                    {
                                        Console.WriteLine($"Note {note.Pitch.Step}{note.Pitch.Octave}{note.Pitch.Alter} key pressed: {keyModel.Step}{Piano.PianoModel.OctaveModelList[note.Pitch.Octave].Position}{keyModel.Alter}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool Start()
        {
            CheckNoteIntersect();
            return true;
        }

        public void SetNote(string note)
        {
            if (note == "Hele noot")
            {
                Note = NoteType.Whole;
            }
            else if (note == "Halve noot")
            {
                Note = NoteType.Half;
            }
            else
            {
                Note = NoteType.Quarter;
            }
        }

    }

    public enum NoteType
    {
        Quarter,
        Half,
        Whole
    }
}
