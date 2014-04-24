using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public Client activeClient;
	public string command;
	public string serverCommand;
	public Player player;
	public Pellet[] pellets;
	char delim = '$';
	public bool move;
	public bool send;
	public bool start;
	
	// Use this for initialization
	void Start () 
	{
		activeClient = GameObject.Find("GameClient").GetComponent<Client>();
		player = GameObject.Find ("Player").GetComponent<Player>();
		command = "";
		start = false;
		move = false;
		send = false;
		pellets = new Pellet[4];
		for(int i = 0; i <4; i++)
		{
			pellets[i] = GameObject.Find ("Pellet" + (i+1).ToString()).GetComponent<Pellet>();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(start)
		{
			sendMove();
			applyMove();
		}
	}
	
	public void sendMove()
	{
		//starts command with new direction (U = up, D = down
		//L = left, and R = right)
		send = true;
		if(Input.GetKeyDown(KeyCode.UpArrow) )
		{
			command = "U$"; 
		}
		
		else if(Input.GetKeyDown(KeyCode.DownArrow) )
		{
			command = "D$";
		}
		
		else if(Input.GetKeyDown(KeyCode.LeftArrow) )
		{
			command = "L$";
		}
		
		else if(Input.GetKeyDown(KeyCode.RightArrow) )
		{
			command = "R$";
		}
		//default command, means no change
		else
		{
			send = false;
		}
		
		//finishes the command with player data (Position x and y, speed, and size)
		if(send == true)
		{
			command = command + player.transform.position.x.ToString() + "$" + player.transform.position.y.ToString() 
			      + "$" + player.speed.ToString() + "$" + player.size.ToString();
			activeClient.requestMove(command);
			send = false;
		}
	}
	
	void applyMove()
	{
		if(move == true)
		{
			//sets player position to match server
			int tempX  =  (int)float.Parse(serverCommand.Substring(0,serverCommand.IndexOf(delim)));
			serverCommand= serverCommand.Substring(serverCommand.IndexOf(delim)+1);
			int tempY  =  (int)float.Parse(serverCommand.Substring(0,serverCommand.IndexOf(delim)));
			player.transform.position = new Vector2(tempX, tempY);
			serverCommand= serverCommand.Substring(serverCommand.IndexOf(delim)+1);
			
			//sets player direction to match server
			tempX = int.Parse(serverCommand.Substring(0,serverCommand.IndexOf(delim)));
			serverCommand= serverCommand.Substring(serverCommand.IndexOf(delim)+1);
			tempY = int.Parse(serverCommand.Substring(0,serverCommand.IndexOf(delim)));
			
			serverCommand = serverCommand.Substring(serverCommand.IndexOf(delim)+1);
			player.setDirection(tempX, tempY);
			//sets player speed
			player.setSpeed(int.Parse(serverCommand.Substring(0,serverCommand.IndexOf(delim))));
			serverCommand = serverCommand.Substring(serverCommand.IndexOf(delim)+1);
			
			//sets player size
			player.size = int.Parse(serverCommand.Substring(0,serverCommand.IndexOf(delim)));
			serverCommand = serverCommand.Substring(serverCommand.IndexOf(delim)+1);
			
			//sets pellet position
			for(int i = 0; i < 4; i++)
			{
				
				print ("i = " + i+ "\ncomand: " + serverCommand);
				tempX = (int)float.Parse(serverCommand.Substring(0,serverCommand.IndexOf(delim)));
				serverCommand = serverCommand.Substring(serverCommand.IndexOf(delim)+1);
				tempY = (int)float.Parse(serverCommand.Substring(0,serverCommand.IndexOf(delim)));
				serverCommand = serverCommand.Substring(serverCommand.IndexOf(delim)+1);
				pellets[i].setPos(tempX, tempY);
			}
			move = false;
		}
	}
}
