using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicXml.Domain;
using PianoApp.Models;

namespace PianoApp.Controllers
{
    public class PianoController
    {
        public PianoModel PianoModel { get; set; } = new PianoModel();

        public void UpdatePianoKeys(Dictionary<Note, float> noteAndTimeoutDictionary)
        {
            //go over all keys and compare to note when true set active true on the corresponding key...
             foreach (var keyValuePair in noteAndTimeoutDictionary){
                foreach (var keyModel in PianoModel.OctaveModelList[keyValuePair.Key.Pitch.Octave].KeyModelList)
                {
                    if (keyValuePair.Key.Pitch.Step.ToString() == keyModel.Step.ToString() && keyValuePair.Key.Pitch.Alter == keyModel.Alter)
                    {
                        keyModel.Active = true;
                        Console.WriteLine(keyValuePair.Key.XPos);
                        Console.WriteLine($"Note {keyValuePair.Key.Pitch.Step}{keyValuePair.Key.Pitch.Octave}{keyValuePair.Key.Pitch.Alter} key pressed: {keyModel.Step}{PianoModel.OctaveModelList[keyValuePair.Key.Pitch.Octave].Position}{keyModel.Alter}");
                    }
                }                
            }            
        }
    }
}
