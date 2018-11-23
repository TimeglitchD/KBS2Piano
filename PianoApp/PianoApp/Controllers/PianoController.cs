using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void UpdatePianoKeys(Dictionary<Note, GuidesController.Timeout> noteAndTimeoutDictionary)
        {
            //go over all keys and compare to note when true set active true on the corresponding key...
            foreach (var keyValuePair in noteAndTimeoutDictionary)
            {
                foreach (var keyModel in PianoModel.OctaveModelList[keyValuePair.Key.Pitch.Octave].KeyModelList)
                {
                    if (keyValuePair.Key.Pitch.Step.ToString() == keyModel.Step.ToString() && keyValuePair.Key.Pitch.Alter == keyModel.Alter)
                    {
                        keyModel.Active = true;
                        keyModel.KeyRect.Dispatcher.BeginInvoke((Action) (() => keyModel.Color()));
                        

                        Console.WriteLine(keyValuePair.Key.XPos);
                        Console.WriteLine($"Note {keyValuePair.Key.Pitch.Step}{keyValuePair.Key.Pitch.Octave}{keyValuePair.Key.Pitch.Alter} key pressed: {keyModel.Step}{PianoModel.OctaveModelList[keyValuePair.Key.Pitch.Octave].Position}{keyModel.Alter}");
                    }
                    else
                    {
                        keyModel.Active = false;
                    }                    
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
