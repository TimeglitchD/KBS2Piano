﻿using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PianoApp.Views;

namespace PianoApp.Controllers
{
    public class KeyboardController
    {
        public Dictionary<int, float> CurrentlyPressedKeys = new Dictionary<int, float>();
        public PianoController PianoController;
        public GuidesController Guide;
        public bool GuideIsNull = false;
        public static int KeyOffset = 48;

        // When key is pressed
        public void KeyDown(KeyEventArgs e)
        {
            int pressedKey = -1;

            // Check which key is pressed
            switch(e.Key)
            {
                case Key.Q:
                    pressedKey = KeyOffset;
                    break;
                case Key.D2:
                    pressedKey = KeyOffset + 1;
                    break;
                case Key.W:
                    pressedKey = KeyOffset + 2;
                    break;
                case Key.D3:
                    pressedKey = KeyOffset + 3;
                    break;
                case Key.E:
                    pressedKey = KeyOffset + 4;
                    break;
                case Key.R:
                    pressedKey = KeyOffset + 5;
                    break;
                case Key.D5:
                    pressedKey = KeyOffset + 6;
                    break;
                case Key.T:
                    pressedKey = KeyOffset + 7;
                    break;
                case Key.D6:
                    pressedKey = KeyOffset + 8;
                    break;
                case Key.Y:
                    pressedKey = KeyOffset + 9;
                    break;
                case Key.D7:
                    pressedKey = KeyOffset + 10;
                    break;
                case Key.U:
                    pressedKey = KeyOffset + 11;
                    break;
                case Key.I:
                    pressedKey = KeyOffset + 12;
                    break;
                case Key.D9:
                    pressedKey = KeyOffset + 13;
                    break;
                case Key.O:
                    pressedKey = KeyOffset + 14;
                    break;
                case Key.D0:
                    pressedKey = KeyOffset + 15;
                    break;
                case Key.P:
                    pressedKey = KeyOffset + 16;
                    break;
                case Key.Z:
                    pressedKey = KeyOffset + 17;
                    break;
                case Key.S:
                    pressedKey = KeyOffset + 18;
                    break;
                case Key.X:
                    pressedKey = KeyOffset + 19;
                    break;
                case Key.D:
                    pressedKey = KeyOffset + 20;
                    break;
                case Key.C:
                    pressedKey = KeyOffset + 21;
                    break;
                case Key.F:
                    pressedKey = KeyOffset + 22;
                    break;
                case Key.V:
                    pressedKey = KeyOffset + 23;
                    break;
                case Key.B:
                    pressedKey = KeyOffset + 24;
                    break;
                case Key.H:
                    pressedKey = KeyOffset + 25;
                    break;
                case Key.N:
                    pressedKey = KeyOffset + 26;
                    break;
                case Key.J:
                    pressedKey = KeyOffset + 27;
                    break;
                case Key.M:
                    pressedKey = KeyOffset + 28;
                    break;
                case Key.OemComma:
                    pressedKey = KeyOffset + 29;
                    break;
                case Key.L:
                    pressedKey = KeyOffset + 30;
                    break;
                case Key.OemPeriod:
                    pressedKey = KeyOffset + 31;
                    break;
                case Key.Up:
                    octaveUp();
                    break;
                case Key.Down:
                    octaveDown();
                    break;
            }

            if (pressedKey < 0 || CurrentlyPressedKeys.ContainsKey(pressedKey))
                return;

            // Play sound based on key press
            MidiOutput.play(pressedKey);

            // Update Dict of pressed key
            CurrentlyPressedKeys.Add(pressedKey, GuidesController.StopWatch.ElapsedMilliseconds);

            // If guide is null -> return
            if (Guide == null)
            {
                GuideIsNull = true;
                return;
            }

            // Update pressed key in guideController
            Guide.Piano.UpdatePressedPianoKeys(CurrentlyPressedKeys);
        }

        // When key is not pressed
        public void KeyUp(KeyEventArgs e)
        {
            int pressedKey = -1;

            // Check which key is not pressed anymore
            switch (e.Key)
            {
                case Key.Q:
                    pressedKey = KeyOffset;
                    break;
                case Key.D2:
                    pressedKey = KeyOffset + 1;
                    break;
                case Key.W:
                    pressedKey = KeyOffset + 2;
                    break;
                case Key.D3:
                    pressedKey = KeyOffset + 3;
                    break;
                case Key.E:
                    pressedKey = KeyOffset + 4;
                    break;
                case Key.R:
                    pressedKey = KeyOffset + 5;
                    break;
                case Key.D5:
                    pressedKey = KeyOffset + 6;
                    break;
                case Key.T:
                    pressedKey = KeyOffset + 7;
                    break;
                case Key.D6:
                    pressedKey = KeyOffset + 8;
                    break;
                case Key.Y:
                    pressedKey = KeyOffset + 9;
                    break;
                case Key.D7:
                    pressedKey = KeyOffset + 10;
                    break;
                case Key.U:
                    pressedKey = KeyOffset + 11;
                    break;
                case Key.I:
                    pressedKey = KeyOffset + 12;
                    break;
                case Key.D9:
                    pressedKey = KeyOffset + 13;
                    break;
                case Key.O:
                    pressedKey = KeyOffset + 14;
                    break;
                case Key.D0:
                    pressedKey = KeyOffset + 15;
                    break;
                case Key.P:
                    pressedKey = KeyOffset + 16;
                    break;
                case Key.Z:
                    pressedKey = KeyOffset + 17;
                    break;
                case Key.S:
                    pressedKey = KeyOffset + 18;
                    break;
                case Key.X:
                    pressedKey = KeyOffset + 19;
                    break;
                case Key.D:
                    pressedKey = KeyOffset + 20;
                    break;
                case Key.C:
                    pressedKey = KeyOffset + 21;
                    break;
                case Key.F:
                    pressedKey = KeyOffset + 22;
                    break;
                case Key.V:
                    pressedKey = KeyOffset + 23;
                    break;
                case Key.B:
                    pressedKey = KeyOffset + 24;
                    break;
                case Key.H:
                    pressedKey = KeyOffset + 25;
                    break;
                case Key.N:
                    pressedKey = KeyOffset + 26;
                    break;
                case Key.J:
                    pressedKey = KeyOffset + 27;
                    break;
                case Key.M:
                    pressedKey = KeyOffset + 28;
                    break;
                case Key.OemComma:
                    pressedKey = KeyOffset + 29;
                    break;
                case Key.L:
                    pressedKey = KeyOffset + 30;
                    break;
                case Key.OemPeriod:
                    pressedKey = KeyOffset + 31;
                    break;
            }

            if (pressedKey < 0 || !CurrentlyPressedKeys.ContainsKey(pressedKey))
                return;

            // Remove pressed key
            CurrentlyPressedKeys.Remove(pressedKey);

            // Stop playing sounds
            MidiOutput.stop(pressedKey);

            // If guide is null -> return
            if (Guide == null)
            {
                GuideIsNull = true;
                return;
            }

            // Update keypress in guideController
            Guide.Piano.UpdatePressedPianoKeys(CurrentlyPressedKeys);
        }

        public void octaveUp()
        {
            if(KeyOffset < 95)
                KeyOffset += 12;

            PianoController.Redraw();

            MidiOutput.play(KeyOffset);


        }

        public void octaveDown()
        {
            if(KeyOffset > 0)
                KeyOffset -= 12;

            PianoController.Redraw();


            MidiOutput.play(KeyOffset);

        }

        public int CurrentOctave()
        {
            return (int)Math.Floor((decimal)KeyOffset / 12);
        }

    }
}
