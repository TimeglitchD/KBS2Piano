using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PianoApp.Models
{
    public class PianoModel
    {
        public List<OctaveModel> OctaveModelList { get; set; } = new List<OctaveModel>();
        public int Amount { get; set; } = 9;
        private StackPanel piano = new StackPanel();

        public PianoModel()
        {
            for (int i = 0; i < Amount; i++)
            {
                OctaveModelList.Add(new OctaveModel(i));
            }
            
        }

        public StackPanel DrawPiano()
        {
            foreach(OctaveModel octaveModel in OctaveModelList)
            {
                piano.Children.Add(octaveModel.DrawOctave());
                Console.WriteLine("Piano added");
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
    