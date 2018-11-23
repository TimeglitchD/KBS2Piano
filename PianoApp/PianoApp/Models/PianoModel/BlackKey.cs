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
        public Rectangle keyRect;

        public BlackKey()
        {
            base.type = Type;
            keyRect = base.KeyRect;
        }

        public override Rectangle Draw(float width)
        {
            Color();
            keyRect.Width = width/2;
            keyRect.Height = 115;
            //blackKey.Stroke = System.Windows.Media.Brushes.FloralWhite;
            
            //blackKey.StrokeThickness = 5;
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
                keyRect.Fill = System.Windows.Media.Brushes.Black;
            }
        }
    }
}
