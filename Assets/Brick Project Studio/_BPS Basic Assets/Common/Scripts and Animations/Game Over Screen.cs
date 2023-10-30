using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    private GameObject player;
    private GameObject waiter;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Capsule");
        waiter = GameObject.Find("Waiter");
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 waiterPos = waiter.transform.position;
        Vector3 playerPos = player.transform.position;
        //Debug.LogError(waiterPos);
        //Debug.LogError(playerPos);
        if (Vector3.Distance(waiterPos, playerPos) < 1.5f && !player.GetComponent<PlayerHiding>().isUnderTable && waiter.GetComponent<WaiterController>().isSprinting)
        {
            InitializeGameOver();
        }
    }

    public void InitializeGameOver()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.GetChild(0).GetComponent<MouseLook>().enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
