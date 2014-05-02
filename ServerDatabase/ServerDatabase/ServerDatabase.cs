using System;

using System.Data.SQLite;

namespace SQLiteTest
{

    class ServerDatabase
    {

        // Holds our connection with the database
        SQLiteConnection m_dbConnection;

        static void Main(string[] args)
        {
            ServerDatabase p = new ServerDatabase();
        }

        public ServerDatabase()
        {

            createNewDatabase();
            connectToDatabase();

            createTable();
            fillTable();

            addElement("farble", "deblarg");

            printHighscores();
        }

        // Creates an empty database file
        void createNewDatabase()
        {
            SQLiteConnection.CreateFile("LoginDatabase.sqlite");
        }

        // Creates a connection with our database file.
        void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source=LoginDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
        }

        // Creates a table named 'highscores' with two columns: name (a string of max 20 characters) and password (a string of max 20 characters)
        // password = pw
        void createTable()
        {
            string sql = "create table users (name varchar(20), pw varchar(12) )";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        // Inserts some values in the highscores table.
        // As you can see, there is quite some duplicate code here, we'll solve this in part two.
        void fillTable()
        {

            string sql = "insert into users (name, pw) values ('Me', 3000)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "insert into users (name, pw) values ('Myself', 6000)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "insert into users (name, pw) values ('And I', 9001)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        void addElement(string userName, string newPW)
        {
            string sql = "insert into users (name, pw) values (" + userName + ", " + newPW + ")";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        // Writes the highscores to the console sorted on score in descending order.
        void printHighscores()
        {

            string sql = "select * from users order by pw desc";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("Name: " + reader["name"] + "\tPassword: " + reader["pw"]);
            }

            Console.ReadLine();
        }
    }
}
