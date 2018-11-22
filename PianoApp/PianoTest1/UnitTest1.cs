using System;
using System.Windows.Controls;
using NUnit.Framework;
using PianoApp.Controllers;
using PianoApp.Models;
using PianoApp.Views;
using Assert = NUnit.Framework.Assert;
using System.Threading;

namespace PianoTest1
{
    [TestFixture]
    public class UnitTest1
    {
        [TestCase(6, 6), RequiresThread(ApartmentState.STA)]
        public void DrawPianoModelTest(int amount, int count)
        {
            PianoModel pm = new PianoModel();
            pm.Amount = amount;

            DockPanel PianoModel = pm.DrawPianoModel();

            Assert.AreEqual(pm.DrawPianoModel().Children.Count, count);
        }

        [TestCase(1,12), RequiresThread(ApartmentState.STA)]
        public void DrawOctaveTest(int amount, int count)
        {
            OctaveModel om = new OctaveModel(amount);

            DockPanel dm = om.DrawOctave(50);

            Assert.AreEqual(dm.Children.Count, count);   
        }

        [TestCase(true, "#FF7FFFD4"), RequiresThread(ApartmentState.STA)] //aquamarine
        [TestCase(false, "#FF000000")] //black
        public void ColorTest(bool active, string color)
        {
            BlackKey bk = new BlackKey();
            bk.Active = active;

            bk.Color();
            Assert.AreEqual(bk.color.ToString(), color);
        }
    }


}
