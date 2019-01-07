using NUnit.Framework;
using PianoApp.Controllers;

namespace ControllerTests
{
    /// <summary>
    /// Summary description for GuidesControllerTests
    /// </summary>
    [TestFixture]
    public class GuidesControllerTests
    {
        GuidesController guidesController;

        public GuidesControllerTests()
        {
            MidiController midiController = new MidiController();
            guidesController = new GuidesController(midiController);
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [Test]
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //
        }
    }
}
