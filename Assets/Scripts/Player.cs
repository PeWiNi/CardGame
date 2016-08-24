using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Player : NetworkBehaviour {
    [SerializeField][SyncVar]
    public Cards Deck = new Cards(15);
    [SerializeField][SyncVar]
    public Cards ActiveCards = new Cards(7);
    [SerializeField][SyncVar]
    public Cards Graveyard = new Cards(15);

    // Use this for initialization
    void Start () {
        
        if (isLocalPlayer) {
            Menu menu = GameObject.Find("NetworkManager").GetComponent<Menu>();
            setupDeck(menu.GetDeck());
        }
    }

    void setupDeck(List<CardStruct.CardFamily> deck) {
        foreach(CardStruct.CardFamily ct in deck) {
            CmdAddToDeck(ct);
        }
    }

    [Command]
    void CmdAddToDeck(CardStruct.CardFamily ct) {
        Deck.Add(new CardStruct(ct));
    }

    void OnEnable() {
    }
    void OnDisable() {
    }
}
