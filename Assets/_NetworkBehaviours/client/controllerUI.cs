using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class controllerUI : MonoBehaviour
{
    // Network Check Time in seconds
    public float NC_TIME = 1;

    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject mainpanelUI;
    public GameObject gyrodataText;
    public GameObject statusdot;
    public GameObject settingsMenuUI;

    public GameObject dataLocalIPAddress;
    public GameObject dataServerIPAddress;
    public GameObject dataClientStatus;
    public GameObject dataServerPort;

    public GameObject dataConnectionStatus;
    public networkclientui networkclientuiScript;

    // declared bool to have more than 1 coroutine at once
    private bool isRunning = false;

    private void Start()
    {
        StopAllCoroutines();
        if (!isRunning) StartCoroutine(checkNetwork());
    }

    private void Update()
    {
        // update the gyroscope's data on the main screen
        gyrodataText.GetComponent<Text>().text = networkclientuiScript.q.ToString();
    }

    #region UI navigation functions
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        mainpanelUI.SetActive(true);
        gameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        mainpanelUI.SetActive(false);
        gameIsPaused = true;
    }

    public void LoadSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }

    public void Back()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }

    // quit the application
    public void QuitGame()
    {
        StopAllCoroutines();
        networkclientuiScript.ShutdownAll();
    }
    #endregion

    // coroutine repeated every NC_TIME second(s) 
    // check and update different varaibles of the connection on the UI
    public IEnumerator checkNetwork()
    {
        while (true)
        {
            isRunning = true;
            // retrive local IP Address
            dataLocalIPAddress.GetComponent<Text>().text = networkclientuiScript.LocalIPAddress();

            // retrive Server IP Address Broadcasted by the server
            dataServerIPAddress.GetComponent<Text>().text = networkclientuiScript.ServerIPFound().Replace("::ffff:", "");

            // retrive Server Port Broadcasted by the server
            dataServerPort.GetComponent<Text>().text = networkclientuiScript.ServerPortFound();

            // check if the client is Active (ready to start a connection)
            if (networkclientuiScript.NetworkClientActive())
            {
                dataClientStatus.GetComponent<Text>().color = new Color(0, 1, 0, 1);
                dataClientStatus.GetComponent<Text>().text = "Active";
            }
            else
            {
                dataClientStatus.GetComponent<Text>().color = new Color(1, 0, 0, 1);
                dataClientStatus.GetComponent<Text>().text = "Inactive";
            }

            // check if the client is actually connected to the server
            if (networkclientuiScript.ConnectionIsActive())
            {
                dataConnectionStatus.GetComponent<Text>().color = new Color(0, 1, 0, 1);
                dataConnectionStatus.GetComponent<Text>().text = "Connected";
                statusdot.GetComponent<Image>().color = new Color(0, 1, 0, 1);
            }
            else
            {
                dataConnectionStatus.GetComponent<Text>().color = new Color(1, 0, 0, 1);
                dataConnectionStatus.GetComponent<Text>().text = "Disconnected";
                statusdot.GetComponent<Image>().color = new Color(1, 0, 0, 1);
            }
            yield return new WaitForSeconds(NC_TIME);
            isRunning = false;
        }
    }
}

