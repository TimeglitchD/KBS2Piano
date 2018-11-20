using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PianoApp.Models
{
    abstract public class KeyModel
    {
        public int Alter { get; set; } = 0;
        public Step Step { get; set; }
        public bool Active { get; set; } = false;

        abstract public Rectangle Draw();
    }
}