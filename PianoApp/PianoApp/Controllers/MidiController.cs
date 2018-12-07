﻿using System;
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
                Console.WriteLine("midicontroller exc");
                return;
            }
            
            if(MidiEvent.IsNoteOn(e.MidiEvent))
            {                
                if (Guide == null) return;
                currentlyPressedKeys.Add(noteEvent.NoteNumber, GuidesController.StopWatch.ElapsedMilliseconds);
                Guide.ActiveKeys = currentlyPressedKeys;
                Guide.UpdatePianoKeys();
                //Thread.Sleep inside GUI is just for example
                MidiOutput.play(noteEvent.NoteNumber);

            } else
            {                
                if (Guide == null) return;
                currentlyPressedKeys.Remove(noteEvent.NoteNumber);
                Guide.ActiveKeys = currentlyPressedKeys;
                Guide.UpdatePianoKeys();
                MidiOutput.stop(noteEvent.NoteNumber);
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
