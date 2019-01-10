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
    public class KeyModel
    {
        public int Alter { get; set; } = 0;
        public Step Step { get; set; }
        public bool Active { get; set; } = false;
        public string type { get; set; }
        public Rectangle KeyRect { get; set; } = new Rectangle();
        public int FingerNum { get; set; } = 0;
        public int StaffNumber;
        public bool FingerSettingEnabled = true;
        public int KeyNumber { get; set; }
        public SolidColorBrush IdleColor { get; set; }
        public SolidColorBrush ActiveStaffOneColor { get; set; }
        public SolidColorBrush ActiveStaffTwoColor { get; set; }
        public SolidColorBrush ActiveColor { get; set; }

        public virtual Rectangle Draw(float width)
        {
            return null;
        }
       public virtual void Color()
        {

        }

        public virtual void ColorUpdate()
        {

        }

    }
}

