using MusicXml.Domain;
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

        public WhiteKey()
        {
            base.type = Type;
        }

        public override Rectangle Draw(float width)
        {
            Color();
            KeyRect.Stroke = System.Windows.Media.Brushes.Black;
            KeyRect.Width = width;
            KeyRect.Height = 182;
            return KeyRect;
        }

        public override void ColorUpdate()
        {
            if (StaffNumber == 1 && Active)
            {
                KeyRect.Fill = System.Windows.Media.Brushes.Aquamarine;
            }
            else if (StaffNumber == 2 && Active)
            {
                KeyRect.Fill = System.Windows.Media.Brushes.DarkOrchid;
            }
            else
            {
                Color();
            }
        }


        public override void Color()
        {
            KeyRect.Fill = System.Windows.Media.Brushes.FloralWhite;
        }
    }
}
