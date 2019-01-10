using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PianoApp.Controllers;
using PianoApp.Models;
using System.Windows.Controls;
using System.Threading;
using MusicXml.Domain;

namespace ControllerTests
{
    /// <summary>
    /// Summary description for MusicPieceControllerTests
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class MusicPieceControllerTests
    {
        public MusicPieceControllerTests()
        {
                  
        }

        //Test case for testing the amount of great staves added based on measures.
        //Test case: amount of measures.
        [Test]
        [TestCase(0)]
        [TestCase(12)]
        [TestCase(120)]
        [TestCase(86)]
        [TestCase(50)]
        public void AddGreatStaffsToSheet_ShouldAddRightAmountOfGreatStaffs(int parts)
        {
            var musicPieceController = new MusicPieceController();
            var score = new Score();
            var sheetController = new SheetController();
            //Add parts to score.
            score.Systems = parts;                       
            musicPieceController.Sheet = sheetController.SheetModel;
            musicPieceController.Score = score;
            //Add great staves based on amount of parts in score.
            musicPieceController.AddGreatStaffsToSheet();
            //Amount of great staves should equal to amount of parts.
            Assert.AreEqual(parts, musicPieceController.Sheet.GreatStaffModelList.Count);
        }        

        //Test case for testing the amount of measures added to a stave based on measures.
        //Test case: amount of measures.
        [Test]
        [TestCase(0)]
        [TestCase(99)]
        [TestCase(220)]
        [TestCase(3)]
        [TestCase(5)]
        public void AddMeasuresToGreatStaffs_ShouldAddRightAmountOfMeasuresToStaff(int measures)
        {
            var musicPieceController = new MusicPieceController();
            var score = new Score();
            var sheetController = new SheetController();
            //Add a part to the score.
            score.Parts.Add(new Part());
            //Add measures to the part.
            for (var i = 0; i < measures; i++)
            {
                score.Parts.First().Measures.Add(new Measure());
            }
                                   
            musicPieceController.Sheet = sheetController.SheetModel;
            musicPieceController.Sheet.GreatStaffModelList.Add(new GreatStaffModel());
            musicPieceController.Score = score;

            musicPieceController.AddMeasuresToGreatStaffs();

            Assert.AreEqual(measures, musicPieceController.Sheet.GreatStaffModelList[0].MeasureList.Count);
        }
    }
}
