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

        public void UpdatePianoKeys(Note note)
        {
            //go over all keys and compare to note when true color the key...
            foreach (var keyModel in PianoModel.OctaveModelList[note.Pitch.Octave].KeyModelList)
            {
                if (note.Pitch.Step.ToString() == keyModel.Step.ToString() && note.Pitch.Alter == keyModel.Alter)
                {
                    keyModel.Active = true;
                    Console.WriteLine($"Note {note.Pitch.Step}{note.Pitch.Octave}{note.Pitch.Alter} key pressed: {keyModel.Step}{PianoModel.OctaveModelList[note.Pitch.Octave].Position}{keyModel.Alter}");
                }
            }

            Console.WriteLine("Updating piano keys...");
        }
    }
}
