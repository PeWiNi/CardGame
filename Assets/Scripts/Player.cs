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

    void setupDeck(List<CardStruct.CardType> deck) {
        foreach(CardStruct.CardType ct in deck) {
            CmdAddToDeck(ct);
        }
    }

    [Command]
    void CmdAddToDeck(CardStruct.CardType ct) {
        Deck.Add(new CardStruct(ct));
    }
    
    public void DrawCards() {
        CmdGiveHand();
    }

    public void Cleanup() {
        CmdRemoveDead();
    }

    [Command]
    void CmdGiveHand() { //POLISH: make it not call a cmd but assign hand on server
        HashSet<int> rng = new HashSet<int>();
        //print("I am gonna go in there");
        while (ActiveCards.Count < ActiveCards.Size && ActiveCards.Count < Deck.Count - Graveyard.Count) {
            //print("I was here!");
            int rnd = Random.Range(0, Deck.Count);
            if (!Deck.GetItem(rnd).destroyed) {
                rng.Add(rnd);
                //print("Card #" + rnd + " is being added " + (rng.Count > ActiveCards.Count) + ", total count: " + rng.Count);
                if (rng.Count > ActiveCards.Count) {
                    ActiveCards.Add(Deck.GetItem(rnd));
                }
            }
        }
    }

    [Command]
    void CmdRemoveDead() {
        List<CardStruct> temp = new List<CardStruct>(15);
        foreach (CardStruct ct in ActiveCards) {
            if (ct.destroyed) {
                Deck.Remove(ct);
                Graveyard.AddCard(ct);
            } else {
                temp.Add(ct);
            }
        } ActiveCards.Renew(temp);
    }

    void OnEnable() {
        GetComponent<Events>().EventDrawCards += DrawCards;
        GetComponent<Events>().EventCleanupDeck += Cleanup;
    }
    void OnDisable() {
        GetComponent<Events>().EventDrawCards -= DrawCards;
        GetComponent<Events>().EventCleanupDeck -= Cleanup;
    }
}
