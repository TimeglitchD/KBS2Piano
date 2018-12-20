using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using Microsoft.Win32;
using PianoApp.Controllers;
using PianoApp.Events;

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
        private DataView beginnerView;
        private DataView middelmatigView;
        private DataView gevorderdView;
        private DataView expertView;
        private ButtonView bv;
        public string id;
        public string selectedTab;

        public int Bpm;

        public event EventHandler<BpmEventArgs> updateBpm;

        //selected piece's file location
        private string selectedPiece;

        public MusicChooseView(StaveView sv, NoteView nv, ButtonView bv)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            connection = new DatabaseConnection();
            this.sv = sv;
            this.nv = nv;
            this.bv = bv;

            this.selectedTab = "";

            musicView = connection.getSheetMusic().Tables["Music"].DefaultView;
            beginnerView = connection.getCourseMusic("Beginner").Tables["Beginner"].DefaultView;
            middelmatigView = connection.getCourseMusic("Middelmatig").Tables["Middelmatig"].DefaultView;
            gevorderdView = connection.getCourseMusic("Gevorderd").Tables["Gevorderd"].DefaultView;
            expertView = connection.getCourseMusic("Expert").Tables["Expert"].DefaultView;

            scoreView = connection.getSheetScore().Tables["Score"].DefaultView;
            scoreView.RowFilter = "Id = 0";

            //add sheet records to tab

            populateTab(SheetMusic, musicView);
            populateTab(Beginner, beginnerView);
            populateTab(Middelmatig, middelmatigView);
            populateTab(Gevorderd, gevorderdView);
            populateTab(Expert, expertView);

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

            if (selected != null)
            {
                //make delete button visible
                DeleteBtn.Visibility = Visibility.Visible;
            }
            else
            {
                DeleteBtn.Visibility = Visibility.Hidden;
            }


            //update textBoxes
            titleBox.Text = selected.Row["Title"].ToString();
            descBox.Text = selected.Row["Description"] as String;

            string Id = selected.Row["Id"].ToString();
            id = "";
            id = Id;

            //change the score to selected item score
            ChangeScoreView(Id);

        }

        private void SearchTerm_TextChanged(object sender, TextChangedEventArgs e)
        {
            musicView.RowFilter = $"Description LIKE '%{SearchTerm.Text}%' OR Title Like '%{SearchTerm.Text}%'";
            beginnerView.RowFilter = $"Description LIKE '%{SearchTerm.Text}%' OR Title Like '%{SearchTerm.Text}%'";
            middelmatigView.RowFilter = $"Description LIKE '%{SearchTerm.Text}%' OR Title Like '%{SearchTerm.Text}%'";
            gevorderdView.RowFilter = $"Description LIKE '%{SearchTerm.Text}%' OR Title Like '%{SearchTerm.Text}%'";
            expertView.RowFilter = $"Description LIKE '%{SearchTerm.Text}%' OR Title Like '%{SearchTerm.Text}%'";
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

                DatabaseConnection dbCon = new DatabaseConnection();
                var result = dbCon.GetDataFromDB($"Select Bpm from Music where Id = {id}", "Music");
                Bpm = Convert.ToInt32(result.Tables[0].Rows[0].ItemArray.GetValue(0).ToString());
                // trigger bv
                if (updateBpm != null)
                {
                    updateBpm(this, new BpmEventArgs { bpm = Bpm, Id = id });
                }
                this.Close();
            }
            catch (ArgumentNullException)
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //Open file opener dialog
            OpenFileDialog ofd = new OpenFileDialog();

            //set filextension to only XML
            ofd.Filter = "XML files (*.xml)|*.xml";

            if (ofd.ShowDialog() == true)
            {
                //set filename
                string fileName = ofd.SafeFileName;

                // open dialog to declare attributes
                FileDescriptionDialog fdd = new FileDescriptionDialog();
                fdd.Owner = this;
                fdd.ShowDialog();

                if (fdd.DialogResult == true)
                {
                    // set fileinfo
                    FileInfo fInfo = new FileInfo(ofd.FileName);

                    // set attributes for db
                    string strFileName = fInfo.Name;
                    string strFilePath = fInfo.DirectoryName;
                    string location = strFilePath + "\\" + strFileName;
                    string date = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
                    string title = fdd.Titel;
                    string description = fdd.Omschrijving;

                    // add to db
                    connection.addMusic(title, description, date, location, selectedTab);
                    txtEditor.Content = "[" + title + "] is added to DB";

                    RefreshAll();
                }
            }
        }

        private void RefreshMusicView()
        {
            SheetMusic.ItemsSource = null;
            SheetMusic.ItemsSource = connection.getSheetMusic().Tables["Music"].DefaultView;
        }

        private void RefreshCourseView(DataGrid dg)
        {
            string header = dg.Name.ToString();
            dg.ItemsSource = null;
            dg.ItemsSource = connection.getCourseMusic(header).Tables[header].DefaultView;
        }

        private void RefreshAll()
        {
            RefreshMusicView();
            RefreshCourseView(Beginner);
            RefreshCourseView(Middelmatig);
            RefreshCourseView(Gevorderd);
            RefreshCourseView(Expert);
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to delete this piece?",
                                          "Confirmation",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                connection.DeleteFromDb(id);
                RefreshAll();
            }
        }

        void LesTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get TabControl reference.
            var item = sender as TabControl;

            // ... Get selected header name
            var selected = item.SelectedItem as TabItem;
            string text = selected.Header.ToString();

            if (e.Source is TabControl)
            {
                this.selectedTab = text;
            } 
        }

        void SheetTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.Source == TabControl)
            {
                this.selectedTab = "";
            } 
        }
    }


}

