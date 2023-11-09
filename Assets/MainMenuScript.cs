using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject tutorialCanvas; // assign the tutorial canvas in the inspector

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    public void PlayButton()
    {
        Time.timeScale = 1.0f;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "FFK Sample Scene")
        {
            tutorialCanvas = GameObject.Find("TutorialCanvas"); // replace with your tutorial canvas name
            tutorialCanvas.SetActive(true);
        }
    }
}