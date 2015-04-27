using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

public class UDPSender : MonoBehaviour {
    public static UDPSender networkManager;

    UdpClient sender;
    public bool connectionStarted, connected, isHost;

    int remotePort = 19784;
    int serverPort = 2500;
    string myIP;
    public string gameName, peerIP;
    public int maxConnections;

    //singleton and carryover to keep network connection active
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (networkManager == null)
        {
            networkManager = this;
        }
        else if (networkManager != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        myIP = Network.player.ipAddress;
        sender = new UdpClient(25000, AddressFamily.InterNetwork);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, remotePort);
        sender.Connect(groupEP);
    }

    void Update()
    {
        if (!isHost && peerIP != "" && !connectionStarted)
        {
            connectionStarted = true;
            Network.Connect(peerIP, serverPort);
            Debug.Log("trying connection");
            CancelInvoke("CheckIfServer");
        }

        //stop broadcasting IP once maximun number of connections have been made
        if (isHost && Network.connections.Length == maxConnections)
        {
            CancelInvoke("SendData");
        }        
    }

    //detect that connection is complete
    void OnConnectedToServer()
    {
        Debug.Log("Connection Established");
    }

    /*these are connected to their respective buttons.
     * Host creates a server and begins broadcasting its IP & game ID
     * Connect listens for the IP + ID string and filters it out
     * If it finds a match, it will connect to the server
    */

    public void Connect()
    {
        StartReceivingIP();
        Invoke("CheckIfServer", 5);
    }

    //start server if there is none
    void CheckIfServer()
    {
        if (peerIP == "")
        {
            isHost = true;
            Network.InitializeServer(32, serverPort, false);
            SendData();
            InvokeRepeating("SendData", 0, 0.5f);
        }
    }


    //begin broadcasting IP and Network
    void SendData()
    {
        //game name must not contain spaces
        string customMessage = myIP + " " + gameName;
        if (customMessage != "")
        {
            sender.Send(Encoding.ASCII.GetBytes(customMessage), customMessage.Length);
        }
    }

    //receive data on the chosen port
    UdpClient receiver;

    public void StartReceivingIP()
    {
        try
        {
            if (receiver == null)
            {
                receiver = new UdpClient(remotePort);
                receiver.BeginReceive(new AsyncCallback(ReceiveData), null);
            }
        }
        catch (SocketException e)
        {
            Debug.Log(e.Message);
        }
    }

    private void ReceiveData(IAsyncResult result)
    {
        IPEndPoint receiveIPGroup = new IPEndPoint(IPAddress.Any, remotePort);
        byte[] received;
        if (receiver != null)
        {
            received = receiver.EndReceive(result, ref receiveIPGroup);
        }
        else
        {
            return;
        }
        
        string receivedString = Encoding.ASCII.GetString(received);
        Debug.Log(receivedString);
        string[] filteredString = receivedString.Split();
        Debug.Log(filteredString[0]);
        Debug.Log(filteredString[1]);
        if (filteredString[0] != myIP && filteredString[1] == gameName)
        {
            peerIP = filteredString[0];
        }
        else if (!isHost)
        {
            receiver.BeginReceive(new AsyncCallback(ReceiveData), null);
        }
    }

}
