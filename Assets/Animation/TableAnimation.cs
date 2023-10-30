using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableAnimation : MonoBehaviour
{
    private Animator nearestTableAnimator;

    private Collider nearestTable;

    public GameObject waiter;
    private WaiterController waiterController;

    public bool UnderForNow;

    void Start()
    {
        nearestTableAnimator = null; // Initialize the nearestTableAnimator to null
        waiterController = waiter.GetComponent<WaiterController>();
    }

    void Update()
    {
        FindNearestTable();
        //Debug.LogError(Vector3.Distance(waiter.transform.position, nearestTable.transform.position));
        if (Input.GetKeyDown(KeyCode.F) && nearestTableAnimator != null && nearestTable.tag == "Unflipped") 
        {
            TableFlip();
        }  

        if ((Input.GetKeyDown(KeyCode.G) && nearestTableAnimator != null && nearestTable.tag == "Flipped")) 
        {
            TableUnflip();
        }
        
    }

    public void TableUnflip()
    {
        nearestTableAnimator.SetTrigger("Unflip");
        nearestTable.tag = "Unflipped";
        nearestTable.transform.GetChild(0).tag = "Unflipped";
    }
    public void TableFlip()
    {
        nearestTableAnimator.SetTrigger("Flip");
        nearestTable.tag = "Flipped";
        nearestTable.transform.GetChild(0).tag = "Flipped";

        AudioSource[] audioNearTable = nearestTable.GetComponents<AudioSource>();
        foreach (var audioTable in audioNearTable)
        {
            if(!audioTable.isPlaying)
            audioTable.Play();    
        }
        
    }

    void FindNearestTable()
    {
        Collider[] tables = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("Table"));
        float nearestDistance = float.MaxValue;
        nearestTable = null;

        foreach (var table in tables)
        {
            float distance = Vector3.Distance(transform.position, table.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTable = table;
            }
        }

        if (nearestTable != null)
        {
            nearestTableAnimator = nearestTable.GetComponent<Animator>();
        }
        else
        {
            nearestTableAnimator = null;
        }


    }        
}