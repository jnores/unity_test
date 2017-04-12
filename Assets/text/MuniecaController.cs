using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuniecaController : MonoBehaviour {
	int speed = 30;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.I))
			transform.Rotate(Vector3.up * speed * Time.deltaTime);

		if (Input.GetKey(KeyCode.K))
			transform.Rotate(-Vector3.up * speed * Time.deltaTime);

	}
}
