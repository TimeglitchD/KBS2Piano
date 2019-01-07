using System;
using NUnit.Framework;
using PianoApp;
using System.Threading;


namespace PianoAppTests
{
    [TestFixture]
    public class DatabaseConnectionTests
    {
        //[Test]
        //public void Database_AddRecord()
        //{
        //    //arrange
        //    DatabaseConnection connection = new DatabaseConnection();

        //    //act
        //    bool test = connection.addMusic("test", "test", "121212", "test");

        //    //assert
        //    Assert.IsTrue(test);

        //}

        //[Test]
        //public void Database_AddRecord_WrongData()
        //{
        //    //arrange
        //    DatabaseConnection connection = new DatabaseConnection();

        //    //act
        //    bool test = connection.addMusic("test", "test", "121212", "test");

        //    //assert
        //    Assert.IsFalse(test);

        //}

        //[Test]
        //public void Database_GetMusic()
        //{
        //    //arrange
        //    DatabaseConnection connection = new DatabaseConnection();

        //    //act
        //    var test = connection.getSheetMusic();

        //    //assert
        //    Assert.IsNotNull(test);

        //}

        //[TestCase(1)]
        //public void Database_UpdateBpmToMusicPiece_DataHasBeenUpdatedInDatabase(int result)
        //{
        //    //arrange
        //    DatabaseConnection dbCon = new DatabaseConnection();
        //    dbCon.addMusic("test", "test", "121212", "test", "test");
        //    dbCon.ExcecuteCommandNoOutput($"UPDATE Music SET Bpm = (100) WHERE Id = 1");

        //    var dbRecord = dbCon.GetDataFromDB($"Select id from Music where Id = 2", "Music");
        //    var bpm = Convert.ToInt32(dbRecord.Tables[0].Rows[0].ItemArray.GetValue(0).ToString());
        //    //assert
        //    Assert.AreEqual(bpm, result);
        //}
    }
}