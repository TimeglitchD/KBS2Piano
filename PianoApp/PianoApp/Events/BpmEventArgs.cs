using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoApp.Events
{
    public class BpmEventArgs : EventArgs
    {
        public int bpm { get; set; }
        public string Id { get; set; }
    }
}
