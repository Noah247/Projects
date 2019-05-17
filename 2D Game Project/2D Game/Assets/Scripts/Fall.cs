using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("thwompFall", 1f);
	}
	
	// Update is called once per frame
	void Update () {

	}

    void thwompFall()
    {
        this.GetComponent<Rigidbody2D>().gravityScale = 3f;
    }
}
