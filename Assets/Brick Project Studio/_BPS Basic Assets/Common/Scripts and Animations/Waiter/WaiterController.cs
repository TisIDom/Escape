using System.Runtime.CompilerServices;
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

    public GameObject questionMark; // Reference to the question mark GameObject
    private bool isQuestionMarkVisible = false;
    public float questionMarkDisplayDuration = 3.0f;
    private float questionMarkDisplayStartTime;

    public GameObject exclamationMark; // Reference to the exclamation mark GameObject
    private bool isExclamationMarkVisible = false;


    private bool isSprinting;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timeToStand = Time.time + standTime;
        FindNewTable();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tableLayerMask = LayerMask.GetMask("Table");
        isSprinting = false;

        // Initialize the question mark GameObject and hide it
        questionMark.SetActive(false);
        exclamationMark.SetActive(false);
    }

    void Update()
    {
        if (Time.time >= timeToStand)
        {
            FindNewTable();
            timeToStand = Time.time + standTime;
        }

        CheckForPlayer();

        //if(isSprinting)
        //{
        //    //Debug.LogError("gotta go fast");
        //}
    }

    void FindNewTable()
    {
        Collider[] tableColliders = Physics.OverlapSphere(transform.position, 100.0f, tableLayerMask);
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


        



        questionMark.transform.LookAt(player);
        exclamationMark.transform.LookAt(player);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, 5000.0f) && (!pHiding.isUnderTable || isSprinting ))
            {
                if (hit.collider.CompareTag("Player") && !isSprinting)
                {
                    agent.SetDestination(player.position);
                    agent.isStopped = true; // Stop the waiter
                    Invoke("StartSprinting", questionMarkDisplayDuration);

                    

                    ShowQuestionMarkAboveHead();
                    

            }
                if (isSprinting)
                {
                    agent.SetDestination(player.position);

                }

            }
            else if (!isQuestionMarkVisible)
            {
                agent.isStopped = false;
            }



    }

    void ShowQuestionMarkAboveHead()
    {

        // NOTE: Sometimes turns to exclamation mark without it turning to question mark first.


        SmoothLookAtPlayer();
        if (!isQuestionMarkVisible)
        {

            questionMark.SetActive(true);
            isQuestionMarkVisible = true;
            questionMarkDisplayStartTime = Time.time;
            //Debug.LogError("Showing Question Mark ???");
            
        }
        if (pHiding.isUnderTable)
        {
            CancelInvoke();
            agent.isStopped = false;
            HideQuestionMark();
        }
        if (Time.time - questionMarkDisplayStartTime >= questionMarkDisplayDuration)
        {
            HideQuestionMark();
            
        }
    }

    void ShowExclamationMarkAboveHead()
    {
            questionMark.SetActive(false);
            isQuestionMarkVisible = false;
            exclamationMark.SetActive(true);
            isExclamationMarkVisible = true;
        //Debug.LogError("Showing Exclamation Mark !!!");
    }

    void HideQuestionMark()
    {
        agent.isStopped = false;
        questionMark.SetActive(false);
        isQuestionMarkVisible = false;
        //Debug.LogError("Coast is Clear !!!");
    }

    void StartSprinting()
    {
        if (!pHiding.isUnderTable)
        {
            ShowExclamationMarkAboveHead();
            agent.isStopped = false;
            agent.speed *= 2;
            exclamationMark.SetActive(true) ;
            isExclamationMarkVisible=true;
            isSprinting = true;
        }
        else
        {
            questionMark.SetActive(false);
            isQuestionMarkVisible=false;
        }

        return;
    }


    void SmoothLookAtPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly interpolate the rotation over 0.5 seconds.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2.0f * Time.deltaTime);
    }
}
