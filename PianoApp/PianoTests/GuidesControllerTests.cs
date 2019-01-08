using System;
using System.Collections.Generic;
using MusicXml.Domain;
using NUnit.Framework;
using PianoApp.Controllers;
using PianoApp.Models.Exception;

namespace ControllerTests
{
    /// <summary>
    /// Summary description for GuidesControllerTests
    /// </summary>
    [TestFixture]
    public class GuidesControllerTests
    {
        GuidesController guidesController;

        public GuidesControllerTests()
        {
            MidiController midiController = new MidiController();
            guidesController = new GuidesController(midiController);
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //Test the calculate score.
        //Test case: amount of good played notes, amount of total notes in a music piece, actual score.       
        [Test]
        [TestCase (10, 100, 10)]
        [TestCase (10, 20, 50)]
        public void CalcScore_ShouldReturnPercentage(int gn, int an, int sc)
        {
            guidesController._goodNotes = gn;
            guidesController._amountOfNotes = an;

            Assert.AreEqual(sc, guidesController.CalcScore());
        }

        //Test for checking the last note in a piece.
        //Test case: note number fromm 0 - 88 or 10 octaves, the octave 
        [Test]
        [TestCase(12, 1)]
        [TestCase(14, 1)]
        [TestCase(32, 2)]
        [TestCase(64, 5)]
        [TestCase(88, 7)]
        public void getNoteFromNoteNumber_ShouldReturnOctaveBasedOnNoteNumber(int nn, int octave)
        {           
            Assert.AreEqual(octave, guidesController.getNoteFromNoteNumber(nn).Octave);
        }

        [TestCase(12, 'C')]
        [TestCase(14, 'D')]
        [TestCase(32, 'G')]
        [TestCase(64, 'E')]
        [TestCase(88, 'E')]
        public void getNoteFromNoteNumber_ShouldReturnStepBasedOnNoteNumber(int nn, Char step)
        {
            Assert.AreEqual(step, guidesController.getNoteFromNoteNumber(nn).Step);
        }

        [TestCase(12, 0)]
        [TestCase(19, 0)]
        [TestCase(32, 1)]
        [TestCase(13, 1)]
        [TestCase(88, 0)]
        public void getNoteFromNoteNumber_ShouldReturnAlterBasedOnNoteNumber(int nn, int alter)
        {
            Assert.AreEqual(alter, guidesController.getNoteFromNoteNumber(nn).Alter);
        }

        //Test case for testing the note enum.
        //Test case: string, enum note type.
        [Test]
        [TestCase("Hele noot", NoteType.Whole)]
        [TestCase("Gibberishhhhh", NoteType.Quarter)]
        [TestCase("Halve noot", NoteType.Half)]
        [TestCase("", NoteType.Quarter)]
        public void SetNote_ShouldReturnCorrectEnumType(string noteString, NoteType noteType)
        {
            guidesController.SetNote(noteString);
            Assert.AreEqual(noteType, guidesController.Note);
        }

        [TestCase(301, typeof(BpmOutOfRangeException))]
        [TestCase(1001, typeof(BpmOutOfRangeException))]
        [TestCase(-101, typeof(BpmOutOfRangeException))]
        [TestCase(-1, typeof(BpmOutOfRangeException))]
        public void SetBpm_BpmOutsideRange_ReturnException(int bpm, Type expectedException)
        {
            Assert.Throws(expectedException, () => guidesController.Bpm = bpm);

        }
    }
}
