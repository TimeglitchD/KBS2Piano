using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PianoApp.Models
{
    public class BlackKey : KeyModel
    {
        public string Type { get; set; } = "Black";
        public Rectangle blackKey { get; set; } = new Rectangle();
        public string color { get; set; }

        public BlackKey()
        {
            base.type = Type;
        }

        public override Rectangle Draw(float width)
        {
            Color();
            blackKey.Width = width/2;
            blackKey.Height = 115;
            //blackKey.Stroke = System.Windows.Media.Brushes.FloralWhite;
            
            //blackKey.StrokeThickness = 5;
            return blackKey;
        }

        public override void Color()
        {
            if (Active)
            {
                blackKey.Fill = System.Windows.Media.Brushes.Aquamarine;
                color = System.Windows.Media.Brushes.Aquamarine.ToString();
            }
            else
            {
                blackKey.Fill = System.Windows.Media.Brushes.Black;
                color = System.Windows.Media.Brushes.Black.ToString();
            }
        }
    }
}
