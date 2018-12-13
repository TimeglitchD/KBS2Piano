﻿using System;
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
        private void ExcecuteCommandNoOutput(string queryString)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;
                command.Connection = connection;

                command.Connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


        //method for adding music record to database. Unused
        public bool addMusic(string title, string description, string type, string date,  string location)
        {
            string query = "INSERT INTO Music (Title,Description,Date,Type,Location)" +
                                " VALUES (@Title,@Description,@Date,@Type,@Location)";

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                //try
                //{
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Type", type);
                    command.Parameters.AddWithValue("@Location", location);

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    connection.Close();

                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");

                    return true;
                //}
                //catch (SqlException ex)
                //{
                //    return false;
                //}
            }
        }

    }
}