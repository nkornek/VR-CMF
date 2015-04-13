using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

public class UDPSender : MonoBehaviour {
    UdpClient sender;
    bool connectionStarted;

    int remotePort = 19784;

    void Start()
    {
        sender = new UdpClient(25000, AddressFamily.InterNetwork);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, remotePort);
        sender.Connect(groupEP);
    }

    void SendData()
    {
        string customMessage = Network.player.ipAddress + " * " + "VR";

        if (customMessage != "")
        {
            sender.Send(Encoding.ASCII.GetBytes(customMessage), customMessage.Length);
        }

    }

    public void StartConnection()
    {
        if (!connectionStarted)
        {
            InvokeRepeating("SendData", 0, 5f);
            connectionStarted = true;
            StartReceivingIP();
        }
    }

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
        receiver.BeginReceive(new AsyncCallback(ReceiveData), null);
        string receivedString = Encoding.ASCII.GetString(received);
        print(receivedString);
    }
}
