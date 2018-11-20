using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PianoApp.Models
{
    class BlackKey : KeyModel
    {
        public string Type { get; set; } = "Black";

        public override Rectangle Draw()
        {
            Rectangle blackKey = new Rectangle();
            blackKey.Fill = System.Windows.Media.Brushes.Black;
            blackKey.Width = 25;
            blackKey.Height = 60;
            return blackKey;
        }
    }
}
