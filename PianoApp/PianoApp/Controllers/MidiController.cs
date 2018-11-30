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

        public GuidesController Guide;

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
            if (e.MidiEvent == null || Guide == null)
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
                //                OnMidiInputChanged(midiArgs);  
                Guide.ActiveKeys = currentlyPressedKeys;
                Guide.UpdatePianoKeys(currentlyPressedKeys);
                Console.WriteLine("key press");

            } else
            {
                currentlyPressedKeys.Remove(noteEvent.NoteNumber);
                midiArgs.ActiveKeys = currentlyPressedKeys;
//                OnMidiInputChanged(midiArgs);
                Guide.ActiveKeys = currentlyPressedKeys;
                Guide.UpdatePianoKeys(currentlyPressedKeys);
            }
        }

        void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            Console.WriteLine(String.Format("Time {0} Message 0x{1:X8} Event {2}",
                e.Timestamp, e.RawMessage, e.MidiEvent));
        }

        protected virtual void OnMidiInputChanged(MidiControllerEventArgs e)
        {
            midiInputChanged?.Invoke(this, e);
        }

    }
}
