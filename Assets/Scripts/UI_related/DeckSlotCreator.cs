using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DeckSlotCreator : MonoBehaviour {
	GameObject[] decks;
	public int offset=20;
	public GameObject uiHandler;
	UIHandler uiH;

	void Awake()
	{
		uiH = uiHandler.GetComponent<UIHandler> ();
	}
	// Use this for initialization
	void Start () {

		GameObject cv = GameObject.Find ("Canvas");
		Transform deckList=cv.transform.FindChild("Card_Collection").FindChild("Decks");

		decks = new GameObject[10];

		int screenArea=Screen.width*Screen.height;
		int deckButtonWidth = 200; 
		int deckButtonHeight = 60;


			for (int j = 1; j <10; j++) 
			{
				decks [j] = Instantiate (Resources.Load ("Prefabs/deck_ui"), transform.position, Quaternion.identity)as GameObject;
				decks [j].transform.name = "deck" + j.ToString ();

				decks [j].transform.parent = deckList;
				decks [j].GetComponent<RectTransform> ().sizeDelta = new Vector2 (deckButtonWidth, deckButtonHeight);
				decks [j].transform.localScale =new Vector3(1,1,1);
				Vector3 newPos=new Vector3 (-1*deckButtonWidth/2,-j*deckButtonHeight- offset,0);

				decks [j].GetComponent<RectTransform> ().anchoredPosition = newPos;

				Button b = decks [j].GetComponent<Button> ();
				GameObject temp = decks [j];

				b.onClick.AddListener (delegate {
					uiH.DeckViewer(temp);
				}); 

			}

	}

}
