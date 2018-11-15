using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicXml.Domain;

namespace PianoApp.Models
{
    class StaffModel
    {        
        public int Number { get; set; }
        public List<Note> NoteList { get; set; } = new List<Note>();

        StaffModel()
        {

        }
    }
}
