using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            whiteKey.Stroke = Brushes.Black;
            whiteKey.Width = width;
            whiteKey.Height = 182;
            return whiteKey;
        }

        public override void Color()
        {
            if (Active)
            {
                whiteKey.Fill = Brushes.Aquamarine;
            }
            else
            {
                whiteKey.Fill = Brushes.FloralWhite;
            }

            if(FingerNum > 0)
            {
                Label fingerNumLabel = new Label();

                whiteKey.SetValue(Grid.ColumnProperty, 1);
                fingerNumLabel.FontSize = 40;
                fingerNumLabel.Margin = new Thickness(5, 0, 5, -30);
                fingerNumLabel.Content = FingerNum.ToString();
                fingerNumLabel.Background = Brushes.Aquamarine;
                BitmapCacheBrush bcb = new BitmapCacheBrush(fingerNumLabel);
                whiteKey.Fill = bcb;
            }
        }
    }
}
