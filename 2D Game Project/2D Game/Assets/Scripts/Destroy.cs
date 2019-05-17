using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {

	void Start () 
    {
        if (gameObject.tag == "Platform")
        {
            Object.Destroy(gameObject, 4.0f);
        }
        else
        {
            Object.Destroy(gameObject, 3.0f);
        }
	}
	
	void Update () {
        if (SpawnLevel.gameEnded)
        {
            Object.Destroy(gameObject);
        }
	}
}
