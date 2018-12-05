using PianoApp.Controllers;
using PianoApp.Models.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PianoApp.Views
{
    public class ButtonView
    {
        private MusicChooseView mCv;
        private Grid myGrid;
        private Grid menuGrid = new Grid();
        private StaveView sv;
        private NoteView nv;
        private MusicPieceController mPc;

        private TextBox bpmTB = new TextBox();
        private ComboBox notesCB = new ComboBox();
        private bool paused = false;
        private Button startBtn = new Button();
        private Button StopBtn = new Button();
        private Button resetButton = new Button();
        private Button SelectSheetMusic = new Button();

        public metronomeSound metronome = new metronomeSound();
        private bool metronomeEnabled = false;
        private Button metronomeButton;
        public bool _isStarted = false;

        private Button pianoButton;
        public bool pianoEnabled = true;
        public event EventHandler pianoStateChanged;

        float bpmValue = -1.0f;

        private TextBlock pianoText;
        private TextBlock metronomeText;
        private TextDecorationCollection strikeTrough = new TextDecorationCollection();

        private bool running = false;

        public ButtonView(Grid myGrid, StaveView sv, NoteView nv)
        {
            this.myGrid = myGrid;
            this.nv = nv;
            this.sv = sv;
            this.mPc = sv.MusicPieceController;
            //menuGrid.ShowGridLines = true;
            // Define all columns for menuGrid
            DefineGridRowsMenuGrid();

            // Draw menu items
            DrawBpmMenu();

            drawMetronomeMenu();

            drawPianoMenu();

            // Start button
            startBtn.FontSize = 25;
            startBtn.Name = "startBtn";
            startBtn.Content = "▶";
            startBtn.Width = 40;
            startBtn.Height = 40;
            startBtn.HorizontalAlignment = HorizontalAlignment.Center;
            startBtn.Click += StartBtn_Click;
            Grid.SetColumn(startBtn, 5);

            StopBtn.FontSize = 25;
            StopBtn.Name = "stopBtn";
            StopBtn.Content = "◼";
            StopBtn.Width = 40;
            StopBtn.Height = 40;
            StopBtn.HorizontalAlignment = HorizontalAlignment.Center;
            StopBtn.Click += StopBtn_Click;
            Grid.SetColumn(StopBtn, 6);


            // Add the button to the Grid
            SelectSheetMusic = new Button();
            SelectSheetMusic.Name = "SelectSheetMusic";
            SelectSheetMusic.Content = "Selecteer \n muziekstuk";

            // Set height/width based on column height/widht
            SelectSheetMusic.Width = menuGrid.ColumnDefinitions[0].Width.Value - 15;
            SelectSheetMusic.Height = 40;

            // Center and stick to bottom in column
            SelectSheetMusic.HorizontalAlignment = HorizontalAlignment.Left;
            SelectSheetMusic.VerticalAlignment = VerticalAlignment.Bottom;
            SelectSheetMusic.Click += SelectSheetMusic_Click;
            Grid.SetColumn(SelectSheetMusic, 0);

            // Add the TextBlock elements to the Grid Children collection
            menuGrid.Children.Add(startBtn);
            menuGrid.Children.Add(StopBtn);

            // Add menuGrid to myGrid
            Grid.SetRow(menuGrid, 0);
            menuGrid.Children.Add(SelectSheetMusic);
            myGrid.Children.Add(menuGrid);
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SelectSheetMusic_Click(object sender, RoutedEventArgs e)
        {
            mCv = new MusicChooseView(sv, nv);
            mCv.Show();
        }

        public void TriggerStartBtnBySpaceKeyDown()
        {
            StartButtonFunc();
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            StartButtonFunc();
        }

        private void StartButtonFunc()
        {
            try
            {
                if (mPc.Guide.Score != null) ;
            }
            catch (Exception)
            {
                MessageBox.Show("Je hebt geen muziekstuk ingeladen!", "Foutmelding");
                return;
            }

            try
            {
                // Set value to int
                bpmValue = (float)int.Parse(bpmTB.Text);

                // Set values in GuideController
                mPc.Guide.Bpm = bpmValue;
                mPc.Guide.SetNote(notesCB.Text);

                // Check if piece is starten
                if (_isStarted)
                {
                    // If piece is paused, check if piece is paused
                    if (mPc.Guide.paused)
                    {
                        // If paused continu piece by metronome
                        if (metronomeEnabled)
                        {
                            metronome.startMetronome(bpmValue, 4, 1);
                        }
                        else
                        {
                            metronome.startMetronomeCountDownOnly(bpmValue, 4, 1);
                        }
                    }
                    // If piece is not paused, pause piece
                    else
                    {
                        mPc.Guide.Pause();
                    }
                }
                else
                {
                    //set value in metronome and start it.
                    if (metronomeEnabled)
                    {
                        metronome.startMetronome(bpmValue, 4, 1);
                    }
                    else
                    {
                        metronome.startMetronomeCountDownOnly(bpmValue, 4, 1);
                    }
                }

                // Set value startbutton
                CheckPause();
            }
            catch (FormatException)
            {
                MessageBox.Show("De ingevoerde waarde moet een getal zijn!");
                return;
            }
            catch (BpmOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            // Set buttons enabled false or readonly if playing
            resetButton.IsEnabled = true;
            notesCB.IsEnabled = false;
            bpmTB.IsReadOnly = true;
            SelectSheetMusic.IsEnabled = false;
            metronomeButton.IsEnabled = false;
            bpmTB.Background = Brushes.LightGray;
        }

        private void DrawBpmMenu()
        {
            // Add the first text cell to the Grid
            TextBlock txt1 = new TextBlock();
            txt1.Text = "BPM =";
            txt1.FontSize = 30;
            txt1.HorizontalAlignment = HorizontalAlignment.Right;
            txt1.VerticalAlignment = VerticalAlignment.Bottom;
            txt1.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt1, 0);
            Grid.SetColumn(txt1, 1);

            // Add textbox to set bpm
            bpmTB = new TextBox();
            bpmTB.Width = menuGrid.ColumnDefinitions[1].Width.Value - 10;
            bpmTB.Height = 40;
            bpmTB.HorizontalAlignment = HorizontalAlignment.Left;
            bpmTB.VerticalAlignment = VerticalAlignment.Bottom;
            bpmTB.FontSize = 30;
            bpmTB.Name = "bpmTB";
            bpmTB.Text = "60";
            bpmTB.Height = 40;
            Grid.SetColumn(bpmTB, 2);

            // Add combobox to set bpm to notes
            notesCB = new ComboBox();
            notesCB.Height = 40;
            notesCB.FontSize = 20;

            notesCB.Width = menuGrid.ColumnDefinitions[3].Width.Value - 20;
            notesCB.HorizontalAlignment = HorizontalAlignment.Left;
            notesCB.VerticalAlignment = VerticalAlignment.Bottom;

            // Add items to combobox
            notesCB.Items.Add("Hele noot");
            notesCB.Items.Add("Halve noot");
            notesCB.Items.Add("Kwart noot");

            // Set first item selected
            notesCB.SelectedIndex = 2;
            Grid.SetColumn(notesCB, 3);

            // Create resetbutton if isPlaying
            resetButton = new Button();
            resetButton.FontSize = 25;
            resetButton.Name = "resetBtn";
            resetButton.Content = "⟲";
            resetButton.Width = 40;
            resetButton.Height = 40;
            resetButton.HorizontalAlignment = HorizontalAlignment.Right;
            resetButton.Click += ResetButton_Click;
            resetButton.IsEnabled = false;
            Grid.SetColumn(resetButton, 4);

            // Add items to grid
            menuGrid.Children.Add(txt1);
            menuGrid.Children.Add(bpmTB);
            menuGrid.Children.Add(notesCB);
            menuGrid.Children.Add(resetButton);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            mPc.Guide.paused = false;
            _isStarted = false;
            mPc.Guide.ResetMusicPiece();
            metronome.stopMetronome();
            CheckPause();

            if (metronomeEnabled)
            {
                metronome.startMetronome(bpmValue, 4, 1);
            }
            else
            {
                metronome.startMetronomeCountDownOnly(bpmValue, 4, 1);
            }
        }

        private void DefineGridRowsMenuGrid()
        {
            // Create new Columns
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            ColumnDefinition colDef4 = new ColumnDefinition();
            ColumnDefinition colDef5 = new ColumnDefinition();
            ColumnDefinition colDef6 = new ColumnDefinition();
            ColumnDefinition colDef7 = new ColumnDefinition();
            ColumnDefinition colDef8 = new ColumnDefinition();
            ColumnDefinition colDef9 = new ColumnDefinition();

            // Set Lenght of the columns
            colDef1.Width = new GridLength(100, GridUnitType.Star);
            colDef2.Width = new GridLength(110, GridUnitType.Star);
            colDef3.Width = new GridLength(100, GridUnitType.Star);
            colDef4.Width = new GridLength(160, GridUnitType.Star);
            colDef5.Width = new GridLength(80, GridUnitType.Star);
            colDef6.Width = new GridLength(50, GridUnitType.Star);
            colDef7.Width = new GridLength(50, GridUnitType.Star);
            colDef8.Width = new GridLength(50, GridUnitType.Star);
            colDef9.Width = new GridLength(500, GridUnitType.Star);

            // Add columns to menuGrid
            menuGrid.ColumnDefinitions.Add(colDef1);
            menuGrid.ColumnDefinitions.Add(colDef2);
            menuGrid.ColumnDefinitions.Add(colDef3);
            menuGrid.ColumnDefinitions.Add(colDef4);
            menuGrid.ColumnDefinitions.Add(colDef5);
            menuGrid.ColumnDefinitions.Add(colDef6);
            menuGrid.ColumnDefinitions.Add(colDef7);
            menuGrid.ColumnDefinitions.Add(colDef8);
            menuGrid.ColumnDefinitions.Add(colDef9);
        }

        private bool CheckPause()
        {
            if (paused)
            {
                paused = false;
                startBtn.Content = "▶";
            }
            else
            {
                paused = true;
                startBtn.Content = "❚❚";
            }
            return paused;
        }

        public void drawMetronomeMenu()
        {
            metronomeText = new TextBlock();
            metronomeText.Text = "🔇";
            metronomeButton = new Button();
            metronomeButton.Content = metronomeText;
            metronomeButton.Width = 40;
            metronomeButton.Height = 40;
            metronomeButton.FontSize = 25;
            metronomeButton.Click += onMetronomeButtonClick;
            metronomeButton.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetColumn(metronomeButton, 7);
            menuGrid.Children.Add(metronomeButton);
        }

        public void drawPianoMenu()
        {
            TextDecoration strikeTroughDecoration = new TextDecoration();
            strikeTroughDecoration.Location = TextDecorationLocation.Strikethrough;
            strikeTroughDecoration.Pen = new Pen(Brushes.Red, 3);
            strikeTrough.Add(strikeTroughDecoration);
            pianoText = new TextBlock();
            pianoText.Text = "🎹";
            pianoText.TextDecorations = strikeTrough;
            pianoButton = new Button();
            pianoButton.Content = pianoText;
            pianoButton.Width = 40;
            pianoButton.Height = 40;
            pianoButton.FontSize = 20;
            pianoButton.Click += onPianoButtonClick;
            pianoButton.HorizontalAlignment = HorizontalAlignment.Center;
            pianoButton.VerticalAlignment = VerticalAlignment.Bottom;
            Grid.SetColumn(pianoButton, 8);
            menuGrid.Children.Add(pianoButton);
        }

        private void onMetronomeButtonClick(object sender, RoutedEventArgs e)
        {
            if (!metronomeEnabled)
            {
                
                metronomeText.Text = "🔉";
                metronomeEnabled = true;
            }
            else
            {
                
                metronomeText.Text = "🔇";
                metronomeEnabled = false;
            }
        }

        private void onPianoButtonClick(object sender, RoutedEventArgs e)
        {
            if (pianoEnabled)
            {
                pianoEnabled = false;
                pianoText.TextDecorations = null;
            }
            else
            {
                pianoEnabled = true;
                pianoText.TextDecorations = strikeTrough;
            }

            //fire event after changing boolean to get correct state
            if (pianoStateChanged != null)
            {
                pianoStateChanged(this, e);
            }
        }

        public void AddCountdownText()
        {
            TextBlock number = new TextBlock();
            number.Text = (4 - metronome.elapsedBeats).ToString();
            Console.WriteLine(number.Text);
            startBtn.Content = number;
        }
    }
}
