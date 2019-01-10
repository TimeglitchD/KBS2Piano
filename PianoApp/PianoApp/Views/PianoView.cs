using PianoApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PianoApp.Views
{
    /// <summary>
    /// Interaction logic for PianoView.xaml
    /// </summary>
    public class PianoView
    {
        public MusicPieceController MusicPieceController { get; set; }
        private DockPanel piano = new DockPanel();
        public Grid myGrid;

        public PianoView(Grid myGrid, MusicPieceController mPc)
        {
            MusicPieceController = mPc;
            MusicPieceController.Piano.PianoView = this;
            this.myGrid = myGrid;
            DrawPianoView();
        }

        public void DrawPianoView()
        {
            //clear the view
            if (piano != null) piano.Children.Clear();

            //draw the piano
            piano = MusicPieceController.Piano.DrawPianoController();
            //piano.Orientation = Orientation.Horizontal;
            //zet stackpanel in de goede plek op het Grid
            Grid.SetRow(piano, 3);
            
            Border pianoShadow = new Border();
            pianoShadow.Background = Brushes.White;
            pianoShadow.BorderBrush = Brushes.White;
            pianoShadow.BorderThickness = new Thickness(7);
            pianoShadow.Effect = new DropShadowEffect()
            {
                Color = Colors.White,
                Direction = -250,
                BlurRadius = 10,
                ShadowDepth = 10
            };
            pianoShadow.VerticalAlignment = VerticalAlignment.Bottom;
            
            Grid.SetRow(pianoShadow, 2);
            Grid.SetZIndex(pianoShadow, 99999);
            myGrid.Children.Add(pianoShadow);
            myGrid.Children.Add(piano);
            

        }

        public void hidePianoView()
        {
            piano.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void showPianoView()
        {
            piano.Visibility = System.Windows.Visibility.Visible;
        }
    }
}

