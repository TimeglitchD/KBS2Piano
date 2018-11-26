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
            SheetController sC = new SheetController();


            mPc = new MusicPieceController() { Piano = pC ,SheetController = sC};


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
            
            sv = new StaveView(myGrid, mPc);
            pv = new PianoView(myGrid, mPc);




            //Draw the notes
            nv = new NoteView(sv);
            nv.DrawNotes();

            bv = new ButtonView(myGrid, sv, nv);


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
    }

    public enum NoteType
    {
        Quarter,
        Half,
        Whole
    }
}