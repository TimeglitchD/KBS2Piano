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
using PianoApp.Views;
using PianoApp.Models;

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
        
        private Grid myGrid = new Grid();

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
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

            // Add the first text cell to the Grid
            TextBlock txt1 = new TextBlock();
            txt1.Text = "BUTTONS";
            txt1.FontSize = 12;
            txt1.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt1, 0);

            // Add the button to the Grid
            Button SelectSheetMusic = new Button();
            SelectSheetMusic.Name = "SelectSheetMusic";
            SelectSheetMusic.Content = "Select sheet music";
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


            // Add the TextBlock elements to the Grid Children collection
            myGrid.Children.Add(txt1);
            myGrid.Children.Add(txt2);
            myGrid.Children.Add(txt3);

            myGrid.Children.Add(SelectSheetMusic);

            pv = new PianoView(myGrid);
            

            sv = new StaveView(myGrid);

            Content = pv.myGrid;
            
        }



        private void SelectSheetMusic_Click(object sender, RoutedEventArgs e)
        {
            mCv = new MusicChooseView(sv);
            mCv.Show();
        }


    }
}