using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Text;
using System;
using System.IO;
using System.Runtime.InteropServices;

public class TCPManager : MonoBehaviour
{
    TcpClient client;
    string serverIP = "192.168.35.243";
    int port = 8000;
    byte[] receivedBuffer;
    // StreamReader reader;
    bool socketReady = false;
    NetworkStream stream;
    public Vector2 cursorPosition;
    public bool isMouseClicked;

    // Start is called before the first frame update
    void Start()
    {
        CheckReceive();
        cursorPosition = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        if(socketReady)
        {
            if (stream.DataAvailable)
            {
                receivedBuffer = new byte[100];
                stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                string msg = Encoding.UTF8.GetString(receivedBuffer, 0, receivedBuffer.Length); // byte[] to string
                Debug.Log(msg);
                if (msg.Contains("isClosed"))
                {
                    isMouseClicked = msg.Split(' ')[1] == "True";
                }
                else if (msg.Contains("x:"))
                {
                    string xStr = msg.Split(' ')[1].Remove(msg.Split(' ')[1].Length - 1);
                    string yStr = msg.Split(' ')[3];
                    cursorPosition.x = float.Parse(xStr);
                    cursorPosition.y = float.Parse(yStr) * (-1);
                }
            }
        }
    }

    void CheckReceive()
    {
        if (socketReady) return;
        try
        {
            client = new TcpClient(serverIP, port);

            if (client.Connected)
            {                
                stream = client.GetStream();
                Debug.Log("Connect Success");
                socketReady = true;
            }
            
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }

    }

    void OnApplicationQuit()
    {
        CloseSocket();
    }

    void CloseSocket()
    {
        if (!socketReady) return;

        // reader.Close();
        client.Close();
        socketReady = false;
    }

}