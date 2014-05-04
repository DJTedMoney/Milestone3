using System;

using System.Data.SQLite;

namespace SQLiteTest
{

    class ServerDatabase
    {
        
        LoginDatabase dB = new LoginDatabase();
        
        static void Main(string[] args)
        {
            ServerDatabase p = new ServerDatabase();
        }

        public ServerDatabase()
        {
            dB.createNewDatabase();
            dB.connectToDatabase();

            dB.createTable();

            dB.fillTable();

            dB.addElement("farble", "deblarg");
            dB.checkIfUserNameExists("glory");

            dB.printUsers();
        }
        
    }
}
