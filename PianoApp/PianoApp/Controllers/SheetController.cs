using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MusicXml.Domain;
using PianoApp.Models;
using PianoApp.Views;

namespace PianoApp.Controllers
{
    public class SheetController
    {
        public SheetModel SheetModel { get; set; } = new SheetModel();
        public NoteView NoteView { get; set; }

        public void UpdateNotes(Dictionary<Note, GuidesController.Timeout> noteAndTimeoutDictionary)
        {
            var tempDict = new Dictionary<Note, GuidesController.Timeout>(noteAndTimeoutDictionary);

            foreach (var keyValuePair in tempDict)
            {
                foreach (var greatStaffModel in SheetModel.GreatStaffModelList)
                {
                    foreach (var staffModel in greatStaffModel.StaffList.Where(s => s.Number == keyValuePair.Key.Staff))
                    {
                        foreach (var note in staffModel.NoteList)
                        {
                            if (note.Pitch != null)
                            {

                                //                                    if (keyValuePair.Key.Pitch.Step.ToString() == note.Pitch.Step.ToString() &&
                                //                                        keyValuePair.Key.Pitch.Alter == note.Pitch.Alter &&
                                //                                        keyValuePair.Key.Pitch.Octave == note.Pitch.Octave &&
                                //                                        keyValuePair.Key.MeasureNumber == note.MeasureNumber &&
                                //                                        keyValuePair.Key.XPos == note.XPos)
                                //                                    {
                                //                                        note.Active = true;
                                //                                        note.ell.Dispatcher.BeginInvoke((Action)(() => note.Color()));
                                //                                    }

                                if (keyValuePair.Key.XPos == note.XPos && keyValuePair.Key.MeasureNumber == note.MeasureNumber)
                                {
                                    note.Active = true;
                                    note.ell.Dispatcher.BeginInvoke((Action)(() => note.Color()));
                                }
                                else
                                {
                                    note.Active = false;
                                    note.ell.Dispatcher.BeginInvoke((Action)(() => note.Color()));
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
