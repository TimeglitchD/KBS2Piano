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


            if (FingerNum > 0)
            {
                // set color
                SolidColorBrush Color = Brushes.Aquamarine;

                // create new canvas
                Canvas canvas = new Canvas();
                canvas.Background = Color;
                canvas.Height = 600;
                canvas.Width = 300;

                // create new label
                Label fingerNumLabel = new Label();
                fingerNumLabel.FontWeight = FontWeights.Bold;

                fingerNumLabel.FontSize = 120;
                fingerNumLabel.Height = 200;
                fingerNumLabel.Width = 200;
                fingerNumLabel.Content = FingerNum.ToString();
                fingerNumLabel.Background = Color;
                fingerNumLabel.Margin = new Thickness(91, 400, 0, 0);

                // add label to canvas
                canvas.Children.Add(fingerNumLabel);

                // add canvas to bipmapcachebrush
                BitmapCacheBrush bcb = new BitmapCacheBrush(canvas);
                whiteKey.Fill = bcb;
            }
        }
    }
}
