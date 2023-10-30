using UnityEngine;
using System.Collections;

public class TutorialScreenScript : MonoBehaviour
{
    public GameObject inGameUI;
    void OnEnable()
    {
        StartCoroutine(ShowAndHide()); // start the coroutine when canvas is enabled
    }

    IEnumerator ShowAndHide()
    {
        yield return new WaitForSeconds(5); // wait for 5 seconds
        gameObject.SetActive(false); // then hide the tutorial screen
        inGameUI.SetActive(true);
    }
}
