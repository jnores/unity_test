using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour {
	public Transform brazo;



	// Use this for initialization
	IEnumerator Start () {



		WWW www = new WWW("file:///tmp/AssetBundles/Prefabs.1");
		yield return www;

		Instantiate(www.assetBundle.LoadAsset("brazo"), new Vector3(0, 0, 0), Quaternion.identity);
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
