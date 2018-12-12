using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicXml.Domain;

namespace PianoApp.Models
{
    class RecordedNote
    {
        private int note;
        private int elapsed;
        private long start;

        public static float bpm;

        public RecordedNote(int note, int elapsed, long start)
        {
            this.note = note;
            this.elapsed = elapsed;
            this.start = start;

            Console.WriteLine("--------recorded note--------");
            Console.WriteLine(note);
            Console.WriteLine(elapsed);
            Console.WriteLine(start);
            //Console.WriteLine(durationToType(elapsed));
        }

        public Note convertToNote()
        {
            string pitch = intToPitch(note);
            string type = durationToType(elapsed);

            Note thisNote = new Note();
            thisNote.Duration = elapsed;
            thisNote.Type = type;

            return new Note();
        }

        private string intToPitch(int note)
        {
            return null;
        }

        private string durationToType(int duration)
        {
            string type = "whole";
            int beatDuration = (int)(1000.0 / (bpm / 60.0));
            double value = (float)duration / beatDuration;
            Console.WriteLine(value);
            switch(value)
            {
                case double n when ( n > 2.8):
                    type = "whole";
                    break;
                case double n when (n < 2.8 && n > 1.3):
                    type = "half";
                    break;
                case double n when (n < 1.3 && n > 0.9) :
                    type = "quarter";
                    break;
                case double n when (n < 0.9 && n > 0.25):
                    type = "eigth";
                    break;
                case double n when (n < 0.25):
                    type = "16th";
                    break;
            }

            return type;
        }
    }
}
