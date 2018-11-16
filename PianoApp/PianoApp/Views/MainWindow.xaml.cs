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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MusicXml;
using PianoApp.Controllers;
using PianoApp.Models;

namespace PianoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            PianoController pC = new PianoController();
            MusicPieceController mPc = new MusicPieceController() { Piano = pC };
            mPc.CreateMusicPiece("MusicFiles/lg-201059560.xml");
            mPc.Guide.Start();


            // Create the Grid
            Grid myGrid = new Grid
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

            // Add the first text cell to the Grid
            TextBlock txt1 = new TextBlock();
            txt1.Text = "BUTTONS";
            txt1.FontSize = 12;
            txt1.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt1, 0);
            

            // Add the second text cell to the Grid
            TextBlock txt2 = new TextBlock();
            txt2.Text = "STAVES";
            txt2.FontSize = 12;
            txt2.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt2, 1);

            //scrollbar
            ScrollViewer scroll = new ScrollViewer();
            scroll.Visibility = Visibility.Visible;
            Grid.SetRow(scroll, 1);

            //maak een stackpanel met de great staves
            StackPanel staves = new StackPanel();
            foreach (var item in mPc.Sheet.GreatStaffModelList)
            {
                TextBlock tb = new TextBlock();
                tb.Height = 50;
                tb.Text = "great stave";
                staves.Children.Add(tb);
            }

            //zet de stackpanel in de goede plek in het grid
            Grid.SetRow(staves, 1);

            //koppel de scrollbar aan het stackpanel
            scroll.Content = staves;


            // Add the third text cell to the Grid
            TextBlock txt3 = new TextBlock();
            txt3.Text = "PIANO";
            txt3.FontSize = 12;
            txt3.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt3, 2);


            // Add the TextBlock elements to the Grid Children collection
            myGrid.Children.Add(txt1);
            myGrid.Children.Add(txt2);
            myGrid.Children.Add(txt3);
            myGrid.Children.Add(scroll);




            InitializeComponent();
            Content = myGrid;
            Show();
        }
    }
}