using UnityEngine;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour
{
    public void EndGame()
    {
        Application.Quit();
    }

    public void GotoMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
