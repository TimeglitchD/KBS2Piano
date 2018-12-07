using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PianoApp.Controllers
{
    public class KeyboardController
    {
        public Dictionary<int, float> currentlyPressedKeys = new Dictionary<int, float>();

        public GuidesController Guide;

        private int keyOffset = 48;

        public void KeyDown(KeyEventArgs e)
        {
            int pressedKey = -1;

            switch(e.Key)
            {
                case Key.Q:
                    pressedKey = keyOffset;
                    break;
                case Key.D2:
                    pressedKey = keyOffset + 1;
                    break;
                case Key.W:
                    pressedKey = keyOffset + 2;
                    break;
                case Key.D3:
                    pressedKey = keyOffset + 3;
                    break;
                case Key.E:
                    pressedKey = keyOffset + 4;
                    break;
                case Key.R:
                    pressedKey = keyOffset + 5;
                    break;
                case Key.D5:
                    pressedKey = keyOffset + 6;
                    break;
                case Key.T:
                    pressedKey = keyOffset + 7;
                    break;
                case Key.D6:
                    pressedKey = keyOffset + 8;
                    break;
                case Key.Y:
                    pressedKey = keyOffset + 9;
                    break;
                case Key.D7:
                    pressedKey = keyOffset + 10;
                    break;
                case Key.U:
                    pressedKey = keyOffset + 11;
                    break;
                case Key.I:
                    pressedKey = keyOffset + 12;
                    break;
                case Key.D9:
                    pressedKey = keyOffset + 13;
                    break;
                case Key.O:
                    pressedKey = keyOffset + 14;
                    break;
                case Key.D0:
                    pressedKey = keyOffset + 15;
                    break;
                case Key.P:
                    pressedKey = keyOffset + 16;
                    break;
                case Key.Z:
                    pressedKey = keyOffset + 17;
                    break;
                case Key.S:
                    pressedKey = keyOffset + 18;
                    break;
                case Key.X:
                    pressedKey = keyOffset + 19;
                    break;
                case Key.D:
                    pressedKey = keyOffset + 20;
                    break;
                case Key.C:
                    pressedKey = keyOffset + 21;
                    break;
                case Key.F:
                    pressedKey = keyOffset + 22;
                    break;
                case Key.V:
                    pressedKey = keyOffset + 23;
                    break;
                case Key.B:
                    pressedKey = keyOffset + 24;
                    break;
                case Key.H:
                    pressedKey = keyOffset + 25;
                    break;
                case Key.N:
                    pressedKey = keyOffset + 26;
                    break;
                case Key.J:
                    pressedKey = keyOffset + 27;
                    break;
                case Key.M:
                    pressedKey = keyOffset + 28;
                    break;
                case Key.OemComma:
                    pressedKey = keyOffset + 29;
                    break;
                case Key.L:
                    pressedKey = keyOffset + 30;
                    break;
                case Key.OemPeriod:
                    pressedKey = keyOffset + 31;
                    break;
                case Key.Up:
                    octaveUp();
                    break;
                case Key.Down:
                    octaveDown();
                    break;
            }

            if (pressedKey < 0 || currentlyPressedKeys.ContainsKey(pressedKey))
                return;

            MidiOutput.play(pressedKey);
            currentlyPressedKeys.Add(pressedKey, GuidesController.StopWatch.ElapsedMilliseconds);

            if (Guide == null)
                return;

                Guide.Piano.UpdatePressedPianoKeys(currentlyPressedKeys);

            Console.WriteLine("----------On----------");
            Console.WriteLine(pressedKey.ToString());
        }

        public void KeyUp(KeyEventArgs e)
        {
            int pressedKey = -1;
            switch (e.Key)
            {
                case Key.Q:
                    pressedKey = keyOffset;
                    break;
                case Key.D2:
                    pressedKey = keyOffset + 1;
                    break;
                case Key.W:
                    pressedKey = keyOffset + 2;
                    break;
                case Key.D3:
                    pressedKey = keyOffset + 3;
                    break;
                case Key.E:
                    pressedKey = keyOffset + 4;
                    break;
                case Key.R:
                    pressedKey = keyOffset + 5;
                    break;
                case Key.D5:
                    pressedKey = keyOffset + 6;
                    break;
                case Key.T:
                    pressedKey = keyOffset + 7;
                    break;
                case Key.D6:
                    pressedKey = keyOffset + 8;
                    break;
                case Key.Y:
                    pressedKey = keyOffset + 9;
                    break;
                case Key.D7:
                    pressedKey = keyOffset + 10;
                    break;
                case Key.U:
                    pressedKey = keyOffset + 11;
                    break;
                case Key.I:
                    pressedKey = keyOffset + 12;
                    break;
                case Key.D9:
                    pressedKey = keyOffset + 13;
                    break;
                case Key.O:
                    pressedKey = keyOffset + 14;
                    break;
                case Key.D0:
                    pressedKey = keyOffset + 15;
                    break;
                case Key.P:
                    pressedKey = keyOffset + 16;
                    break;
                case Key.Z:
                    pressedKey = keyOffset + 17;
                    break;
                case Key.S:
                    pressedKey = keyOffset + 18;
                    break;
                case Key.X:
                    pressedKey = keyOffset + 19;
                    break;
                case Key.D:
                    pressedKey = keyOffset + 20;
                    break;
                case Key.C:
                    pressedKey = keyOffset + 21;
                    break;
                case Key.F:
                    pressedKey = keyOffset + 22;
                    break;
                case Key.V:
                    pressedKey = keyOffset + 23;
                    break;
                case Key.B:
                    pressedKey = keyOffset + 24;
                    break;
                case Key.H:
                    pressedKey = keyOffset + 25;
                    break;
                case Key.N:
                    pressedKey = keyOffset + 26;
                    break;
                case Key.J:
                    pressedKey = keyOffset + 27;
                    break;
                case Key.M:
                    pressedKey = keyOffset + 28;
                    break;
                case Key.OemComma:
                    pressedKey = keyOffset + 29;
                    break;
                case Key.L:
                    pressedKey = keyOffset + 30;
                    break;
                case Key.OemPeriod:
                    pressedKey = keyOffset + 31;
                    break;
            }

            if (pressedKey < 0 || !currentlyPressedKeys.ContainsKey(pressedKey))
                return;

            currentlyPressedKeys.Remove(pressedKey);
            MidiOutput.stop(pressedKey);

            if (Guide == null)
                return;

            Guide.Piano.UpdatePressedPianoKeys(currentlyPressedKeys);

            Console.WriteLine("----------Off----------");
            Console.WriteLine(pressedKey.ToString());
        }

        private void octaveUp()
        {
            if(keyOffset < 95)
                keyOffset += 12;

            MidiOutput.play(keyOffset);
        }

        private void octaveDown()
        {
            if(keyOffset > 0)
                keyOffset -= 12;
            
            MidiOutput.play(keyOffset);
        }

    }
}
