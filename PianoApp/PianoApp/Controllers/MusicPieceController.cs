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
        public Score _score;
        public PianoController Piano;

        public GuidesController Guide { get; set; } = new GuidesController();

        public SheetModel Sheet { get; set; }

        //1055 is max length of one staff
        private const double _maxStaffWidth = 1055;

        public void CreateMusicPiece(string filename)
        {
            _score = MusicXmlParser.GetScore(filename);

            Guide = new GuidesController() { Score = _score, Piano = Piano };

            Sheet = new SheetModel();

            AddGreatStaffsToSheet();

            AddMeasuresToGreatStaffs();

            AddNotesToMeasures();

        }

        public StackPanel DrawMusicPiece()
        {
            if (Sheet == null)
            {
                Console.WriteLine("No piece found.");
                return StandardSheet();

            }
            else
            {
                Console.WriteLine("Piece found!");
                return Sheet.DrawSheet();
            }

        }

        public StackPanel StandardSheet()
        {
            GreatStaffModel greatStaffModel = new GreatStaffModel();
            Sheet = new SheetModel();
            Sheet.GreatStaffModelList.Add(greatStaffModel);
            return Sheet.DrawSheet();
        }

        //Create Great staffs based on amount of measures in the piece
        private void AddGreatStaffsToSheet()
        {

            for (int i = _score.Systems; i > 0; i--)
            {
                Sheet.GreatStaffModelList.Add(new GreatStaffModel());
            }


            Console.WriteLine($"Amount of Great Staffs added: {Sheet.GreatStaffModelList.Count}");
            Console.WriteLine("================================");
        }

        //Fill staffs with measures based on amount of measures in the piece
        private void AddMeasuresToGreatStaffs()
        {
            double maxWidth = 0;
            int staff = 0;

            foreach (var scorePart in _score.Parts)
            {

                foreach (var measure in scorePart.Measures)
                {
                    maxWidth += (double)measure.Width;
                    Console.WriteLine("WIDTH: " + maxWidth);
                    if (maxWidth > +_maxStaffWidth)
                    {
                        staff++;
                        maxWidth = (double)measure.Width;
                    }
                    Sheet.GreatStaffModelList[staff].MeasureList.Add(measure);

                }
            }
            Console.WriteLine($"Amount measures added: {Sheet.GreatStaffModelList[staff].MeasureList.Count}");
            Console.WriteLine("================================");
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

                foreach (var staffModel in greatStaffModel.StaffList)
                {
                    Console.WriteLine($"Amount of notes added: {staffModel.NoteList.Count}");
                }
            }
            Console.WriteLine("================================");
        }

    }
}
