using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
	public GameObject mainMenu; 
	public GameObject matchMenu;
	public GameObject settingsMenu;
	public GameObject cardCollection;
	public GameObject shopMenu;
	public GameObject achievementList;



	// Use this for initialization
	void Start () {
		//mainMenu = GameObject.Find("Main_Menu");
		mainMenu.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void findGame()
	{
		Debug.Log ("start game");
	}

}
