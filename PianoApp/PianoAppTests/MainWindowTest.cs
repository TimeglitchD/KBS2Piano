using System;
using NUnit.Framework;
using PianoApp;
using PianoApp.Controllers;
using PianoApp.Models.Exception;

namespace PianoAppTests
{
    [TestFixture]
    public class MainWindowTest
    {

        public MusicPieceController GetMusicPieceController()
        {
            PianoController pC = new PianoController();
            MusicPieceController mPc = new MusicPieceController() { Piano = pC };

            return mPc;
        }

        [TestCase("60", 60.0f)]
        public void StartBtn_Click_BpmTextToInt_TextSetToInt(string input, float result)
        {
            // Arrange and Act
            float intVal = (float)int.Parse(input);

            // Assert
            Assert.AreEqual(intVal, result);
        }

        public enum NoteType
        {
            Quarter,
            Half,
            Whole
        }

        [TestCase("Hele noot", NoteType.Whole)]
        public void StartBtn_Click_SaveNoteType_CorrectNoteTypeSaved(string note, NoteType result)
        {
            // Arrange
            MusicPieceController mPc = GetMusicPieceController();

            // Act
            mPc.Guide.SetNote(note);

            Console.WriteLine(result);

            // Assert
            Assert.AreEqual((NoteType)mPc.Guide.Note, result);
        }

        [TestCase(301, typeof(BpmOutOfRangeException))]
        public void StartBtn_Click_BpmOutsideRange_ReturnsException(int input, Type expectedException)
        {
            // Arrange
            MusicPieceController mPc = GetMusicPieceController();

            // Assert
            Assert.Throws(expectedException, () => mPc.Guide.Bpm = input);
        }
    }
}
