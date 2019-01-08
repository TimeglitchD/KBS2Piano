using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using PianoApp.Controllers;
using PianoApp.Models;
using MusicXml.Domain;

namespace ControllerTests
{
    /// <summary>
    /// Summary description for PianoControllerTests
    /// </summary>
    [TestFixture]
    public class PianoControllerTests
    {
        PianoController pianoController;
        public PianoControllerTests()
        {
            pianoController = new PianoController();
        }
    }
}
