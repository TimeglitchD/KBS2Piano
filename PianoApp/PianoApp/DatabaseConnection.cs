using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

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

        public List<Dictionary<string, string>> getLessons()
        {
            List<Dictionary<string, string>> lessons = new List<Dictionary<string, string>>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM music WHERE type = 'lesson'",
                    connection
                );

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Dictionary<string, string> lesson = new Dictionary<string, string>();
                        lesson["title"] = (string)reader["title"];
                        lesson["description"] = (string)reader["description"];
                        lesson["date"] = (string)reader["date"];
                        lesson["location"] = (string)reader["location"];
                        lessons.Add(lesson);
                    }

                    return lessons;
                }

                return null;
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
