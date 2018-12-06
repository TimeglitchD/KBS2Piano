using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PianoApp.Controllers;

namespace PianoApp.Views
{
    /// <summary>
    /// Interaction logic for MusicChooseView.xaml
    /// </summary>
    public partial class MusicChooseView : Window
    {

        private DatabaseConnection connection;
        private StaveView sv;
        private NoteView nv;
        private DataView musicView;
        private DataView scoreView;

        //selected piece's file location
        private string selectedPiece;

        public MusicChooseView(StaveView sv, NoteView nv)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            connection = new DatabaseConnection();
            this.sv = sv;
            this.nv = nv;
            musicView = connection.getSheetMusic(1).Tables["Music"].DefaultView;

            scoreView = connection.getSheetScore().Tables["Score"].DefaultView;
            scoreView.RowFilter = "Id = 0";

            //add sheet records to tab
            populateTab(SheetMusic, musicView);

            //Add Score records to tab
            populateTab(ScoreGrid, scoreView);

        }

        //fill tab based on type
        public void populateTab(DataGrid grid, DataView dv)
        {
            grid.ItemsSource = dv;
        }

        private void OnSelectClick(object sender, RoutedEventArgs e)
        {
            SelectPiece();
        }

        public void Start()
        {
            sv.MusicPieceController.Guide.Start();
        }

        //event updates file path based on selection
        private void SheetMusic_SelectionChanged(object sender, RoutedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView selected = dg.CurrentItem as DataRowView;
            if (selected == null)
                return;
            selectedPiece = selected.Row["Location"] as String;

            //update textBoxes
            titleBox.Text = selected.Row["Title"].ToString();
            descBox.Text = selected.Row["Description"] as String;

           string Id = selected.Row["Id"].ToString();
            ChangeScoreView(Id);
        }

        private void SearchTerm_TextChanged(object sender, TextChangedEventArgs e)
        {
            musicView.RowFilter = $"Description LIKE '%{SearchTerm.Text}%' OR Title Like '%{SearchTerm.Text}%'";
        }

        private void SheetMusic_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectPiece();
        }

        private void SelectPiece()
        {
            Console.WriteLine(selectedPiece);
            try
            {
                sv.MusicPieceController.CreateMusicPiece(selectedPiece);
                Console.WriteLine("Piece loaded.");
                //succesfull at opening xml file.
                sv.DrawMusic();
                nv.DrawNotes();
                //draw the new staves with notes
                this.Close();
            }catch (ArgumentNullException ex)
            {
                MessageBox.Show("Je hebt geen muziekstuk geselecteerd");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error while opening music piece: " + ex.Message);
            }
           
        }

        private void ChangeScoreView(string id)
        {
            scoreView.RowFilter = $"Id = {id}"; 
        }
    }

}

