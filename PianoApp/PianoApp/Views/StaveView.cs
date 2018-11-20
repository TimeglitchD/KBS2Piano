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
        public MusicPieceController MPc { get; }
        private Grid myGrid;

        public StaveView(Grid myGrid)
        {
            this.myGrid = myGrid;
            PianoController pC = new PianoController();
            MPc = new MusicPieceController() { Piano = pC };
            sheet = new StackPanel();
            DrawMusic();

            
        }

        public void DrawMusic()
        {
            //clear the view
            sheet.Children.Clear();
            Console.WriteLine("Sheet cleared.");

            //draw in the new sheet
            sheet = MPc.DrawMusicPiece();
            Console.WriteLine("Sheet drawn.");

            //scrollbar
            ScrollViewer scroll = new ScrollViewer();
            scroll.Visibility = Visibility.Visible;
            Grid.SetRow(scroll, 1);

            //zet de stackpanel in de goede plek in het grid
            Grid.SetRow(sheet, 1);

            //koppel de scrollbar aan het stackpanel
            scroll.Content = sheet;

            myGrid.Children.Add(scroll);

        }


    }
}
