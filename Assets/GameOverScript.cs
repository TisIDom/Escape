using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreenManager : MonoBehaviour
{
    public Button retryButton;
    public Button mainMenuButton;

    void Start()
    {
        // Attach the Retry and MainMenu functions to the respective buttons
        retryButton.onClick.AddListener(Retry);
        mainMenuButton.onClick.AddListener(MainMenu);
    }

    // Function to reload the current game scene
    public void Retry()
    {
        // Reloads the current scene, effectively restarting the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Function to load the main menu scene
    public void MainMenu()
    {
        // Replace "MainMenu" with the name of your main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}