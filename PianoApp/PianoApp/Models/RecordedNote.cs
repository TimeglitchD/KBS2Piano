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
        public long start;
        public double duration;

        public static float bpm;

        private static int lowestNote;

        public RecordedNote(int note, int elapsed, long start)
        {
            this.note = note;
            this.elapsed = elapsed;
            this.start = start;

            if(lowestNote > note)
            {
                lowestNote = note;
            }
        }

        //converts timing and note number to a musicxml note based on bpm
        public Note convertToNote()
        {

            Note thisNote = new Note();

            thisNote.Pitch = intToPitch(note);
            thisNote.Type = durationToType(elapsed);
            thisNote.Stem = calculateStem();
            thisNote.Staff = calculateStaff(note);
            thisNote.MeasureNumber = calculateMeasure();

            return thisNote;
        }
        
        //generate step, octave and alter based on notenumber
        private Pitch intToPitch(int note)
        {
            Pitch pitch = new Pitch();
            char step = 'A';
            int octave = 0;
            int alter = 0;

            note -= 24;

            octave = (int)Math.Floor(note / 12f);

            if (octave < 0)
            {
                pitch.Step = 'A';
                pitch.Octave = 0;
                pitch.Alter = 0;
                return pitch;
            }

            int baseNote = note - (octave * 12);


            switch (baseNote)
            {
                case (0): step = 'C'; alter = 0; break;
                case (1): step = 'C'; alter = 1; break;
                case (2): step = 'D'; alter = 0; break;
                case (3): step = 'D'; alter = 1; break;
                case (4): step = 'E'; alter = 0; break;
                case (5): step = 'F'; alter = 0; break;
                case (6): step = 'F'; alter = 1; break;
                case (7): step = 'G'; alter = 0; break;
                case (8): step = 'G'; alter = 1; break;
                case (9): step = 'A'; alter = 0; break;
                case (10): step = 'A'; alter = 1; break;
                case (11): step = 'B'; alter = 0; break;
            }

            pitch.Step = step;
            pitch.Alter = alter;
            pitch.Octave = octave;

            return pitch;
        }
        
        //generate type based on note duration
        private string durationToType(int duration)
        {
            string type = "whole";
            int beatDuration = (int)(1000.0 / (bpm / 60.0));
            double value = (float)duration / beatDuration;
            switch(value)
            {
                case double n when ( n > 2.8):
                    type = "whole";
                    duration = 4 * beatDuration;
                    break;
                case double n when (n < 2.8 && n > 1.3):
                    type = "half";
                    duration = 2 * beatDuration;
                    break;
                case double n when (n < 1.3 && n > 0.9) :
                    type = "quarter";
                    duration = beatDuration;
                    break;
                case double n when (n < 0.9 && n > 0.25):
                    type = "eigth";
                    duration = beatDuration / 2;
                    break;
                case double n when (n < 0.25):
                    type = "16th";
                    duration = beatDuration / 4;
                    break;
            }

            return type;
        }

        //calculate the stem of a note based on position
        private string calculateStem()
        {
            //add logic to calculate if stem should go up or down
            return "up";
        }

        //calculate the note's staff relative to the lowest note played
        private int calculateStaff(int note)
        {
            if(note < lowestNote + 11)
            {
                return 2;
            } else
            {
                return 1;
            }
        }

        //calculate note's measure based on the elapsed time when it was played.
        private int calculateMeasure()
        {
                int start = (int)this.start;
                return start / (int)((1000.0 / (bpm / 60.0)) * 4);
        }
    }
}
