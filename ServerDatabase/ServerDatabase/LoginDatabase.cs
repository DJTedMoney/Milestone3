using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SQLite;

namespace SQLiteTest
{
    class LoginDatabase
    {
        // Holds our connection with the database
        SQLiteConnection m_dbConnection;

        // Creates an empty database file
        public void createNewDatabase()
        {
            SQLiteConnection.CreateFile("LoginDatabase.sqlite");
        }

        // Creates a connection with our database file.
        public void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source=LoginDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
        }

        // Creates a table named 'highscores' with two columns: name (a string of max 20 characters) and password (a string of max 20 characters)
        // password = pw
        public void createTable()
        {
            string sql = "create table users (name varchar(32), pw varchar(32) )";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        // Inserts some values in the highscores table.
        // As you can see, there is quite some duplicate code here, we'll solve this in part two.
        public void fillTable()
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
            // true if user exists in the database, false if the user is a new user
            bool returningUser = checkIfUserNameExists(userName);
         
            string sql = "insert into users (name, pw) values (' " + userName + " ', ' " + newPW + " ')";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        void verifyPassword(string loginName, string loginPW)
        {
            // check that newPW matches password of userName
            string sql = "select * from users where name= ' " + loginName + " ', ' " + loginPW + " ')";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            bool pwMatch = reader.Read();

            if (pwMatch)
            {
                Console.WriteLine("    Password is a match    ");
                // return true;
            }

            Console.WriteLine("  &&  Password is NOT a match  ");
            // return false;
        }

        bool checkIfUserNameExists(string userName)
        {
            string sql = "select * from users where name= ' " + userName + " ' ";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            bool userExists = reader.Read();

            if (userExists )
            {
                Console.WriteLine("  found it!");
                return true;
            }

            Console.WriteLine("   didn't find it");
            return false;
        }

        public void login(string checkName, string checkPW)
        {
            bool returningUser = checkIfUserNameExists(checkName);

            if (returningUser)
            {
                verifyPassword(checkName, checkPW);
            }

            else
            {
                addElement(checkName, checkPW);
            }
        }

        // Writes the highscores to the console sorted on score in descending order.
        public void printUsers()
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
