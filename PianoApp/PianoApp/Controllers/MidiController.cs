using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MusicXml.Domain;
using NAudio.Midi;
using PianoApp.Models;

namespace PianoApp.Controllers
{
    public class MidiController
    {
        public MidiIn midiIn;
        public MidiOut midiOut = new MidiOut(0);

        public event EventHandler midiInputChanged;

        public Dictionary<int, float> currentlyPressedKeys = new Dictionary<int, float>();

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
            
            midiOut.Volume = 65535;

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

        public void PlayNotes(Dictionary<Note, Timeout> notes)
        {
            foreach (var keyValuePair in notes)
            {
                var noteNumber = (int)keyValuePair.Key.Pitch.Step;
                midiOut?.Send(MidiMessage.StartNote(noteNumber, 127, 1).RawData);
            }
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
                if (Guide == null) return;
                currentlyPressedKeys.Add(noteEvent.NoteNumber, GuidesController.StopWatch.ElapsedMilliseconds);
                Guide.ActiveKeys = currentlyPressedKeys;
                Guide.UpdatePianoKeys();
                //Thread.Sleep inside GUI is just for example
                
                midiOut.Send(MidiMessage.StartNote(noteEvent.NoteNumber, 127, noteEvent.Channel).RawData);

            } else
            {                
                if (Guide == null) return;
                currentlyPressedKeys.Remove(noteEvent.NoteNumber);
                Guide.ActiveKeys = currentlyPressedKeys;
                Guide.UpdatePianoKeys();
                midiOut.Send(MidiMessage.StopNote(noteEvent.NoteNumber, 127, noteEvent.Channel).RawData);
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
