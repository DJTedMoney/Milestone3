using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

using System.Diagnostics;

namespace SQLiteTest
{
    class TCPServer
    {
        static TcpListener listener;
        const int numberPlayers = 4;
        protected static PlayerSocket[] activePlayers = new PlayerSocket[numberPlayers];
        static Queue<string> movesMade;

        public static LoginDatabase dB;

        public TCPServer()
        { // start constructor
            listener = new TcpListener(8008);
            listener.Start();

            movesMade = new Queue<string>();

            Console.Write("Press Enter to start the server:  ");
            Console.Read();

            // counting by t
            for (int t = 0; t < numberPlayers; ++t)
            { // start for loop 
                Socket sock = listener.AcceptSocket();
                Console.WriteLine("Player Socket has accepted the socket");

                NetworkStream nws = new NetworkStream(sock);
                StreamReader reader = new StreamReader(nws);
                StreamWriter writer = new StreamWriter(nws);
                writer.AutoFlush = true;

                activePlayers[t] = new PlayerSocket(nws, sock, reader, writer);

                activePlayers[t].playerWriter.WriteLine(t);
                activePlayers[t].connected = true;

                string data = activePlayers[t].playerReader.ReadLine();
                Console.Write(data);

                ReadThread thread = new ReadThread(t);

                activePlayers[t].psThread = new Thread(new ThreadStart(thread.Service) );
                activePlayers[t].psThread.Start();

            } // end for loop 
        } // end constructor

        public class ReadThread
        { // start readThread class 
            int client = -1;
            char delimiter = '$';
            string clientString;

            public ReadThread(int newNumber)
            { // begin constructor
                client = newNumber;
                clientString = newNumber.ToString();
            } // end constructor

            public void Service() // for an individual operating thread
            { // begin service
                try
                { // start try

                    while (activePlayers[client].connected)
                    { // actual game loop for an individual player
                        string data = activePlayers[client].playerReader.ReadLine();
                        Console.WriteLine("before delimiter print " + data);

                        movesMade.Enqueue(data);

                        string[] instruction = movesMade.Dequeue().Split(delimiter);

                        // if string[0] == "l" -> command to attempt login 

                        // counting by w
                        for (int w = 0; w < numberPlayers; ++w)
                        {
                            activePlayers[w].playerWriter.WriteLine(instruction);
                        }

                    } // end game loop for a player

                } // end try

                catch (Exception beiber)
                {
                    Console.WriteLine("Exception " + beiber.Message);
                }
                
            } // end service

        } // end readThread class
    }
}
