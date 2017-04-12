using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodoController : MonoBehaviour {

	int speed = 30;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.J))
			transform.Rotate(Vector3.right * speed * Time.deltaTime);

		if (Input.GetKey(KeyCode.L))
			transform.Rotate(-Vector3.right * speed * Time.deltaTime);

	}
}
