using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevel : MonoBehaviour
{

    public GameObject ground;

    public int i = 0;

    public static bool gameEnded;

    public GameObject[] challenges;

    void Start()
    {
        gameEnded = false;
        InvokeRepeating("Spawn", 0, 0.25f);
    }

    void Update()
    {
        if (gameEnded)
        {
            CancelInvoke();
        }
    }

    void Spawn()
    {
        Instantiate(ground, new Vector3(i * 2.5f, -3.5f, 0), Quaternion.identity);
        if (i % 10 == 0)
        {
            Instantiate(challenges[Random.Range(0, challenges.Length)], new Vector3(i * 2.5f, -1.5f, 0), Quaternion.identity);
        }
        i++;
    }
}
