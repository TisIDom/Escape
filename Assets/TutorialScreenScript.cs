using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScreenScript : MonoBehaviour
{
    public GameObject inGameUI;
    public Button okayButton;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
        Time.timeScale = 0; // Pause the game
        okayButton.onClick.AddListener(HideTutorial); // Add a listener to the "Okay" button
    }

    private void HideTutorial()
    {
        

        gameObject.SetActive(false); // Hide the tutorial screen
        inGameUI.SetActive(true); // Show the in-game UI
        Time.timeScale = 1; // Resume the game

        okayButton.onClick.RemoveListener(HideTutorial); // Remove the listener to prevent multiple calls
    }

}

