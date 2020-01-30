using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine;

public class networkclientui : MonoBehaviour
{
    // Connection Check Time in seconds
    public float CC_TIME = 2;

    static NetworkClient client = new NetworkClient();

    public Quaternion q;

    private string serverIPAddress = "...";
    private string serverListeningPort = "...";

    // declared bool to have more than 1 coroutine at once
    private bool isRunning = false;


    // setup very basic properties 
    private void Start()
    {
        Application.runInBackground = true;
        DontDestroyOnLoad(gameObject);
    }

    // initialize connection with the server
    public void Init(string broadcastedIP, string broadcastedPort)
    {
        Input.gyro.enabled = true;

        // IP and Port returned by the server after successful broadcast call
        serverIPAddress = broadcastedIP.Replace("::ffff:", "");
        serverListeningPort = broadcastedPort;

        StopAllCoroutines();
        if (!isRunning) StartCoroutine(CheckConnection());
        Debug.Log("Initialized");
    }


    // retrive Gyro's positon from the current device and send it to the server
    private void Update()
    {
        // assign to the variable q the reflected input of the gyroscope
        q = ConvertRightHandedToLeftHandedQuaternion(Input.gyro.attitude);

        // send a message to the server if client is both connected and active
        if (client.isConnected && NetworkClient.active) SendViaNetwork(q.ToString("F6"));
    }

    // send a message to the server including the data of the Gyro
    static public void SendViaNetwork(string gyroQuaternion)
    {
        StringMessage msg = new StringMessage();
        msg.value = gyroQuaternion;
        client.Send(888, msg);
    }

    // coroutine repeated every CC_TIME second(s) 
    // check connection and attempt reconnection 
    public IEnumerator CheckConnection()
    {
        while (true)
        {
            isRunning = true;
            if (!client.isConnected || !NetworkClient.active)
            {
                // reset previus connections
                Debug.Log("Trying to reset client...");
                client.Disconnect();
                client.Shutdown();
                client = null;

                // estabilish new client
                client = new NetworkClient();

                // attempt to connect to the server
                Debug.Log("Trying to connect to server " + serverIPAddress + ", " + int.Parse(serverListeningPort));
                client.Connect(serverIPAddress, int.Parse(serverListeningPort));
                
                if (!Input.gyro.enabled)
                    Input.gyro.enabled = true;
            }
            yield return new WaitForSeconds(CC_TIME);
            isRunning = false;
        }
    }

    public void OnConnected(NetworkConnection conn, NetworkReader reader)
    {
        Debug.Log("Connected to server");
    }

    // return the local IP address of the current device
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


    #region funcions called from outside to get datas about the connection

    // shoutdown all client's routines and connections and then quit the application
    public void ShutdownAll()
    {
        StopAllCoroutines();
        client.Disconnect();
        client.Shutdown();
        NetworkTransport.Shutdown();
        Application.Quit();
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }

    // return if client is ready and active
    public bool NetworkClientActive()
    {
        return NetworkClient.active;
    }

    // return if client is actually connected to the server
    public bool ConnectionIsActive()
    {
        return client.isConnected;
    }

    // return the IP Address broadcasted from the server
    public string ServerIPFound()
    {
        return serverIPAddress;
    }

    // return the Port broadcasted from the server
    public string ServerPortFound()
    {
        return serverListeningPort;
    }

    // reflect the Quaternion to fix Uniy's Left-Handed Coordinate System
    private Quaternion ConvertRightHandedToLeftHandedQuaternion(Quaternion rightHandedQuaternion)
    {
        return new Quaternion(-rightHandedQuaternion.x,
            -rightHandedQuaternion.z,
            -rightHandedQuaternion.y,
            rightHandedQuaternion.w);
    }

    #endregion
}
