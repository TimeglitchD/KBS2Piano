using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace PianoApp
{
    public class DatabaseConnection
    {
        private string connectionString;
        public DataSet dataSet = new DataSet();

        public DatabaseConnection()
        {
            //point connectionstring to local database
            connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename =" + System.AppDomain.CurrentDomain.BaseDirectory
            + @"MusicDatabase.mdf; Integrated Security = True";
        }

        //get a dataset of sheet music based on type specified.
        public DataSet getSheetMusic()
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                try
                {
                    dataSet.Clear();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = $"SELECT * FROM Music";
                    dataAdapter.SelectCommand = command;
                    connection.Open();
                    dataAdapter.Fill(dataSet, "Music");
                    connection.Close();
                    return dataSet;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public DataSet getCourseMusic(string courseType)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = $"SELECT * FROM Music WHERE CourseType = '{courseType}'";
                    dataAdapter.SelectCommand = command;
                    connection.Open();
                    dataAdapter.Fill(dataSet, courseType);
                    connection.Close();
                    return dataSet;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public DataSet getSheetScore()
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = $"SELECT * FROM Score ORDER BY Scored DESC";
                    dataAdapter.SelectCommand = command;
                    connection.Open();
                    dataAdapter.Fill(dataSet, "Score");
                    connection.Close();
                    return dataSet;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public DataSet GetDataFromDB(string query, string table)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = query;
                    dataAdapter.SelectCommand = command;
                    connection.Open();
                    dataAdapter.Fill(dataSet, table);
                    connection.Close();
                    return dataSet;
                }
                catch (Exception)
                {
                    Console.WriteLine("failed..");
                    return null;
                }
            }
        }

        //function for update, insert, delete statements
        public void ExcecuteCommandNoOutput(string queryString)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = queryString;
                    command.Connection = connection;

                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception)
                {

                }
            }
        }

        public bool UpdateRecord(string courseType, string id)
        {
            string query = "UPDATE Music " +
                            "SET CourseType = @CourseType " +
                            "WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@CourseType", courseType);
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    connection.Close();

                    if (result < 0)
                        Console.WriteLine("Error updating data in Database!");

                    return true;
                }
                catch (SqlException ex)
                {
                    return false;
                }
            }

        }


        //method for adding music record to database. Unused
        public bool addMusic(string title, string description, string date, string location, string courseType)
        {
            string query = "INSERT INTO Music (Title,Description,Date,Location, CourseType)" +
                                " VALUES (@Title,@Description,@Date,@Location, @CourseType)";

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Location", location);
                    command.Parameters.AddWithValue("@CourseType", courseType);

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    connection.Close();

                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");

                    return true;
                }
                catch (SqlException ex)
                {
                    return false;
                }
            }
        }

        public bool DeleteFromDb(string id)
        {
            string query = "DELETE FROM Music " +
                           "WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    connection.Close();

                    if (result < 0)
                        Console.WriteLine("Error deleting data from Database!");

                    return true;
                }
                catch (SqlException ex)
                {
                    return false;
                }
            }

        }

        public void updateScore(int id, int score)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO Score VALUES (@id, @date, @time, @scored)";
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@date", DateTime.Now);
                    command.Parameters.AddWithValue("@time", DateTime.Now.ToLocalTime());
                    command.Parameters.AddWithValue("@scored", score);
                    command.Connection = connection;

                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception)
                {

                }
            }
        }

    }
}