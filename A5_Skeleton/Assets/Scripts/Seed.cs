using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public bool isPlanted = false;
    public GameObject plantPrefab;
    public float maxGrowingTime;
    float timer;
    bool isGrown;

    void Start()
    {
        timer = maxGrowingTime;
        isGrown = false;
    }
    
    void Update()
    {
        if(isPlanted)
        {
            timer -= Time.deltaTime;
        }

        if(timer < 0.0f && isPlanted && !isGrown)
        {
            var plant  = Instantiate(plantPrefab);
            plant.transform.parent = gameObject.transform;
            plant.transform.position = transform.position;

            isGrown = true;
        }
    }
}
