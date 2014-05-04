using System;

using System.Data.SQLite;

namespace SQLiteTest
{

    class ServerDatabase
    {
        static LoginDatabase dB;

        static ServerDatabase p = new ServerDatabase();
        
        static void Main(string[] args)
        {
            dB.createNewDatabase();
            dB.connectToDatabase();

            dB.createTable();

            dB.fillTable();

            dB.login("farble", "deblarg");
            // dB.checkIfUserNameExists("glory");
            // dB.checkIfUserNameExists("farble");



            dB.printUsers();
        }

        public ServerDatabase()
        {
            dB = new LoginDatabase();
        }
        
    }
}
