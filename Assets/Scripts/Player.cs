using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : NetworkBehaviour {
    [SerializeField][SyncVar]
    public Cards Deck = new Cards(15);
    [SerializeField][SyncVar]
    public Cards ActiveCards = new Cards(7);
    [SerializeField][SyncVar]
    public Cards Graveyard = new Cards(15);

    [SerializeField][SyncVar]
    public Cards Mulligan = new Cards(15);
    [SyncVar]
    int mulligan = 0;

    public Button MulliganButton;

    // Use this for initialization
    void Start () {
        
        if (isLocalPlayer) {
            Menu menu = GameObject.Find("NetworkManager").GetComponent<Menu>();
            setupDeck(menu.GetDeck());
            MulliganButton.gameObject.SetActive(true);
            MulliganButton.interactable = false;
        }
    }

    void setupDeck(List<CardStruct.CardFamily> deck) {
        foreach(CardStruct.CardFamily ct in deck) {
            CmdAddToDeck(ct);
        }
    }

    [Command]
    void CmdAddToDeck(CardStruct.CardFamily ct) {
        Deck.AddCard(new CardStruct(ct));
    }

    //GameMaster function : start the mulligan
    public void SetUpMulligan(int mulliganCount) {
        mulligan = mulliganCount;
    }

    //GameMaster function : end the mulligan
    public void StopMulligan() {
        mulligan = 0;
        GetComponent<Events>().SendEndMulligan();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="card">Mulligan'ed card</param>
    public void AddMulligan(Card card) {
        if(isLocalPlayer) {
            if (card.selected == false && mulligan > 0 && Graveyard.Count < Deck.Count - ActiveCards.Count) {
                card.selected = true;
                CmdMulliganCard(card.ToCardStruct(), false);
                mulligan--;
                MulliganButton.interactable = true;
            } else if (card.selected == true) {
                card.selected = false;
                CmdMulliganCard(card.ToCardStruct(), true);
                mulligan++;
            }
        }
    }

    [Command]
    void CmdMulliganCard(CardStruct cs, bool remove) {
        if (!remove) {
            Mulligan.AddCard(cs);
            print(gameObject.name + " Mulligan'ed " + CardStruct.determineCard(cs.family));
        } if (remove) {
            Mulligan.Remove(cs);
            print(gameObject.name + " Un-Mulligan'ed " + CardStruct.determineCard(cs.family));
        }
    }

    public void SendMulligan() {
        CmdSendMulligan();
        //print("Told the server to mulligan my cards");
        MulliganButton.interactable = false;
        mulligan = 0;
        RemoveSelected();
    }

    void RemoveSelected() {
        foreach(Card c in FindObjectsOfType<Card>()) {
            c.selected = false;
        }
    }

    [Command]
    void CmdSendMulligan() {
        FindObjectOfType<GameMaster>().Mulligan(this);
    }

    void EndMulligan() { // Unused
        // Set all cards to not-selected
    }

    void OnEnable() {
        GetComponent<Events>().EventEndMulligan += EndMulligan;
    }
    void OnDisable() {
        GetComponent<Events>().EventEndMulligan -= EndMulligan;
    }
}
