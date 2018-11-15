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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MusicXml;
using PianoApp.Controllers;

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
            MusicPieceController mPc = new MusicPieceController(){Piano = pC};
            mPc.CreateMusicPiece("MusicFiles/lg-201059560.xml");
            mPc.Guide.Start();

            InitializeComponent();
        }
    }
}
