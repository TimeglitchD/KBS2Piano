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
    public class KeyboardContollerTests
    {
        KeyboardController kc;
        public KeyboardContollerTests()
        {
            kc = new KeyboardController();
        }

        [Test]
        public void CurrentOctave_When12_ShouldReturn1()
        {
            KeyboardController.KeyOffset = 12;
            Assert.AreEqual(1, kc.CurrentOctave());
        }

        [Test]
        public void CurrentOctave_When24_ShouldReturn2()
        {
            KeyboardController.KeyOffset = 24;
            Assert.AreEqual(2, kc.CurrentOctave());
        }

        [Test]
        public void CurrentOctave_When36_ShouldReturn3()
        {
            KeyboardController.KeyOffset = 36;
            Assert.AreEqual(3, kc.CurrentOctave());
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

        [Test]
        public void OctaveUp_When72_ShouldReturn6()
        {
            KeyboardController.KeyOffset = 72;
            Assert.AreEqual(6, kc.CurrentOctave());
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void KeyDown_GuideIsNull_ReturnTrue()
        {
            KeyEventArgs keyEvent = new KeyEventArgs(
                 Keyboard.PrimaryDevice,
                 new Mock<PresentationSource>().Object,
                 0,
                 Key.Back);
            kc.Guide = new GuidesController(new MidiController());
            kc.KeyDown(keyEvent);

            Assert.AreEqual(kc.guideIsNull, false);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void KeyUp_GuideIsNull_ReturnTrue()
        {
            KeyEventArgs keyEvent = new KeyEventArgs(
                 Keyboard.PrimaryDevice,
                 new Mock<PresentationSource>().Object,
                 0,
                 Key.Back);
            kc.Guide = new GuidesController(new MidiController());
            kc.KeyDown(keyEvent);

            Assert.AreEqual(kc.guideIsNull, false);
        }
    }
}
