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
    public partial class PianoView : Window
    {
        private PianoController pC { get; }
        private StackPanel piano = new StackPanel();

        public PianoView(Grid myGrid)
        {
            PianoController pC = new PianoController();
            piano = new StackPanel();


            //zet stackpanel in de goede plek op het grid
            Grid.SetRow(piano, 2);
        }

        public void DrawPiano()
        {
            //clear the view
            piano.Children.Clear();

            //draw the piano
            piano = pC.DrawPianoModel();
            Console.WriteLine("Piano drawn");
        }
    }
}
