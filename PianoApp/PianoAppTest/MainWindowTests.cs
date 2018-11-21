using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using PianoApp.Controllers;

namespace PianoAppTest
{
    /// <summary>
    /// Summary description for MainWindowTests
    /// </summary>
    [TestFixture]
    public class MainWindowTests
    {
        [TestCase("60", 60)]
        public void StartBtn_Click_BpmFromTextToInt_BpmSetFromTextToIntSuccess(string bpm, int result)
        {
            // Arrange
            //MusicPieceController mPc;
            //PianoController pC = new PianoController();

            //mPc = new MusicPieceController() { Piano = pC };

            int bpmValue = int.Parse(bpm);

            Assert.AreEqual(bpm, result);
        }
    }
}
