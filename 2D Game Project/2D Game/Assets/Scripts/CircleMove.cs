using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMove : MonoBehaviour {

    public Transform rotationCenter;

    float rotationRadius = 2f, angularSpeed = 2f;

    float posX, posY, angle = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        posX = rotationCenter.position.x + Mathf.Cos(angle) * rotationRadius * -1f;
        posY = rotationCenter.position.y + Mathf.Sin(angle) * rotationRadius;
        transform.position = new Vector2(posX, posY);
        angle += Time.deltaTime * angularSpeed;
        if (angle >= 360f)
        {
            angle = 0f;
        }
    }
}
