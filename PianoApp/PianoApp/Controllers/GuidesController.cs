using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public NoteType ChosenNote = NoteType.Quarter;

        private float _divs;

        private Dictionary<Note, float> _activeNoteAndTimeoutDict = new Dictionary<Note, float>();
        private Stopwatch _stopwatch;

        private System.Threading.Thread guideThread;

        private bool _isPlaying = false;


        private void CheckNoteIntersect()
        {        
            _isPlaying = true;

            //            while (_isPlaying)
            //            {                
            //                
            //            }    

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

                                if (!_activeNoteAndTimeoutDict.ContainsKey(note))
                                {
                                    _activeNoteAndTimeoutDict.Add(note, timeout);
                                }

                                var tempDict = new Dictionary<Note, float>(_activeNoteAndTimeoutDict);

                                foreach (var keyValuePair in _activeNoteAndTimeoutDict)
                                {
                                    //Remove the active note if time is elapsed
                                    if ((keyValuePair.Value * 1000) > _stopwatch.ElapsedMilliseconds)
                                    {
                                        tempDict.Remove(keyValuePair.Key);
                                    }
                                }

                                _activeNoteAndTimeoutDict = new Dictionary<Note, float>(tempDict);

                                Piano.UpdatePianoKeys(_activeNoteAndTimeoutDict);

                                Thread.Sleep((int)(timeout * 1000));
                            }
                        }
                    }
                }
            }
        }

        public bool Start()
        {
            _stopwatch = Stopwatch.StartNew();
            guideThread = new System.Threading.Thread(new System.Threading.ThreadStart(CheckNoteIntersect));
            guideThread.Start();
            return true;
        }

        public bool Stop()
        {
            _isPlaying = false;
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
