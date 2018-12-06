using MusicXml.Domain;
using NAudio.Midi;
using PianoApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoApp
{
    public class MidiOutput
    {
        private static MidiOut midiOut = new MidiOut(0);
        
        public static void play(int note)
        {
            midiOut?.Send(MidiMessage.StartNote(note, 127, 1).RawData);
        }

        public static void stop(int note)
        {
            midiOut.Send(MidiMessage.StopNote(note, 127, 1).RawData);
        }

        public static void PlayNotes(Dictionary<Note, Timeout> notes)
        {
            foreach (var keyValuePair in notes)
            {
                var noteNumber = (int)keyValuePair.Key.Pitch.Step;
                midiOut?.Send(MidiMessage.StartNote(noteNumber, 127, 1).RawData);
            }
        }
    }
}
