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
        public Rectangle blackKey { get; set; } = new Rectangle();

        public BlackKey()
        {
            base.type = Type;
        }

        public override Rectangle Draw(float width)
        {
            Color();
            blackKey.Width = width/2;
            blackKey.Height = 115;
            return blackKey;
        }

        //decide color of key weither it's active or not
        public override void Color()
        {
            if (Active)
            {
                blackKey.Fill = System.Windows.Media.Brushes.Aquamarine;
            }
            else
            {
                blackKey.Fill = System.Windows.Media.Brushes.Black;
            }
        }
    }
}
