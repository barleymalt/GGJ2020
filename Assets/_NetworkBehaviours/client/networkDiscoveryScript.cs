using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class networkDiscoveryScript : NetworkDiscovery
{
    public bool serverFound = false;
    public string ipServer;

    public networkclientui networkclientuiScript;

    private void Start()
    {
        startClient();
    }
    public void startClient()
    {
        serverFound = false;
        Debug.Log("Start Broadcasting (looking for a server) ...");
        Initialize();
        StartAsClient();
    }

    public void Reset()
    {
        StopBroadcast();
        StopAllCoroutines();
        SceneManager.UnloadSceneAsync(2);
        SceneManager.LoadScene(2);
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        StopBroadcast();
        serverFound = true;
        Debug.Log("Server Found!");
        networkclientuiScript = GetComponent<networkclientui>();
        networkclientuiScript.Init(fromAddress, data);
    }
}