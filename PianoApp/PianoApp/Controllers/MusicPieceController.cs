using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using MusicXml;
using MusicXml.Domain;
using PianoApp.Models;

namespace PianoApp.Controllers
{
    public class MusicPieceController
    {
        public Score Score;
        public PianoController Piano;

        public SheetController SheetController;

        public GuidesController Guide { get; set; }

        public MidiController MidiController;

        public KeyboardController KeyboardController;

        public SheetModel Sheet { get; set; }
        public Grid Grid { get; set; }

        public event EventHandler StaffEndReached;

        public event EventHandler GoToFirstStaff;

        public event EventHandler HoldPosition;

        //1055 is max length of one staff
        private const double _maxStaffWidth = 1055;

        public void CreateMusicPiece(string filename)
        {
            Score = MusicXmlParser.GetScore(filename);
            Guide = new GuidesController(MidiController) { Score = Score, Piano = Piano, Sheet = SheetController, grid = Grid};
            Sheet = SheetController.SheetModel;

            //stop program from crashing when reloading musicpiece.
            Sheet.Reset();

            AddGreatStaffsToSheet();
            AddMeasuresToGreatStaffs();
            AddNotesToMeasures();

            // Subscribe events
            Guide.StaffEndReached += StaffEndReached;
            Guide.GoToFirstStaff += GoToFirstStaff;
            Guide.HoldPosition += this.HoldPosition;

            MidiController.Guide = Guide;
            KeyboardController.Guide = Guide;
            
        }

        // Draw music piece
        public StackPanel DrawMusicPiece()
        {
            // If sheet is null -> draw standard sheet
            if (Sheet == null)
            {
                return StandardSheet();

            }
            // else draw sheet bases on Sheet
            else
            {
                return Sheet.DrawSheet();
            }

        }

        // Function to draw standard sheet
        public StackPanel StandardSheet()
        {
            GreatStaffModel greatStaffModel = new GreatStaffModel();
            Sheet = new SheetModel();
            Sheet.GreatStaffModelList.Add(greatStaffModel);
            return Sheet.DrawSheet();
        }

        //Create Great staffs based on amount of measures in the piece
        public void AddGreatStaffsToSheet()
        {

            for (int i = Score.Systems; i > 0; i--)
            {
                Sheet.GreatStaffModelList.Add(new GreatStaffModel());
            }


        }

        //Fill staffs width measures based on amount of measures in the piece
        public void AddMeasuresToGreatStaffs()
        {
            int greatstaff = 0;

            foreach (var scorePart in Score.Parts)
            {
                foreach (var measure in scorePart.Measures)
                {
                    if (measure.NewSystem && measure.Number != 1)
                    {
                        greatstaff++;
                    }

                    Sheet.GreatStaffModelList[greatstaff].MeasureList.Add(measure);

                }

            }
        }

        //Fill staffs with notes
        private void AddNotesToMeasures()
        {
            foreach (var greatStaffModel in Sheet.GreatStaffModelList)
            {
                foreach (var measure in greatStaffModel.MeasureList)
                {
                    foreach (var measureElement in measure.MeasureElements)
                    {
                        if (measureElement.Type.Equals(MeasureElementType.Note))
                        {
                            var note = (Note)measureElement.Element;

                            foreach (var staffModel in greatStaffModel.StaffList)
                            {
                                if (staffModel.Number == note.Staff)
                                {
                                    staffModel.NoteList.Add(note);
                                }
                            }
                        }
                    }
                }

            }
        }
    }
}
