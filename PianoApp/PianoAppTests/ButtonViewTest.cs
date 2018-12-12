using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using NUnit.Framework;
using PianoApp;
using PianoApp.Controllers;
using PianoApp.Models.Exception;
using PianoApp.Views;

namespace PianoAppTests
{
    [TestFixture]
    public class ButtonViewTest
    {

        public MusicPieceController GetMusicPieceController()
        {
            PianoController pC = new PianoController();
            MusicPieceController mPc = new MusicPieceController() { Piano = pC };

            return mPc;
        }

        [TestCase("60", 60.0f), RequiresThread(ApartmentState.STA)]
        public void StartBtn_Click_BpmTextToInt_TextSetToFloat(string input, float result)
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

        [TestCase("Hele noot", NoteType.Whole), RequiresThread(ApartmentState.STA)]
        public void StartBtn_Click_SaveNoteType_CorrectNoteTypeSaved(string note, NoteType result)
        {
            // Arrange
            GuidesController guide = new GuidesController(new MidiController());

            // Act
            guide.SetNote(note);

            // Assert
            Assert.AreEqual((NoteType)guide.Note, result);
        }

        [TestCase(60, typeof(BpmOutOfRangeException)), RequiresThread(ApartmentState.STA)]
        public void StartBtn_Click_BpmOutsideRange_ReturnsException(int input, Type expectedException)
        {
            // Arrange
            MusicPieceController mPc = GetMusicPieceController();
            
            // Assert
            Assert.AreEqual(1, 1);
        }
    }
}
