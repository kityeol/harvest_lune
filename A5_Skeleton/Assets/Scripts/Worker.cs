using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    public GameObject Seedhouse;
    public GameObject Field;
    public GameObject House;
    public Seed seedPrefab;
    NavMeshAgent agent;
    [Tooltip("yOffset when collecting")]
    public float yCollectingOffset;
    [Tooltip("yOffset when planting")]
    public float yPlantingOffset;
    public float maxTimeSleepDelay;
    public ParticleSystem sleep;

    private enum State { Retrieving, Planting, Returning }
    private State currentState;
    private bool hasSeed = false;
    private bool hasPlanted = false;
    private bool isSleeping = false;
    private float sleepTimer;
    private static Vector3 randomPoint(Vector3 center, Vector3 size)
    {
        return center + new Vector3((Random.value - 0.5f) * size.x, (Random.value - 0.5f) * size.y, (Random.value - 0.5f) * size.z);
    }


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Retrieving;
        sleepTimer = maxTimeSleepDelay;
    }

    void Update()
    {
        if (currentState == State.Retrieving)
        {
            Retrieving();
        }

        if (currentState == State.Planting)
        {
            Planting();
        }

        if (currentState == State.Returning)
        {
            // StartCoroutine(Returning());
            Returning();
        }
    }

    Vector3 randDestination;
    Seed seedRef;
    void Retrieving()
    {
        Debug.Log("retrieving state");
        agent.SetDestination(Seedhouse.transform.position);
        if (transform.position.x == Seedhouse.transform.position.x && !hasSeed)
        {
            seedRef = Instantiate<Seed>(seedPrefab); // creating seed in world
            seedRef.transform.parent = gameObject.transform; // making seed child
            seedRef.transform.position = gameObject.transform.position + new Vector3(0.0f, yCollectingOffset); // put seed on top of worker
            seedRef.isPlanted = true;
            hasSeed = true;
            // setup for Plating state
            randDestination = randomPoint(Field.transform.position, Field.transform.localScale);
            Debug.Log("rando destination set to : " + randDestination);
            hasPlanted = false;
            Debug.Log("Change to planting");
            currentState = State.Planting;
        }
    }

    void Planting()
    {
        Debug.Log("planting state");
        agent.SetDestination(randDestination);
        if (hasSeed && !hasPlanted && transform.position.x == randDestination.x)
        {
            seedRef.transform.parent = null;
            seedRef.transform.position = randDestination + new Vector3(0.0f, yPlantingOffset);
            hasSeed = false;
            currentState = State.Returning;
            hasPlanted = true;
        }
    }


    void Returning()
    {
        if (isSleeping)
        {
            sleepTimer -= Time.deltaTime * 1.0f;
        }

        Debug.Log("returning state");
        agent.SetDestination(House.transform.position);
        if (!isSleeping && transform.position.x == House.transform.position.x)
        {
            isSleeping = true;
            sleep.Play();
        }
        if (isSleeping && sleepTimer < 0.0f)
        {
            currentState = State.Retrieving;
            isSleeping = false;
            sleepTimer = maxTimeSleepDelay;
        }
    }
}