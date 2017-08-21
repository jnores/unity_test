using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
  

public class HombroController : MonoBehaviour {

	SortedDictionary<string, GameObject> nombreEjeToGameObject =
		new SortedDictionary<string, GameObject>();

	/**
	 * la estructura es keyCode { 'nombreEje': '', 'sentido': [+/-]1 }
	 */
	const string TAG_EJE = "nombreEje", TAG_SENTIDO = "sentido";

	SortedDictionary<KeyCode, SortedDictionary<string,string> > teclaToProperties =
		new SortedDictionary< KeyCode, SortedDictionary<string,string>>();

	bool started = false;

	int speed = 30;


	// Use this for initialization
	void Start () {
		if (!started) {
			started = true;
			print (">>> Start");
			GameObject[] itemList = GameObject.FindGameObjectsWithTag ("ejeServo");

			foreach (GameObject item in itemList) {
				print ("Registro Eje en Dictionary: " + item.name);	
				nombreEjeToGameObject.Add (item.name, item);
			}

			parseKeys ();

			print (">>> CONFIG");
			foreach (KeyValuePair<KeyCode, SortedDictionary<string,string>> entry in teclaToProperties) {
				print ("KEYCODE " + entry.Key);
				printDictionary (entry.Value);
			}
			print ("<<< CONFIG");
			print ("<<< Start");
		}
	}
	
	// Update is called once per frame
	void Update () {
		string eje, sentido;
		foreach(KeyValuePair<KeyCode, SortedDictionary<string,string>> entry in teclaToProperties)
		{
			if (Input.GetKey (entry.Key)) {
				if (
					entry.Value.TryGetValue (TAG_EJE, out eje) &&
					entry.Value.TryGetValue (TAG_SENTIDO, out sentido)
				) {
					GameObject obj;
					int intSentido;
					if (
						nombreEjeToGameObject.TryGetValue (eje, out obj) &&
						int.TryParse (sentido, out intSentido))
						obj.transform.Rotate (intSentido * Vector3.up * speed * Time.deltaTime);
					else
						print ("No se encontro el objeto grafico");
				}
				else
					print ("la configuracion es invalida");
			}
		}		
	}
		
		/**
		 * 
		 * Configuracion JSON
{
  'controls': [
      {
        'eje': 'hombro',
        'adelante':'Q',
        'atras': 'A'
      },
      {
        'eje': 'codo',
        'adelante':'W',
        'atras': 'S'
      }, 
      {
        'eje': 'munieca',
        'adelante':'E',
        'atras': 'D'
      },
      {
        'eje': 'pinza',
        'adelante':'R',
        'atras': 'F'
      }
  ]
}

		 * 
		 * Configuracion XML
<?xml version="1.0" encoding="UTF-8" ?>
<config>
	<control>
	    <eje>hombro</eje>
	    <adelante>Q</adelante>
	    <atras>A</atras>
	</control>
	<control>
	    <eje>codo</eje>
	    <adelante>W</adelante>
	    <atras>S</atras>
	</control>
	<control>
	    <eje>munieca</eje>
	    <adelante>E</adelante>
	    <atras>D</atras>
	</control>
	<control>
	    <eje>pinza</eje>
	    <adelante>R</adelante>
	    <atras>F</atras>
	</control>
</config>
	

		 * 
		 * */
	void parseKeys ()
	{
//		string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n<config>\n\t<control>\n\t    <eje>hombro</eje>\n\t    <adelante>Q</adelante>\n\t    <atras>A</atras>\n\t</control>\n\t<control>\n\t    <eje>codo</eje>\n\t    <adelante>W</adelante>\n\t    <atras>S</atras>\n\t</control>\n\t<control>\n\t    <eje>munieca</eje>\n\t    <adelante>E</adelante>\n\t    <atras>D</atras>\n\t</control>\n\t<control>\n\t    <eje>pinza</eje>\n\t    <adelante>R</adelante>\n\t    <atras>F</atras>\n\t</control>\n</config>\n";
		string path = Application.dataPath + "/config.xml";

		XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
		//xmlDoc.LoadXml(xml); // load the string/stream.
		xmlDoc.Load(path); // load the file.
		XmlNodeList controlsList = xmlDoc.GetElementsByTagName("control"); // array of the control nodes.

		foreach (XmlNode controlInfo in controlsList)
		{
			XmlNodeList controlContent = controlInfo.ChildNodes;

			string eje="", keyAdelante="", keyAtras="";
				

			foreach (XmlNode controlItem in controlContent) // control item node.
			{
				if(controlItem.Name == "eje")
				{
					eje = controlItem.InnerText;

				} 
				else if(controlItem.Name == "adelante")
				{
					keyAdelante = controlItem.InnerText; 
				} else if(controlItem.Name == "atras")
				{
					keyAtras = controlItem.InnerText; 
				}
			}
			SortedDictionary<string,string> obj;
			KeyCode key;
			if (keyAdelante != "") {
				obj = new SortedDictionary<string,string>();

				obj.Add (TAG_EJE, eje);
				obj.Add (TAG_SENTIDO, "1");

				//key = (KeyCode)System.Enum.Parse (typeof(KeyCode), keyAdelante.ToLower());
				//teclaToProperties.Add (key, obj);

				key = (KeyCode)System.Enum.Parse (typeof(KeyCode), keyAdelante.ToUpper());
				teclaToProperties.Add (key, obj);
			}
			if (keyAtras != "") {
				obj = new SortedDictionary<string,string> ();

				obj.Add (TAG_EJE, eje);
				obj.Add (TAG_SENTIDO, "-1");

				//key = (KeyCode)System.Enum.Parse (typeof(KeyCode), keyAtras.ToLower());
				//teclaToProperties.Add (key, obj);

				key = (KeyCode)System.Enum.Parse (typeof(KeyCode), keyAtras.ToUpper());
				teclaToProperties.Add (key, obj);
			}
		}
		// printDictionary(nombreArticulacionParameters);
	}

	void printDictionary (SortedDictionary<string,string> myDic)
	{	
		print (">>> printDictionary:  {");
		foreach(KeyValuePair<string, string> entry in myDic)
		{
			print("\t"+entry.Key+" => "+entry.Value);
		}
		print ("} \n<<< printDictionary:  ");
	}
}
