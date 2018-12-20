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
        private StackPanel Sheet = new StackPanel();


        public StackPanel DrawSheet()
        {
            foreach (var item in GreatStaffModelList)
            {
                Sheet.Children.Add(item.DrawGreatStaff());
            }
            return Sheet;

        }

        //function to reset sheetmodel, so new staffs don't get the old sheet as parent
        public void Reset()
        {
            GreatStaffModelList = new List<GreatStaffModel>();
            Sheet = new StackPanel();
        }
    }

}
