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

namespace PianoApp
{
    /// <summary>
    /// Interaction logic for XMLChoose.xaml
    /// </summary>
    public partial class XMLChoose : Window
    {
        public XMLChoose()
        {
            InitializeComponent();
                DatabaseConnection connection = new DatabaseConnection();
                var test = connection.getLessons();
                MessageBox.Show(test.ToString());
        }
    }
}
