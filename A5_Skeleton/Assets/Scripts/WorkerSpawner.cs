using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerSpawner : MonoBehaviour
{
    Transform whereToSpawnPos;
    public float maxDelaySpawnTime;
    float timer;
    public Worker workerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        timer = maxDelaySpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            Instantiate(workerPrefab);
            timer = maxDelaySpawnTime;
        }
    }
}
