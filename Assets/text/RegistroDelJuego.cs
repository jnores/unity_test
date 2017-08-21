using UnityEngine;
using System.Collections;
using System;

public class RegistroDelJuego : MonoBehaviour
{

	ServerUDP servidor;
	public GameObject[] servos;
	GameObject[] players;
	GameObject[] firePoints;



	void Start ()
	{
		servidor = gameObject.AddComponent<ServerUDP> () as ServerUDP;

	}

	void Update ()
	{

		if (servidor.getState ()) {
			string peticion;
			peticion = servidor.getMessage ();
			resolverPeticion (peticion);

		}

	}


	private void resolverPeticion (string pet)
	{
		if (pet != null) {
			print ("Peticion {" + pet + "}");
			char[] c = { '(' };
			string[] partesPeticion = pet.Split (c); //pet.Substring(pos_i,pet.IndexOf (SEPARADOR_F)) 
			if (partesPeticion.Length >= 2) {
				string metodo = partesPeticion [0];
				string parametro = partesPeticion [1].Substring (0, partesPeticion [1].Length - 1);

				print ("METODO: " + metodo);
				print ("PARAMETRO: " + parametro);
				if (validarMetodoYParametro (metodo, parametro)) {
					ejecutar (metodo, parametro);
				}
			}

		}

	}

	private bool validarMetodoYParametro (string metodo, string param)
	{
		//validar que el metodo sea valido, y que los parametros se correspondan
		return true;
	}

	private void ejecutar (string metodo, string parametro)
	{	
		switch (metodo) {
		case "ACCION":
			ejecutarAccion (parametro);
			break;
		case "ROTAR":
			rotarServo (parametro);
			break;
		case "GET_ESTADO": 
			getEstado (parametro);
			break;
		case "QUIT": 
			this.OnApplicationQuit ();
			break;
		}


	}

	private void ejecutarAccion (string param)
	{
		print ("Todo");
	}

	private void rotarServo (string param)
	{
		char[] SEPARADOR_PARAM = { ',' };
		string[] partesParametro = param.Split (SEPARADOR_PARAM);
		if (partesParametro.Length >= 2) {
			int n_servo = int.Parse (partesParametro [0]);
			int valor = int.Parse (partesParametro [1]);

			BrazoController brazo = BrazoController.getInstance ();
			if (brazo) {
				brazo.moverById (n_servo, valor);
			}
		}


	}


	private void getEstado (string param)
	{
		print ("ToDo");
	}



	void OnApplicationQuit ()
	{
		print ("QUIT APP");
		servidor.stopThread ();
		Application.Quit ();
	}



}

