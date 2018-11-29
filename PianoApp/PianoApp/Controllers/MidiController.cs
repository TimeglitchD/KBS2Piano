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
            midiIn.Start();
            currentlyPressedKeys = new List<int>();
            midiIn.MessageReceived += new EventHandler<MidiInMessageEventArgs>(midiInReceived);
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

            if(e.MidiEvent.CommandCode == MidiCommandCode.NoteOn)
            {
                NoteEvent noteEvent;
                noteEvent = (NoteEvent)e.MidiEvent;
                currentlyPressedKeys.Add(noteEvent.NoteNumber);
                Console.WriteLine(noteEvent.NoteNumber.ToString() + noteEvent.NoteName);
            }

            //if(e.MidiEvent.CommandCode == MidiCommandCode.NoteOff)
            //{
            //    NoteEvent noteEvent;
            //    noteEvent = (NoteEvent)e.MidiEvent;
            //    currentlyPressedKeys.Remove(noteEvent.NoteNumber);
            //    System.Windows.MessageBox.Show(noteEvent.NoteNumber.ToString() + noteEvent.NoteName);
            //}
        }

    }
}
