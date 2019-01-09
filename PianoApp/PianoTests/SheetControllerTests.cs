using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MusicXml;
using MusicXml.Domain;
using NUnit.Framework;
using PianoApp.Controllers;

namespace ControllerTests
{
    /// <summary>
    /// Summary description for SheetControllerTests
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class SheetControllerTests
    {
        private SheetController sC;
        private Score _score;
        public SheetControllerTests()
        {
            sC = new SheetController();
            _score = MusicXmlParser.GetScore("TestData/TestMusicPiece.xml");
        }

        //Test case for testing the pressed keys.
        //Test case: step, alter, staff.
        [Test]
        //All White Keys:
        [TestCase('A', 0, 1)]
        [TestCase('B', 0, 1)]
        [TestCase('C', 0, 2)]
        [TestCase('D', 0, 1)]
        [TestCase('E', 0, 2)]
        [TestCase('F', 0, 1)]
        [TestCase('G', 0, 1)]
        //All Black keys:
        [TestCase('A', 1, 1)]
        [TestCase('C', -1, 2)]
        [TestCase('D', 1, 2)]
        [TestCase('F', -1, 1)]
        [TestCase('G', 1, 1)]
        public void UpdateNotes_ShouldSetCorrespondingNoteToActive(char step, int alter, int staff)
        {           
            var note = new Note() { Pitch = new Pitch() { Alter = alter, Octave = 0, Step = step }, Staff = staff };
            var noteDict = new Dictionary<Note, PianoApp.Controllers.Timeout>() { { note, new PianoApp.Controllers.Timeout() } };
            sC.UpdateNotes(noteDict);
            var found = false;

            foreach (var scorePart in _score.Parts)
            {
                //Access all measures inside the music piece
                foreach (var scorePartMeasure in scorePart.Measures)
                {
                    //Access te elements inside a measure
                    foreach (var measureElement in scorePartMeasure.MeasureElements)
                    {
                        //Access the element if it is a note
                        if (measureElement.Type.Equals(MeasureElementType.Note))
                        {
                            var measureElementNote = (Note)measureElement.Element;

                            if (measureElementNote.Active &&
                                measureElementNote.Pitch.Step.Equals(step) &&
                                measureElementNote.Pitch.Alter.Equals(alter) &&
                                measureElementNote.Staff.Equals(staff))
                                found = true;
                        }
                    }
                }
            }         

            Assert.AreEqual(true, found);
        }
    }
}
