using MusicXml.Domain;
using PianoApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoApp.Controllers
{
    public class RecordController
    {
        private Dictionary<int, long> activeNotes = new Dictionary<int, long>();
        public static List<RecordedNote> recordedSheet = new List<RecordedNote>();
        private Stopwatch stopWatch = new Stopwatch();
        private GuidesController guide;
        private static float bpm;
        private bool recording = false;

        public RecordController(GuidesController guide)
        {
            this.guide = guide;
        }

        //start. use stoprecording before this method if reset is needed.
        public void startRecording()
        {
            bpm = guide.Bpm;
            RecordedNote.bpm = bpm;
            stopWatch.Start();
            recording = true;
        }

        //pause
        public void pauseRecording()
        {
            stopWatch.Stop();
            recording = false;
        }

        //stop and reset
        public void stopRecording()
        {
            stopWatch.Reset();
            recording = false;
            resetAttributes();
        }

        //returns a generated score based on pressed keys
        public Score getScore()
        {
            Score score = new Score();
            score.Parts.Add(getPart());
            return score;
        }

        //resets the recorded keys
        private void resetAttributes()
        {
            activeNotes = new Dictionary<int, long>();
            recordedSheet = new List<RecordedNote>();
        }

        //record a keypress and add it to list to record time pressed
        public void StartRecordNote(int note)
        {
            if (!recording)
                return;

            activeNotes.Add(note, stopWatch.ElapsedMilliseconds);
        }

        //stop recording time pressed and create new recordedNote
        public void StopRecordNote(int note)
        {
            if (!recording)
                return;
            if (!activeNotes.ContainsKey(note))
                return;

            long start = activeNotes[note];
            long stop = stopWatch.ElapsedMilliseconds;
            int elapsed = (int)Math.Round( (float)stop - start );
            activeNotes.Remove(note);

            recordedSheet.Add(new RecordedNote(note, elapsed, start));
        }

        //generates a list with all recorded notes
        private static List<Note> getNotes()
        {
            List<Note> noteList = new List<Note>();
            foreach(RecordedNote recorded in recordedSheet)
            {
                Note note = recorded.convertToNote();
                noteList.Add(note);
            }

            return noteList;
        }

        //generates a list with all recorded notes inside their corresponding measures
        private static List<Measure> GetMeasures()
        {
            List<Note> notes = null;
            Dictionary<int, Measure> measures = new Dictionary<int, Measure>();

            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                notes = getNotes();
            });

            //temporary save notes in dictionary to keep track of created measures
            foreach (Note note in notes)
            {
                int measureNumber = note.MeasureNumber;
                if (measures.ContainsKey(measureNumber)) {
                    MeasureElement element = new MeasureElement();
                    element.Element = note;
                    measures[measureNumber].MeasureElements.Add(element);
                } else
                {
                    Measure measure = new Measure();
                    measures.Add(measureNumber, measure);
                    MeasureElement element = new MeasureElement();
                    element.Element = note;
                    measures[measureNumber].MeasureElements.Add(element);
                }
                
            }

            //convert dictionary to list
            List<Measure> measureList = addRests(measures);
            return measureList;
        }

        private static List<Measure> addRests(Dictionary<int, Measure> measures)
        {
            List<Measure> newMeasures = new List<Measure>();
            foreach(Measure measure in measures.Values)
            {
                newMeasures.Add(measure);
            }

            for(int i = 1; i <= measures.Keys.Max(); i++)
            {
                if(!measures.ContainsKey(i))
                {
                    int measureLength = (int)((1000.0 / (bpm / 60.0)) * 4);
                    Measure measure = new Measure();
                    MeasureElement element = new MeasureElement();
                    Note note = new Note();
                    note.IsRest = true;
                    note.Duration = measureLength;
                    note.Type = "whole";
                    element.Element = note;
                    measure.MeasureElements.Add(element);
                    newMeasures.Add(measure);
                }
            }

            return newMeasures;
        }

        //generates a part with all measures
        private static Part getPart()
        {
            Part part = new Part();
            part.Measures = GetMeasures();
            return part;
        }
    }
}
