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

        private MusicPieceController mPc;

        private StackPanel staves = new StackPanel();
        private StackPanel piano = new StackPanel();

        private StaveView sv;
        private NoteView nv;
        
        private Grid myGrid = new Grid();

        private TextBox bpmTB = new TextBox();
        private ComboBox notesCB = new ComboBox();

        private bool paused = false;
        private Button startBtn = new Button();

        int bpmValue = -1;

        private metronomeSound metronome = new metronomeSound();
        private bool metronomeEnabled = false;
        private Button metronomeButton;

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            PianoController pC = new PianoController();


            mPc = new MusicPieceController() { Piano = pC };

            DrawMenu();
            InitializeComponent();
            Show();
        }

        private void DrawMenu()
        {
            // Create the Grid
            myGrid = new Grid
            {
                ShowGridLines = true
            };

            
            // Define the Rows
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();
            rowDef1.Height = new GridLength(1, GridUnitType.Star);
            rowDef2.Height = new GridLength(5, GridUnitType.Star);
            rowDef3.Height = new GridLength(2, GridUnitType.Star);
            myGrid.RowDefinitions.Add(rowDef1);
            myGrid.RowDefinitions.Add(rowDef2);
            myGrid.RowDefinitions.Add(rowDef3);

            // Draw menu items
            DrawBpmMenu();

            drawMetronomeMenu();

            // Add the button to the Grid
            Button SelectSheetMusic = new Button();
            SelectSheetMusic.Name = "SelectSheetMusic";
            SelectSheetMusic.Content = "Select sheet music";
            SelectSheetMusic.VerticalAlignment = VerticalAlignment.Center;
            SelectSheetMusic.Width = 100;
            SelectSheetMusic.Height = 80;
            SelectSheetMusic.Click += SelectSheetMusic_Click;
            Grid.SetRow(SelectSheetMusic, 0);

            // Add the second text cell to the Grid
            TextBlock txt2 = new TextBlock();
            txt2.Text = "STAVES";
            txt2.FontSize = 12;
            txt2.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt2, 1);


            // Add the third text cell to the Grid
            TextBlock txt3 = new TextBlock();
            txt3.Text = "PIANO";
            txt3.FontSize = 12;
            txt3.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt3, 2);


            // Start button
            startBtn.FontSize = 25;
            startBtn.Name = "startBtn";
            startBtn.Content = "▶";
            startBtn.Width = 40;
            startBtn.Height = 40;
            startBtn.Margin = new Thickness(-500, -5, 0, -40);
            startBtn.Click += StartBtn_Click;
            Grid.SetRow(startBtn, 0);

            // Add the TextBlock elements to the Grid Children collection
            myGrid.Children.Add(txt2);
            myGrid.Children.Add(txt3);
            myGrid.Children.Add(SelectSheetMusic);
            myGrid.Children.Add(startBtn);

            Content = myGrid;

            //Create the staves
            pv = new PianoView(myGrid);
            sv = new StaveView(myGrid);
            Content = pv.myGrid;
                       

            //Draw the notes
            nv = new NoteView(sv);
            nv.DrawNotes();

        }

        private void DrawBpmMenu()
        {
            // Add the first text cell to the Grid
            TextBlock txt1 = new TextBlock();
            txt1.Text = "BPM =";
            txt1.FontSize = 35;
            txt1.Margin = new Thickness(0, 40, 0, -40);
            txt1.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt1, 0);

            // Add textbox to set bpm
            bpmTB = new TextBox();
            bpmTB.Width = 90;
            bpmTB.FontSize = 30;
            bpmTB.Name = "bpmTB";
            bpmTB.Text = "60";
            bpmTB.Height = 40;
            bpmTB.Margin = new Thickness(-940, -5, 0, -40);
            Grid.SetRow(bpmTB, 0);

            // Add combobox to set bpm to notes
            notesCB = new ComboBox();
            notesCB.Width = 130;
            notesCB.Height = 40;
            notesCB.FontSize = 20;

            // Add items to combobox
            notesCB.Items.Add("Hele noot");
            notesCB.Items.Add("Halve noot");
            notesCB.Items.Add("Kwart noot");

            // Set first item selected
            notesCB.SelectedIndex = 0;
            notesCB.Margin = new Thickness(-700, -5, 0, -40);
            Grid.SetRow(notesCB, 0);

            // Add items to grid
            myGrid.Children.Add(txt1);
            myGrid.Children.Add(bpmTB);
            myGrid.Children.Add(notesCB);
        }

        public void drawMetronomeMenu()
        {
            metronomeButton = new Button();
            metronomeButton.Width = 120;
            metronomeButton.Height = 40;
            metronomeButton.Content = "Metronoom: Uit";
            metronomeButton.Click += onMetronomeButtonClick;
            metronomeButton.Margin = new Thickness(-300, -5, 0, -40);
            myGrid.Children.Add(metronomeButton);

        }

        private void onMetronomeButtonClick(object sender, RoutedEventArgs e)
        {
            if(metronomeEnabled)
            {
                metronomeEnabled = false;
                metronomeButton.Content = "Metronoom: Uit";
            } else
            {
                metronomeEnabled = true;
                metronomeButton.Content = "Metronoom: Aan";
            }
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckPause();
                // Set value to int
                bpmValue = int.Parse(bpmTB.Text);

                // Throw custom exception
                if(bpmValue < 0 || bpmValue > 300) throw new BpmOutOfRangeException($"Bpm waarde ligt niet tussen de 0 en de 300 ({bpmValue})");

                // Set values in GuideController
                mPc.Guide.Bpm = bpmValue;
                mPc.Guide.SetNote(notesCB.Text);

                //set value in metronome and start it.
                if(metronomeEnabled)
                {
                    metronome.startMetronome(bpmValue, 4);
                }

                //mPc.Guide.Start();
            }
            catch (FormatException)
            {
                MessageBox.Show("De ingevoerde waarde moet een getal zijn!");
            }
            catch (BpmOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
            }

            Console.WriteLine(mPc.Guide.Bpm);
            Console.WriteLine(mPc.Guide.Note);
        }

        private void SelectSheetMusic_Click(object sender, RoutedEventArgs e)
        {
            mCv = new MusicChooseView(sv, nv);
            mCv.Show();
        }

        private void CheckPause()
        {
            if (paused)
            {
                paused = false;
                startBtn.Content = "▶";
            } else
            {
                paused = true;
                startBtn.Content = "||";
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