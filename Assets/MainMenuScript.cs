using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("PLAY");
        // Replace "YourGameSceneName" with the name of your game's first scene
        SceneManager.LoadScene("FFK Sample Scene");
    }

    public void OpenSettings()
    {
        // Implement your settings logic here, like opening a settings panel
    }

    public void ExitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}