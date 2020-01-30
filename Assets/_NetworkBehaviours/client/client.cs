using System;
using System.Collections;
using System.Collections.Generic;

using System.Globalization;

using System.Net;
using System.Net.Sockets;

using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine;
using UnityEngine.UI;

public class client : MonoBehaviour
{
    private byte reliableChannel;

    private const int MAX_USERS = 2; 
    private const int SERVER_PORT = 25000;
    private const int WEBSERVER_PORT = 26000;
    private const string SERVER_IP = "127.0.0.1";

    private int hostId;
    private bool isStarted = false;

    #region Monobehavior
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }
    #endregion
    
    public void Init()
    {
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topology = new HostTopology(cc, MAX_USERS);

        // Client-only code
        hostId = NetworkTransport.AddHost(topology, 0);


        Debug.Log(string.Format("Opening connection on port {0} and webport {1}", SERVER_PORT, WEBSERVER_PORT));
        isStarted = true;

    }
    public void Shoutdown()
    {
        isStarted = false;
        NetworkTransport.Shutdown();
    }
}
