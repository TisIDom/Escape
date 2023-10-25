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

    private void Start()
    {
        timer = Time.frameCount;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && Time.frameCount - timer > 60)
        {
            if (isUnderTable)
            {
                // Return to original scale and move out from under the table.
                
                isUnderTable = false;
                if (nearestTable != null)
                {
                    // Move the character out from under the table.
                    transform.position = locBeforeHiding;
                    transform.localScale = originalScale;
                }
                
                //transform.position = locBeforeHiding;
            }
            else
            {
                locBeforeHiding = transform.position;
                // Find the nearest table and check for obstacles before going under.
                FindNearestTable();
                if (nearestTable != null)
                {
                    isUnderTable = true;
                    transform.localScale = originalScale * 0.5f;
                    // Move the character under the table.
                    Vector3 targetPosition = nearestTable.transform.position  - new Vector3(0f, 0.66f, 0f);
                    transform.position = targetPosition;
                }
            }
            timer = Time.frameCount;
        }
        //Debug.LogError(Time.frameCount);
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
