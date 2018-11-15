using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoApp.Models
{
    class PianoModel
    {
        public List<OctaveModel> OctaveModelList { get; set; } = new List<OctaveModel>();
        public int Amount { get; set; } = 6;

        public PianoModel()
        {
            for (int i = 0; i < Amount; i++)
            {
                OctaveModelList.Add(new OctaveModel(){Position = i});
            }
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
    