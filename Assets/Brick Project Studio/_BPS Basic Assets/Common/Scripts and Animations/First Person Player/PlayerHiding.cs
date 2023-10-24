using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHiding : MonoBehaviour
{
    private bool isUnderTable;
    private Vector3 originalScale;
    private Collider nearestTable;
    private Vector3 locationBeforeHiding;

    public bool movingAllowed = true;

    private void Start()
    {
        isUnderTable = false;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (isUnderTable)
        {
            movingAllowed = false;
        }
        else
        {
            movingAllowed = true;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            locationBeforeHiding = transform.localPosition;
            if (isUnderTable)
            {
                
                // Return to original scale and move out from under the table.
                transform.localScale = originalScale;
                isUnderTable = false;
                if (nearestTable != null)
                {
                    // Move the character out from under the table.
                    transform.position = locationBeforeHiding;
                }
            }
            else
            {
                //not done yet!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                // Find the nearest table and go under it.
                FindNearestTable();
                if (nearestTable != null && !IsObstacleInWay())
                {
                    isUnderTable = true;
                    transform.localScale = originalScale * 0.5f;

                    // Move the character under the table.
                    Vector3 targetPosition = nearestTable.transform.position + new Vector3(0f, transform.localScale.y / 2f - nearestTable.bounds.extents.y, 0f);
                    transform.position = targetPosition;
                }
            }
        }
    }

    private void FindNearestTable()
    {
        Collider[] tables = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Tables"));
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


    private bool IsObstacleInWay()
    {
        if (nearestTable == null)
            return true; // No table to go under.

        Vector3 direction = nearestTable.transform.position - transform.position;
        float distance = direction.magnitude;
        Ray ray = new Ray(transform.position, direction.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, LayerMask.GetMask("Obstacles")))
        {
            // There's an obstacle in the way.
            return true;
        }

        return false;
    }
}
