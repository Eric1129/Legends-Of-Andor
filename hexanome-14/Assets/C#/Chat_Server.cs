using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public class Chat_Server
{
    // Port to connect to
    public static int port = 5230;

    List<Client_Connection> clientList = new List<Client_Connection>();
    TcpListener listener;


    public Chat_Server()
    {
        //            TcpClient client = new TcpClient();
        Console.WriteLine("Server-Side: Initializing Server...");
        IPAddress serverIP = Chat_Client.SERVER_ADDRESS;
        listener = new System.Net.Sockets.TcpListener(localaddr: serverIP, port: port);
        Console.WriteLine("Server-Side: Server Setup!");
        listener.Start();
        listener.BeginAcceptTcpClient(OnServerConnect, null);
    }

    void OnServerConnect(IAsyncResult ar)
    {
        Console.WriteLine("Server-Side: Looking for connections...");
        TcpClient tcpClient = listener.EndAcceptTcpClient(ar);
        clientList.Add(new Client_Connection(tcpClient, this));
        Console.WriteLine("Server-Side: Added Client!");


        listener.BeginAcceptTcpClient(OnServerConnect, null);

    }

    // Initial check to see if you can make a server
    public static Boolean isOccupied()
    {
        System.Net.NetworkInformation.IPGlobalProperties ipGlobalProperties = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();
        TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

        foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
        {
            if (tcpi.LocalEndPoint.Port == port)
            {
                return true;
            }
        }

        return false;
    }

    public void OnDisconnect(Client_Connection client)
    {
        clientList.Remove(client);
    }

    public class Client_Connection
    {
        TcpClient connection;
        Chat_Server server;
        readonly byte[] readBuffer = new byte[5000];
        public string username = "unknown";

        NetworkStream stream
        {
            get
            {
                return connection.GetStream();
            }
        }

        public Client_Connection(TcpClient connection, Chat_Server server)
        {
            Console.WriteLine("Server-Side: Connecting to client...");
            this.connection = connection;
            this.server = server;

            stream.BeginRead(readBuffer, 0, readBuffer.Length, OnRead, null);
        }

        private string getTextFromStream(IAsyncResult ar)
        {
            int length = stream.EndRead(ar);
            if (length <= 0)
            {
                // Close the connection
                server.OnDisconnect(this);
                return "NULL";
            }
            Console.WriteLine("Reading....");

            return System.Text.Encoding.UTF8.GetString(readBuffer, 0, length);
        }

        void OnRead(IAsyncResult ar)
        {
            string message = getTextFromStream(ar);
            string[] parts = message.Split(Chat_Client.delem[0]);
            string code = parts[0];

            // Decode code
            if (code.Equals("0"))   // Update username
            {
                this.username = parts[1];
                Console.WriteLine("Updating Username!");
            }
            else if (code.Equals("1"))  // Send to specific players
            {
                ReadMsgToSend(parts[1].Split(','), parts[2]);
                Console.WriteLine("Sending Message!");
            }
            else if (code.Equals("2"))  // Send to all players
            {
                ReadMsgToSend(null, parts[1]);
                Console.WriteLine("Broadcasting Message!");
            }
            else
            {
                Console.WriteLine("Unknown Command!");
                Console.WriteLine(code);
            }
            stream.BeginRead(readBuffer, 0, readBuffer.Length, OnRead, null);


        }

        void ReadMsgToSend(string[] username_list, string msg)
        {
            // If null, just broadcast to all players
            if (username_list == null)
            {
                for (int i = 0; i < this.server.clientList.Count; i++)
                {
                    this.server.clientList[i].Send(msg);
                }
            }
            else
            {
                foreach (string user in username_list)
                {
                    for (int i = 0; i < this.server.clientList.Count; i++)
                    {
                        if(this.server.clientList[i].username.Equals(user))
                            this.server.clientList[i].Send(msg);
                    }
                }
            }
        }

        internal void Send(string message)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(this.username + ": " + message);
            stream.Write(buffer, 0, buffer.Length);
        }


    }

}




