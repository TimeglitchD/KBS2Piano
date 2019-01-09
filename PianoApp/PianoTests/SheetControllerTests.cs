using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MusicXml;
using MusicXml.Domain;
using NUnit.Framework;
using PianoApp.Controllers;
using PianoApp.Models;

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
            _score = new Score();
            _score.Parts.Add(new Part());
            _score.Parts.First().Measures.Add(new Measure());
            
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
            var dummyNote = new Note() {Pitch = new Pitch() {Alter = alter, Octave = 0, Step = step}, Staff = staff};
            //Add note to dummy music piece.
            var dummyMeasureElement = new MeasureElement() {Element = dummyNote, Type = MeasureElementType.Note};
            _score.Parts.First().Measures.First().MeasureElements.Add(dummyMeasureElement);            
            //Add note to active note list.
            var noteDict = new Dictionary<Note, PianoApp.Controllers.Timeout>() { { dummyNote, new PianoApp.Controllers.Timeout() } };
            //Fill dummy sheet model with the note.
            sC.SheetModel.GreatStaffModelList.Add(new GreatStaffModel());
            sC.SheetModel.GreatStaffModelList.First().StaffList.Find(s => s.Number.Equals(staff)).NoteList.Add(dummyNote);
            //Update the dummyNote state.
            sC.UpdateNotes(noteDict);

            Assert.AreEqual(Note.NoteState.Active, dummyNote.State);            
            //Remove the dummy note from the piece.
            _score.Parts.First().Measures.First().MeasureElements.Clear();
        }
    }
}
