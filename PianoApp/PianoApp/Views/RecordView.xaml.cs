using PianoApp.Controllers;
using System;
using System.Collections.Generic;
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

namespace PianoApp.Views
{
    /// <summary>
    /// Interaction logic for RecordView.xaml
    /// </summary>
    public partial class RecordView : Window
    {
        private StaveView recordSv;
        private NoteView noteV;
        private StaveView sv;
        private NoteView nv;
        private MusicPieceController mPc;

        private Grid grid = new Grid();

        public RecordView()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            grid.ShowGridLines = true;
            DefineRowGrid();
            this.Content = grid;
            InitializeComponent();
        }

        public void SetModels(StaveView recordSv, NoteView noteV, StaveView sv, NoteView nv, MusicPieceController mPc)
        {
            

            sv = new StaveView(grid, mPc, 0);

            //Draw the notes
            nv = new NoteView(sv);
            nv.DrawNotes();
        }

        private void DefineRowGrid()
        {
            // Define new row
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();

            // Add lenght to rows
            rowDef1.Height = new GridLength(400, GridUnitType.Star);
            rowDef2.Height = new GridLength(400, GridUnitType.Star);

            // Add row to mainGrid
            grid.RowDefinitions.Add(rowDef1);
            grid.RowDefinitions.Add(rowDef2);
        }
    }
}
