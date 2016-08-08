using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Player : NetworkBehaviour {
    //[SerializeField]
    public Cards Deck = new Cards(15);
    public Cards ActiveCards = new Cards(7);
    public Cards Graveyard = new Cards(15);

    // Use this for initialization
    void Start () {
        
        if (isLocalPlayer) {
            Menu menu = GameObject.Find("NetworkManager").GetComponent<Menu>();
            setupDeck(menu.GetDeck());
        }
    }

    void setupDeck(List<CardStruct.CardType> deck) {
        foreach(CardStruct.CardType ct in deck) {
            Deck.Add(new CardStruct(ct));
        }
    }

    // Update is called once per frame
    void Update () {
	    
	}
}
