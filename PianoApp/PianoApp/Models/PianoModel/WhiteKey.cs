using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PianoApp.Models
{
    public class WhiteKey : KeyModel
    {
        public string Type { get; set; } = "White";
        public Rectangle whiteKey { get; set; } = new Rectangle();

        public WhiteKey()
        {
            base.type = Type;
        }

        public override Rectangle Draw(float width)
        {
            Color();
            whiteKey.Stroke = System.Windows.Media.Brushes.Black;
            whiteKey.Width = width;
            whiteKey.Height = 182;
            return whiteKey;
        }

        public override void Color()
        {
            if (Active)
            {
                whiteKey.Fill = System.Windows.Media.Brushes.Aquamarine;
            }
            else
            {
                whiteKey.Fill = System.Windows.Media.Brushes.FloralWhite;
            }
        }
    }
}
