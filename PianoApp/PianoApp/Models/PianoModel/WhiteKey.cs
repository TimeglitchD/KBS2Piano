using MusicXml.Domain;
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
            SolidColorBrush color = Brushes.FloralWhite;
            if (StaffNumber == 1 && Active)
            {
                color = Brushes.Aquamarine;
                KeyRect.Fill = color;
                SetFingerNum(color);
            }
            else if (StaffNumber == 2 && Active)
            {
                color = Brushes.DarkOrchid;
                KeyRect.Fill = color;
                SetFingerNum(color);
            }
            else
            {
                Color();
            }
        }

        private void SetFingerNum(SolidColorBrush Color)
        {
            if (FingerNum > 0)
            {
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
                KeyRect.Fill = bcb;
            }
        }


        public override void Color()
        {
            KeyRect.Fill = System.Windows.Media.Brushes.FloralWhite;
        }
    }
}