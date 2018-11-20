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

        public Rectangle DrawWhiteKey()
        {
            Rectangle whiteKey = new Rectangle();
            whiteKey.Width = 25;
            whiteKey.Height = 60;
            return whiteKey;
        }
    }
}
