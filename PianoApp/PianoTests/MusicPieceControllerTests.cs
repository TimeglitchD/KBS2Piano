using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using PianoApp.Controllers;
using PianoApp.Models;
using System.Windows.Controls;

namespace ControllerTests
{
    /// <summary>
    /// Summary description for MusicPieceControllerTests
    /// </summary>
    [TestFixture]
    public class MusicPieceControllerTests
    {
        MusicPieceController musicPieceController;

        public MusicPieceControllerTests()
        {
            musicPieceController = new MusicPieceController();
        }

        [Test]
        [Apartment(System.Threading.ApartmentState.STA)]
        public void StandardSheet_ReturnStackPanel()
        {
            GreatStaffModel greatStaffModel = new GreatStaffModel();
            SheetModel Sheet = new SheetModel();
            Sheet.GreatStaffModelList.Add(greatStaffModel);

            Assert.That(Sheet.DrawSheet(), Is.TypeOf(typeof(StackPanel)));
        }
    }
}
