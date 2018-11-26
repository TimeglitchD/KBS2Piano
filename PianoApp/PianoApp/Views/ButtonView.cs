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
        private Button resetButton = new Button();
        private Button SelectSheetMusic = new Button();

        public metronomeSound metronome = new metronomeSound();
        private bool metronomeEnabled = false;
        private Button metronomeButton;

        float bpmValue = -1.0f;


        public ButtonView(Grid myGrid, StaveView sv, NoteView nv)
        {
            this.myGrid = myGrid;
            this.nv = nv;
            this.sv = sv;
            this.mPc = sv.MusicPieceController;

            // Define all columns for menuGrid
            DefineGridRowsMenuGrid();

            // Draw menu items
            DrawBpmMenu();

            drawMetronomeMenu();

            // Start button
            
            startBtn.FontSize = 25;
            startBtn.Name = "startBtn";
            startBtn.Content = "▶";
            startBtn.Width = 40;
            startBtn.Height = 40;
            startBtn.HorizontalAlignment = HorizontalAlignment.Center;
            startBtn.Click += StartBtn_Click;
            Grid.SetColumn(startBtn, 6);


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

            // Add menuGrid to myGrid
            Grid.SetRow(menuGrid, 0);
            menuGrid.Children.Add(SelectSheetMusic);
            myGrid.Children.Add(menuGrid);
        }
        private void SelectSheetMusic_Click(object sender, RoutedEventArgs e)
        {
            mCv = new MusicChooseView(sv, nv);
            mCv.Show();
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {

            if (mPc.Guide.Score == null)
            {
                MessageBox.Show("Je hebt geen muziekstuk ingeladen!", "Foutmelding");
                return;
            }
            try
            {
                // Set value to int
                bpmValue = (float)int.Parse(bpmTB.Text);
                //bpmValue = (float)bpmValueInt;
                // Set values in GuideController
                mPc.Guide.Bpm = bpmValue;
                mPc.Guide.SetNote(notesCB.Text);

                if (!paused)
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
                else
                {
                    mPc.Guide.Stop();
                }
                
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
            
            CheckPause();
            Console.WriteLine(mPc.Guide.Bpm);
            Console.WriteLine(mPc.Guide.Note);

            // Set buttons enabled false or readonly if playing
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
            bpmTB.Height = myGrid.RowDefinitions[1].Height.Value;
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
            resetButton.Content = "◼";
            resetButton.Width = 40;
            resetButton.Height = 40;
            resetButton.HorizontalAlignment = HorizontalAlignment.Right;
            resetButton.Click += ResetButton_Click;
            Grid.SetColumn(resetButton, 5);

            // Add items to grid
            menuGrid.Children.Add(txt1);
            menuGrid.Children.Add(bpmTB);
            menuGrid.Children.Add(notesCB);
            menuGrid.Children.Add(resetButton);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Muziekstuk is gestopt....!");

            //// Set buttons enabled or readonly false if resetting music
            bpmTB.IsReadOnly = false;
            bpmTB.Background = Brushes.White;
            notesCB.IsEnabled = true;
            metronomeButton.IsEnabled = true;
            metronome.stopMetronome();

            mPc.Guide.Start();
            //start countdown and start guide from beginning
            if (metronomeEnabled)
            {
                metronome.startMetronome(bpmValue, 4, 1);
            } else
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
            colDef2.Width = new GridLength(100, GridUnitType.Star);
            colDef3.Width = new GridLength(100, GridUnitType.Star);
            colDef4.Width = new GridLength(160, GridUnitType.Star);
            colDef5.Width = new GridLength(120, GridUnitType.Star);
            colDef6.Width = new GridLength(100, GridUnitType.Star);
            colDef7.Width = new GridLength(100, GridUnitType.Star);
            colDef8.Width = new GridLength(100, GridUnitType.Star);
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

        private void CheckPause()
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
        }

        public void drawMetronomeMenu()
        {
            metronomeButton = new Button();
            metronomeButton.Width = menuGrid.ColumnDefinitions[4].Width.Value -10;
            metronomeButton.Height = 40;
            metronomeButton.Content = "Metronoom: Uit";
            metronomeButton.Click += onMetronomeButtonClick;
            metronomeButton.HorizontalAlignment = HorizontalAlignment.Right;
            metronomeButton.VerticalAlignment = VerticalAlignment.Bottom;
            Grid.SetColumn(metronomeButton, 4);
            menuGrid.Children.Add(metronomeButton);

        }

        private void onMetronomeButtonClick(object sender, RoutedEventArgs e)
        {
            if (metronomeEnabled)
            {
                metronomeEnabled = false;
                metronomeButton.Content = "Metronoom: Uit";
            }
            else
            {
                metronomeEnabled = true;
                metronomeButton.Content = "Metronoom: Aan";
            }
        }
    }
}
