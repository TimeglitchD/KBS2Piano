using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicXml.Domain;
using PianoApp.Controllers;

namespace PianoApp.Models
{
    public class RecordedNote
    {
        private int note;
        private int elapsed;
        public long start;
        public double duration = 0;

        public static float bpm;

        private static int lowestNote = 127;

        public RecordedNote(int note, int elapsed, long start)
        {
            this.note = note;
            this.elapsed = elapsed;
            this.start = start;

            if(lowestNote > note)
            {
                lowestNote = note;
            }

            duration = roundOffElapsed(elapsed);

            if (!noteFits())
            {
                double newLength = splitNote();
                this.elapsed = (int)newLength;
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
            thisNote.XPos = xPos();

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
        private string durationToType(int elapsed)
        {
            string type = "whole";
            int beatDuration = (int)(1000.0 / (bpm / 60.0));
            double value = (float)duration / beatDuration;
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

        //round off the elapsed note time to a value relative to bpm
        private int roundOffElapsed(int elapsed)
        {
            int beatDuration = (int)(1000.0 / (bpm / 60.0));
            double value = (float)elapsed / beatDuration;
            int duration = 0;
            switch (value)
            {
                case double n when (n > 2.8):
                    duration = 4 * beatDuration;
                    break;
                case double n when (n < 2.8 && n > 1.3):
                    duration = 2 * beatDuration;
                    break;
                case double n when (n < 1.3 && n > 0.9):
                    duration = beatDuration;
                    break;
                case double n when (n < 0.9 && n > 0.25):
                    duration = beatDuration / 2;
                    break;
                case double n when (n < 0.25):
                    duration = beatDuration / 4;
                    break;
            }

            return duration;
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
                return start / (int)((1000.0 / (bpm / 60.0)) * 4) + 1;
        }

        //calculate x position based on timing. Probably needs finetuning
        private float xPos()
        {
            int measureLength = (int)((1000.0 / (bpm / 60.0)) * 4);
            int thisNoteMeasureNumber = calculateMeasure();
            int notePositionInMeasure = (int)this.start - ((thisNoteMeasureNumber - 1) * measureLength);
            float xPos = (float)notePositionInMeasure / 10;
            return xPos;
        }

        //check if rounded note fits in its measure
        private bool noteFits()
        {
            int measureLength = (int)((1000.0 / (bpm / 60.0)) * 4);
            int thisNoteMeasureNumber = calculateMeasure();
            int notePositionInMeasure = (int)this.start - ((thisNoteMeasureNumber - 1) * measureLength);
            //int leftOverLength = measureLength - notePositionInMeasure;
            return measureLength >= notePositionInMeasure + this.duration;
        }

        //split note if it's too long to fit in its measure. Add it to the sheetlist.
        private double splitNote()
        {
            int measureLength = (int)((1000.0 / (bpm / 60.0)) * 4);
            int thisNoteMeasureNumber = calculateMeasure();
            int notePositionInMeasure = (int)this.start - ((thisNoteMeasureNumber - 1) * measureLength);
            int leftOverLength = (int)this.duration - (measureLength - notePositionInMeasure);
            long usedLength = measureLength - notePositionInMeasure;
            long newNoteStart = (long)this.start + (long)usedLength;

            RecordController.recordedSheet.Add(new RecordedNote(this.note, leftOverLength, newNoteStart));

            return usedLength;
        }
    }
}
