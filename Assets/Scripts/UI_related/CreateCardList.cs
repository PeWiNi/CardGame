using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateCardList : MonoBehaviour {
	GameObject[,] cards;
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
		Transform cardPage=cv.transform.FindChild("Card_Collection").FindChild("CardPage");

		cards = new GameObject[2,5];

		int screenArea=Screen.width*Screen.height;
		int cardWidth = 126;
		int cardHeight = 176;

		for (int i = 1; i >=0; i--)
			for (int j = 4; j >=0; j--) 
			{
				cards [i,j] = Instantiate (Resources.Load ("Prefabs/card_ui"), transform.position, Quaternion.identity)as GameObject;
				cards [i, j].transform.name = "card" + i.ToString () + j.ToString ();

				cards [i,j].transform.parent = cardPage;
				cards [i, j].transform.localScale =new Vector3(1,1,1);
				Vector3 sizeOffset=new Vector3	( cards [i, j].GetComponent<RectTransform> ().sizeDelta.x,  cards [i, j].GetComponent<RectTransform> ().sizeDelta.y+30,0);

				cards [i, j].GetComponent<RectTransform> ().sizeDelta=new Vector2(cardWidth, cardHeight);

				Vector3 newPos=new Vector3 (-j*sizeOffset.x/4 - j*sizeOffset.x + offset, (i==0) ? 220:-40,0);
				cards [i, j].GetComponent<RectTransform> ().anchoredPosition = newPos;

				Button b = cards [i,j].GetComponent<Button> ();
				GameObject temp = cards [i, j];

				b.onClick.AddListener (delegate {
					//uiH.AddToDeck ();
					uiH.AddToDeck (temp);
				}); 

			}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
