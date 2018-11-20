using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PianoApp.Models;

namespace PianoApp.Controllers
{
    public class PianoController
    {
        public PianoModel PianoModel { get; set; } = new PianoModel();
        public void UpdatePianoKeys()
        {
            Console.WriteLine("Updating piano keys...");
        }

        public OctaveModel DrawPianoModel()
        {
            
        }
    }
}
