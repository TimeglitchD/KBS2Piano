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
        public DataSet getSheetMusic(int id)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                try
                {
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
        
        public DataSet get5SheetScore(int id)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = $"SELECT TOP(5) Id, Date, Time, Scored FROM Score WHERE Id = {id} ORDER BY Scored DESC";
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
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                } catch (Exception)
                {

                }
            }
        }


        //method for adding music record to database. Unused
        public bool addMusic(string title, string description, string date, string type, string location)
        {
            try
            {
                StringBuilder command = new StringBuilder("INSERT INTO music (Title, Description, Date, Type, Location) VALUES (");
                command.Append("'" + title + "' ,");
                command.Append("'" + description + "' ,");
                command.Append("'" + date + "' ,");
                command.Append("'" + type + "' ,");
                command.Append("'" + location + "' )");

                this.ExcecuteCommandNoOutput(command.ToString());
                return true;

            }
            catch (SqlException)
            {
                return false;
            }
        }
    }
}
