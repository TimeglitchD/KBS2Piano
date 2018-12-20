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
        public bool show = true;

        private Grid myGrid = new Grid();

        private metronomeSound metronome;

        private KeyboardController kC;

        Button startBtn = new Button();
        Button resetButton = new Button();
        private MusicPieceController mPc;

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            PianoController pC = new PianoController() { };

            SheetController sC = new SheetController();
            MidiController mC = new MidiController();

            sC.MidiController = mC;

            kC = new KeyboardController() { PianoController = pC };
            mPc = new MusicPieceController() { Piano = pC, SheetController = sC, MidiController = mC, KeyboardController = kC };

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
                ShowGridLines = false
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
            RowDefinition rowDef4 = new RowDefinition();

            // Add lenght to rows
            rowDef1.Height = new GridLength(50, GridUnitType.Star);
            rowDef2.Height = new GridLength(500, GridUnitType.Star);
            rowDef3.Height = new GridLength(20, GridUnitType.Star);
            rowDef4.Height = new GridLength(200, GridUnitType.Star);

            // Add row to mainGrid
            myGrid.RowDefinitions.Add(rowDef1);
            myGrid.RowDefinitions.Add(rowDef2);
            myGrid.RowDefinitions.Add(rowDef3);
            myGrid.RowDefinitions.Add(rowDef4);


        }

        private void countdownFinished(object sender, EventArgs e)
        {
            mPc.Guide.guideStopped += guideStopped;
            this.Dispatcher.Invoke(() =>
            {
                bv.EnableStartBtn();
                bv.CheckPause();
                bv.EnableStopBtn();
                bv.EnableResetBtn();
            });

            //start guide here
            if (mPc.Guide.Score == null)
            {
                return;

            }
            if (bv._isStarted)
            {
                sv.MusicPieceController.Guide.Pause();
            }
            else if (!bv._isStarted)
            {
                sv.MusicPieceController.Guide.Start();
                bv._isStarted = true;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            kC.KeyDown(e);

            // ... Test for F5 key.
            if (e.Key == Key.Space)
            {
                bv.TriggerStartBtnBySpaceKeyDown();
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            kC.KeyUp(e);
        }


        private void CountDownTickElapsed(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(bv.AddCountdownText);
        }

        private void pianoStateChanged(object sender, EventArgs e)
        {
            if (bv.pianoEnabled)
            {
                myGrid.RowDefinitions[3].Height = new GridLength(200, GridUnitType.Star);
                myGrid.RowDefinitions[1].Height = new GridLength(500, GridUnitType.Star);
            }
            else
            {
                myGrid.RowDefinitions[3].Height = new GridLength(0);
                myGrid.RowDefinitions[1].Height = new GridLength(700, GridUnitType.Star);
            }
        }

        private void guideStopped(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                bv._isStarted = false;
                bv.DisableStartBtn();
                //bv.DisableStopBtn();
            });
            metronome.stopMetronome();
        }
    }

    public enum NoteType
    {
        Quarter,
        Half,
        Whole
    }
}