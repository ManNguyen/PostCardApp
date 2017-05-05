using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Web;

namespace WebAuthenticatePostCard.SQLiteContext
{
    public class WebPostCardDb
    {
        private static string _MyDataBase;
        public static DataTable GetUser(string email, string password)
        {
            using (var m_dbConnection = new SQLiteConnection("Data Source=" + _MyDataBase + ";Version=3;"))
            {
                string sql = "SELECT * FROM [UserTbl] WHERE email = @email and password = @password";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);

                SQLiteDataAdapter sqldata = new SQLiteDataAdapter(command);
                DataTable dt = new DataTable();
                sqldata.Fill(dt);
                return dt;
            }
        }
        public static bool InsertUser(string email, string name, string password)
        {
            using (var m_dbConnection = new SQLiteConnection("Data Source=" + _MyDataBase + ";Version=3;"))
            {
                //Check if the email already in the database
                string sql = "SELECT * FROM [UserTbl] WHERE email = @email";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.Parameters.AddWithValue("@email", email);

                SQLiteDataAdapter sqldata = new SQLiteDataAdapter(command);
                DataTable dt = new DataTable();
                sqldata.Fill(dt);
                //If the email's already used
                if(dt.Rows.Count>0) return false;
                sql = "insert into [UserTbl](email, name, password) values (@email, @name, @password)";
                command = new SQLiteCommand(sql, m_dbConnection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@password", password);

                sqldata = new SQLiteDataAdapter(command);
                return true;
            }
        }
        public static void CreateDB()
        {
            _MyDataBase = HttpContext.Current.Server.MapPath("~") + "MyDatabase.sqlite";
            if (!File.Exists(_MyDataBase))
            {
                SQLiteConnection.CreateFile(_MyDataBase);
                using (var m_dbConnection = new SQLiteConnection("Data Source=" + _MyDataBase + ";Version=3;"))
                {
                    m_dbConnection.Open();
                    //Create table UserTbl to store email, name and password
                    string sql = "create table [UserTbl] (id INTEGER PRIMARY KEY AUTOINCREMENT,email varchar(50),  name varchar(50), password varchar(50))";
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                    // Make 1 user that is me; 
                    sql = "insert into [UserTbl](email, name, password) values ('man@postcard.com','Me', '12345')";
                    command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                }
 
            }


        }
    }
}