using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class gameUI : MonoBehaviour
{
    // Network Check Time in seconds
    public float NC_TIME = 1;

    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    public GameObject dataIPAddress;
    public GameObject dataServerStatus;
    public GameObject dataConnectionStatus;
    
    public networkserverui networkserveruiScript;

    // declared bool to have more than 1 coroutine at once
    private bool isRunning = false;

    private void Start()
    {
        StopAllCoroutines();
        if (!isRunning) StartCoroutine(checkNetwork());
    }

    private void Update()
    {
        // open/close menù on ESC key pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // coroutine repeated every NC_TIME second(s) 
    // check and update different varaibles of the connection on the UI
    public IEnumerator checkNetwork()
    {
        while (true)
        {
            isRunning = true;
            // retrive local IP Address
            dataIPAddress.GetComponent<Text>().text = networkserveruiScript.LocalIPAddress();

            // check if the server is Active (ready to start a connection)
            if (networkserveruiScript.NetworkServerActive())
            {
                dataServerStatus.GetComponent<Text>().color = new Color(0, 1, 0, 1);
                dataServerStatus.GetComponent<Text>().text = "Active";
            }
            else
            {
                dataServerStatus.GetComponent<Text>().color = new Color(1, 0, 0, 1);
                dataServerStatus.GetComponent<Text>().text = "Inactive";
            }

            // check if the client is actually connected to the server
            if (networkserveruiScript.ConnectionIsActive())
            {
                dataConnectionStatus.GetComponent<Text>().color = new Color(0, 1, 0, 1);
                dataConnectionStatus.GetComponent<Text>().text = "Connected";
            }
            else
            {
                dataConnectionStatus.GetComponent<Text>().color = new Color(1, 0, 0, 1);
                dataConnectionStatus.GetComponent<Text>().text = "Disconnected";
            }
            yield return new WaitForSeconds(NC_TIME);
            isRunning = false;
        }
    }

    #region UI navigation functions

    // close main menù
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    // open main menù
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    // quit the application
    public void QuitGame()
    {
        StopAllCoroutines();
        Application.Quit();
    }

    // open settings menù
    public void LoadSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }

    // close setting menù
    public void Back()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }
    #endregion
}
