using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MusicXml.Domain;

namespace PianoApp.Models
{
    public class PianoModel
    {
        public List<OctaveModel> OctaveModelList { get; set; } = new List<OctaveModel>();
        public int Amount { get; set; } = 5;
        public int Center { get; set; } = 0;
        private DockPanel piano = new DockPanel();


        public PianoModel()
        {
            if (Amount % 2 == 0)
            {
                Center = Amount / 2;
            }
            else
            {
                Center = Amount / 2 + 1;
            }
            Center = 4 - Center;

            piano.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            for (int i = 0; i < Amount; i++)
            {
                OctaveModelList.Add(new OctaveModel(i+Center));
            }
            
        }

        public DockPanel DrawPianoModel()
        {
            float width = (float)1266 / (Amount * 7);
            foreach(OctaveModel octaveModel in OctaveModelList)
            {
                piano.Children.Add(octaveModel.DrawOctave(width));
                Console.WriteLine("Octave added");
            }

            return piano;
        }
    }

    public enum Step
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        F = 5,
        G = 6
    }


}
    