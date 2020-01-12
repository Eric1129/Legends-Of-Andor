using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class Chat_Client
{
    #region variables
    public static IPAddress SERVER_ADDRESS = IPAddress.Parse("127.0.0.1");
    public static string delem = "" + (char)(10);

    // Username as a unique identifier
    private string username;

    // Connection reference
    private TcpClient connection;
    readonly byte[] readBuffer = new byte[5000];

    // After a read, this will be updated to the latest message
    private string message = "";

    NetworkStream stream
    {
        get
        {
            return connection.GetStream();
        }
    }
    #endregion

    #region init
    public Chat_Client(string username)
    {
        this.username = username;

        // Check to make sure there is a server running
        if (!Chat_Server.isOccupied())
        {
            //throw new Exception("Chat Server is not initialized");
        }

        this.connection = new TcpClient();
        this.connection.BeginConnect(SERVER_ADDRESS, Chat_Server.port, (ar) => EndConnect(ar), null);
    }

    public Boolean connected()
    {
        if(connection == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    internal void EndConnect(IAsyncResult ar)
    {
        connection.EndConnect(ar);

        stream.BeginRead(readBuffer, 0, readBuffer.Length, OnRead, null);

        UpdateUsername(this.username);
        Console.WriteLine("Client-Side: Client Connected!");

    }
    #endregion

    internal void OnRead(IAsyncResult ar)
    {
        Console.WriteLine("Client-Side: Reading...");

        int length = stream.EndRead(ar);
        if (length <= 0)
        {
            Console.WriteLine("Closing connection...");
            this.connection.Close();
            return;
        }

        this.message = System.Text.Encoding.UTF8.GetString(readBuffer, 0, length);

        Console.WriteLine(this.message);
        stream.BeginRead(readBuffer, 0, readBuffer.Length, OnRead, null);
        Console.WriteLine("Client-Side: Read!");


    }
    /// <summary>
    /// 
    /// CODES FOR SENDING DATA:
    /// 0 - update username
    /// EX: 0 ~ <username>
    /// 1 - send message to players
    /// EX: 1 ~ <player1>,<player2>,... ~ <message>
    /// 2 - send message to all players
    /// EX: 1 ~ <message>
    /// 
    /// </summary>

    // Generic send function. Shouldn't mess with this...
    internal void Send(string message)
    {
        Console.WriteLine("Client-Side: Sending message");

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
        stream.Write(buffer, 0, buffer.Length);
    }

    #region sending api's
    // Updates username known to server
    internal void UpdateUsername(string username)
    {
        // Send code
        this.Send("0" + delem + username);
    }
    // Sends a message to a set of players
    internal void SendMessage(string message, string[] players)
    {
        // Send code
        this.Send("1" + delem + String.Join(",", players) + delem + message);

        // HAVE TO MAKE SURE NO USERNAME CONTAINS COMMA, MIGHT HAVE PROBLEMS OTHERWISE
    }
    // Sends a message to all players
    internal void SendMessage(string message)
    {
        // Send code
        this.Send("2" + delem + message);
    }
    #endregion



    static void Main(string[] args)
    {
        /* READ ME:
         *
         * - Need to have a server running before it can work
         * - Need to specify the server IP before connecting the Clients
         *
         * Clients can do three things:
         *    - Update username known to server (happens automaticly when connecting to server)
         *    - Send a message to a set of usernames
         *    - Send a message to all usernames
         */


        Console.WriteLine("Running Server");
        Chat_Server server = new Chat_Server();
        Console.WriteLine("Running Client");
        Chat_Client client = new Chat_Client("max");

        string[] list = { "max" };
        string input = Console.ReadLine();
        input.ToUpper();
        while (!input.ToUpper().StartsWith("QUIT"))
        {
            client.SendMessage(input, list);
            input = Console.ReadLine();
        }
    }
}
