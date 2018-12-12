using MusicXml.Domain;
using PianoApp.Models.Exception;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using static MusicXml.Domain.Note;


namespace PianoApp.Controllers
{

    public struct Timeout
    {
        public float NoteTimeout { get; set; }
        public float TimeAdded { get; set; }
    }

    public struct MockupNote
    {
        public int Octave;
        public char Step;
        public int Alter;
    }

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

        public RecordController record;

        public Score Score;

        public NoteType ChosenNote = NoteType.Quarter;

        private float _divs;
        private float _bpsecond;
        private float _definedBpm = 180;
        private int _milsecperbeat;

        private Dictionary<Note, Timeout> _activeNoteAndTimeoutDict = new Dictionary<Note, Timeout>();
        private Dictionary<int, Timeout> _pressedKeyAndTimeDict = new Dictionary<int, Timeout>();
        private Dictionary<Note, float> _toDoNoteDict = new Dictionary<Note, float>();
        public static Stopwatch StopWatch = new Stopwatch();

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

        public Dictionary<int, float> ActiveKeys = new Dictionary<int, float>();

        public bool AtEnd = false;

        public GuidesController(MidiController midi)
        {
            this.midi = midi;
//            midi.midiInputChanged += inputChanged;
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

                            var timeout = _bpsecond * (dur / _divs);

                            Console.WriteLine(timeout +"SECS");

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
            _bpsecond = _bpm / 60;

            if(Note == NoteType.Half)
            {
                _bpsecond = _bpsecond / 2;
            }else if(Note == NoteType.Whole)
            {
                _bpsecond = _bpsecond / 4;
            }

            _milsecperbeat = (int)(1000.0 / (_bpsecond));

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
                try
                {
                    if (!_activeNoteAndTimeoutDict.ContainsKey(tempList[i].Key))
                        _activeNoteAndTimeoutDict.Add(tempList[i].Key, new Timeout()
                        {
                            NoteTimeout = tempList[i].Value,
                            TimeAdded = StopWatch.ElapsedMilliseconds
                        });
                }
                catch (Exception)
                {
                    //
                }


                //Remove the note from to do 
                try
                {
                    RemoveFirstNoteFromToDoDict(tempList[i].Key);
                }
                catch (Exception)
                {
    
                }

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
                            TimeAdded = StopWatch.ElapsedMilliseconds
                        });
                        //Remove the note with same pos from to do 
                        RemoveFirstNoteFromToDoDict(tempList[j].Key);
                    }
                }               
            }
            goToNextStaff();

            Dictionary<Note, Timeout> tempActiveNoteDict = new Dictionary<Note, Timeout>(_activeNoteAndTimeoutDict);

            //Check key press is correct or not
            CheckPressedKeysToActiveNotes();

            //Remove keys that are dones
            foreach (var keyValuePair in _activeNoteAndTimeoutDict.Where(t => StopWatch.ElapsedMilliseconds >= (t.Value.NoteTimeout * 1000) + t.Value.TimeAdded))
            {
                tempActiveNoteDict.Remove(keyValuePair.Key);
                if (keyValuePair.Key.State != NoteState.Good)
                {
                    keyValuePair.Key.State = NoteState.Wrong;
                }
            }


            _activeNoteAndTimeoutDict = tempActiveNoteDict;

            Piano.UpdatePianoKeys(_activeNoteAndTimeoutDict);
            Sheet.UpdateNotes(_activeNoteAndTimeoutDict);

            if(HoldPosition != null)
            {
                HoldPosition(this, EventArgs.Empty);
            }
        }



        private MockupNote getNoteFromNoteNumber(int nn)
        {
            int octave = (int)Math.Floor((decimal)nn / 12);
            int noteNumber = (int)Math.Floor((decimal)nn - ((12 * octave) - 1));
            char step = '*';
            int alter = 0;

//            Console.WriteLine(noteNumber);

            switch (noteNumber)
            {
                case (1): step = 'C'; alter = 0; break;
                case (2): step = 'C'; alter = 1; break;
                case (3): step = 'D'; alter = 0; break;
                case (4): step = 'D'; alter = 1; break;
                case (5): step = 'E'; alter = 0; break;
                case (6): step = 'F'; alter = 0; break;
                case (7): step = 'F'; alter = 1; break;
                case (8): step = 'G'; alter = 0; break;
                case (9): step = 'G'; alter = 1; break;
                case (10): step = 'A'; alter = 0; break;
                case (11): step = 'A'; alter = 1; break;
                case (12): step = 'B'; alter = 0; break;
            }

            return new MockupNote(){Step = step, Alter = alter, Octave = octave};
        }


        private void CheckPressedKeysToActiveNotes()
        {
            foreach (var activeNote in _activeNoteAndTimeoutDict.Where(n => n.Key.State == NoteState.Active))
            {
                if (ActiveKeys.Count > 0)
                {
                    foreach (var activeKey in ActiveKeys)
                    {
                        var keyTimeAdded = activeKey.Value;
                        var noteTimeAdded = activeNote.Value.TimeAdded;
                        var noteTime = activeNote.Value.NoteTimeout;

                        var activeKeynote = getNoteFromNoteNumber(activeKey.Key);

                        Console.WriteLine($"note: {activeNote.Key.Pitch.Step} key: {activeKeynote.Step}");

                        if (activeNote.Key.Pitch.Step == activeKeynote.Step &&
                            activeNote.Key.Pitch.Alter == activeKeynote.Alter &&
                            activeNote.Key.Pitch.Octave == activeKeynote.Octave &&
                            keyTimeAdded >= noteTimeAdded - (noteTimeAdded * .05) &&
                            keyTimeAdded <= (noteTimeAdded + (noteTimeAdded * .05)) + (noteTime * 1000))
                        {
                            //if the pressed keys time is in between note marge
                            activeNote.Key.State = NoteState.Good;
                        }                        
                    }
                }
                else
                {
                    activeNote.Key.State = NoteState.Wrong;
                }               
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
                record.startRecording();
            }
            else
            {
                _timerStaffOne.Enabled = false;
                record.pauseRecording();
            }
           
            paused = !paused;
        }

        public bool Start()
        {            
            ////Set the attributes from the current music piece
            SetAttributes();

            //Fill the to do list with notes from the current music     
            FillToDoList();

            StopWatch = Stopwatch.StartNew();

            _timerStaffOne = new System.Timers.Timer();
            _timerStaffOne.Elapsed += (sender, e) => NoteIntersectEvent(sender, e, 1);
            _timerStaffOne.Interval = _milsecperbeat;
            NoteIntersectEvent(this, EventArgs.Empty, 1);
            _timerStaffOne.Enabled = true;
            record.startRecording();
            return true;
        }

        public void ResetMusicPiece()
        {
            currentStaff = 0;

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

            StopWatch.Stop();


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

            StopWatch.Stop();
            
            _timerStaffOne.Stop();
            record.stopRecording();
            _toDoNoteDict.Clear();
            _activeNoteAndTimeoutDict.Clear();
            SetAttributes();
        }

        public bool Stop()
        {
            _timerStaffOne.Enabled = false;
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

        public void UpdatePianoKeys()
        {
            Piano.UpdatePressedPianoKeys(ActiveKeys);
        }
    }

    public enum NoteType
    {
        Quarter,
        Half,
        Whole
    }
}