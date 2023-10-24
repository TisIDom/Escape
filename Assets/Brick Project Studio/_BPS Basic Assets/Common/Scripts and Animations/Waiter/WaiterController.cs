using UnityEngine;
using UnityEngine.AI;

public class WaiterController : MonoBehaviour
{
    public LayerMask tableLayer;
    public LayerMask playerLayer;
    public float standTime = 3.0f;
    public float noticeDistance = 10.0f;
    private NavMeshAgent agent;
    private Transform target;
    private float timeToStand;
    public float fieldOfViewAngle = 90.0f;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timeToStand = Time.time + standTime;
        FindNewTable();
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        Collider[] tables = Physics.OverlapSphere(transform.position, noticeDistance, tableLayer);
        if (tables.Length > 0)
        {
            target = tables[Random.Range(0, tables.Length)].transform;
            agent.SetDestination(target.position);
        }
    }

    void CheckForPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, noticeDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    agent.SetDestination(player.position);
                }
            }
        }
    }
}