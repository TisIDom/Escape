using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableAnimation : MonoBehaviour
{
    private Animator nearestTableAnimator;
    private Collider nearestTable;


    void Start()
    {
        nearestTableAnimator = null; // Initialize the nearestTableAnimator to null
    }

    void Update()
    {
        FindNearestTable();
        
        if (Input.GetKeyDown(KeyCode.F) && nearestTableAnimator != null) 
        {
            nearestTableAnimator.SetTrigger("Flip");
        }
        Debug.LogError(nearestTable.name);
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