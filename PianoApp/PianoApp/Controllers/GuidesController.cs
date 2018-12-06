using MusicXml.Domain;
using PianoApp.Models.Exception;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using static MusicXml.Domain.Note;

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
                if (value < 1 || value > 300) throw new BpmOutOfRangeException($"Bpm waarde ligt niet tussen de 1 en de 300 ({value})");

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
        private float _definedBpm = 180;
        private int _interval;

        private Dictionary<Note, Timeout> _activeNoteAndTimeoutDict = new Dictionary<Note, Timeout>();
        private Dictionary<Note, float> _toDoNoteDict = new Dictionary<Note, float>();
        private Stopwatch _stopwatch;

        private System.Timers.Timer _timerStaffOne;
        private System.Timers.Timer _timerStaffTwo;

        private bool _isPlaying = false;
        public bool paused = false;

        private System.Timers.Timer _timer;

        private List<PianoApp.Models.StaffModel> stafflist;
        private int currentStaff = 0;
        private bool atStaffEndOne = false;
        private bool atStaffEndTwo = false;
        public event EventHandler staffEndReached;
        public event EventHandler GoToFirstStaff;
        public event EventHandler HoldPosition;
        public event EventHandler guideStopped;

        public MidiController midi;

        public List<int> ActiveKeys;

        public GuidesController(MidiController midi)
        {
            this.midi = midi;
//            midi.midiInputChanged += inputChanged;
        }

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
            _timing = _bpm / 60;

            if(Note == NoteType.Half)
            {
                _timing = _timing / 2;
            }else if(Note == NoteType.Whole)
            {
                _timing = _timing / 4;
            }

            _interval = (int)(1000.0 / (_timing));

            //Set the divisions
            foreach (var scorePart in Score.Parts)
            {
                //Access all measures inside the music piece
                foreach (var scorePartMeasure in scorePart.Measures)
                {
                    if (scorePartMeasure.Attributes != null)
                    {
                        //get the amount of divisions and beats from current part.
                        _divs = scorePartMeasure.Attributes.Divisions;
                        break;
                    }
                }
            }

            stafflist = Sheet.SheetModel.GreatStaffModelList[currentStaff].StaffList;
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

        private void NoteIntersectEvent(object source, EventArgs e, int staffNumber)
        {
            var tempList = _toDoNoteDict.ToList();

            for (int i = 0; i < 1; i++)
            {
                //Add note to active Dictionary
                if (!_activeNoteAndTimeoutDict.ContainsKey(tempList[i].Key))
                    _activeNoteAndTimeoutDict.Add(tempList[i].Key, new Timeout()
                    {
                        NoteTimeout = tempList[i].Value,
                        TimeAdded = _stopwatch.ElapsedMilliseconds
                    });

                //Remove the note from to do 
                RemoveFirstNoteFromToDoDict(tempList[i].Key);

                checkLastNote(_activeNoteAndTimeoutDict);


                for (int j = i + 1; j < tempList.Count; j++)
                {
                    if (tempList[i].Key != tempList[j].Key &&
                        tempList[j].Key.XPos > tempList[i].Key.XPos - 1 &&
                        tempList[j].Key.XPos < tempList[i].Key.XPos + 1 &&
                        tempList[i].Key.MeasureNumber == tempList[j].Key.MeasureNumber)
                    {
                        //Add note with same pos to active Dictionary
                        if(!_activeNoteAndTimeoutDict.ContainsKey(tempList[j].Key))
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
            goToNextStaff();

            Dictionary<Note, Timeout> tempActiveNoteDict = new Dictionary<Note, Timeout>(_activeNoteAndTimeoutDict);

            //Remove keys that are dones
            foreach (var keyValuePair in _activeNoteAndTimeoutDict.Where(t => _stopwatch.ElapsedMilliseconds >= (t.Value.NoteTimeout * 1000) + t.Value.TimeAdded))
            {
                tempActiveNoteDict.Remove(keyValuePair.Key);
                keyValuePair.Key.State = NoteState.Wrong;
            }

            //            _activeNoteAndTimeoutDict = tempActiveNoteDict;
            //tempActiveNoteDict.
            Piano.UpdatePianoKeys(tempActiveNoteDict);
            Sheet.UpdateNotes(tempActiveNoteDict);
            if(HoldPosition != null)
            {
                HoldPosition(this, EventArgs.Empty);
            }
        }


        private void checkLastNote(Dictionary<Note, Timeout> noteDict)
        {
            Note lastNoteStaffOne = stafflist[0].NoteList.Last();
            Note lastNoteStaffTwo = stafflist[1].NoteList.Last();
            Timeout temp = new Timeout();
            if(noteDict.TryGetValue(lastNoteStaffOne, out temp))
            {
                atStaffEndOne = true;
            }

            if(noteDict.TryGetValue(lastNoteStaffTwo, out temp))
            {
                atStaffEndTwo = true;
            }
        }

        private void goToNextStaff()
        {
            if(atStaffEndOne && atStaffEndTwo)
            {
                atStaffEndOne = false;
                atStaffEndTwo = false;

                currentStaff++;

                if (currentStaff >= Sheet.SheetModel.GreatStaffModelList.Count)
                {
                    this.Stop();
                    return;
                }


                stafflist = Sheet.SheetModel.GreatStaffModelList[currentStaff].StaffList;

                if (staffEndReached != null)
                {
                    staffEndReached(this, EventArgs.Empty);
                }
            }
        }

        public void Pause()
        {
            if (paused)
            {
                _timerStaffOne.Enabled = true;
            }
            else
            {
                _timerStaffOne.Enabled = false;
            }
           
            paused = !paused;
        }

        public bool Start()
        {            
            ////Set the attributes from the current music piece
            SetAttributes();

            //Fill the to do list with notes from the current music     
            FillToDoList();

            _stopwatch = Stopwatch.StartNew();

            _timerStaffOne = new System.Timers.Timer();
            _timerStaffOne.Elapsed += (sender, e) => NoteIntersectEvent(sender, e, 1);
            _timerStaffOne.Interval = _interval;
            NoteIntersectEvent(this, EventArgs.Empty, 1);
            _timerStaffOne.Enabled = true;
            return true;
        }

        public void ResetMusicPiece()
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
                            note.setIdle();
                            note.ell.Dispatcher.BeginInvoke((Action)(() => note.Color()));
                        }
                    }
                }
            }

            foreach (var octaveModel in Piano.PianoModel.OctaveModelList)
            {
                foreach (var keyModel in octaveModel.KeyModelList)
                {
                    keyModel.Active = false;
                    keyModel.FingerNum = 0;
                    keyModel.KeyRect.Dispatcher.BeginInvoke((Action)(() => keyModel.Color()));
                }
            }
            if (GoToFirstStaff != null)
            {
                GoToFirstStaff(this, EventArgs.Empty);
            }

            _stopwatch.Stop();
            _timerStaffOne.Stop();
            _toDoNoteDict.Clear();
            _activeNoteAndTimeoutDict.Clear();
            SetAttributes();
        }

        public bool Stop()
        {
            _timerStaffOne.Enabled = false;
            onGuideStopped();
            return true;
        }

        public void onGuideStopped()
        {
            if (guideStopped != null)
                guideStopped(this, EventArgs.Empty);
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

        public void UpdatePianoKeys(List<int> activeKeys)
        {
            Piano.UpdatePressedPianoKeys(activeKeys);
        }
    }

    public enum NoteType
    {
        Quarter,
        Half,
        Whole
    }
}