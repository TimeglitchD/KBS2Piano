using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PianoApp.Models
{
    public static class ScoreLabel
    {
        public static Label ScoreLabelObj { get; set; }
        public static int Score { get; set; } = 0;

        public static Label DrawScore()
        {
            ScoreLabelObj = new Label()
            {
                Content = Convert.ToString(Score + "%"),
                FontSize = 300,
                Foreground = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontWeight = FontWeights.SemiBold,
                Opacity = 0
            };

            return ScoreLabelObj;
        }

        public static void UpdateScore()
        {
            ScoreLabelObj.Content = Convert.ToString(Score + "%");
            ScoreLabelObj.Opacity = 100;
        }

        public static void HideScore()
        {
            ScoreLabelObj.Opacity = 0;
        }
    }
}
