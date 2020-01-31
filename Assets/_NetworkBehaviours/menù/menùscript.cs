using UnityEngine.SceneManagement;
using UnityEngine;


public class menùscript : MonoBehaviour
{
    public GameObject oggettiDaAttivare;
    public GameObject menu;


    public void selectServer()
    {
        Debug.Log("Attivazione oggetti server");
        oggettiDaAttivare = GameObject.FindGameObjectWithTag("Server");
        foreach (Transform t in oggettiDaAttivare.transform)
        {
            t.gameObject.SetActive(true);
        }
        menu.SetActive(false);
    }

    public void selectClient()
    {
        Debug.Log("Attivazione oggetti client");
        oggettiDaAttivare = GameObject.FindGameObjectWithTag("Client");
        foreach (Transform t in oggettiDaAttivare.transform)
        {
            t.gameObject.SetActive(true);
        }
        menu.SetActive(false);
    }
}
