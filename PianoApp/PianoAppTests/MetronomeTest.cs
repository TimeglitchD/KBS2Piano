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

        [Test]
        public void Metronome_countdownEvent()
        {
            //arrange
            var sound = new metronomeSound();
            bool eventRaised = false;
            sound.countdownFinished += (s, e) => { eventRaised = true; };
            ////act
            sound.startMetronome(240, 1, 1);
            Thread.Sleep(2000);
            sound.stopMetronome();

            ////Assert
            Assert.IsTrue(eventRaised);
        }

        [Test]
        public void Metronome_countdownEvent_countDownOnly()
        {
            //arrange
            var sound = new metronomeSound();
            bool eventRaised = false;
            sound.countdownFinished += (s, e) => { eventRaised = true; };
            ////act
            sound.startMetronomeCountDownOnly(240, 1, 1);
            Thread.Sleep(1000);

            ////Assert
            Assert.IsTrue(eventRaised);
        }
    }
}
