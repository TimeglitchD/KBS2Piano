﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using MusicXml.Domain;
using Timer = System.Threading.Timer;

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
        private float _timing;
        private float _definedBpm = 60;

        private Dictionary<Note, Timeout> _activeNoteAndTimeoutDict = new Dictionary<Note, Timeout>();
        private Dictionary<Note, float> _toDoNoteDict = new Dictionary<Note, float>();
        private Stopwatch _stopwatch;

        private bool _isPlaying = false;

        private System.Timers.Timer _timer;

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
            _timing = 60 / _definedBpm;

            _divs = 6;
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
            int timeout = (int)(_toDoNoteDict.First(n => n.Key.Staff == staffNumber).Value * 1000);
            return timeout;
        }

        private void RemoveFirstNoteFromToDoDict(int staffNumber)
        {
            _toDoNoteDict.Remove(_toDoNoteDict.Keys.First());
        }

        private void NoteIntersectEvent(object source, ElapsedEventArgs e, int staffNumber)
        {
            try
            {
                //First remove the first note
                RemoveFirstNoteFromToDoDict(staffNumber);
                _activeNoteAndTimeoutDict.Add(_toDoNoteDict.Keys.First(), new Timeout()
                {
                    NoteTimeout = _toDoNoteDict.Values.First(),
                    TimeAdded = _stopwatch.ElapsedMilliseconds
                });
            }
            catch
            {
                Console.WriteLine("STOP");
            }
            

            var tempActiveNoteDict = new Dictionary<Note, Timeout>(_activeNoteAndTimeoutDict);

            //Remove keys that are done
            foreach (var keyValuePair in _activeNoteAndTimeoutDict.Where(t => _stopwatch.ElapsedMilliseconds > t.Value.NoteTimeout + t.Value.TimeAdded))
            {
                tempActiveNoteDict.Remove(keyValuePair.Key);
            }

            _activeNoteAndTimeoutDict = new Dictionary<Note, Timeout>(tempActiveNoteDict);

            Piano.UpdatePianoKeys(_activeNoteAndTimeoutDict);
        }



        public bool Start()
        {
            _stopwatch = Stopwatch.StartNew();
            //Set the attributes from the current music piece
            SetAttributes();
            //Fill the to do list with notes from the current music piece
            FillToDoList();

            System.Timers.Timer timerStaffOne = new System.Timers.Timer();
            timerStaffOne.Elapsed += (sender, e) => NoteIntersectEvent(sender, e, 1);
            timerStaffOne.Interval = (ReturnFirstNoteTimeout(1));
            timerStaffOne.Enabled = true;

            System.Timers.Timer timerStaffTwo = new System.Timers.Timer();
            timerStaffOne.Elapsed += (sender, e) => NoteIntersectEvent(sender, e, 2);
            timerStaffTwo.Interval = (ReturnFirstNoteTimeout(2));
            timerStaffTwo.Enabled = true;

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