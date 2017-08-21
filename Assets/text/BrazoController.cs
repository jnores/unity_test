using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
  

public class BrazoController : MonoBehaviour {

	SortedDictionary<string, GameObject> nombreEjeToGameObject =
		new SortedDictionary<string, GameObject>();

	/**
	 * la estructura es keyCode { 'nombreEje': '', 'sentido': [+/-]1 }
	 */
	const string    TAG_NOMBRE = "nombre",
					TAG_SENTIDO = "sentido",
					TAG_ID = "id",
					TAG_EJE = "eje",
					TAG_ADELANTE = "adelante",
					TAG_ATRAS = "atras"
				;

	SortedDictionary<KeyCode, SortedDictionary<string,string> > teclaToProperties =
		new SortedDictionary< KeyCode, SortedDictionary<string,string>>();

	SortedDictionary<int, SortedDictionary<string,string> > intToProperties =
		new SortedDictionary< int, SortedDictionary<string,string>>();
	
	bool started = false;

	int speed = 30;

	static BrazoController sInstance;



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
		sInstance = this;
	}
	
	// Update is called once per frame
	void Update () {
		string nombre, eje, sentido;
		foreach(KeyValuePair<KeyCode, SortedDictionary<string,string>> entry in teclaToProperties)
		{
			if (Input.GetKey (entry.Key)) {
				print ("nombre: " + entry.Value.ContainsKey (TAG_NOMBRE) +
					"eje: " + entry.Value.ContainsKey (TAG_EJE) + 
					"sentido: " + entry.Value.ContainsKey (TAG_SENTIDO));
				if (
					entry.Value.TryGetValue (TAG_NOMBRE, out nombre) &&
					entry.Value.TryGetValue (TAG_EJE, out eje) &&
					entry.Value.TryGetValue (TAG_SENTIDO, out sentido)
				) {
					GameObject obj;
					int intSentido;
					if (
						nombreEjeToGameObject.TryGetValue (nombre, out obj) &&
						int.TryParse (sentido, out intSentido)
					) {
						rotar (obj, eje,intSentido);

					} else
					{
						print ("No se encontro el objeto grafico");
					}
				}
				else
					print ("la configuracion es invalida - "+entry.Key);
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

			string  keyAdelante="", keyAtras="";
			int id=0;
			SortedDictionary<string,string> obj = new SortedDictionary<string,string> ();	
			SortedDictionary<string,string> objAdelante = new SortedDictionary<string,string> ();	
			SortedDictionary<string,string> objAtras = new SortedDictionary<string,string> ();	

			foreach (XmlNode controlItem in controlContent) // control item node.
			{
				switch (controlItem.Name) {
				case TAG_ADELANTE:
					keyAdelante = controlItem.InnerText; 
					break;
				case TAG_ATRAS:
					keyAtras = controlItem.InnerText; 
					break;
				case TAG_ID:
					int.TryParse(controlItem.InnerText, out id); 
					break;
				}
				objAdelante.Add (controlItem.Name, controlItem.InnerText);
				objAtras.Add (controlItem.Name, controlItem.InnerText);
				obj.Add (controlItem.Name, controlItem.InnerText);
			}
			intToProperties.Add (id, obj);
			print ("ID: " + id + "keyAtras: " + keyAtras + " || keyAdelante: " + keyAdelante);
			if (keyAdelante != "") {
				KeyCode key;
				objAdelante.Add (TAG_SENTIDO, "1");

				//key = (KeyCode)System.Enum.Parse (typeof(KeyCode), keyAdelante.ToLower());
				//teclaToProperties.Add (key, obj);

				key = (KeyCode)System.Enum.Parse (typeof(KeyCode), keyAdelante.ToUpper());
				teclaToProperties.Add (key, objAdelante);
			}
			if (keyAtras != "") {
				KeyCode key;
				objAtras.Add (TAG_SENTIDO, "-1");

				//key = (KeyCode)System.Enum.Parse (typeof(KeyCode), keyAtras.ToLower());
				//teclaToProperties.Add (key, obj);

				key = (KeyCode)System.Enum.Parse (typeof(KeyCode), keyAtras.ToUpper());
				teclaToProperties.Add (key, objAtras);
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

	static public BrazoController getInstance()
	{
		return sInstance;
	}

	public void moverById (int ejeName, int valor)
	{
		string nombre, eje;
		SortedDictionary<string,string> entry;
		if (intToProperties.TryGetValue (ejeName, out entry))
		{
			
			if (
				entry.TryGetValue (TAG_NOMBRE, out nombre) &&
				entry.TryGetValue (TAG_EJE, out eje)
				
			) {
				GameObject obj;
				if (
					nombreEjeToGameObject.TryGetValue (nombre, out obj)
				) {
					print ("Rotar BY ID: "+ ejeName + "  - "+valor);
					rotar (obj,eje, valor);
				} else
				{
					print ("No se encontro el objeto grafico");
				}
			}
			else
				print ("la configuracion es invalida");
		}

	}

	private void rotar(GameObject obj, string eje, int intSentido)
	{

		switch (eje) {
		case "X":
			obj.transform.Rotate (intSentido * Vector3.right * speed * Time.deltaTime);
			break;
		case "Y":
			obj.transform.Rotate (intSentido * Vector3.up * speed * Time.deltaTime);
			break;
		case "Z":
			obj.transform.Rotate (intSentido * Vector3.back * speed * Time.deltaTime);
			break;
		}
	}
}
