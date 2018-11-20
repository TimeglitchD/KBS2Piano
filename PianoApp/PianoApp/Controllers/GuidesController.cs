using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MusicXml.Domain;

namespace PianoApp.Controllers
{
    public class GuidesController
    {
        public int Bpm { get; set; }
        public NoteType Note { get; set; }

        public PianoController Piano;

        public Score Score;

        public NoteType ChosenNote = NoteType.Quarter;

        private float _divs;
    
        private void CheckNoteIntersect()
        {
            foreach (var scorePart in Score.Parts)
            {
                //Access all measures inside the music piece
                foreach (var scorePartMeasure in scorePart.Measures)
                {

                    if (scorePartMeasure.Attributes != null)
                    {
                        //get the amount of divisions and beats from current part.
                        _divs = scorePartMeasure.Attributes.Divisions;
                    }

                    //Test BPM lentgth tel is 60 sec/ defined bpm
                    float userDefinedBpm = 60;
                    float bpm = 60 / userDefinedBpm;

                    //Access te elements inside a measure
                    foreach (var measureElement in scorePartMeasure.MeasureElements)
                    {
                        //Access the element if it is a note
                        if (measureElement.Type.Equals(MeasureElementType.Note))
                        {
                            var note = (Note)measureElement.Element;
                            if (note.Pitch != null)
                            {

                                //Get the duration of the current note
                                var dur = note.Duration;

                                float timeout = bpm * (dur / _divs);

                                //show the amount of time a key has to be active and a note should be colored
                                Console.WriteLine(timeout);

                                Piano.UpdatePianoKeys();
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

    }

    public enum NoteType
    {
        Quarter,
        Half,
        Whole
    }
}
