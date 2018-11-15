using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MusicXml.Domain;

namespace PianoApp.Controllers
{
    class GuidesController
    {
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
//                            Console.WriteLine($"Note: {note.Type}");
//                            Piano.UpdatePianoKeys();
                            //TODO: color piano and notes here... In a loop or something...
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

    }

    public enum NoteType
    {
        Quarter,
        Half,
        Whole
    }
}
