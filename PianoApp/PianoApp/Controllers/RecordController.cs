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
        private static List<RecordedNote> recordedSheet = new List<RecordedNote>();
        private Stopwatch stopWatch = new Stopwatch();
        private GuidesController guide;
        private static float bpm;
        private bool recording = false;

        public RecordController(GuidesController guide)
        {
            this.guide = guide;
        }

        public void startRecording()
        {
            bpm = guide.Bpm;
            RecordedNote.bpm = bpm;
            stopWatch.Start();
            recording = true;
        }

        public void pauseRecording()
        {
            stopWatch.Stop();
            recording = false;
        }

        public void stopRecording()
        {
            stopWatch.Reset();
            recording = false;
            resetAttributes();
        }

        private void resetAttributes()
        {
            activeNotes = new Dictionary<int, long>();
            recordedSheet = new List<RecordedNote>();
        }

        public void StartRecordNote(int note)
        {
            if (!recording)
                return;

            activeNotes.Add(note, stopWatch.ElapsedMilliseconds);
        }

        public void StopRecordNote(int note)
        {
            if (!recording)
                return;

            long start = activeNotes[note];
            long stop = stopWatch.ElapsedMilliseconds;
            int elapsed = (int)Math.Round( (float)stop - start );
            activeNotes.Remove(note);

            recordedSheet.Add(new RecordedNote(note, elapsed, start));
        }

        public static List<Note> getNotes()
        {
            List<Note> noteList = new List<Note>();
            foreach(RecordedNote recorded in recordedSheet)
            {
                Note note = recorded.convertToNote();
                noteList.Add(note);
            }

            return noteList;
        }

        public static List<Measure> GetMeasures()
        {
            Measure measure = new Measure();
            int measureLength = (int)(1000.0 / (bpm / 60.0)) * 4;
            int currentMeasureLength = 0;

            foreach (RecordedNote recorded in recordedSheet)
            {
                long start = recorded.start;
                double duration = recorded.duration;
                long end = start + (long)duration;
                if(duration + currentMeasureLength <= measureLength)
                {
                    measureLength += (int)duration;
                    MeasureElement element = new MeasureElement();
                    element.Type = MeasureElementType.Note;
                    element.Element = recorded.convertToNote();
                    measure.MeasureElements.Add(recorded.convertToNote());
                } else
                {
                    
                }
            }
        }

        public void NoteIntersect()
        {
            
        }
    }
}
