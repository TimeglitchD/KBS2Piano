using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using PianoApp.Controllers;

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
        [TestCase(2)]
        public void UpdatePressedPianoKeys_ShouldSetCorrespondingPianoKeyToActive(int nn)
        {
            pC.UpdatePressedPianoKeys(new Dictionary<int, float>(){ { nn, 1000 } });
            Assert.AreEqual(true, pC.PianoModel.OctaveModelList[0].KeyModelList.Where(k => k.Alter != -1).ToList()[nn].Active);
        }
    }
}
