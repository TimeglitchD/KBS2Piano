using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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

        public DockPanel DrawPianoController()
        {
            if(PianoModel == null)
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
