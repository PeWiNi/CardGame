using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class BoardScript : NetworkBehaviour {
    public GameObject p1HandArea;
    public GameObject p2HandArea;

    //private Card[] p1Cards;
    //private Card[] p2Cards;
    //public Cards p1Hand = new Cards(0);
    //public Cards p2Hand = new Cards(0);

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        /*
        //Card[] p1Cards = p1HandArea.GetComponentsInChildren<Card>();
        if(p1Hand.Size > 0) {
            for (int i = 0; i < p1Cards.Length; i++) {
                if (i <= p1Hand.Size) {
                    gameObject.SetActive(false);
                    continue;
                }
                UpdateCard(p1Cards[i].gameObject, p1Hand, i);
            }
        }
        //Card[] p2Cards = p2HandArea.GetComponentsInChildren<Card>();
        if (p2Hand.Size > 0) {
            for (int i = 0; i < p2Cards.Length; i++) {
                if (i <= p2Hand.Size) {
                    gameObject.SetActive(false);
                    continue;
                }
                UpdateCard(p2Cards[i].gameObject, p2Hand, i);
            }
        }
        */
    }

    void UpdateCard(GameObject cardObject, Cards hand, int handIndex) {
        Card card = cardObject.GetComponent<Card>();
        if (card.type != hand.GetItem(handIndex).type) {
            card.type = hand.GetItem(handIndex).type;
            cardObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Textures/" + CardStruct.determineCard(card.type).ToUpper());
        }
        if (!card.dead && hand.GetItem(handIndex).destroyed) {
            gameObject.GetComponent<MeshRenderer>().material.color -= new Color(0, 1, 1, .5f);
            card.dead = true;
        }
        if (card.dead && !hand.GetItem(handIndex).destroyed) {
            card.dead = false;
        }
    }

    public void UpdateHandCards(GameObject p1, GameObject p2) {
        Card[] p1Cards = p1HandArea.GetComponentsInChildren<Card>();
        Card[] p2Cards = p2HandArea.GetComponentsInChildren<Card>();
        //p1Hand = p1;
        //p2Hand = p2;
        //Transform[] p1Cards = p1HandArea.GetComponentsInChildren<Transform>();
        //Transform[] p2Cards = p2HandArea.GetComponentsInChildren<Transform>();

        for (int i = 0; i < 7; i++) {
            if (p1.GetComponent<Player>().ActiveCards.Count > i) {
                
                GameObject go = Instantiate(Resources.Load("Prefabs/Card"), p1Cards[i].transform.position, p1Cards[i].transform.rotation) as GameObject;
                //go.GetComponent<Card>().SetReference(p1Hand, i);
                go.GetComponent<Card>().SetReference(p1, i);
                go.transform.position += new Vector3(0, 0, -1);
                NetworkServer.Spawn(go);
                
                //p1Cards[i].SetReference(p1, i);
            }
            if (p2.GetComponent<Player>().ActiveCards.Count > i) {
                
                GameObject go = Instantiate(Resources.Load("Prefabs/Card"), p2Cards[i].transform.position, p2Cards[i].transform.rotation) as GameObject;
                //go.GetComponent<Card>().SetReference(p2Hand, i);
                go.GetComponent<Card>().SetReference(p2, i);
                go.transform.position += new Vector3(0, 0, -1);
                NetworkServer.Spawn(go);
                
                //p2Cards[i].SetReference(p2, i);
            }
        }
    }
}
