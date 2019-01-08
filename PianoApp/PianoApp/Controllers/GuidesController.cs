using MusicXml.Domain;
using PianoApp.Models.Exception;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PianoApp.Models;
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
        private int _milsecperbeat;

        private Dictionary<Note, Timeout> _activeNoteAndTimeoutDict1 = new Dictionary<Note, Timeout>();
        private Dictionary<Note, Timeout> _activeNoteAndTimeoutDict2 = new Dictionary<Note, Timeout>();
        private Dictionary<int, Timeout> _pressedKeyAndTimeDict = new Dictionary<int, Timeout>();
        private Dictionary<Note, float> _toDoNoteDict1 = new Dictionary<Note, float>();
        private Dictionary<Note, float> _toDoNoteDict2 = new Dictionary<Note, float>();
        public static Stopwatch StopWatch = new Stopwatch();

        private System.Timers.Timer _timerStaffOne;
        private System.Timers.Timer _timerStaffTwo;

        public bool paused = false;


        private List<PianoApp.Models.StaffModel> stafflist;
        private int currentStaff = 0;
        private bool atStaffEndOne = false;
        private bool atStaffEndTwo = false;
        public event EventHandler StaffEndReached;
        public event EventHandler GoToFirstStaff;
        public event EventHandler HoldPosition;
        public event EventHandler guideStopped;

        public MidiController midi;

        public Dictionary<int, float> ActiveKeys = new Dictionary<int, float>();

        public bool AtEnd = false;
        private int[] staffdivs;
        private List<Note>[] prevnote;
        private List<Note> newlistprevnote;

        private int _scoreVal = 0;
        public int _amountOfNotes = 0;
        public int _goodNotes = 0;
        private int _endReached = 0;
        private int _amountOfGreatStaffs = 0;
        public Grid grid;

        public bool _finished { get; set; }

        public GuidesController(MidiController midi)
        {
            this.midi = midi;
            //            midi.midiInputChanged += inputChanged;
        }

        private void FillToDoList()
        {
            _amountOfNotes = 0;
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
                            _amountOfNotes++;
                            var note = (Note)measureElement.Element;

                            if (note.Pitch == null) continue;
                            //Get the duration of the current note
                            var dur = note.Duration;

                            var timeout = _bpsecond * (dur / _divs);

                            if (note.Staff == 1)
                            {
                                if (!_toDoNoteDict1.ContainsKey(note))
                                    _toDoNoteDict1.Add(note, timeout);
                            }
                            else
                            {
                                if (!_toDoNoteDict2.ContainsKey(note))
                                    _toDoNoteDict2.Add(note, timeout);
                            }

                        }
                    }
                }
            }
        }

        private void SetAttributes()
        {
            _amountOfGreatStaffs = Sheet.SheetModel.GreatStaffModelList.Count;

            //set timing of music piece
            _bpsecond = _bpm / 60;

            if (Note == NoteType.Half)
            {
                _bpsecond = _bpsecond / 2;
            }
            else if (Note == NoteType.Whole)
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

        public int ReturnFirstNoteTimeout(int staffNumber)
        {
            int timeout = 0;
            if (staffNumber == 1)
            {
                timeout = (int)(_toDoNoteDict1.First(n => n.Key.Staff == staffNumber && !n.Key.Duplicate).Value * 1000);

            }
            else
            {
                timeout = (int)(_toDoNoteDict2.First(n => n.Key.Staff == staffNumber && !n.Key.Duplicate).Value * 1000);

            }
            return timeout;
        }

        private void RemoveFirstNoteFromToDoDict(Note note)
        {
            if (note.Staff == 1)
            {
                if (_toDoNoteDict1.Count > 0) _toDoNoteDict1.Remove(_toDoNoteDict1.Keys.First(t => t.Equals(note)));

            }
            else
            {
                if (_toDoNoteDict2.Count > 0) _toDoNoteDict2.Remove(_toDoNoteDict2.Keys.First(t => t.Equals(note)));

            }
        }

        private void NoteIntersectEvent(EventArgs e, int staffNumber)
        {
            if (staffdivs[staffNumber] > 0)
                staffdivs[staffNumber]--;

            if (staffdivs[staffNumber] == 0)
            {
                //Check key press is correct or not
                CheckPressedKeysToActiveNotes(staffNumber);

                var tempDict = _activeNoteAndTimeoutDict1;

                if (staffNumber == 2)
                {
                    tempDict = _activeNoteAndTimeoutDict2;
                }
                //remove keys that are done

                var _activ = new Dictionary<Note, Timeout>(tempDict).ToDictionary(k => k.Key, k => k.Value);
                checkLastNote(_activ);


                goToNextStaff();
                HoldPosition?.Invoke(this, EventArgs.Empty);


                var tempList = _toDoNoteDict1.ToList();
                if (staffNumber == 2)
                {
                    tempList = _toDoNoteDict2.ToList();
                }

                for (int i = 0; i < 1; i++)
                {
                    //Add note to active Dictionary
                    if (tempList.Count > 0)
                    {
                        if (!tempDict.ContainsKey(tempList[i].Key))
                        {
                            //Add note to active Dictionary

                            if (!tempDict.ContainsKey(tempList[i].Key))
                            {
                                tempDict.Add(tempList[i].Key, new Timeout()
                                {
                                    NoteTimeout = tempList[i].Value,
                                    TimeAdded = StopWatch.ElapsedMilliseconds
                                });

                                staffdivs[staffNumber] = tempList[i].Key.Duration;
                                prevnote[staffNumber].Add(tempList[i].Key);
                            }


                            RemoveFirstNoteFromToDoDict(tempList[i].Key);

                            for (int j = i + 1; j < tempList.Count; j++)
                            {
                                if (tempList[j].Key.IsChordTone)
                                {
                                    //Add note with same pos to active Dictionary
                                    if (!tempDict.ContainsKey(tempList[j].Key))
                                        tempDict.Add(tempList[j].Key, new Timeout()
                                        {
                                            NoteTimeout = tempList[j].Value,
                                            TimeAdded = StopWatch.ElapsedMilliseconds
                                        });
                                    //Remove the note with same pos from to do 
                                    RemoveFirstNoteFromToDoDict(tempList[j].Key);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    

                    Piano.UpdatePianoKeys(tempDict);
                    Sheet.UpdateNotes(tempDict);

                }
                foreach (Note note in _activ.Keys)
                {
                    tempDict.Remove(note);
                    if (note.State != NoteState.Good)
                    {
                        note.State = NoteState.Wrong;
                    }

                    if (note.IsRest)
                    {
                        note.State = NoteState.Idle;
                    }

                }
            }




            if (_endReached.Equals(_amountOfGreatStaffs) && !_finished)
            {
                _finished = true;

                ScoreLabel.Score = CalcScore();              

                grid.Dispatcher.BeginInvoke((Action)(() => ScoreLabel.UpdateScore()));
                Thread.Sleep(3000);
                grid.Dispatcher.BeginInvoke((Action)(() => ScoreLabel.HideScore()));
            }
        }



        public int CalcScore()
        {
            return (100 * _goodNotes) / _amountOfNotes;
        }

        public MockupNote getNoteFromNoteNumber(int nn)
        {
            int octave = (int)Math.Floor((decimal)nn / 12);
            int noteNumber = (int)Math.Floor((decimal)nn - ((12 * octave) - 1));
            char step = '*';
            int alter = 0;           

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

            return new MockupNote() { Step = step, Alter = alter, Octave = octave };
        }

        private void CheckPressedKeysToActiveNotes(int staffNumber)
        {
            var tempDict = _activeNoteAndTimeoutDict1;

            if (staffNumber == 2)
            {
                tempDict = _activeNoteAndTimeoutDict2;
            }

            foreach (var activeNote in tempDict.Where(n => n.Key.State == NoteState.Active))
            {
                if (ActiveKeys.Count > 0)
                {
                    foreach (var activeKey in ActiveKeys)
                    {
                        var keyTimeAdded = activeKey.Value;
                        var noteTimeAdded = activeNote.Value.TimeAdded;
                        var noteTime = activeNote.Value.NoteTimeout;

                        var activeKeynote = getNoteFromNoteNumber(activeKey.Key);


                        if (activeNote.Key.Pitch.Step == activeKeynote.Step &&
                            activeNote.Key.Pitch.Alter == activeKeynote.Alter &&
                            activeNote.Key.Pitch.Octave == activeKeynote.Octave &&
                            keyTimeAdded >= noteTimeAdded - (noteTimeAdded * .05) &&
                            keyTimeAdded <= (noteTimeAdded + (noteTimeAdded * .05)) + (noteTime * 1000))
                        {
                            //if the pressed keys time is in between note marge
                            activeNote.Key.State = NoteState.Good;
                            _goodNotes++;
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
            if (noteDict.TryGetValue(lastNoteStaffOne, out temp))
            {
                atStaffEndOne = true;
                _endReached++;
            }

            if (noteDict.TryGetValue(lastNoteStaffTwo, out temp))
            {
                atStaffEndTwo = true;
               
            }
        }

        private void goToNextStaff()
        {
            if (atStaffEndOne && atStaffEndTwo)
            {
                atStaffEndOne = false;
                atStaffEndTwo = false;

                currentStaff++;

                if (currentStaff >= Sheet.SheetModel.GreatStaffModelList.Count)
                {
                    this.Stop();
                    
                    return;
                }


                if (currentStaff < Sheet.SheetModel.GreatStaffModelList.Count)
                {
                    stafflist = Sheet.SheetModel.GreatStaffModelList[currentStaff].StaffList;
                }

                if (StaffEndReached != null)
                {
                    StaffEndReached(this, EventArgs.Empty);
                }
            }
        }

        public void Pause()
        {
            if (paused)
            {
                _timerStaffOne.Enabled = true;
                record.startRecording();
                _timerStaffTwo.Enabled = true;
            }
            else
            {
                _timerStaffOne.Enabled = false;
                record.pauseRecording();
                _timerStaffTwo.Enabled = false;
            }

            paused = !paused;
        }

        public bool Start()
        {
            _finished = false;
            _endReached = 0;
            _amountOfGreatStaffs = 0;
            _goodNotes = 0;
            ////Set the attributes from the current music piece
            SetAttributes();

            //Fill the to do list with notes from the current music     
            FillToDoList();

            StopWatch = Stopwatch.StartNew();
            staffdivs = new int[3] { 0, 0, 0 };
            prevnote = new List<Note>[3] { new List<Note>(), new List<Note>(), new List<Note>(), };


            _timerStaffOne = new System.Timers.Timer();
            _timerStaffOne.Elapsed += (sender, e) => NoteIntersectEvent(e, 1);
            _timerStaffOne.Interval = _milsecperbeat / _divs;
            NoteIntersectEvent(EventArgs.Empty, 1);
            _timerStaffOne.Enabled = true;
            record.startRecording();

            _timerStaffTwo = new System.Timers.Timer();
            _timerStaffTwo.Elapsed += (sender, e) => NoteIntersectEvent(e, 2);
            _timerStaffTwo.Interval = _milsecperbeat / _divs;
            NoteIntersectEvent(EventArgs.Empty, 2);
            _timerStaffTwo.Enabled = true;
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
            _timerStaffTwo.Stop();
            _toDoNoteDict1.Clear();
            _toDoNoteDict2.Clear();
            _activeNoteAndTimeoutDict1.Clear();
            _activeNoteAndTimeoutDict2.Clear();
            SetAttributes();
        }

        public bool Stop()
        {
            _timerStaffOne.Enabled = false;
            _timerStaffTwo.Enabled = false;
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