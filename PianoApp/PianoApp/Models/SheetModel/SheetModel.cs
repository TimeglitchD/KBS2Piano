using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PianoApp.Models
{
    public class SheetModel
    {
        public List<GreatStaffModel> GreatStaffModelList { get; set; } = new List<GreatStaffModel>();
        private StackPanel Sheet = new StackPanel() { Background = Brushes.AliceBlue };


        public StackPanel DrawSheet()
        {
            foreach (var item in GreatStaffModelList)
            {
                Sheet.Children.Add(item.DrawGreatStaff());
                Console.WriteLine("Great stave added");
            }
            Console.WriteLine("Sheet finished.");
            return Sheet;

        }
    }

}
