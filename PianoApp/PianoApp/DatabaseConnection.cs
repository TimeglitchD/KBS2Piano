﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace PianoApp
{
    class DatabaseConnection
    {
        private string connectionString;

        public DatabaseConnection()
        {
            connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename =" + System.AppDomain.CurrentDomain.BaseDirectory
            + @"MusicDatabase.mdf; Integrated Security = True";
        }

        public void addMusic(string title, string description, string date, string type, string location)
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

            } catch(SqlException ex)
            {
                System.Windows.MessageBox.Show("Database error insert" + ex.Message);
            }
        }

        public DataSet getSheetMusic(int type)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    SqlCommand command = connection.CreateCommand();
                    DataSet dataSet = new DataSet();
                    command.CommandText = "SELECT * FROM music WHERE type =" + type;
                    dataAdapter.SelectCommand = command;
                    connection.Open();
                    dataAdapter.Fill(dataSet, "Music");
                    connection.Close();
                    return dataSet;
                } catch(Exception ex)
                {
                    return null;
                }
            }
        }

        private void ExcecuteCommandNoOutput(string queryString)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}