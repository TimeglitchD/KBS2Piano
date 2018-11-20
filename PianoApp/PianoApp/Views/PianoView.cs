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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PianoApp.Views
{
    /// <summary>
    /// Interaction logic for PianoView.xaml
    /// </summary>
    public partial class PianoView
    {
        private PianoController PC { get; }
        private StackPanel piano = new StackPanel();
        public Grid myGrid;


        public PianoView(Grid myGrid)
        {
            PC = new PianoController();
            
           
            this.myGrid = myGrid;
            DrawPiano();


        }

        public void DrawPiano()
        {
            //clear the view
           // piano.Children.Clear();
            //zet stackpanel in de goede plek op het grid
            Grid.SetRow(piano, 2);
            //draw the piano
            piano = PC.DrawPiano();
            myGrid.Children.Add(piano);
            Console.WriteLine("Piano drawn");
        }
    }
}

