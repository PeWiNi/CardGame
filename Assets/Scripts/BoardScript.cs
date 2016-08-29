using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class BoardScript : NetworkBehaviour {
    public GameObject p1HandArea;
    public GameObject p2HandArea;

    int drawIteration = 1;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    }

    void UpdateCard(GameObject cardObject, Cards hand, int handIndex) {
        Card card = cardObject.GetComponent<Card>();
        if (card.type != hand.GetItem(handIndex).family) {
            card.type = hand.GetItem(handIndex).family;
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

    public void UpdateHandCards(GameObject p1, GameObject p2) { // TODO: Enable selective drawing (based on player) and rotate according to player (current: p1 top, p2: bottom)
        Card[] p1Cards = p1HandArea.GetComponentsInChildren<Card>();
        Card[] p2Cards = p2HandArea.GetComponentsInChildren<Card>();

        for (int i = 0; i < 7; i++) {
            if (p1.GetComponent<Player>().ActiveCards.Count > i) {
                GameObject go = Instantiate(Resources.Load("Prefabs/Card"), p1Cards[i].transform.position, p1Cards[i].transform.rotation) as GameObject;
                go.GetComponent<Card>().SetReference(p1, i);
                go.GetComponent<Card>().interaction = Card.Interaction.Select;
                go.GetComponent<Card>().SetRotation(Quaternion.identity);
                go.GetComponent<Card>().generation = drawIteration;
                go.transform.position += new Vector3(0, 0, -1);
                NetworkServer.Spawn(go);
            }
            if (p2.GetComponent<Player>().ActiveCards.Count > i) {
                GameObject go = Instantiate(Resources.Load("Prefabs/Card"), p2Cards[i].transform.position, p2Cards[i].transform.rotation) as GameObject;
                go.GetComponent<Card>().SetReference(p2, i);
                go.GetComponent<Card>().interaction = Card.Interaction.Select;
                go.GetComponent<Card>().SetRotation(Quaternion.Euler(0, 180, 0));
                go.GetComponent<Card>().generation = drawIteration;
                go.transform.position += new Vector3(0, 0, -1);
                NetworkServer.Spawn(go);
            }
        }
        foreach (Card c in FindObjectsOfType<Card>()) {
            if (c.generation != drawIteration && c.generation != 0)
                Destroy(c.gameObject);
        }
        drawIteration++;
    }
}
