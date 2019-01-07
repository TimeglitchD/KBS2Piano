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
        public bool fingerSettingEnabled = true;

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
                int octave = (int)Math.Floor((float)pressedKey.Key / 12);
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

            foreach (var octaveModel in PianoModel.OctaveModelList.ToList())
            {
                foreach (var keyModel in octaveModel.KeyModelList.ToList())
                {
                    keyModel.Active = false;                      
                }
            }

            var tempDict = new Dictionary<Note, Timeout>(noteAndTimeoutDictionary).ToDictionary(k => k.Key, k => k.Value);
            
//            //go over all keys and compare to note when true set active true on the corresponding key...
//            foreach (var keyValuePair in tempDict)
//            {
//                //.Where(n => n.Position.Equals(keyValuePair.Key.Pitch.Octave))
//                foreach (var octaveModel in PianoModel.OctaveModelList.Where(n => n.Position.Equals(keyValuePair.Key.Pitch.Octave)))
//                {
//                     //Select active keys only
//                    var keyModelList = octaveModel.KeyModelList.Where(n => n.Alter != -1).ToList();
//
//                    for (var i = 1; i < keyModelList.Count - 1; i++)
//                    {
//                        keyModelList[i].fingerSettingEnabled = fingerSettingEnabled;
//
//                        if (keyValuePair.Key.Pitch.Step.ToString() != keyModelList[i].Step.ToString() && keyModelList[i].Alter == 0)
//                        {
//                            var j = i;
//                            if (keyValuePair.Key.Pitch.Alter == -1) j--;                            
//                            else if (keyValuePair.Key.Pitch.Alter == 1) j++;
//
//                            keyModelList[j].Active = false;
//                            keyModelList[j].FingerNum = 0;
//                        }          
//                    }
//                }
//            }

            //go over all keys and compare to note when true set active true on the corresponding key...
            foreach (var keyValuePair in tempDict)
            {
                //.Where(n => n.Position.Equals(keyValuePair.Key.Pitch.Octave))
                foreach (var octaveModel in PianoModel.OctaveModelList.Where(n => n.Position.Equals(keyValuePair.Key.Pitch.Octave)))
                {
                    //Select not active keys only
                    var keyModelList = octaveModel.KeyModelList.Where(n => n.Alter != -1).ToList();

                    for (var i = 0; i < keyModelList.Count; i++)
                    {
                        keyModelList[i].fingerSettingEnabled = fingerSettingEnabled;

                        if (keyValuePair.Key.Pitch.Step.ToString() == keyModelList[i].Step.ToString() && keyModelList[i].Alter == 0)
                        {
                            var j = i;
                            if (keyValuePair.Key.Pitch.Alter == -1) j--;
                            else if (keyValuePair.Key.Pitch.Alter == 1) j++;

                            if (j == -1)
                            {
                                j = 0;
                            }

                            if (j == keyModelList.Count)
                            {
                                j = keyModelList.Count - 1;
                            }

                            keyModelList[j].Active = true;
                            keyModelList[j].FingerNum = keyValuePair.Key.FingerNum;
                            keyModelList[j].StaffNumber = keyValuePair.Key.Staff;
                        }    
                    }
                }
            }

            Redraw();  
        }

        public void Redraw()
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
                return null;
            }
            else
            {
                return PianoModel.DrawPianoModel();
            }
        }
    }
}
