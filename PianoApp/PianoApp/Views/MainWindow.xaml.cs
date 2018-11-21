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
        private StaveView sv;
        
        private StackPanel staves = new StackPanel();
        private Grid mainGrid = new Grid();

        private Grid menuGrid = new Grid();

        private TextBox bpmTB = new TextBox();
        private ComboBox notesCB = new ComboBox();

        int bpmValue = -1;
        private MusicPieceController mPc;

        Button startBtn = new Button();
        Button resetButton = new Button();

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
            sv = new StaveView(mainGrid);

            //metronomeSound sound = new metronomeSound();
            //sound.startMetronome(80, 3, 1);
        }

        private void DrawMenu()
        {
            // Create the Grid
            mainGrid = new Grid
            {
                ShowGridLines = true
            };

            // show menuGrid lines
            //menuGrid.ShowGridLines = true;

            // Define all rows for mainGrid
            DefineRowMyGrid();

            // Define all columns for menuGrid
            DefineGridRowsMenuGrid();

            // Draw menu items
            DrawBpmMenu();

            // Add the button to the Grid
            Button SelectSheetMusic = new Button();
            SelectSheetMusic.Name = "SelectSheetMusic";
            SelectSheetMusic.Content = "Selecteer \n muziekstuk";

            // Set height/width based on column height/widht
            SelectSheetMusic.Width = menuGrid.ColumnDefinitions[0].Width.Value - 10;
            SelectSheetMusic.Height = 40;

            // Center and stick to bottom in column
            SelectSheetMusic.HorizontalAlignment = HorizontalAlignment.Center;
            SelectSheetMusic.VerticalAlignment = VerticalAlignment.Bottom;
            SelectSheetMusic.Click += SelectSheetMusic_Click;
            Grid.SetColumn(SelectSheetMusic, 0);

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
            startBtn = new Button();
            startBtn.FontSize = 25;
            startBtn.Name = "startBtn";
            startBtn.Content = "▶";
            startBtn.Width = 40;
            startBtn.Height = 40;
            startBtn.HorizontalAlignment = HorizontalAlignment.Center;
            startBtn.Click += StartBtn_Click;
            Grid.SetRow(startBtn, 0);

            // Add the TextBlock elements to the Grid Children collection
            mainGrid.Children.Add(txt2);
            mainGrid.Children.Add(txt3);
            mainGrid.Children.Add(startBtn);
            mainGrid.Children.Add(menuGrid);

            menuGrid.Children.Add(SelectSheetMusic);

            Content = mainGrid;
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
            Grid.SetColumn(txt1, 1);

            // Add textbox to set bpm
            bpmTB = new TextBox();
            bpmTB.Width = menuGrid.ColumnDefinitions[1].Width.Value - 10;
            bpmTB.Height = mainGrid.RowDefinitions[1].Height.Value;
            bpmTB.HorizontalAlignment = HorizontalAlignment.Left;
            bpmTB.VerticalAlignment = VerticalAlignment.Bottom;
            bpmTB.FontSize = 30;
            bpmTB.Name = "bpmTB";
            bpmTB.Text = "60";
            bpmTB.Height = 40;
            Grid.SetColumn(bpmTB, 2);

            Content = mainGrid;
            sv = new StaveView(mainGrid);


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
            notesCB.SelectedIndex = 0;
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

            // Set buttons enabled or readonly false if resetting music
            bpmTB.IsReadOnly = false;
            bpmTB.Background = Brushes.White;
            notesCB.IsEnabled = true;
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Set value to int
                bpmValue = int.Parse(bpmTB.Text);
                
                // Set values in GuideController
                mPc.Guide.Bpm = bpmValue;
                mPc.Guide.SetNote(notesCB.Text);
                
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

            // Set buttons enabled false or readonly if playing
            notesCB.IsEnabled = false;
            bpmTB.IsReadOnly = true;
            bpmTB.Background = Brushes.LightGray;
        }

        private void SelectSheetMusic_Click(object sender, RoutedEventArgs e)
        {
            mCv = new MusicChooseView(sv);
            mCv.Show();
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
            mainGrid.RowDefinitions.Add(rowDef1);
            mainGrid.RowDefinitions.Add(rowDef2);
            mainGrid.RowDefinitions.Add(rowDef3);

            // Add menuGrid to mainGrid
            Grid.SetRow(menuGrid, 0);
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
            colDef5.Width = new GridLength(40, GridUnitType.Star);
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
    }
    public enum NoteType
    {
        Quarter,
        Half,
        Whole
    }
}