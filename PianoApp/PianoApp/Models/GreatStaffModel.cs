using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicXml.Domain;

namespace PianoApp.Models
{
    public class GreatStaffModel
    {
        public List<StaffModel> StaffList { get; set; } = new List<StaffModel>();
        public List<Measure> MeasureList { get; set; } = new List<Measure>();

        public GreatStaffModel()
        {
            StaffList.Add(new StaffModel(){Number = 1});
            StaffList.Add(new StaffModel(){Number = 2});
        }
    }
}
