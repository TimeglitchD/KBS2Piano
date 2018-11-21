using System;
using NUnit.Framework;
using PianoApp;
using System.Threading;


namespace PianoAppTests
{
    [TestFixture]
    public class DatabaseConnectionTests
    {
        [Test]
        public void Database_AddRecord()
        {
            //arrange
            DatabaseConnection connection = new DatabaseConnection();

            //act
            bool test = connection.addMusic("test", "test", "121212", "1", "test");

            //assert
            Assert.IsTrue(test);

        }

        [Test]
        public void Database_GetMusic()
        {
            //arrange
            DatabaseConnection connection = new DatabaseConnection();

            //act
            var test = connection.getSheetMusic(1);

            //assert
            Assert.IsNotNull(test);

        }
    }
}
