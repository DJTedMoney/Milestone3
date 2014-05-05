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

            dB.login("Farble", "deblarg");
            dB.login("glory", "power");
            dB.login("Farble", "deblarg");

            dB.login("Myself", "6000");
            dB.login("Myself", "asdf");

            dB.printUsers();
        }

        public ServerDatabase()
        {
            dB = new LoginDatabase();
        }
        
    }
}
