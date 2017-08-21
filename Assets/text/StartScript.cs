using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour {
	//public Transform brazo;
	bool ShowThisGUI = false;
	string message;



	// Use this for initialization
	void Start () {
		print ("DataPath: " + Application.dataPath);
		print ("PersistentDataPath: " + Application.persistentDataPath);
		message = Application.dataPath + "/config.xml";
		ShowThisGUI = !System.IO.File.Exists (message);
		if (!ShowThisGUI) {
			message = Application.dataPath + "/AssetBundles/Prefabs.1";
			ShowThisGUI = !System.IO.File.Exists (message);
			if (!ShowThisGUI) {
				AssetBundle assetBundle = AssetBundle.LoadFromFile (message);

				Instantiate(assetBundle.LoadAsset("brazo"), new Vector3(0, 0, 0), Quaternion.identity);
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI () {

		if (ShowThisGUI) {
			GUI.Box (new Rect (Screen.width / 2-200, Screen.height / 2-150, 400f, 300f), "ERROR");
			GUI.Label (new Rect (Screen.width / 2 - 180, Screen.height / 2 - 95, 380f, 250f), "Falta el archivo: " + message);

		}

	}
}
