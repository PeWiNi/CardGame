using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMockup : NetworkBehaviour {
    public bool player1 = false;

    public Cards deck1 = new Cards(15);
    public Cards deck2 = new Cards(15);

    // Use this for initialization
    void Start () {

    }
    public void AddCard(Card card) { // For 3D
        if (Check(card.type, deck1) &&  player1) deck1.Add(new CardStruct(card.type));
        if (Check(card.type, deck2) && !player1) deck2.Add(new CardStruct(card.type));
    }

    public void SetPlayer(bool p1) {
        player1 = p1;
    }

    public void PrintDeck() {
        print("Player 1: " + deck1.ToString());
        print("Player 2: " + deck2.ToString());
    }

    public void MockupMatchup() {
        GameObject.Find("GameMaster").GetComponent<GameMaster>().Matchup(deck1, deck2);
    }

    bool Check(CardStruct.CardType card, Cards deck) {
        int occurances = 0;
        foreach (CardStruct c in deck) {
            if (c.type == card)
                occurances++;
        }
        return occurances < 3 && deck.Count < 15;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //var select = GameObject.FindWithTag("select").transform;
            if (Physics.Raycast(ray, out hit, 100)) {
                //select.tag = "none";
                //hit.collider.transform.tag = "select";
                try {
                    Card card = hit.collider.GetComponent<Card>();
                    AddCard(card);
                }
                catch { print("Not a card.."); };
            }
        }
    }
}
