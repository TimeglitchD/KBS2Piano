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
            if(midiAvailable())
            {
                initializeMidi();
            }
        }

        public void initializeMidi()
        {
            MidiIn = new MidiIn(0); 
            

            MidiIn.MessageReceived += midiInReceived;
            MidiIn.ErrorReceived += midiIn_ErrorReceived;            



            MidiThread = new Thread(() => {
                MidiIn.Start();
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
                if (CurrentlyPressedKeys.ContainsKey(calculatedNote)) return;

                if (Guide == null) return;

                CurrentlyPressedKeys.Add(calculatedNote, GuidesController.StopWatch.ElapsedMilliseconds);
                Guide.ActiveKeys = CurrentlyPressedKeys;
                Guide.UpdatePianoKeys();
                //Thread.Sleep inside GUI is just for example
                

            } else
            {                
                
                int calculatedNote = offsetNote(noteEvent.NoteNumber, KeyboardController.KeyOffset);
                MidiOutput.stop(calculatedNote);
                if (Guide == null) return;
                CurrentlyPressedKeys.Remove(calculatedNote);
                Guide.ActiveKeys = CurrentlyPressedKeys;
                Guide.UpdatePianoKeys();
                
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
                if (!keyValuePair.Key.IsRest)
                {                                       
                    var noteNumber = 0;
                    var oct = keyValuePair.Key.Pitch.Octave;
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
                    else if (keyValuePair.Key.Pitch.Alter == 1)
                    {
                        switch (keyValuePair.Key.Pitch.Step)
                        {
                            case 'C': noteNumber = 1; break;
                            case 'D': noteNumber = 3; break;
                            case 'E': noteNumber = 5; break;
                            case 'F': noteNumber = 6; break;
                            case 'G': noteNumber = 8; break;
                            case 'A': noteNumber = 10; break;
                            case 'B':
                                noteNumber = 0; oct++; break;
                        }
                    }
                    else if (keyValuePair.Key.Pitch.Alter == -1)
                    {
                        switch (keyValuePair.Key.Pitch.Step)
                        {
                            case 'C': noteNumber = 11;
                                oct--; break;
                            case 'D': noteNumber = 1; break;
                            case 'E': noteNumber = 3; break;
                            case 'F': noteNumber = 4; break;
                            case 'G': noteNumber = 6; break;
                            case 'A': noteNumber = 8; break;
                            case 'B': noteNumber = 10; break;
                        }
                    }

                    noteNumber = ((noteNumber) + (oct * 12));
                    MidiOutput.play(noteNumber);
                }
            }   
        }

        protected virtual void OnMidiInputChanged(MidiControllerEventArgs e)
        {
            MidiInputChanged?.Invoke(this, e);
        }

        private int offsetNote(int note, int offset)
        {
            return note;
        }

    }
}
