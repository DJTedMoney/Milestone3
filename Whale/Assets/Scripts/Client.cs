using UnityEngine;
using System.Collections;

public class Client : MonoBehaviour 
{
	public GameManager manager;
	public FakeServerInputs server;
	public string message;
	
	
	// Use this for initialization
	void Start () 
	{
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
		server = GameObject.Find ("FakeServer").GetComponent<FakeServerInputs>();
		message = "";
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	//sends move request to "server" from gameManager
	public void requestMove(string inputMove)
	{
		//sends the movement change command to server
		//(Will eventualy be TCP code to send inputMove to server)
		server.getMessage(inputMove);		
	}
	
	//gets move data from server and sends it to gameManager
	public void doMove(string newMove)
	{
		//sends velocity change comand to gameManager
			manager.serverCommand = newMove;
			manager.move = true;
	}
	
}
