using UnityEngine;
using UnityEngine.AI;

public class WaiterController : MonoBehaviour
{
    public float standTime = 3.0f;
    public float fieldOfViewAngle = 1000000.0f;
    private NavMeshAgent agent;
    private Transform target;
    private float timeToStand;
    private Transform player;
    private LayerMask tableLayerMask;
    public PlayerHiding pHiding;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timeToStand = Time.time + standTime;
        FindNewTable();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tableLayerMask = LayerMask.GetMask("Table");
    }

    void Update()
    {
        if (Time.time >= timeToStand)
        {
            FindNewTable();
            timeToStand = Time.time + standTime;
        }

        CheckForPlayer();
    }

    void FindNewTable()
    {
        Collider[] tableColliders = Physics.OverlapSphere(transform.position, 100.0f, tableLayerMask); // Adjust the radius as needed.
        if (tableColliders.Length > 0)
        {
            target = tableColliders[Random.Range(0, tableColliders.Length)].transform;
            agent.SetDestination(target.position);
        }
    }

    void CheckForPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        //if (angleToPlayer < fieldOfViewAngle * 0.5f)
        //{
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, 5000.0f)) // Adjust the distance as needed.
            {
                if (hit.collider.CompareTag("Player") && !pHiding.isUnderTable)
                {
                    agent.SetDestination(player.position);
                }
            }
        //}
    }
}
