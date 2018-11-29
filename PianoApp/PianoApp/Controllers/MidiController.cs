using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Midi;
using PianoApp.Models;

namespace PianoApp.Controllers
{
    class MidiController
    {
        public MidiIn midiIn;

        public event EventHandler midiInputChanged;

        public List<int> currentlyPressedKeys;

        public MidiController()
        {
            if(midiAvailable())
            {
                initializeMidi();
            }
        }

        public void initializeMidi()
        {
            midiIn = new MidiIn(0);            
            currentlyPressedKeys = new List<int>();
            midiIn.MessageReceived += midiInReceived;
            midiIn.ErrorReceived += midiIn_ErrorReceived;
            midiIn.Start();
        }

        public bool midiAvailable()
        {
            return MidiIn.NumberOfDevices > 0;
        }

        private void midiInReceived(object sender, MidiInMessageEventArgs e)
        {
            if (e.MidiEvent == null)
            {
                return;
            }

            if(MidiEvent.IsNoteOn(e.MidiEvent))
            {
                NoteEvent noteEvent;
                noteEvent = (NoteEvent)e.MidiEvent;
                currentlyPressedKeys.Add(noteEvent.NoteNumber);
                Console.WriteLine("----------on----------");
                Console.WriteLine(noteEvent.NoteNumber.ToString() + noteEvent.NoteName);
            }else
            {
                NoteEvent noteEvent;
                noteEvent = (NoteEvent)e.MidiEvent;
                currentlyPressedKeys.Remove(noteEvent.NoteNumber);
                Console.WriteLine("----------off----------");
                Console.WriteLine(noteEvent.NoteNumber.ToString() + noteEvent.NoteName);
            }
        }

        void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            Console.WriteLine(String.Format("Time {0} Message 0x{1:X8} Event {2}",
                e.Timestamp, e.RawMessage, e.MidiEvent));
        }

    }
}
