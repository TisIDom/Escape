using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHiding : MonoBehaviour
{
    public bool isUnderTable;
    private Vector3 originalScale;
    private Collider nearestTable;
    private Vector3 locBeforeHiding;
    private float timer;
    public float timeBeforeUnder = 1f;

    private TableAnimation tableAnim;

    private void Start()
    {
        timer = Time.time;
        originalScale = transform.localScale;
        tableAnim = GetComponent<TableAnimation>();
        isUnderTable = false;
    }

    private void Update()
    {

        FindNearestTable();

        if (isUnderTable && nearestTable.transform.parent.tag == "Flipped" && nearestTable != null)
        {
            transform.position = locBeforeHiding;
            isUnderTable = false;
            transform.localScale = originalScale;
        }

        if ((Input.GetKeyDown(KeyCode.C) && Time.time - timer > timeBeforeUnder && nearestTable != null) && nearestTable.transform.parent.tag == "Unflipped")
        //bug: can reset timer before getting close to table
        {
            tableAnim.UnderForNow = true;
            //Debug.LogError("Pressed C");
            
            if (isUnderTable)
            {
                //Debug.LogError("Out of Table --->");
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
                locBeforeHiding = transform.position;

                // Find the nearest table and check for obstacles before going under.
                if (nearestTable != null )
                {
                    isUnderTable = true;
                    transform.localScale = originalScale * 0.5f;
                    // Move the character under the table.
                    Vector3 targetPosition = nearestTable.transform.position - new Vector3(0f, 0.66f, 0f);
                    transform.position = targetPosition;
                }
            }
            timer = Time.time;

        }

    }

    public Collider FindNearestTable()
    {
        Collider[] tables = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("TableTops"));
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
        return nearestTable;
    }


}