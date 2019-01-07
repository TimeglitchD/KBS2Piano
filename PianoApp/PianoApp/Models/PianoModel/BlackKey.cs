using MusicXml.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PianoApp.Models
{
    class BlackKey : KeyModel
    {
        public string Type { get; set; } = "Black";

        public BlackKey()
        {
            base.type = Type;
        }

        public override Rectangle Draw(float width)
        {
            Color();
            KeyRect.Width = width / 2;
            KeyRect.Height = 115;
            return KeyRect;
        }


        public override void ColorUpdate()
        {
            if (StaffNumber == 1 && Active)
            {
                KeyRect.Fill = System.Windows.Media.Brushes.MediumBlue;
            }
            else if (StaffNumber == 2 && Active)
            {
                KeyRect.Fill = System.Windows.Media.Brushes.DarkOrchid;
            }
            else if (Active)
            {
                KeyRect.Fill = System.Windows.Media.Brushes.DarkSlateGray;
            }
            else
            {
                Color();
            }
        }


        public override void Color()
        {
            KeyRect.Fill = System.Windows.Media.Brushes.Black;
        }
    }
}
