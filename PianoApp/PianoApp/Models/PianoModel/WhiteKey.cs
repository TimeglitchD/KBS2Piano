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
        public Rectangle keyRect;

        public WhiteKey()
        {
            base.type = Type;
            keyRect = base.KeyRect;
        }

        public override Rectangle Draw(float width)
        {
            Color();
            keyRect.Stroke = System.Windows.Media.Brushes.Black;
            keyRect.Width = width;
            keyRect.Height = 171;
            return keyRect;
        }

        public override void Color()
        {
            if (Active)
            {
                keyRect.Fill = System.Windows.Media.Brushes.Red;
            }
            else
            {
                keyRect.Fill = System.Windows.Media.Brushes.FloralWhite;
            }
        }
    }
}
