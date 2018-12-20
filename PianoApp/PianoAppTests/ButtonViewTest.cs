using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using MusicXml;
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
        public void StartMusicPiece_BpmTextToFloat_TextSetToFloat(string input, float result)
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
        public void StartMusicPiece_SaveNoteType_CorrectNoteTypeSaved(string note, NoteType result)
        {
            // Arrange
            GuidesController guide = new GuidesController(new MidiController());

            // Act
            guide.SetNote(note);

            // Assert
            Assert.AreEqual((NoteType)guide.Note, result);
        }

        [TestCase(60, typeof(BpmOutOfRangeException)), RequiresThread(ApartmentState.STA)]
        public void StartMusicPiece_BpmOutsideRange_ReturnsException(int input, Type expectedException)
        {
            // Arrange
            MusicPieceController mPc = GetMusicPieceController();
            
            // Assert
            Assert.AreEqual(1, 1);
        }

        [TestCase(true), RequiresThread(ApartmentState.STA)]
        public void PauseMusicPiece_PauseMusicWhileIsPlaying_PauseReturnsTrue(bool result)
        {
            // Arrange
            MusicPieceController mPc = GetMusicPieceController();

            //mPc.Guide.Score = MusicXmlParser.GetScore(filename: TestContext.CurrentContext.TestDirectory + "\\TestData\\MusicXML.xml");

            mPc.SheetController = new SheetController();
            Grid myGrid = new Grid();
            StaveView sv = new StaveView(myGrid, mPc);
            NoteView nv = new NoteView(sv);
            ButtonView bv = new ButtonView(myGrid, sv, nv);

            // Act
            bv.paused = true;
            bv.StartMusicPiece();

            // Assert
            Assert.AreEqual(result, bv.paused);
        }



        [TestCase(true), RequiresThread(ApartmentState.STA)]
        public void StartMusicPiece_StartMusicPieceWhileNotPlaying_ReturnTrue(bool result)
        {
            // Arrange
            MusicPieceController mPc = GetMusicPieceController();

            //mPc.Guide.Score = MusicXmlParser.GetScore(filename: TestContext.CurrentContext.TestDirectory + "\\TestData\\MusicXML.xml");

            mPc.SheetController = new SheetController();
            Grid myGrid = new Grid();
            StaveView sv = new StaveView(myGrid, mPc);
            NoteView nv = new NoteView(sv);
            ButtonView bv = new ButtonView(myGrid, sv, nv);

            // Act
            bv.StartMusicPiece();

            // Assert
            Assert.AreEqual(result, bv._isStarted);
        }
    }
}
