using MusicXml.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows;
using MusicXml.Domain;
using PianoApp.Models.Exception;
using Timer = System.Threading.Timer;

namespace PianoApp.Controllers
{
    public class GuidesController
    {
        public float _bpm;

        public float Bpm
        {
            get
            {
                return _bpm;
            }
            set
            {
                if (value < 0 || value > 300) throw new BpmOutOfRangeException($"Bpm waarde ligt niet tussen de 0 en de 300 ({value})");

                _bpm = value;
            }
        }

        public NoteType Note { get; set; }

        public PianoController Piano;
        public SheetController Sheet;

        public Score Score;

        public NoteType ChosenNote = NoteType.Quarter;

        private float _divs;
        private float _timing;
        private System.Timers.Timer _timerStaffOne;
        private System.Timers.Timer _timerStaffTwo;

        private Dictionary<Note, Timeout> _activeNoteAndTimeoutDict = new Dictionary<Note, Timeout>();
        private Dictionary<Note, float> _toDoNoteDict = new Dictionary<Note, float>();
        private Stopwatch _stopwatch;
      
        public bool _isPlaying = true;

        private System.Timers.Timer _timer;
        private int _interval;

        public struct Timeout
        {
            public float NoteTimeout { get; set; }
            public float TimeAdded { get; set; }
        }

        private void FillToDoList()
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

                            if (note.Pitch == null) continue;
                            //Get the duration of the current note
                            var dur = note.Duration;

                            var timeout = _timing * (dur / _divs);

                            if (!_toDoNoteDict.ContainsKey(note))
                                _toDoNoteDict.Add(note, timeout);
                        }
                    }
                }
            }
        }

        private void SetAttributes()
        {
            //set timing of music piece
            _timing = 60 / _bpm;

            _divs = 2;

            _interval = (int)(1000.0 / (_bpm / 60.0));

            //            //Set the divisions
            //            foreach (var scorePart in Score.Parts)
            //            {
            //                //Access all measures inside the music piece
            //                foreach (var scorePartMeasure in scorePart.Measures)
            //                {
            //
            //                    if (scorePartMeasure.Attributes != null)
            //                    {
            //                        //get the amount of divisions and beats from current part.
            //                        _divs = scorePartMeasure.Attributes.Divisions;
            //                    }
            //                }
            //            }
        }

        private int ReturnFirstNoteTimeout(int staffNumber)
        {
            int timeout = (int)(_toDoNoteDict.First(n => n.Key.Staff == staffNumber && !n.Key.Duplicate).Value * 1000);
            return timeout;
        }

        private void RemoveFirstNoteFromToDoDict(Note note)
        {
            if (_toDoNoteDict.Count > 0) _toDoNoteDict.Remove(_toDoNoteDict.Keys.First(t => t.Equals(note)));
        }

        private void NoteIntersectEvent(object source, ElapsedEventArgs e, int staffNumber)
        {
            try
            {
                var tempList = _toDoNoteDict.ToList();

                for (int i = 0; i < 1; i++)
                {
                    //Add note to active Dictionary
                    _activeNoteAndTimeoutDict.Add(tempList[i].Key, new Timeout()
                    {
                        NoteTimeout = tempList[i].Value,
                        TimeAdded = _stopwatch.ElapsedMilliseconds
                    });
                    //Remove the note from to do 
                    RemoveFirstNoteFromToDoDict(tempList[i].Key);

                    for (int j = i + 1; j < tempList.Count; j++)
                    {
                        if (tempList[i].Key != tempList[j].Key &&
                            tempList[i].Key.XPos == tempList[j].Key.XPos &&
                            tempList[i].Key.MeasureNumber == tempList[j].Key.MeasureNumber)
                        {
                            //Add note with same pos to active Dictionary
                            _activeNoteAndTimeoutDict.Add(tempList[j].Key, new Timeout()
                            {
                                NoteTimeout = tempList[j].Value,
                                TimeAdded = _stopwatch.ElapsedMilliseconds
                            });
                            //Remove the note with same pos from to do 
                            RemoveFirstNoteFromToDoDict(tempList[j].Key);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            Dictionary<Note, Timeout> tempActiveNoteDict = new Dictionary<Note, Timeout>(_activeNoteAndTimeoutDict);

            //Remove keys that are done
            foreach (var keyValuePair in _activeNoteAndTimeoutDict.Where(t => _stopwatch.ElapsedMilliseconds >= (t.Value.NoteTimeout * 1000) + t.Value.TimeAdded))
            {
                tempActiveNoteDict.Remove(keyValuePair.Key);
            }

            _activeNoteAndTimeoutDict = new Dictionary<Note, Timeout>(tempActiveNoteDict);

            Piano.UpdatePianoKeys(_activeNoteAndTimeoutDict);
            Sheet.UpdateNotes(_activeNoteAndTimeoutDict);
        }

        public bool Start()
        {
            //Set the attributes from the current music piece
            SetAttributes();
            //Fill the to do list with notes from the current music piece
            FillToDoList();

            _stopwatch = Stopwatch.StartNew();

            _timerStaffOne = new System.Timers.Timer();
            _timerStaffOne.Elapsed += (sender, e) => NoteIntersectEvent(sender, e, 1);
            _timerStaffOne.Interval = _interval;

            _timerStaffTwo = new System.Timers.Timer();
            _timerStaffTwo.Elapsed += (sender, e) => NoteIntersectEvent(sender, e, 2);
            _timerStaffTwo.Interval = (ReturnFirstNoteTimeout(2));

            _timerStaffOne.Enabled = true;
            _timerStaffTwo.Enabled = true;

            return true;
        }

        public bool Stop()
        {
            _timerStaffOne.Enabled = false;
            _timerStaffTwo.Enabled = false;
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