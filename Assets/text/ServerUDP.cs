using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;


public class ServerUDP : MonoBehaviour
{
    // read Thread
    Thread readThread;
	 // udpclient object
    UdpClient client;
	  // port number
    public int port = 8008;
	 // UDP packet store
  
	string message;
	bool state;

	void Start()
    {
		print("INIT THREAD SERVER UDP");
		// create thread for reading UDP messages
		readThread = new Thread(new ThreadStart(ReceiveData));
		readThread.IsBackground = true;
		readThread.Start();
		print("THREAD SERVER UDP START");

		     
    }

   

    // Stop reading UDP messages
    public void stopThread()
    {
		print("STOP THREAD SERVER UDP.");
		if (readThread !=null && readThread.IsAlive)
        {
            readThread.Abort();
        }
		if (client != null) {
			client.Close ();
		}
    }

    // receive thread function
    private void ReceiveData()
    {
        client = new UdpClient(port);
		print ("SERVER UDP ESCUCHANDO EN EL PUERTO:" + port);
        while (true)
        {
            try
            {
                // receive bytes
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
				byte[] data = client.Receive(ref anyIP);
				// encode UTF8-coded bytes to text format
				message = Encoding.UTF8.GetString(data);
				if(message!=null){
						this.state=true;
				}

			}
	    catch (Exception err)
            {
				print ("ERROR: " + err);
            }
        }
    }

	public string getMessage(){
		state = false;
		return this.message;

	}

	public bool getState(){
		return state;
	}

}