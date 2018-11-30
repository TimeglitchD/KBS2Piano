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
        public List<int> currentlyPressedKeys;

        private int keyOffset = 32;

        public void KeyDown(KeyEventArgs e)
        {
            int pressedKey = 0;
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


            }

            System.Windows.MessageBox.Show(pressedKey.ToString());
        }

        public void KeyUp(KeyEventArgs e)
        {

        }
    }
}
