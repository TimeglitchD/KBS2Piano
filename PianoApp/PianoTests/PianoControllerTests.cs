using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using NUnit.Framework;
using PianoApp.Controllers;
using PianoApp.Models;
using MusicXml.Domain;
using Timeout = System.Threading.Timeout;

namespace ControllerTests
{
    /// <summary>
    /// Summary description for PianoControllerTests
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class PianoControllerTests
    {
        private PianoController pC;
        public PianoControllerTests()
        {
            pC = new PianoController();
        }

        //Test case for testing the pressed keys.
        //Test case: the note to test, the octave the note is in. 
        [Test]
        [TestCase(0)]
        [TestCase(3)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        public void UpdatePressedPianoKeys_ShouldSetCorrespondingPianoKeyToActive(int nn)
        {
            pC.UpdatePressedPianoKeys(new Dictionary<int, float>() {{nn, 0}});
            Assert.AreEqual(true,pC.PianoModel.OctaveModelList[1].KeyModelList.FindAll(n => n.Alter != -1)[nn].Active);
        }

        //Test case for testing the pressed keys.
        //Test case: step, alter, octave, staff.
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
        [TestCase('C', 1, 2)]
        [TestCase('D', 1, 2)]
        [TestCase('F', 1, 1)]
        [TestCase('G', 1, 1)]
        public void UpdatePianoKeys_ShouldSetCorrespondingPianoKeyToActive(char step, int alter, int staff)
        {
            var note = new Note(){Pitch = new Pitch(){Alter = alter, Octave = 0, Step = step}, Staff = staff};
            var noteDict = new Dictionary<Note, PianoApp.Controllers.Timeout>() { { note, new PianoApp.Controllers.Timeout() } };
            pC.UpdatePianoKeys(noteDict);           
            Assert.AreEqual(true, pC.PianoModel.OctaveModelList[1].KeyModelList.Find(n => n.Alter == alter && Convert.ToString(n.Step).Equals(step.ToString())).Active);
        }
    }
}
