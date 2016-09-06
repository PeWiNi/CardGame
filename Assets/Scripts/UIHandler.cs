using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UIHandler : MonoBehaviour {
	public GameObject[] menus;
	public GameObject mainMenu; 
	public GameObject matchMenu;
	public GameObject settingsMenu;
	public GameObject cardCollection;
	public GameObject shopMenu;
	public GameObject achievementList;
	public GameObject spinWheel;
	public GameObject mainMatchMenu;
	public GameObject playerHistory;
	public GameObject purchaseDMenu;
	public GameObject lossImage;
	public GameObject winImage;
	public GameObject drawImage;
	List <GameObject> history; 
	GameObject currentMenu;
	public bool inGame=false;

	// Use this for initialization
	void Start () {
		//maybe add them all to an array ? menus ?

		mainMenu.SetActive (true);
		matchMenu.SetActive (false);
		settingsMenu.SetActive (false);
		cardCollection.SetActive (false);
		shopMenu.SetActive (false);
		achievementList.SetActive (false);
		spinWheel.SetActive (false);
		playerHistory.SetActive (false);
		mainMatchMenu.SetActive (false);

		lossImage.SetActive (false);
		winImage.SetActive (false);
		drawImage.SetActive (false);

		history = new List<GameObject> ();
		currentMenu = mainMenu;

	}
	
	// Update is called once per frame
	void Update () {
		float axis = Input.GetAxis ("Cancel");
		if (axis != 0)
			if (inGame) 
			{
				currentMenu = matchMenu;
				currentMenu.SetActive (true);
			}
			else GoBack ();
	}

	public void FindGame()
	{
		Debug.Log ("start game");
		//need to have a mainMatchMenu
		inGame=true;
		currentMenu.SetActive (false);
		mainMatchMenu.SetActive (true);
		history=new List<GameObject>();
	}

	public void ResumeMatch()
	{
		currentMenu.SetActive (false);
		currentMenu = mainMatchMenu;
		currentMenu.SetActive (true);	
	}

	public void Surrender()
	{
		currentMenu.SetActive (false);
		Debug.Log ("surrendered, you no get no experience. Cheater! ");
		currentMenu = lossImage;
		currentMenu.SetActive (true);
	}

	public void GoBack()
	{
		
		if (history.Count == 0)
			 QuitApp ();
		else {
				currentMenu.SetActive (false);
				currentMenu = history [history.Count - 1];
				currentMenu.SetActive (true);
				history.Remove (history [history.Count - 1]);
			}


	}
		

	public void QuitApp()
	{ 
		Application.Quit ();
	}

	public void OpenMenu(string button)
	{
		try{
		//Debug.Log (history + " " + currentMenu);
		history.Add (currentMenu);
		currentMenu.SetActive (false);
		currentMenu =(GameObject) this.GetType ().GetField (button).GetValue (this);
		currentMenu.SetActive (true);
		}
		catch (NullReferenceException e) {
			Debug.Log ("can't add...but why do i want to add????");
		}

	}


	//needs to know how to handle stores.
	public void PurchaseItem(int value)
	{
		switch (value) {
		case 500: //2euros, 15 dkk
			//call purchase amount to stores; return if successfull or not
			Debug.Log ("bought 500 gold");
			break;
		case 1200: //4 euros, 30 dkk
			Debug.Log ("bought 1200 gold");
			break;
		case 2500: //8 euros, 60dkk
			Debug.Log ("bought 2500 gold");
			break;
		case 5000: //15 euros, 112.5 dkk
			break;
		case 12000: //25 euros, 225 dkk
			break;
		case 20000: //everything for life: 100euros, 750 dkk
			break; 
		case 50000: // card back
			break;
		}
		history.Add (currentMenu);
		currentMenu.SetActive (false);
		currentMenu = purchaseDMenu;
		currentMenu.SetActive (true);
		currentMenu.GetComponent<Text> ().text = "purchased or not purchased something. idk yet";
	}
}
