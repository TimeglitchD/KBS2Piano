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

        // Update notes in musicpiece by note type and time
        public void UpdateNotes(Dictionary<Note, Timeout> noteAndTimeoutDictionary)
        {

            // Create temporary dictionary
            var tempDict = new Dictionary<Note, Timeout>(noteAndTimeoutDictionary);

            // Get the right note
            foreach (var keyValuePair in tempDict)
            {
                // Get all great staffs in the sheetModel
                foreach (var greatStaffModel in SheetModel.GreatStaffModelList)
                {
                    // Get all staffs in the right greatstaff
                    foreach (var staffModel in greatStaffModel.StaffList.Where(s => s.Number == keyValuePair.Key.Staff))
                    {
                        // Get all notes
                        foreach (var note in staffModel.NoteList)
                        {
                            // Check if pitch is not null
                            if (note.Pitch != null)
                            {
                                // Check if note in sheet is the same note as the gained note in the dictionary
                                if (keyValuePair.Key == note)
                                {
                                    // if true -> set noteState to active
                                    note.State = NoteState.Active;
                                }

                                // Set new color to the notes
                                note.ell.Dispatcher.BeginInvoke((Action)(() => note.Color()));
                            }
                        }
                    }
                }
            }
        }

    }
}
