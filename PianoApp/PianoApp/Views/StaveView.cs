using PianoApp.Controllers;
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

        public StaveView(Grid myGrid, MusicPieceController mPc)
        {
            this.myGrid = myGrid;
            MusicPieceController = mPc;
            sheet = new StackPanel();
            DrawMusic();

            MusicPieceController.staffEndReached += scrollToNext;
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

        }

        public void scrollToNext(object sender, EventArgs e)
        {
            double location = scroll.VerticalOffset;

            scroll.Dispatcher.BeginInvoke((Action)(() => scroll.ScrollToVerticalOffset(location + 200)));
            scroll.Dispatcher.BeginInvoke((Action)(() => scroll.UpdateLayout()));

//            scroll.ScrollToVerticalOffset(location + 200);
//            scroll.UpdateLayout();
        }
    }
}
