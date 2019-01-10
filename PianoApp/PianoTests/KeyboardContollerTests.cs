using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using PianoApp.Controllers;
using System.Windows.Input;
using System.Windows;
using Moq;
using System.Threading;

namespace ControllerTests
{
    /// <summary>
    /// Summary description for KeyboardContollerTests
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class KeyboardContollerTests
    {
        KeyboardController kc;
        public KeyboardContollerTests()
        {
            kc = new KeyboardController();
        }

        [Test]
        [TestCase(12, 1)]
        [TestCase(24, 2)]
        [TestCase(36, 3)]
        [TestCase(48, 4)]
        public void CurrentOctave_ShouldReturnRightOctave(int offset, int octave)
        {
            KeyboardController.KeyOffset = offset;
            Assert.AreEqual(octave, kc.CurrentOctave());
        }

        [Test]
        [TestCase(60, 5)]
        [TestCase(72, 6)]
        public void OctaveUp_ShouldReturnRightOctave(int offset, int octave)
        {
            KeyboardController.KeyOffset = offset;
            Assert.AreEqual(octave, kc.CurrentOctave());
        }

        [Test]
        public void KeyDown_GuideIsNull_ReturnTrue()
        {
            KeyEventArgs keyEvent = new KeyEventArgs(
                 Keyboard.PrimaryDevice,
                 new Mock<PresentationSource>().Object,
                 0,
                 Key.Back);
            kc.Guide = new GuidesController(new MidiController());
            kc.KeyDown(keyEvent);

            Assert.AreEqual(kc.GuideIsNull, false);
        }

        [Test]
        public void KeyUp_GuideIsNull_ReturnTrue()
        {
            KeyEventArgs keyEvent = new KeyEventArgs(
                 Keyboard.PrimaryDevice,
                 new Mock<PresentationSource>().Object,
                 0,
                 Key.Back);
            kc.Guide = new GuidesController(new MidiController());
            kc.KeyDown(keyEvent);

            Assert.AreEqual(kc.GuideIsNull, false);
        }
    }
}
