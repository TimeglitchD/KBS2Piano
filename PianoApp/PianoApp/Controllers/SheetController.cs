using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MusicXml.Domain;
using PianoApp.Models;
using PianoApp.Views;
using static MusicXml.Domain.Note;

namespace PianoApp.Controllers
{
    public class SheetController
    {
        public SheetModel SheetModel { get; set; } = new SheetModel();
        public NoteView NoteView { get; set; }
        public MidiController MidiController { get; set; }

        public void UpdateNotes(Dictionary<Note, Timeout> noteAndTimeoutDictionary)
        {
            var tempDict = new Dictionary<Note, Timeout>(noteAndTimeoutDictionary);

            foreach (var keyValuePair in tempDict)
            {
                foreach (var greatStaffModel in SheetModel.GreatStaffModelList)
                {
                    foreach (var staffModel in greatStaffModel.StaffList.Where(s => s.Number == keyValuePair.Key.Staff))
                    {
                        //this.staffNumber = staffModel.;
                        
                        foreach (var note in staffModel.NoteList)
                        {
                            if (note.Pitch != null)
                            {
                                if (keyValuePair.Key == note)
                                {
                                    note.State = NoteState.Active;
//                                    MidiController.PlayNotes(noteAndTimeoutDictionary);
                                }
                                note.ell.Dispatcher.BeginInvoke((Action)(() => note.Color()));
                            }
                        }
                    }
                }
            }
        }

    }
}
