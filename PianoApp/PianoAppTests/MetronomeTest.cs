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
        public void Metronome_Stop()
        {
            //arrange
            var sound = new metronomeSound();

            //act
            sound.startMetronome(5, 5);

            //assert
            Assert.IsTrue(sound.stopMetronome());
        }

        [Test]
        public void MetronomeWithCountdown_Stop_StartWithoutCountDown()
        {
            //arrange
            var sound = new metronomeSound();

            //act
            sound.startMetronome(300, 1, 1);
            //wait until countdown is finished
            Thread.Sleep(1000);
            sound.stopMetronome();

            //Assert
            Assert.IsFalse(sound.getCountdown());
        }
    }
}
