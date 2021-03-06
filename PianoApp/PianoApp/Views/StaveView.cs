﻿using PianoApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PianoApp.Views
{
    public class StaveView
    {
        private StackPanel sheet;
        public MusicPieceController MusicPieceController { get; }
        private Grid myGrid;
        private ScrollViewer scroll;
        private double location;

        public StaveView(Grid myGrid, MusicPieceController mPc)
        {
            this.myGrid = myGrid;
            MusicPieceController = mPc;
            sheet = new StackPanel();
            DrawMusic();

            MusicPieceController.staffEndReached += scrollToNext;
            MusicPieceController.GoToFirstStaff += ScrollToTop;
            MusicPieceController.HoldPosition += ScrollToCurrent;
        }

        public void DrawMusic()
        {
            //clear the view
            sheet.Children.Clear();
            Console.WriteLine("Sheet cleared.");

            //draw in the new sheet
            sheet = MusicPieceController.DrawMusicPiece();
            Console.WriteLine("Sheet drawn.");

            //scrollbar
            scroll = new ScrollViewer();
            scroll.Visibility = Visibility.Visible;
            Grid.SetRow(scroll, 1);

            //zet de stackpanel in de goede plek in het grid
            Grid.SetRow(sheet, 1);

            //koppel de scrollbar aan het stackpanel
            scroll.Content = sheet;

            myGrid.Children.Add(scroll);
            location = 0;
        }

        public void ScrollToTop(object sender, EventArgs e)
        {
            location = 0;

            scroll.Dispatcher.BeginInvoke((Action)(() => scroll.ScrollToVerticalOffset(0)));
            scroll.Dispatcher.BeginInvoke((Action)(() => scroll.UpdateLayout()));
        }

        public void scrollToNext(object sender, EventArgs e)
        {
            location = scroll.VerticalOffset;

            scroll.Dispatcher.BeginInvoke((Action)(() => scroll.ScrollToVerticalOffset(location + 200)));
            scroll.Dispatcher.BeginInvoke((Action)(() => scroll.UpdateLayout()));
            location += 200;

//            scroll.ScrollToVerticalOffset(location + 200);
//            scroll.UpdateLayout();
        }

        public void ScrollToCurrent(object sender, EventArgs e)
        {
            scroll.Dispatcher.BeginInvoke((Action)(() => scroll.ScrollToVerticalOffset(location)));
            scroll.Dispatcher.BeginInvoke((Action)(() => scroll.UpdateLayout()));
        }
    }
}
