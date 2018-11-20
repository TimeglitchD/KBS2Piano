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
        private StackPanel staves = new StackPanel();
        public MusicPieceController MPc { get;}

        public StaveView(Grid myGrid)
        {
            PianoController pC = new PianoController();
            MPc = new MusicPieceController() { Piano = pC };
            staves = new StackPanel();
            DrawStaves();

            //scrollbar
            ScrollViewer scroll = new ScrollViewer();
            scroll.Visibility = Visibility.Visible;
            Grid.SetRow(scroll, 1);

            //zet de stackpanel in de goede plek in het grid
            Grid.SetRow(staves, 1);

            //koppel de scrollbar aan het stackpanel
            scroll.Content = staves;

            myGrid.Children.Add(scroll);
        }

        public void DrawStaves()
        {
            staves.Children.Clear();
            if (MPc.Sheet != null)
            {
                Console.WriteLine("Piece found!");
                foreach (var item in MPc.Sheet.GreatStaffModelList)
                {
                    TextBlock tb = new TextBlock();
                    tb.Height = 50;
                    tb.Text = "great stave";
                    staves.Children.Add(tb);
                    Console.WriteLine("Stave added");
                }
            }
            else
            {
                Console.WriteLine("No piece found.");
                TextBlock tb = new TextBlock();
                tb.Height = 50;
                tb.Text = "great stave";
                staves.Children.Add(tb);
                Console.WriteLine("Stave added");
            }
        }


    }
}