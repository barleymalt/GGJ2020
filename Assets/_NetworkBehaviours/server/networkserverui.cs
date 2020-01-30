using System;
using System.Collections;
using System.Collections.Generic;

using System.Globalization;

using System.Net;
using System.Net.Sockets;

using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine;

public class networkserverui : NetworkDiscovery
{
    // cached reference
    public GameObject cubo;
    public Rigidbody rigid;

    private float RotationSmoothnes = 1;

    private Quaternion gyroRot;
    private string ipaddress;

    int minPort = 25000;
    int maxPort = 25010;
    int defaultPort = 25000;


    #region Monobehavior
    public void Start()
    {
        Debug.Log("NetworkServerUIscript started");
        Application.runInBackground = true;
        DontDestroyOnLoad(gameObject);
        Init();
    }
    #endregion


    public void Init()
    {
        cubo = GameObject.Find("Cube");
        rigid = cubo.GetComponent<Rigidbody>();

        ipaddress = LocalIPAddress();
        Debug.Log(ipaddress);
        

        startServer();

        StartListen();
    }



    public void startServer()
    {
        int serverPort = createServer();
        if (serverPort != -1)
        {
            broadcastData = serverPort.ToString();
            Initialize();
            StartAsServer();
        }
        else
        {
            Debug.Log("Failed to create Server");
        }
    }




    //Creates a server then returns the port the server is created with. Returns -1 if server is not created
    private int createServer()
    {
        int serverPort = -1;
        //Connect to default port
        bool serverCreated = NetworkServer.Listen(defaultPort);
        if (serverCreated)
        {
            serverPort = defaultPort;
            Debug.Log("Server created on port : " + serverPort);
        }
        else
        {
            Debug.Log("Failed to create with the default port");
            //Try to create server with other port from min to max except the default port which we trid already
            for (int tempPort = minPort; tempPort <= maxPort; tempPort++)
            {
                //Skip the default port since we have already tried it
                if (tempPort != defaultPort)
                {
                    //Exit loop if successfully create a server
                    if (NetworkServer.Listen(tempPort))
                    {
                        serverPort = tempPort;
                        break;
                    }

                    //If this is the max port and server is not still created, show, failed to create server error
                    if (tempPort == maxPort)
                    {
                        Debug.LogError("Failed to create server");
                    }
                }
            }
        }
        return serverPort;
    }



    public bool NetworkServerActive()
    {
        return NetworkServer.active;
    }

    public bool ConnectionIsActive()
    {
        for (int i = 0; i < NetworkServer.connections.Count; i++)
        {
            if (NetworkServer.connections[i] != null)
            {
                return true;
            }
        }
        return false;
    }

    public void StartListen()
    {
        //NetworkServer.ResetConnectionStats();

        //NetworkServer.Reset();

        //NetworkServer.Listen(int.Parse(listeningPortText.GetComponent<Text>().text));

        NetworkServer.RegisterHandler(888, ServerReceiveMessage);

    }

    // receive a message from the client
    private void ServerReceiveMessage(NetworkMessage message)
    {
        StringMessage msg = new StringMessage();
        msg.value = message.ReadMessage<StringMessage>().value;

        gyroRot = StringToQuaternion(msg.value);

        Debug.Log(gyroRot);

        cubo.transform.rotation = gyroRot;
    }


    private void Update()
    {
        cubo.transform.rotation = Quaternion.LerpUnclamped(cubo.transform.rotation, gyroRot, RotationSmoothnes * Time.deltaTime);
    }

    // return the local IP address of the device
    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

    // return a String into a Quaternion
    public static Quaternion StringToQuaternion(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Quaternion
        Quaternion result = new Quaternion(
            float.Parse(sArray[0]) / 10,
            float.Parse(sArray[1]) / 10,
            float.Parse(sArray[2]) / 10,
            float.Parse(sArray[3]) / 10);

        return result;
    }

}