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
    HashSet<int> usedCards = new HashSet<int>(); // Lists are better in small sizes

    [SerializeField][SyncVar]
    public Cards Mulligan = new Cards(15);
    [SyncVar]
    int mulligan = 0;

    [SyncVar]
    public bool ready;

    Button MulliganButton;
    GameObject PhaseText;

    Menu menu;

    [SyncVar]
    public GameMaster.Phase currentPhase = GameMaster.Phase.Startup;

    // Use this for initialization
    void Start () {
        
        if (isLocalPlayer) {
            Menu[] menus = GameObject.FindObjectsOfType<Menu>();
            menu = menus[0];
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < menus.Length; i++) { 
                if (menus[i].GetComponent<NetworkIdentity>().playerControllerId == GetComponent<NetworkIdentity>().playerControllerId) {
                    if (players[i].GetComponent<NetworkIdentity>().isLocalPlayer && i == 0) { FindObjectOfType<Camera>().transform.Rotate(0, 0, 180); }
                    menu = menus[i];
                }
            }
            setupDeck(menu.GetDeck());

            menu.SetPlayer(this);
        }
    }

    void Update () {

    }

    void setupDeck(List<CardStruct.CardFamily> deck) {
        foreach(CardStruct.CardFamily ct in deck) {
            CmdAddToDeck(ct);
        }
    }

    public void SetMulliganButton(Button mb) {
        MulliganButton = mb;
        MulliganButton.interactable = false;
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
        // Reveal cards to the other player (for the sake of keeping cards revealed for future rounds)
        List<CardStruct> hand2 = new List<CardStruct>(15);
        foreach (CardStruct c in ActiveCards)
            hand2.Add(c.Reveal());
        ActiveCards.Renew(hand2);

        mulligan = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="card">Mulligan'ed card</param>
    public void AddMulligan(Card card) {
        if(isLocalPlayer) {
            if (!card.Revealed()) {
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

    public void SendReady() {
        if(currentPhase != GameMaster.Phase.Ready)
            CmdSendReady(!ready);
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

    [Command]
    void CmdSendReady(bool isReady) {
        ready = isReady;
    }

    void PhaseChange(GameMaster.Phase newPhase) {
        if(isLocalPlayer) {
            if (newPhase == GameMaster.Phase.Ready)
                CmdSendReady(true);
            else { CmdSendReady(false); }
            SetText(PhaseTranslate(newPhase));
        }
        currentPhase = newPhase;
    }

    public void SetText(int state) {
        if(isLocalPlayer) {
            SetText(state == 0 ? "Draw" : state == 1 ? "Winner" : "Better luck next time!");
            currentPhase = GameMaster.Phase.Endstate;
        }
    }

    public void SetText(string txt) {
        PhaseText.GetComponent<Text>().text = txt;
    }

    public void PointsFromEndGame(int state) {
        if (isLocalPlayer) 
            menu.GetComponent<PlayerData>().RewardExperience(state == 1 ? ScoreData.EndState.Win : state == 2 ? ScoreData.EndState.Loose : ScoreData.EndState.Draw);
    }

    string PhaseTranslate(GameMaster.Phase phase) {
        string txt = "";
        switch(phase) {
            case (GameMaster.Phase.Startup):
                txt = "Waiting for players to connect..";
                break;
            case (GameMaster.Phase.Mulligan):
                txt = "Choose cards to throw back in the deck";
                break;
            case (GameMaster.Phase.Ready):
                txt = "Waiting on other player";
                break;
            case (GameMaster.Phase.PrimaryMatchup):
                txt = "Primary matchup";
                break;
            case (GameMaster.Phase.SecondaryMatchup):
                txt = "Secondary matchup";
                break;
            case (GameMaster.Phase.BonusMatchup):
                txt = "Magic!";
                break;
            case (GameMaster.Phase.Aftermath):
                txt = "Round ended press ready to continue";
                break;
        }
        return txt;
    }

    void OnEnable() {
        GetComponent<Events>().EventPhaseChange += PhaseChange;
        GetComponent<Events>().EventEnd += SetText;
        GetComponent<Events>().EventEnd += PointsFromEndGame;
    }
    void OnDisable() {
        GetComponent<Events>().EventPhaseChange -= PhaseChange;
        GetComponent<Events>().EventEnd -= SetText;
        GetComponent<Events>().EventEnd -= PointsFromEndGame;
    }

    public HashSet<int> GetUsedCards() {
        return usedCards;
    }

    public void SetTextField(GameObject go) {
        PhaseText = go;
    }
}
