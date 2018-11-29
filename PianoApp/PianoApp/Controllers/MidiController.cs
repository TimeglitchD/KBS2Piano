using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Midi;
using PianoApp.Models;

namespace PianoApp.Controllers
{
    public class MidiController
    {
        public MidiIn midiIn;

        public event EventHandler midiInputChanged;

        public List<int> currentlyPressedKeys;

        public Thread MidiThread { get; set; }

        private MidiControllerEventArgs midiArgs = new MidiControllerEventArgs();

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

            MidiThread = new Thread(() => {
                midiIn.Start();
            });

            MidiThread.Start();
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

            NoteEvent noteEvent;

            try
            {
                noteEvent = (NoteEvent)e.MidiEvent;
            } catch (Exception)
            {
                return;
            }

            if(MidiEvent.IsNoteOn(e.MidiEvent))
            {
                currentlyPressedKeys.Add(noteEvent.NoteNumber);
                midiArgs.ActiveKeys = currentlyPressedKeys;
                OnMidiInputChanged(midiArgs);
                Console.WriteLine("----------on----------");
                Console.WriteLine(noteEvent.NoteNumber.ToString() + noteEvent.NoteName);
            } else
            {
                currentlyPressedKeys.Remove(noteEvent.NoteNumber);
                midiArgs.ActiveKeys = currentlyPressedKeys;
                OnMidiInputChanged(midiArgs);
                Console.WriteLine("----------off----------");
                Console.WriteLine(noteEvent.NoteNumber.ToString() + noteEvent.NoteName);
            }
        }

        void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            Console.WriteLine(String.Format("Time {0} Message 0x{1:X8} Event {2}",
                e.Timestamp, e.RawMessage, e.MidiEvent));
        }

        private void OnMidiInputChanged(MidiControllerEventArgs e)
        {
            if(midiInputChanged != null)
            {
                midiInputChanged(this, e);
            }
        }

    }
}
