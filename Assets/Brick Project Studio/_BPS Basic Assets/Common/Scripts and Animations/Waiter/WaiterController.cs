using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class WaiterController : MonoBehaviour
{
    public float standTime = 2.0f;
    public float fieldOfViewAngle = 135.0f;
    private NavMeshAgent agent;
    private Transform target;
    private float timeToStand;
    private Transform player;
    private LayerMask tableLayerMask;
    public PlayerHiding pHiding;

    public GameObject questionMark; // Reference to the question mark GameObject
    private bool isQuestionMarkVisible = false;
    public float questionMarkDisplayDuration = 2.5f;
    private float questionMarkDisplayStartTime;

    public GameObject exclamationMark; // Reference to the exclamation mark GameObject

    private bool isExclamationMarkVisible;

    // Add these fields for stereo audio
    private AudioSource audioSource;
    public AudioClip channelSound;

    private bool isSprinting;

    private List<Transform> flippedTables = new List<Transform>();
    private TableAnimation tableAnim;

    void Start()
    {
        tableAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<TableAnimation>();
        agent = GetComponent<NavMeshAgent>();
        timeToStand = Time.time + standTime;
        FindNewTable();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tableLayerMask = LayerMask.GetMask("Table");
        isSprinting = false;

        // Initialize the question mark GameObject and hide it
        questionMark.SetActive(false);
        exclamationMark.SetActive(false);


        // Get the audioSource component from this GameObject
        audioSource = gameObject.AddComponent<AudioSource>();

        // Configure the audio sources
        audioSource.spatialBlend = 1.0f;  // Full 3D spatialization

        isExclamationMarkVisible = false;
    }

    void Update()
    {
        FindFlippedTables();
        if (Time.time >= timeToStand)
        {

            FindNewTable();
            timeToStand = Time.time + standTime;
        }

        if (Vector3.Distance(transform.position, flippedTables[0].position) < 3f)
        {
            Animator nearestTableAnimator = flippedTables[0].GetComponent<Animator>();
            nearestTableAnimator.SetTrigger("Unflip");
            flippedTables[0].tag = "Unflipped";
            flippedTables[0].transform.GetChild(0).tag = "Unflipped";
        }

        CheckForPlayer();

        //if(isSprinting)
        //{
        //    //Debug.LogError("gotta go fast");
        //}
    }

    private void FindFlippedTables()  //BIski nesigauna dar. Fuck, kodėl neišeina iš Gameobject masyvo ištraukt colliderių masyvo >:(
    {

        GameObject[] tables = GameObject.FindGameObjectsWithTag("Flipped");

        foreach (var table in tables)
        {
            if (!flippedTables.Contains(table.transform) && table.layer == 7)
                flippedTables.Add(table.GetComponent<Transform>());
            if (flippedTables != null && flippedTables.Count > 0)
            {
                Debug.LogError(flippedTables.Count);
            }
            //if (Vector3.Distance(transform.position, table.transform.position) < 2f)
            //    tableAnim.TableUnflip();

            if (Vector3.Distance(transform.position, flippedTables[0].transform.position) < 3f)
            {
                Animator nearestTableAnimator = flippedTables[0].GetComponent<Animator>();
                nearestTableAnimator.SetTrigger("Unflip");
                flippedTables[0].tag = "Unflipped";
                flippedTables[0].transform.GetChild(0).tag = "Unflipped";
                flippedTables.RemoveAt(0);
            }
        }
    }



    void FindNewTable()
    {
        
        if (flippedTables.Count > 0) {

            target = flippedTables[0].transform;
            agent.SetDestination(target.position);
            flippedTables.Remove(flippedTables[0]);

        } 
        else
        {
            Collider[] tableColliders = Physics.OverlapSphere(transform.position, 100.0f, tableLayerMask);
            if (tableColliders.Length > 0)
            {
                target = tableColliders[Random.Range(0, tableColliders.Length)].transform;
                agent.SetDestination(target.position);
            }
        }

    }

    void CheckForPlayer()
    {
        
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        questionMark.transform.LookAt(player);
        exclamationMark.transform.LookAt(player);

        if (angleToPlayer <= fieldOfViewAngle / 2 || angleToPlayer >= 360 - fieldOfViewAngle / 2 || isSprinting)
        {


            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, 10000f) && (!pHiding.isUnderTable || isSprinting))
            {
                if (hit.collider.CompareTag("Player") && !isSprinting)
                {
                    agent.SetDestination(player.position);
                    agent.isStopped = true; // Stop the waiter
                    Invoke("StartSprinting", questionMarkDisplayDuration);
                    if (Vector3.Distance(player.position, transform.position) < 5.0f)
                        Invoke("StartSprinting", 0f);



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


            if ((isQuestionMarkVisible && !pHiding.isUnderTable) || isExclamationMarkVisible)
                SmoothLookAtPlayer();



            if (pHiding.isUnderTable)
            {
                CancelInvoke();
                agent.isStopped = false;
                if (isQuestionMarkVisible)
                    HideQuestionMark();
            }

        }

    }

    void ShowQuestionMarkAboveHead()
    {

        // NOTE: Sometimes turns to exclamation mark without it turning to question mark first

        if (!isQuestionMarkVisible)
        {
            // Question mark
            questionMark.SetActive(true);
            isQuestionMarkVisible = true;
            questionMarkDisplayStartTime = Time.time;

            // AUDIO
            if (channelSound != null)
            {
                audioSource.clip = channelSound;

                // Set positions for stereo effect
                audioSource.transform.position = transform.position - transform.right;
                audioSource.transform.position = transform.position + transform.right;

                audioSource.Play();
            }
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
        //if (!pHiding.isUnderTable)
        //{
            ShowExclamationMarkAboveHead();
            agent.isStopped = false;
            agent.speed *= 2;
            exclamationMark.SetActive(true);
        //}
        questionMark.SetActive(false);
        isQuestionMarkVisible=false;
        isSprinting = true;
    }


    void SmoothLookAtPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly interpolate the rotation over 0.5 seconds.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1.0f * Time.deltaTime);
    }
}
