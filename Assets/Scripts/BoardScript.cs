using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BoardScript : NetworkBehaviour {
    public GameObject p1HandArea;
    public GameObject p2HandArea;
    
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateHandCards(Cards p1, Cards p2) {
        Card[] p1Cards = p1HandArea.GetComponentsInChildren<Card>();
        Card[] p2Cards = p2HandArea.GetComponentsInChildren<Card>();
        for (int i = 0; i < 7; i++) {
            p1Cards[i].SetCardStruct(p1.GetItem(i));
            p2Cards[i].SetCardStruct(p2.GetItem(i));
        }
    }
}
