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
using System.Threading;

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
        private NonKeyboardInputController nKiC = new NonKeyboardInputController();

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            PianoController pC = new PianoController() {NonKeyboardInputController = nKiC};
            SheetController sC = new SheetController();
            MidiController mC = new MidiController();
            mPc = new MusicPieceController() { Piano = pC , SheetController = sC , MidiController = mC};

            //mPc.Guide.Start();
            DrawMenu();
            InitializeComponent();
            Show();
        }

        private void DrawStaves()
        {
            sv = new StaveView(myGrid, mPc);

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
            pv = new PianoView(myGrid, mPc);
            sv = new StaveView(myGrid, mPc);
            Content = pv.myGrid;


            //Draw the notes
            nv = new NoteView(sv);
            nv.DrawNotes();

            bv = new ButtonView(myGrid, sv, nv);
            bv.pianoStateChanged += pianoStateChanged;

            metronome = bv.metronome;
            metronome.countdownFinished += countdownFinished;
            metronome.countDownTickElapsed += CountDownTickElapsed;
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
            if (mPc.Guide.Score == null)
            {
                Console.WriteLine("Error");
                return;
            }
            sv.MusicPieceController.Guide.Start();
        }

        private void CountDownTickElapsed(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(bv.AddCountdownText);
        }

        private void pianoStateChanged(object sender, EventArgs e)
        {
            if(bv.pianoEnabled)
            {
                myGrid.RowDefinitions[2].Height = new GridLength(200, GridUnitType.Star);
                myGrid.RowDefinitions[1].Height = new GridLength(500, GridUnitType.Star);
            } else
            {
                myGrid.RowDefinitions[2].Height = new GridLength(0);
                myGrid.RowDefinitions[1].Height = new GridLength(700, GridUnitType.Star);
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