using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using MusicXml.Domain;
using PianoApp.Models;
using PianoApp.Views;

namespace PianoApp.Controllers
{
    public class PianoController
    {
        public PianoModel PianoModel { get; set; } = new PianoModel();
        public PianoView PianoView { get; set; }
        public NonKeyboardInputController NonKeyboardInputController { get; set; }
        


        public void UpdatePressedPianoKeys(Dictionary<int, float> activeKeysFromKeyboard)
        {
             foreach (var octaveModel in PianoModel.OctaveModelList)
            {
                foreach (var keyModel in octaveModel.KeyModelList)
                {
                    keyModel.Active = false;                    
                }
            }

            foreach (var pressedKey in activeKeysFromKeyboard)
            {
                int octave = (int)Math.Floor((decimal)pressedKey.Key / 12);
                int keyNumber = pressedKey.Key - ((12 * octave) - 1);

                foreach (var octaveModel in PianoModel.OctaveModelList.Where(o => o.Position == octave))
                {
                    var tempList = new List<KeyModel>(octaveModel.KeyModelList);
                    var blackKeysRemovedList = tempList.FindAll(n => n.Alter != -1);
                    blackKeysRemovedList[keyNumber - 1].Active = true;
                }
            }

            Redraw();
        }

        public void UpdatePianoKeys(Dictionary<Note, Timeout> noteAndTimeoutDictionary)
        {
            var tempDict = new Dictionary<Note, Timeout>(noteAndTimeoutDictionary);

            foreach (var octaveModel in PianoModel.OctaveModelList)
            {
                foreach (var keyModel in octaveModel.KeyModelList)
                {
                    keyModel.Active = false;                    
                }
            }

            //go over all keys and compare to note when true set active true on the corresponding key...
            foreach (var keyValuePair in tempDict)
            {
                //.Where(n => n.Position.Equals(keyValuePair.Key.Pitch.Octave))
                foreach (var octaveModel in PianoModel.OctaveModelList.Where(n => n.Position.Equals(keyValuePair.Key.Pitch.Octave)))
                {
                    foreach (var keyModel in octaveModel.KeyModelList)
                    {
                        if (keyValuePair.Key.Pitch.Step.ToString() == keyModel.Step.ToString() &&
                            keyValuePair.Key.Pitch.Alter           == keyModel.Alter)
                        {
                            keyModel.Active = true;
                            keyModel.FingerNum = keyValuePair.Key.FingerNum;
                            keyModel.StaffNumber = keyValuePair.Key.Staff;
                        }
                        else
                        {
                            keyModel.Active = false;
                            keyModel.FingerNum = 0;
                        }
                       
                    }
                }
            }


            Redraw();
  
        }

        private void Redraw()
        {
            foreach (var octaveModel in PianoModel.OctaveModelList)
            {
                foreach (var keyModel in octaveModel.KeyModelList)
                {
                    keyModel.KeyRect.Dispatcher.BeginInvoke((Action)(() => keyModel.ColorUpdate()));
                }
            }
        }

        public DockPanel DrawPianoController()
        {
            if (PianoModel == null)
            {
                Console.WriteLine("No piano found");
                return null;
            }
            else
            {
                Console.WriteLine("Piano found");
                return PianoModel.DrawPianoModel();
            }
        }

    }
}
