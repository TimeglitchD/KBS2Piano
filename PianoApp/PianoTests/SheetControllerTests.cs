using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using PianoApp.Controllers;

namespace ControllerTests
{
    /// <summary>
    /// Summary description for SheetControllerTests
    /// </summary>
    [TestFixture]
    public class SheetControllerTests
    {
        private PianoController pC;
        private Dictionary<int, float> activeKeys;
        public SheetControllerTests()
        {
            pC = new PianoController();
            activeKeys.Add(0, 0);
        }

        //Test case for testing the note enum.
        //Test case: string, enum note type.
//        [Test]
//        [TestCase("Hele noot", NoteType.Whole)]
//        public void UpdatePressedPianoKeys_ShouldReturnShouldSetCorrespondingPianoKeyToActive(string noteString, NoteType noteType)
//        {
//            guidesController.SetNote(noteString);
//            Assert.AreEqual(noteType, guidesController.Note);
//        }

    }
}
