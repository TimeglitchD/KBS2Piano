﻿using System;
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

        //selected piece's file location
        private string selectedPiece;

        public MusicChooseView(StaveView sv)
        {
            InitializeComponent();
            connection = new DatabaseConnection();
            this.sv = sv;

            //add sheet records to tab
            populateTab(1, SheetMusic);
        }

        //fill tab based on type
        public void populateTab(int Type, DataGrid grid)
        {
            grid.ItemsSource = connection.getSheetMusic(1).Tables["Music"].DefaultView;
        }

        private void OnSelectClick(object sender, RoutedEventArgs e)
        {
            try
            {
                sv.MPc.CreateMusicPiece(selectedPiece);
                //succesfull at opening xml file.

                sv.DrawStaves();
                //draw the new staves

                this.Close();
                
            } catch(Exception ex)
            {
                System.Windows.MessageBox.Show("Error while opening music piece: " + ex.Message);
            }
        }

        //event updates file path based on selection
        private void DataGrid_SelectionChanged(object sender, RoutedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView selected = dg.CurrentItem as DataRowView;
            selectedPiece = selected.Row["Location"] as String;
        }
    }
}
