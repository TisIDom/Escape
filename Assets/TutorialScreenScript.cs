using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScreenScript : MonoBehaviour
{
    public GameObject inGameUI;
    public Button okayButton;

    private void OnEnable()
    {

        Time.timeScale = 0; // Pause the game
        okayButton.onClick.AddListener(HideTutorial); // Add a listener to the "Okay" button
    }

    private void Update()
    {
        if (okayButton.IsActive())
        {
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Make the cursor visible
        }
    }

    private void HideTutorial()
    {
        

        gameObject.SetActive(false); // Hide the tutorial screen
        inGameUI.SetActive(true); // Show the in-game UI
        Time.timeScale = 1; // Resume the game

        Cursor.lockState = CursorLockMode.Locked; // Unlock the cursor
        Cursor.visible = false; // Make the cursor visible

        okayButton.onClick.RemoveListener(HideTutorial); // Remove the listener to prevent multiple calls
    }

}

