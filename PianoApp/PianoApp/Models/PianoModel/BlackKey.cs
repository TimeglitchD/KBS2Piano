using MusicXml.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
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

            IdleColor = Brushes.Black;
            ActiveStaffOneColor = Brushes.MediumBlue;
            ActiveStaffTwoColor = Brushes.DarkOrchid;
            ActiveColor = Brushes.DarkSlateGray;

        }

        public override Rectangle Draw(float width)
        {
            Color();
            KeyRect.Width = width / 2;
            KeyRect.Height = 115;
            return KeyRect;
        }

        //Updates color based on state.
        public override void ColorUpdate()
        {
            if (StaffNumber == 1 && Active)
            {
                KeyRect.Fill = ActiveStaffOneColor;
            }
            else if (StaffNumber == 2 && Active)
            {
                KeyRect.Fill = ActiveStaffTwoColor;
            }
            else if (Active)
            {
                KeyRect.Fill = ActiveColor;
            }
            else
            {
                Color();
            }
        }

        //Color the keys with idle color.
        public override void Color()
        {
            KeyRect.Fill = IdleColor;
        }
    }
}
