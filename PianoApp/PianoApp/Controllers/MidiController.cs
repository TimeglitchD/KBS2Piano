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
        public MidiIn MidiIn;

        public event EventHandler MidiInputChanged;

        public Dictionary<int, float> CurrentlyPressedKeys = new Dictionary<int, float>();

        public Thread MidiThread { get; set; }

        private MidiControllerEventArgs _midiArgs = new MidiControllerEventArgs();

        public GuidesController Guide;

        public MidiController()
        {
            //check if midi keyboard is plugged in before initialization
            if(midiAvailable())
            {
                initializeMidi();
            }
        }

        //function to create midiIn and add handlers, and start listening in new thread
        public void initializeMidi()
        {
            MidiIn = new MidiIn(0); 
            

            MidiIn.MessageReceived += midiInReceived;
            MidiIn.ErrorReceived += midiIn_ErrorReceived;            

            //listen for midi messages in new thread
            MidiThread = new Thread(() => {
                MidiIn.Start();
            });

            MidiThread.Start();
        }

        public bool midiAvailable()
        {
            //if midiin detects more than zero devices return true
            return MidiIn.NumberOfDevices > 0;
        }

        //function executes when midi message is received
        private void midiInReceived(object sender, MidiInMessageEventArgs e)
        {
            //check if message is empty
            if (e.MidiEvent == null)
            {
                return;
            }

            //create new noteevent and cast midi message
            NoteEvent noteEvent;

            try
            {
                noteEvent = (NoteEvent)e.MidiEvent;
            } catch (Exception)
            {
                return;
            }

            //check if midi message was key down press
            if(MidiEvent.IsNoteOn(e.MidiEvent))
            {                
                //unimplemented offset for octave set on midi keyboard
                int calculatedNote =  offsetNote(noteEvent.NoteNumber, KeyboardController.KeyOffset);

                //play sound
                MidiOutput.play(calculatedNote);

                //check if pressed key was already pressed to prevent errors
                if (CurrentlyPressedKeys.ContainsKey(calculatedNote)) return;
                //check if musicpiece was selected before adding note to dict to prevent errors
                if (Guide == null) return;

                //add pressed note with elapsed time to dictionary
                CurrentlyPressedKeys.Add(calculatedNote, GuidesController.StopWatch.ElapsedMilliseconds);
                Guide.ActiveKeys = CurrentlyPressedKeys;
                //color keys
                Guide.UpdatePianoKeys();
            }
            //midi message was key up
            else
            {                
                int calculatedNote = offsetNote(noteEvent.NoteNumber, KeyboardController.KeyOffset);
                MidiOutput.stop(calculatedNote);

                if (Guide == null) return;

                //remove key from list
                CurrentlyPressedKeys.Remove(calculatedNote);
                Guide.ActiveKeys = CurrentlyPressedKeys;
                //color keys
                Guide.UpdatePianoKeys();
                
            }
        }

        //write midi error to console
        void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            Console.WriteLine(String.Format("Time {0} Message 0x{1:X8} Event {2}",
                e.Timestamp, e.RawMessage, e.MidiEvent));
        }

        //route event
        protected virtual void OnMidiInputChanged(MidiControllerEventArgs e)
        {
            MidiInputChanged?.Invoke(this, e);
        }

        //unimplemented calculation for offsetting note
        private int offsetNote(int note, int offset)
        {
            return note;
        }

    }
}
