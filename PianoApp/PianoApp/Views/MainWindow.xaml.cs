using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MusicXml;
using PianoApp.Controllers;
using PianoApp.Views;
using PianoApp.Models;
using PianoApp.Models.Exception;

namespace PianoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MusicChooseView mCv;
        private PianoView pv;
        private StaveView sv;

        private StackPanel staves = new StackPanel();
        private StackPanel piano = new StackPanel();

        private NoteView nv;
        public ButtonView bv;
        
        private Grid myGrid = new Grid();

        private metronomeSound metronome;

        Button startBtn = new Button();
        Button resetButton = new Button();
        private MusicPieceController mPc;

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            PianoController pC = new PianoController();
            mPc = new MusicPieceController() { Piano = pC };

            //mPc.Guide.Start();
            DrawMenu();
            InitializeComponent();
            Show();
        }

        private void DrawStaves()
        {
            sv = new StaveView(myGrid);

            metronomeSound sound = new metronomeSound();
            sound.startMetronome(120, 4, 1);
        }

        private void DrawMenu()
        {
            // Create the Grid
            myGrid = new Grid
            {
                ShowGridLines = true
            };

            // show menuGrid lines
            //menuGrid.ShowGridLines = true;

            // Define all rows for mainGrid
            DefineRowMyGrid();
            //Create the staves
            pv = new PianoView(myGrid);
            sv = new StaveView(myGrid);
            Content = pv.myGrid;


            //Draw the notes
            nv = new NoteView(sv);
            nv.DrawNotes();

            bv = new ButtonView(myGrid, sv, nv);
            bv.pianoStateChanged += pianoStateChanged;

            metronome = bv.metronome;
            metronome.countdownFinished += countdownFinished;
        }

        private void DefineRowMyGrid()
        {
            // Define new row
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();

            // Add lenght to rows
            rowDef1.Height = new GridLength(50, GridUnitType.Star);
            rowDef2.Height = new GridLength(500, GridUnitType.Star);
            rowDef3.Height = new GridLength(200, GridUnitType.Star);

            // Add row to mainGrid
            myGrid.RowDefinitions.Add(rowDef1);
            myGrid.RowDefinitions.Add(rowDef2);
            myGrid.RowDefinitions.Add(rowDef3);

            
        }

        private void countdownFinished(object sender, EventArgs e)
        {
            //start guide here
            MessageBox.Show("countdown finished");
            if(mPc.Guide.Score == null)
            {
                Console.WriteLine("Doet niet");
                return;
            }
            mPc.Guide.Start();
        }

        private void pianoStateChanged(object sender, EventArgs e)
        {
            if(bv.pianoEnabled)
            {
                pv.showPianoView();
                myGrid.RowDefinitions[2].Height = new GridLength(200, GridUnitType.Star);
            } else
            {
                pv.hidePianoView();
                myGrid.RowDefinitions[2].Height = new GridLength(0);
            }
        }
    }

    public enum NoteType
    {
        Quarter,
        Half,
        Whole
    }
}