using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public Client activeClient;
	public string command;
	public Queue serverCommand;
	public Player player;
	public Pellet[] pellets;
	char delim = '$';
	public bool move;
	public bool send;
	public bool start;
	LoginBox guiBox;
	
	// Use this for initialization
	void Start () 
	{
		activeClient = GameObject.Find("GameClient").GetComponent<Client>();
		player = GameObject.Find ("Player").GetComponent<Player>();
		guiBox = GameObject.Find("GUI").GetComponent<LoginBox>();
		serverCommand = new Queue();
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
			sendMove();
			applyMove();
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
		//loads the next server command and reads the first command
		//The first command is the command type (0 = disconect, 1 = connect, 2 = move)
		if(serverCommand.Count != 0)
		{
			string tempCommand = serverCommand.Dequeue().ToString();
			string comType = tempCommand.Substring(0,tempCommand.IndexOf(delim));
			
			//Server Disconectd Client
			if(comType.Equals("0"))
			{
				guiBox.grafxText.text = "error, disconected from server";
				activeClient.Disconnect();
			}
			//Server connected client
			else if(comType.Equals("1"))
			{
				start = true;
				guiBox.showLogin = !guiBox.showLogin;
				guiBox.grafxText.text = "Connected";
			}
			//Server sent Move commands to client
			if(move == true && comType.Equals("2"))
			{
				//sets player position to match server
				int tempX  =  (int)float.Parse(tempCommand.Substring(0,tempCommand.IndexOf(delim)));
				tempCommand= tempCommand.Substring(tempCommand.IndexOf(delim)+1);
				int tempY  =  (int)float.Parse(tempCommand.Substring(0,tempCommand.IndexOf(delim)));
				player.transform.position = new Vector2(tempX, tempY);
				tempCommand= tempCommand.Substring(tempCommand.IndexOf(delim)+1);
				
				//sets player direction to match server
				tempX = int.Parse(tempCommand.Substring(0,tempCommand.IndexOf(delim)));
				tempCommand= tempCommand.Substring(tempCommand.IndexOf(delim)+1);
				tempY = int.Parse(tempCommand.Substring(0,tempCommand.IndexOf(delim)));
				
				tempCommand = tempCommand.Substring(tempCommand.IndexOf(delim)+1);
				player.setDirection(tempX, tempY);
				//sets player speed
				player.setSpeed(int.Parse(tempCommand.Substring(0,tempCommand.IndexOf(delim))));
				tempCommand = tempCommand.Substring(tempCommand.IndexOf(delim)+1);
				
				//sets player size
				player.size = int.Parse(tempCommand.Substring(0,tempCommand.IndexOf(delim)));
				tempCommand = tempCommand.Substring(tempCommand.IndexOf(delim)+1);
				
				//sets pellet position
				for(int i = 0; i < 4; i++)
				{
					
					print ("i = " + i+ "\ncomand: " + tempCommand);
					tempX = (int)float.Parse(tempCommand.Substring(0,tempCommand.IndexOf(delim)));
					tempCommand = tempCommand.Substring(tempCommand.IndexOf(delim)+1);
					tempY = (int)float.Parse(tempCommand.Substring(0,tempCommand.IndexOf(delim)));
					tempCommand = tempCommand.Substring(tempCommand.IndexOf(delim)+1);
					pellets[i].setPos(tempX, tempY);
				}
				move = false;
			}
			//writes the server command to the gui
			else if(comType.Equals("3"))
			{
				guiBox.grafxText.text = tempCommand.Substring(tempCommand.IndexOf(delim));
			}
		}
	}
}
