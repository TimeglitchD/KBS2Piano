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

        public event EventHandler midiInputChanged;

        public Dictionary<int, float> currentlyPressedKeys = new Dictionary<int, float>();

        public Thread MidiThread { get; set; }

        private MidiControllerEventArgs midiArgs = new MidiControllerEventArgs();

        public GuidesController Guide;

        public RecordController record;

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
               
                
                int calculatedNote =  offsetNote(noteEvent.NoteNumber, KeyboardController.KeyOffset);
                MidiOutput.play(calculatedNote);
                if (currentlyPressedKeys.ContainsKey(calculatedNote)) return;

                if (Guide == null) return;

                currentlyPressedKeys.Add(calculatedNote, GuidesController.StopWatch.ElapsedMilliseconds);
                Guide.ActiveKeys = currentlyPressedKeys;
                Guide.UpdatePianoKeys();
                record.StartRecordNote(calculatedNote);
                //Thread.Sleep inside GUI is just for example
                

            } else
            {                
                
                int calculatedNote = offsetNote(noteEvent.NoteNumber, KeyboardController.KeyOffset);
                MidiOutput.stop(calculatedNote);
                if (Guide == null) return;
                currentlyPressedKeys.Remove(calculatedNote);
                Guide.ActiveKeys = currentlyPressedKeys;
                Guide.UpdatePianoKeys();
                record.StopRecordNote(calculatedNote);
                
            }
        }

        void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            Console.WriteLine(String.Format("Time {0} Message 0x{1:X8} Event {2}",
                e.Timestamp, e.RawMessage, e.MidiEvent));
        }

        public void PlayNotes(Dictionary<Note, Timeout> notes)
        {
            foreach (var keyValuePair in notes)
            {
                var noteNumber = 0;
                if (keyValuePair.Key.Pitch.Alter == 0)
                {
                    switch (keyValuePair.Key.Pitch.Step)
                    {
                        case 'C': noteNumber = 0; break;
                        case 'D': noteNumber = 2; break;
                        case 'E': noteNumber = 4; break;
                        case 'F': noteNumber = 5; break;
                        case 'G': noteNumber = 7; break;
                        case 'A': noteNumber = 9; break;
                        case 'B': noteNumber = 11; break;
                    }
                }
                else 
                {
                    switch (keyValuePair.Key.Pitch.Step)
                    {
                        case 'C': noteNumber = 1; break;
                        case 'D': case 'E': noteNumber = 3; break;
                        case 'F': noteNumber = 6; break;
                        case 'G': noteNumber = 8; break;
                        case 'A': case 'B': noteNumber = 10; break;
                    }
                }

                Console.WriteLine($"{keyValuePair.Key.Pitch.Step} {noteNumber} * {keyValuePair.Key.Pitch.Octave} = {((noteNumber + 1) * (keyValuePair.Key.Pitch.Octave)) - 1}");

                noteNumber = ((noteNumber) + (keyValuePair.Key.Pitch.Octave * 12));

                //noteNumber + (octave * 12)
                MidiOutput.play(noteNumber);
            }   
        }

        protected virtual void OnMidiInputChanged(MidiControllerEventArgs e)
        {
            midiInputChanged?.Invoke(this, e);
        }

        private int offsetNote(int note, int offset)
        {
            return note;
        }

    }
}
