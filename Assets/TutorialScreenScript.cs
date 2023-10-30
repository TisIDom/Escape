using UnityEngine;
using System.Collections;

public class TutorialScreenScript : MonoBehaviour
{
    public GameObject inGameUI;
    void OnEnable()
    {
        Time.timeScale = 0; // pause the game
        StartCoroutine(ShowAndHide()); // start the coroutine when canvas is enabled
    }

    IEnumerator ShowAndHide()
    {
        yield return new WaitForSecondsRealtime(5); // wait for 5 seconds in real time
        gameObject.SetActive(false); // then hide the tutorial screen
        inGameUI.SetActive(true); // and show the in-game UI
        Time.timeScale = 1; // resume the game
    }
}
