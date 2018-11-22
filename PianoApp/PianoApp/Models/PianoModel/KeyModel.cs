using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PianoApp.Models
{
    public class KeyModel
    {
        public int Alter { get; set; } = 0;
        public Step Step { get; set; }
        public bool Active { get; set; } = false;
        public string type { get; set; }

       public virtual Rectangle Draw(float width)
        {
            return null;
        }
       public virtual void Color()
        {

        }
    }
}

