using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinzaController : MonoBehaviour {
	int speed = 30;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.UpArrow))
			transform.Rotate(Vector3.up * speed * Time.deltaTime);

		if (Input.GetKey(KeyCode.DownArrow))
			transform.Rotate(-Vector3.up * speed * Time.deltaTime);
		

	}
}
