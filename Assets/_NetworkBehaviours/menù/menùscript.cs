using UnityEngine.SceneManagement;
using UnityEngine;


public class menùscript : MonoBehaviour
{

    public void selectServer()
    {
        SceneManager.LoadScene(1);
    }

    public void selectClient()
    {
        SceneManager.LoadScene(2);
    }
}
