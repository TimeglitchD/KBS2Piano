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
            bool test = connection.addMusic("test", "test", "121212", "test");

            //assert
            Assert.IsTrue(test);

        }

        [Test]
        public void Database_AddRecord_WrongData()
        {
            //arrange
            DatabaseConnection connection = new DatabaseConnection();

            //act
            bool test = connection.addMusic("test", "test", "121212", "test");

            //assert
            Assert.IsFalse(test);

        }

        [Test]
        public void Database_GetMusic()
        {
            //arrange
            DatabaseConnection connection = new DatabaseConnection();

            //act
            var test = connection.getSheetMusic();

            //assert
            Assert.IsNotNull(test);

        }
    }
}