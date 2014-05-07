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
        const int numberPlayers = 1;
        protected static PlayerSocket[] activePlayers = new PlayerSocket[numberPlayers];
        static Queue<string> movesMade;

        public static LoginDatabase dB;

        public TCPServer()
        { // start constructor

            dB = new LoginDatabase();

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

                Console.WriteLine("stream created");

                activePlayers[t] = new PlayerSocket(nws, sock, reader, writer);
                activePlayers[t].connected = true;

                Console.WriteLine(" connected true");

                // string data = activePlayers[t].playerReader.ReadLine();
                // Console.Write(data);

                // creates a ReadThread, passes in the player value t
                ReadThread thread = new ReadThread(t);

                activePlayers[t].psThread = new Thread(new ThreadStart(thread.Service) );
                activePlayers[t].psThread.Start();
                Console.Write("bottom of for loop!\n");
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

                        // if instruction[0] == "1" -> command to attempt login
                        // indexes of instruction   [0]     [1]                         [2]                         [3]
                        // expected sentence:       1   $   userName (pre-encrypted) $  elephant (pre-crypted) $    password (pre-encrypted) $ 
                        if (instruction[0] == "1")
                        {
                            dB.login(instruction[1], instruction[3]);

                            activePlayers[client].playerWriter.WriteLine("1$" + client.ToString());
                        }

                        // if instruction[0] == "2" -> command to change directions
                        // indexes of instruction   [0]     [1]                         [2]                                     [3]
                        // expected sentence:       2 $     tostada (pre-crypted) $     { U D L R } $ "*****" (pre-crypt) $     { number of player who made the move }

                        if (instruction[0] == "2")
                        {
                            // update direction that the indicated player is traveling 

                            // counting by w
                            for (int w = 0; w < numberPlayers; ++w)
                            {
                                activePlayers[w].playerWriter.WriteLine(instruction);
                            }
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
