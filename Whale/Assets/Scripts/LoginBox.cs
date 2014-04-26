using UnityEngine;
using System.Collections;

public class LoginBox : MonoBehaviour 
{
	public string userName;
	public string passWord;
	
	public GUIText grafxText;
	public GameManager manager;
	
	public bool showLogin;
	
	// Use this for initialization
	void Start () 
	{
		manager = GameObject.Find("GameManager").GetComponent<GameManager>();
		
		userName = "0000";
		passWord = "";
		
	}
	
	// Update is called once per frame
	
	void OnGUI()
	{	
		if(!showLogin)
		{
			GUI.Label(new Rect(130, 200, 100, 20), "UserName : ");
			
			userName = GUI.TextField(new Rect(200, 200, 100, 20), userName );
			
			GUI.Label(new Rect(130, 220, 100, 20), "PassWord : ");
			
			passWord = GUI.PasswordField(new Rect(200, 220, 100, 20), passWord, "%"[0], 25);
			
			if(GUI.Button (new Rect (200, 170, 100, 20), "Connect") )
			{
				//grafxText.text = "Connect";
				
				showLogin = !showLogin;
				manager.start = true;
			}
		}
		
		else
		{
			GUI.Label(new Rect(10, 40, 100, 20), userName);
			
			if(GUI.Button (new Rect(10, 10, 100, 20), "Disconnect") )
			{
				//grafxText.text = "Hello";
				
				showLogin = !showLogin;
				manager.start = false;
			}
		}
	}
}
