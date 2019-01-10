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
using PianoApp.Controllers;

namespace PianoApp.Models
{
    public class WhiteKey : KeyModel
    {
        public string Type { get; set; } = "White";

        public WhiteKey()
        {
            base.type = Type;

            IdleColor = Brushes.FloralWhite;
            ActiveStaffOneColor = Brushes.Blue;
            ActiveStaffTwoColor = Brushes.Purple;
            ActiveColor = Brushes.Gray;

        }

        public override Rectangle Draw(float width)
        {
            Color();
            KeyRect.Stroke = System.Windows.Media.Brushes.Black;
            KeyRect.Width = width;
            KeyRect.Height = 182;
            return KeyRect;
        }

        //Updates color based on state.
        public override void ColorUpdate()
        {            
            if (StaffNumber == 1 && Active)
            {
                KeyRect.Fill = ActiveStaffOneColor;
                if (FingerSettingEnabled)
                {
                    SetFingerNum(ActiveStaffOneColor);
                }
            }
            else if (StaffNumber == 2 && Active)
            {
                KeyRect.Fill = ActiveStaffTwoColor;
                if (FingerSettingEnabled)
                {
                    SetFingerNum(ActiveStaffTwoColor);
                }
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

        //Sets the finger numbers.
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

                fingerNumLabel.Foreground = Brushes.White;
                fingerNumLabel.FontSize = 180;
                fingerNumLabel.FontFamily = new FontFamily("Arial");
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

        //Color the keys with idle color.
        public override void Color()
        {
            KeyRect.Fill = IdleColor;
        }
    }
}