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

namespace PianoApp
{
    /// <summary>
    /// Interaction logic for XMLChoose.xaml
    /// </summary>
    public partial class XMLChoose : Window
    {
        DatabaseConnection connection;
        public XMLChoose()
        {
            InitializeComponent();
            connection = new DatabaseConnection();
            populateTab(1, SheetMusic);
        }

        public void populateTab(int Type, DataGrid grid)
        {
            grid.ItemsSource = connection.getSheetMusic(1).Tables["Music"].DefaultView;
        }

        private void OnSelectClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
