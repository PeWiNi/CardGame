using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    //[SerializeField]
    public List<CardStruct.CardFamily> deck = new List<CardStruct.CardFamily>();
    public GameObject slot;

    public Player player;
    bool setup = false;

    public Button MulliganButton;
    public Button ReadyButton;
    public Text GameStateText;
    //public Cards deck = new Cards(15);

    public void Start() {
        // Turn on the cards
        UnityEngine.Networking.NetworkIdentity[] uvs = Resources.FindObjectsOfTypeAll<UnityEngine.Networking.NetworkIdentity>();
        foreach (UnityEngine.Networking.NetworkIdentity ni in uvs) {
            if(ni.GetComponent<Card>())
                ni.gameObject.SetActive(true);
        }
        if (!slot) {
            slot = GameObject.Find("Deck");
        }
        // Add listeners to buttons
        if (!MulliganButton)
            MulliganButton = transform.FindChild("PlayerCanvas").GetComponentsInChildren<Button>()[0];
        if (!ReadyButton)
            ReadyButton = transform.FindChild("PlayerCanvas").GetComponentsInChildren<Button>()[1];
        if (!GameStateText)
            GameStateText = transform.FindChild("PlayerCanvas").FindChild("PhaseText").GetComponentInChildren<Text>();
        MulliganButton.onClick.AddListener(delegate { SendMulligan(); });
        ReadyButton.onClick.AddListener(delegate { SendReady(); });
    }

    public void SetPlayer(Player p) {
        print("Player set!");
        player = p;
        p.SetTextField(GameStateText.gameObject);
    }

    public void AddCard() {
        CardStruct.CardFamily ct = CardStruct.RandomCardType();
        if (Check(ct)) deck.Add(ct);
    }

    public void AddCard(string type) { // For UI
        CardStruct.CardFamily ct = CardStruct.determineCard(type);
        if (Check(ct)) deck.Add(ct);
    }

    public void AddCard(Card card) { // For 3D
        if (Check(card.cardEntry)) {
            deck.Add(CardStruct.determineCard(card.cardEntry));
            if (slot) {
                float size = slot.GetComponentsInChildren<Card>().Length;
                GameObject go = Resources.Load<GameObject>("Prefabs/Card");
                go.GetComponent<Card>().cardEntry = card.cardEntry;
                //go.GetComponent<Card>().SetCardStruct(card.ToCardStruct());
                go.GetComponent<Card>().interaction = Card.Interaction.Remove;
                Instantiate(go, new Vector3(slot.transform.position.x + (0.85f * size), slot.transform.position.y, slot.transform.position.z + (-0.01f * size)), Quaternion.Euler(0, 180, 0), slot.transform);
            }
        }
    }

    public void RemoveCard(Card card) {
        deck.Remove(CardStruct.determineCard(card.cardEntry));
        bool killed = false;
        if (slot) {
            foreach (Card c in slot.GetComponentsInChildren<Card>()) {
                if (c.cardEntry == card.cardEntry && !killed) {
                    Destroy(c.gameObject);
                    killed = true;
                    continue;
                } if (killed) c.transform.position -= new Vector3(0.85f, 0, -0.01f);
            }
        }
    }

    public List<CardStruct.CardFamily> GetDeck() {
        while (deck.Count < 15)
            AddCard();
        return deck;
    }

    bool Check(CardStruct.CardFamily card) {
        int occurances = 0;
        foreach(CardStruct.CardFamily c in deck) {
            if (c == card)
                occurances++;
        }
        return occurances < 3 && deck.Count < 15;
    }

    bool Check(int card) {
        return Check(CardStruct.determineCard(card));
    }
    
    #region UI Hooks
    public void SendMulligan() {
        player.SendMulligan();
    }

    public void SendReady() {
        player.SendReady();
    }

    public void StartGame() {

    }
    #endregion

    void Update() {
        if(player != null && !setup) {
            // Setup ready buttons
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length == 2) {
                foreach (GameObject go in players) {
                    Player pl = go.GetComponent<Player>();
                    if (pl == player) {
                        continue;
                    }
                    FindObjectsOfType<ReadyScript>()[1].player = pl;
                }
                FindObjectsOfType<ReadyScript>()[0].player = player;
                player.SetMulliganButton(MulliganButton);
                setup = true;
            }
        }
        RaycastHit hit;
#if UNITY_ANDROID
        //RaycastHit hit;
        for (int i = 0; i < Input.touchCount; ++i) {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began)) {
                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                if (Physics.Raycast(ray, out hit)) {
                    //hit.transform.gameObject.SendMessage("OnMouseDown");
                    try {
                        Card card = hit.collider.GetComponent<Card>();
                        if(card.interaction == Card.Interaction.Add)
                            AddCard(card);
                        if (card.interaction == Card.Interaction.Remove)
                            RemoveCard(card);
                        if (card.interaction == Card.Interaction.Select) {
                            card.owner.GetComponent<Player>().AddMulligan(card);
                        }
                        print(CardStruct.determineCard(card.cardEntry));
                    } catch { print("Not a card.."); };
                }
            }
        }
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //var select = GameObject.FindWithTag("select").transform;
            if (Physics.Raycast(ray, out hit, 100)) {
                //select.tag = "none";
                //hit.collider.transform.tag = "select";
                try {
                    Card card = hit.collider.GetComponent<Card>();
                    if(card.interaction == Card.Interaction.Add)
                        AddCard(card);
                    if (card.interaction == Card.Interaction.Remove)
                        RemoveCard(card);
                    if (card.interaction == Card.Interaction.Select) {
                        card.owner.GetComponent<Player>().AddMulligan(card);
                    }
                    print(CardStruct.determineCard(card.cardEntry));
                } catch { print("Not a card.."); };
            }
        }
#endif
    }

    public void PrintUsedCards() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        string s1 = "";
        string s2 = "";
        foreach (int i in players[0].GetComponent<Player>().GetUsedCards()) {
            s1 += " [" + i + "]";
        }
        foreach (int i in players[1].GetComponent<Player>().GetUsedCards()) {
            s2 += " [" + i + "]";
        }
        print("Player 1 used cards:" + s1 + ", " + players[0].GetComponent<Player>().GetUsedCards().Count);
        print("Player 2 used cards:" + s2 + ", " + players[1].GetComponent<Player>().GetUsedCards().Count);
    }
}
