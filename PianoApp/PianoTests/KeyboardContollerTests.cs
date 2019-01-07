using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using PianoApp.Controllers;

namespace ControllerTests
{
    /// <summary>
    /// Summary description for KeyboardContollerTests
    /// </summary>
    [TestFixture]
    public class KeyboardContollerTests
    {
        KeyboardController kc;
        public KeyboardContollerTests()
        {
            kc = new KeyboardController();
        }

        [Test]
        public void CurrentOctave_When48_ShouldReturn4()
        {
            KeyboardController.KeyOffset = 48;
            Assert.AreEqual(4, kc.CurrentOctave());
        }

        [Test]
        public void OctaveUp_When60_ShouldReturn5()
        {
            KeyboardController.KeyOffset = 60;
            Assert.AreEqual(5, kc.CurrentOctave());
        }
    }
}
