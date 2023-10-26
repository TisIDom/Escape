using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHiding : MonoBehaviour
{
    public bool isUnderTable = false;
    private Vector3 originalScale;
    private Collider nearestTable;
    private Vector3 locBeforeHiding;
    private float timer;
    public float timeBeforeUnder = 1f;

    private void Start()
    {
        timer = Time.time;
        originalScale = transform.localScale;
    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.C) && Time.time - timer > timeBeforeUnder)
        //bug: can reset timer before getting close to table
        {
            Debug.LogError("Pressed C");
            FindNearestTable();
            if (isUnderTable)
            {
                Debug.LogError("Out of Table --->");
                // Return to original scale and move out from under the table.

                isUnderTable = false;
                if (nearestTable != null)
                {
                    transform.position = locBeforeHiding;
                    transform.localScale = originalScale;
                }

            }
            else
            {
                Debug.LogError("In Table --->");
                locBeforeHiding = transform.position;
                //Debug.LogError("location overwriten ");
                // Find the nearest table and check for obstacles before going under.
                if (nearestTable != null)
                {
                    isUnderTable = true;
                    transform.localScale = originalScale * 0.5f;
                    // Move the character under the table.
                    Vector3 targetPosition = nearestTable.transform.position - new Vector3(0f, 0.66f, 0f);
                    transform.position = targetPosition;
                }
            }
            timer = Time.time;


            //Debug.LogError("Pressed C");
            //Debug.LogError("Current position: " + transform.position);
            //Debug.LogError("Previous position: " + locBeforeHiding + "\n" +
            //           "Table center location: " + (nearestTable.transform.position - new Vector3(0f, 0.66f, 0f)) + "\n");

        }

        
        
        Debug.LogError("Current position: " + transform.position);
        Debug.LogError("Previous position: " + locBeforeHiding + "\n" +
                   "Table center location: " + (nearestTable.transform.position - new Vector3(0f, 0.66f, 0f)) + "\n");
    }

    private void FindNearestTable()
    {
        Collider[] tables = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Table"));
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
    }


}