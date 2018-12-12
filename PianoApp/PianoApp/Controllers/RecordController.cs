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
        private List<RecordedNote> recordedSheet = new List<RecordedNote>();
        private Stopwatch stopWatch = new Stopwatch();
        private GuidesController guide;
        private float bpm;
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

        public static SheetModel getSheet()
        {
            return new SheetModel();
        }
    }
}
