using System;
using NUnit.Framework;
using PianoApp;
using System.Threading;

namespace PianoAppTests
{
    [TestFixture]
    public class MetronomeTest
    {
        [Test]
        public void Metronome_stop()
        {
            //arrange
            var sound = new metronomeSound();

            ////act
            sound.startMetronome(120, 2, 1);
            Thread.Sleep(200);


            ////Assert
            Assert.IsTrue(sound.stopMetronome());
        }

        [Test]
        public void MetronomeWithCountdown_Stop_StartWithoutCountDown()
        {
            //arrange
            var sound = new metronomeSound();

            ////act
            sound.startMetronome(120, 2, 1);
            Thread.Sleep(1000);
            sound.stopMetronome();
            sound.startMetronome(50, 2);

            ////Assert
            Assert.IsFalse(sound.getCountdown());
        }
    }
}
