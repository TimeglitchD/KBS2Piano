using PianoApp.Controllers;
using PianoApp.Events;
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
        private string _musicPieceId;

        private TextBox bpmTB = new TextBox();
        private ComboBox notesCB = new ComboBox();
        public bool paused = false;
        private Button startBtn = new Button();
        private Button StopBtn = new Button();
        private Button resetButton = new Button();
        private Button SelectSheetMusic = new Button();
        private Button fingerSettingBtn;
        private Button introductionBtn = new Button();
        private TextBlock fingerSettingTxt;
        public bool fingerEnabled = true;

        public metronomeSound metronome = new metronomeSound();
        private bool metronomeEnabled = false;
        private Button metronomeButton;
        public bool _isStarted = false;
        private TextBlock txt1 = new TextBlock();

        private Button pianoButton;
        public bool pianoEnabled = true;
        public event EventHandler pianoStateChanged;

        float bpmValue = -1.0f;

        private TextBlock pianoText;
        private TextBlock metronomeText;
        private TextDecorationCollection strikeTrough = new TextDecorationCollection();

        private bool spaceButtonEnabled = true;

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
            DrawRightMenu();
            DrawLeftMenu();

            Grid.SetRow(menuGrid, 0);

            Grid.SetColumn(SelectSheetMusic, 0);
            Grid.SetColumn(txt1, 1);
            Grid.SetColumn(bpmTB, 2);
            Grid.SetColumn(resetButton, 3);
            Grid.SetColumn(startBtn, 4);
            Grid.SetColumn(StopBtn, 5);
            Grid.SetColumn(metronomeButton, 6);
            Grid.SetColumn(pianoButton, 8);
            Grid.SetColumn(fingerSettingBtn, 9);
            Grid.SetColumn(introductionBtn, 10);

            Grid.SetColumn(notesCB, 3);
            //Grid.SetColumn(resetButton, 4);

            // Add items to grid
            menuGrid.Children.Add(txt1);
            menuGrid.Children.Add(bpmTB);
            menuGrid.Children.Add(notesCB);
            menuGrid.Children.Add(resetButton);

            menuGrid.Children.Add(metronomeButton);
            menuGrid.Children.Add(pianoButton);
            menuGrid.Children.Add(startBtn);
            menuGrid.Children.Add(StopBtn);
            menuGrid.Children.Add(SelectSheetMusic);
            menuGrid.Children.Add(fingerSettingBtn);
            menuGrid.Children.Add(introductionBtn);

            myGrid.Children.Add(menuGrid);
        }

        internal void EnableStopBtn()
        {
            StopBtn.IsEnabled = true;
        }

        internal void EnableResetBtn()
        {
            resetButton.IsEnabled = true;
        }

        private void introductionBtn_Click(object sender, RoutedEventArgs e)
        {
            IntroductionView iV = new IntroductionView();
            iV.Show();
        }

        private void DrawLeftMenu()
        {
            // Metronome enable/disable button
            metronomeText = new TextBlock();
            metronomeText.Text = "🔇";
            metronomeButton = new Button();
            metronomeButton.Content = metronomeText;
            metronomeButton.Width = 60;
            metronomeButton.Height = 40;
            metronomeButton.FontSize = 20;
            metronomeButton.Click += onMetronomeButtonClick;
            //metronomeButton.HorizontalAlignment = HorizontalAlignment.Left;

            // Start button
            startBtn.FontSize = 20;
            startBtn.Name = "startBtn";
            startBtn.Content = "▶";
            startBtn.Width = 60;
            startBtn.Height = 40;
            //startBtn.HorizontalAlignment = HorizontalAlignment.Center;
            startBtn.Click += StartBtn_Click;

            // Stop Button
            StopBtn.FontSize = 20;
            StopBtn.Name = "stopBtn";
            StopBtn.Content = "◼";
            StopBtn.Width = 60;
            StopBtn.Height = 40;
            //StopBtn.HorizontalAlignment = HorizontalAlignment.Center;
            StopBtn.Click += StopBtn_Click;
            StopBtn.IsEnabled = false;

            // Reset button
            resetButton = new Button();
            resetButton.FontSize = 20;
            resetButton.Name = "resetBtn";
            resetButton.Content = "⟲";
            resetButton.Width = 60;
            resetButton.Height = 40;
            resetButton.HorizontalAlignment = HorizontalAlignment.Right;
            resetButton.Click += ResetButton_Click;
            resetButton.IsEnabled = false;


            // Add the button to the Grid
            SelectSheetMusic = new Button();
            SelectSheetMusic.Name = "SelectSheetMusic";
            SelectSheetMusic.Content = "Inladen";
            SelectSheetMusic.Width = menuGrid.ColumnDefinitions[0].Width.Value;
            SelectSheetMusic.Height = 40;
            //SelectSheetMusic.HorizontalAlignment = HorizontalAlignment.Left;
            //SelectSheetMusic.VerticalAlignment = VerticalAlignment.Bottom;
            SelectSheetMusic.Click += SelectSheetMusic_Click;
        }

        private void DrawRightMenu()
        {
            // Piano enable/disable button
            TextDecoration strikeTroughDecoration = new TextDecoration();
            strikeTroughDecoration.Location = TextDecorationLocation.Strikethrough;
            strikeTroughDecoration.Pen = new Pen(Brushes.Red, 3);
            strikeTrough.Add(strikeTroughDecoration);
            pianoText = new TextBlock();
            pianoText.Text = "🎹";
            pianoText.TextDecorations = strikeTrough;
            pianoButton = new Button();
            pianoButton.Content = pianoText;
            pianoButton.Width = 60;
            pianoButton.Height = 40;
            pianoButton.FontSize = 20;
            pianoButton.Click += onPianoButtonClick;
            //pianoButton.HorizontalAlignment = HorizontalAlignment.Center;
            //pianoButton.VerticalAlignment = VerticalAlignment.Bottom;

            // Fingersetting enable/disable
            TextDecoration strikeTroughDecorationFing = new TextDecoration();
            strikeTroughDecorationFing.Location = TextDecorationLocation.Strikethrough;
            strikeTroughDecorationFing.Pen = new Pen(Brushes.Red, 3);
            strikeTrough.Add(strikeTroughDecorationFing);
            fingerSettingTxt = new TextBlock();
            fingerSettingTxt.Text = "☝";
            fingerSettingTxt.TextDecorations = strikeTrough;
            fingerSettingBtn = new Button();
            fingerSettingBtn.Content = fingerSettingTxt;
            fingerSettingBtn.Width = 60;
            fingerSettingBtn.IsEnabled = false;
            fingerSettingBtn.Height = 40;
            fingerSettingBtn.FontSize = 20;
            fingerSettingBtn.Click += onFingerButtonClick;
            //fingerSettingBtn.HorizontalAlignment = HorizontalAlignment.Center;
            //fingerSettingBtn.VerticalAlignment = VerticalAlignment.Bottom;


            // Introduction button
            introductionBtn.FontSize = 20;
            introductionBtn.Name = "introductionBtn";
            introductionBtn.Content = "?";
            introductionBtn.Width = 60;
            introductionBtn.Height = 40;
            //introductionBtn.HorizontalAlignment = HorizontalAlignment.Center;
            introductionBtn.Click += introductionBtn_Click;
        }

        private void onFingerButtonClick(object sender, RoutedEventArgs e)
        {
            if (fingerEnabled)
            {
                fingerEnabled = false;
                fingerSettingTxt.TextDecorations = null;
                mPc.Guide.Piano.fingerSettingEnabled = fingerEnabled;
            }
            else
            {
                fingerEnabled = true;
                fingerSettingTxt.TextDecorations = strikeTrough;
                mPc.Guide.Piano.fingerSettingEnabled = fingerEnabled;
            }
        }

        private void DrawBpmMenu()
        {
            // Add the first text cell to the Grid

            txt1.Text = "BPM =";
            txt1.FontSize = 30;
            txt1.HorizontalAlignment = HorizontalAlignment.Right;
            txt1.VerticalAlignment = VerticalAlignment.Bottom;
            txt1.FontWeight = FontWeights.Bold;

            // Add textbox to set bpm
            bpmTB = new TextBox();
            bpmTB.Width = menuGrid.ColumnDefinitions[1].Width.Value - 10;
            bpmTB.Height = 40;
            bpmTB.HorizontalAlignment = HorizontalAlignment.Left;
            bpmTB.VerticalAlignment = VerticalAlignment.Bottom;
            bpmTB.FontSize = 30;
            bpmTB.Name = "bpmTB";
            bpmTB.Text = "60";
            bpmTB.Height = 45;


            // Combobox 
            notesCB = new ComboBox();
            notesCB.Height = 40;
            notesCB.FontSize = 20;
            notesCB.Width = menuGrid.ColumnDefinitions[3].Width.Value - 20;
            notesCB.HorizontalAlignment = HorizontalAlignment.Left;
            notesCB.VerticalAlignment = VerticalAlignment.Bottom;
            notesCB.Items.Add("Hele noot");
            notesCB.Items.Add("Halve noot");
            notesCB.Items.Add("Kwart noot");
            notesCB.SelectedIndex = 2;

            

            
        }

        private void updateBpm(object sender, BpmEventArgs e)
        {
            bpmTB.Text = e.bpm.ToString();
            _musicPieceId = e.Id;
        }

        public bool CheckPause()
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

        private void StopMusicPiece()
        {
            // Stuk stoppen
            sv.ScrollToTop(this, EventArgs.Empty);
            StopBtn.IsEnabled = true;
            mPc.Guide.paused = false;
            SelectSheetMusic.IsEnabled = true;
            _isStarted = false;
            mPc.Guide.ResetMusicPiece();
            metronome.stopMetronome();
            bpmTB.IsReadOnly = false;
            bpmTB.Background = Brushes.White;
            notesCB.IsEnabled = false;
            notesCB.IsEnabled = true;
            CheckPause();
        }

        public void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            StopMusicPiece();
            metronomeButton.IsEnabled = true;
        }

        private void SelectSheetMusic_Click(object sender, RoutedEventArgs e)
        {
            mCv = new MusicChooseView(sv, nv, this);
            mCv.ShowDialog();


            mCv.updateBpm += updateBpm;
        }

        public void TriggerStartBtnBySpaceKeyDown()
        {
            if(spaceButtonEnabled)
            {
                spaceButtonEnabled = false;
                StartMusicPiece();
            }
                
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            StartMusicPiece();
        }

        public void StartMusicPiece()
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
                        startBtn.IsEnabled = false;

                        // If paused continu piece by metronome
                        if (metronomeEnabled)
                        {
                            metronome.startMetronome(bpmValue, mPc.Sheet.GreatStaffModelList.First().MeasureList.First().Attributes.Time.Beats, 1);
                        }
                        else
                        {
                            metronome.startMetronomeCountDownOnly(bpmValue, mPc.Sheet.GreatStaffModelList.First().MeasureList.First().Attributes.Time.Beats, 1);
                        }
                    }
                    // If piece is not paused, pause piece
                    else
                    {
                        mPc.Guide.Pause();
                        metronome.stopMetronome();
                        metronomeButton.IsEnabled = true;
                        spaceButtonEnabled = true;
                    }
                }
                else
                {
                    startBtn.IsEnabled = false;
                    StopBtn.IsEnabled = true;

                    //set value in metronome and start it.
                    if (metronomeEnabled)
                    {
                        metronome.startMetronome(bpmValue, mPc.Sheet.GreatStaffModelList.First().MeasureList.First().Attributes.Time.Beats, 1);
                    }
                    else
                    {
                        metronome.startMetronomeCountDownOnly(bpmValue, mPc.Sheet.GreatStaffModelList.First().MeasureList.First().Attributes.Time.Beats, 1);
                    }
                    DatabaseConnection dbCon = new DatabaseConnection();
                    dbCon.ExcecuteCommandNoOutput($"UPDATE Music SET Bpm = ({Convert.ToInt32(bpmValue)}) WHERE Id = {_musicPieceId}");
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
            StopBtn.IsEnabled = false;
            resetButton.IsEnabled = false;
            bpmTB.IsReadOnly = true;
            bpmTB.Background = Brushes.LightGray;
            notesCB.IsEnabled = false; CheckPause();
        }


        public void ResetMusicPiece()
        {
            sv.ScrollToTop(this, EventArgs.Empty);
            StopBtn.IsEnabled = false;
            mPc.Guide.paused = false;
            _isStarted = false;
            SelectSheetMusic.IsEnabled = false;
            mPc.Guide.ResetMusicPiece();
            metronome.stopMetronome();
            paused = false;
            CheckPause();

            if (metronomeEnabled)
            {
                metronome.startMetronome(bpmValue, mPc.Sheet.GreatStaffModelList.First().MeasureList.First().Attributes.Time.Beats, 1);
            }
            else
            {
                metronome.startMetronomeCountDownOnly(bpmValue, mPc.Sheet.GreatStaffModelList.First().MeasureList.First().Attributes.Time.Beats, 1);
            }

        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ResetMusicPiece();
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

        public void DisableStartBtn()
        {
            startBtn.IsEnabled = false;
        }

        public void EnableStartBtn()
        {
            startBtn.IsEnabled = true;

        }

        public void DisableStopBtn()
        {
            StopBtn.IsEnabled = false;
        }

        public void AddCountdownText()
        {
            startBtn.IsEnabled = false;
            TextBlock number = new TextBlock();
            number.Text = (mPc.Sheet.GreatStaffModelList.First().MeasureList.First().Attributes.Time.Beats - metronome.elapsedBeats).ToString();
            Console.WriteLine(number.Text);
            startBtn.Content = number;
            if (mPc.Sheet.GreatStaffModelList.First().MeasureList.First().Attributes.Time.Beats - metronome.elapsedBeats == mPc.Sheet.GreatStaffModelList.First().MeasureList.First().Attributes.Time.Beats)
            {
                startBtn.IsEnabled = true;
                startBtn.Content = "❚❚";
                StopBtn.IsEnabled = true;

                // Set buttons enabled false or readonly if playing
                resetButton.IsEnabled = true;
                fingerSettingBtn.IsEnabled = true;
                SelectSheetMusic.IsEnabled = false;
                metronomeButton.IsEnabled = false;
                spaceButtonEnabled = true;

                CheckPause();
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
            ColumnDefinition colDef10 = new ColumnDefinition();
            ColumnDefinition colDef11 = new ColumnDefinition();
            ColumnDefinition colDef12 = new ColumnDefinition();

            // Set Lenght of the columns
            colDef1.Width = new GridLength(80, GridUnitType.Star);
            colDef2.Width = new GridLength(60, GridUnitType.Star);
            colDef3.Width = new GridLength(90, GridUnitType.Star);
            colDef4.Width = new GridLength(140, GridUnitType.Star);
            colDef5.Width = new GridLength(37, GridUnitType.Star);
            colDef6.Width = new GridLength(37, GridUnitType.Star);
            colDef7.Width = new GridLength(37, GridUnitType.Star);
            colDef8.Width = new GridLength(100, GridUnitType.Star);
            colDef9.Width = new GridLength(37, GridUnitType.Star);
            colDef10.Width = new GridLength(37, GridUnitType.Star);
            colDef11.Width = new GridLength(37, GridUnitType.Star);
            colDef12.Width = new GridLength(37, GridUnitType.Star);


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
            menuGrid.ColumnDefinitions.Add(colDef10);
            menuGrid.ColumnDefinitions.Add(colDef11);
        }
    }
}
