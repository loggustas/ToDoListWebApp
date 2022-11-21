using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;


namespace ToDoListDomainModel
{
    public class DatabaseConnection
    {
#pragma warning disable S2933 // Fields that are only assigned in the constructor should be "readonly"
        private string connectionString;
#pragma warning restore S2933 // Fields that are only assigned in the constructor should be "readonly"

        public DatabaseConnection()
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory());

            builder.AddJsonFile("appsettings.json");

            var config = builder.Build();

            this.connectionString = config.GetConnectionString("DefaultConnection");
        }

        public void InsertIntoDb(string toDoListName, Record[] records)
        {
            if (toDoListName == null)
            {
                throw new ArgumentNullException(nameof(toDoListName));
            }

            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            int n = records.Length;
            for (int i = 0; i < n; i++)
            {
                if (records[i] == null)
                {
                    throw new ArgumentException("records array must contain only initialized values");
                }
            }

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                StringBuilder sql = new StringBuilder();
                int rowAmount = 0;  // how many rows added
                int p = 1;  // parameter index

                foreach (Record c in records)
                {
                    string pToDoList = string.Format("@p{0}", p);
                    string pNumber = string.Format("@p{0}", p + 1);
                    string pTitle = string.Format("@p{0}", p + 2);
                    string pDescription = string.Format("@p{0}", p + 3);
                    string pDueDate = string.Format("@p{0}", p + 4);
                    string pStatus = string.Format("@p{0}", p + 5);
                    p += 6;

                    string row = string.Format("({0}, {1}, {2}, {3}, {4}, {5})", pToDoList, pNumber, pTitle, pDescription, pDueDate, pStatus);

                    if (rowAmount > 0)
                    {
                        sql.AppendLine(",");
                    }
                    sql.Append(row);
                    rowAmount++;

                    cmd.Parameters.Add(pToDoList, SqlDbType.NChar, 150).Value = c.toDoList;
                    cmd.Parameters.Add(pNumber, SqlDbType.Int).Value = c.Number;
                    cmd.Parameters.Add(pTitle, SqlDbType.NChar, 150).Value = c.Title;
                    cmd.Parameters.Add(pDescription, SqlDbType.NChar, 500).Value = c.Description;
                    cmd.Parameters.Add(pDueDate, SqlDbType.Date).Value = c.DueDate;
                    cmd.Parameters.Add(pStatus, SqlDbType.NChar, 20).Value = c.Status;

                    if (rowAmount >= 5)
                    {
                        string sqlCmd = "INSERT INTO Records (To_Do_List, Number, Title, Description, Due_Date, Status) VALUES" + "\r\n" + sql.ToString();
                        cmd.CommandText = sqlCmd;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        sql.Clear();
                        rowAmount = 0;
                        p = 1;
                    }
                }

                if (rowAmount > 0)
                {
                    string sqlCmd = "INSERT INTO Records (To_Do_List, Number, Title, Description, Due_Date, Status) VALUES" + "\r\n" +
                                 sql.ToString();
                    cmd.CommandText = sqlCmd;
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void UpdateByTableAndTitle(string toDoListName, string title, Record record)
        {
            if (toDoListName == null)
            {
                throw new ArgumentNullException(nameof(toDoListName));
            }

            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if(record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string sql = "UPDATE Records SET Title = @TitleChange, Description = @DescriptionChange, Due_Date = @DueDateChange, Status = @StatusChange WHERE Title = @Title AND To_Do_List = @ToDoList;";
                connection.Open();

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add("@ToDoList", SqlDbType.NChar, 150).Value = toDoListName;
                cmd.Parameters.Add("@Title", SqlDbType.NChar, 150).Value = title;
                cmd.Parameters.Add("@TitleChange", SqlDbType.NChar, 150).Value = record.Title;
                cmd.Parameters.Add("@DescriptionChange", SqlDbType.NChar, 500).Value = record.Description;
                cmd.Parameters.Add("@DueDateChange", SqlDbType.Date).Value = record.DueDate;
                cmd.Parameters.Add("@StatusChange", SqlDbType.NChar, 500).Value = record.Status;

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void ToggleByTableAndName(string toDoListName, string title)
        {
            if (toDoListName == null)
            {
                throw new ArgumentNullException(nameof(toDoListName));
            }

            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string sql = "SELECT * FROM Records WHERE To_Do_List = @ToDoList AND Title = @Title;";
                SqlCommand cmd = new SqlCommand(sql, connection);

                connection.Open();
                cmd.Parameters.Add("@ToDoList", SqlDbType.NChar, 150).Value = toDoListName;
                cmd.Parameters.Add("@Title", SqlDbType.NChar, 150).Value = title;

                List<Record> records = ReadFromSql(cmd);
                cmd.CommandText = "UPDATE Records SET Status = @Status WHERE Title = @Title AND To_Do_List = @ToDoList;";
                
                if (records.Count == 0)
                {
                    throw new ArgumentException("There is no such record");
                }
                else if (records[0].Status.Equals("Uncompleted"))
                {
                    cmd.Parameters.Add("@Status", SqlDbType.NChar, 20).Value = "Completed";
                }
                else
                {
                    cmd.Parameters.Add("@Status", SqlDbType.NChar, 20).Value = "Uncompleted";
                }

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public List<Record> Retrieve(string toDoListName)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string sql = "SELECT * FROM Records WHERE To_Do_List = @ToDoList ORDER BY Number;";
                SqlCommand cmd = new SqlCommand(sql, connection);

                connection.Open();
                cmd.Parameters.Add("@ToDoList", SqlDbType.NChar, 150).Value = toDoListName;

                List<Record> records = ReadFromSql(cmd);

                connection.Close();

                return records;
            }
        }

        public List<string> RetrieveNamesOfList()
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string sql = "SELECT DISTINCT To_Do_List FROM Records;";
                SqlCommand cmd = new SqlCommand(sql, connection);

                connection.Open();

                List<string> names = new List<string>();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        names.Add(reader["To_Do_List"].ToString()?.Trim());
                    }
                }

                connection.Close();

                return names;
            }
        }

        private List<Record> ReadFromSql(SqlCommand cmd)
        {
            List<Record> records = new List<Record>();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    records.Add(new Record { Id = (int)reader["Id"], toDoList = reader["To_Do_List"].ToString()?.Trim(), Number = (int)reader["Number"], Title = reader["Title"].ToString()?.Trim(), Description = reader["Description"].ToString()?.Trim(), DueDate = ((DateTime)reader["Due_Date"]).ToString("yyyy-MM-dd")?.Trim(), Status = reader["Status"].ToString()?.Trim() });
                }
            }

            return records;
        }

        public void DeleteFromDb(string toDoList)
        {
            if (toDoList == null)
            {
                throw new ArgumentNullException(nameof(toDoList));
            }

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string sql = "DELETE FROM Records WHERE To_Do_List = @ToDoList;";
                connection.Open();

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add("@ToDoList", SqlDbType.NChar, 150).Value = toDoList;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void DeleteEverything()
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                string sql = "TRUNCATE TABLE Records;";
                SqlCommand cmd = new SqlCommand(sql, connection);
                connection.Open();
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
