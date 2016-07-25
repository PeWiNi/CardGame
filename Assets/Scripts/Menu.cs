using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {
    //[SerializeField]
    public List<CardStruct.CardType> deck = new List<CardStruct.CardType>();
    //public Cards deck = new Cards(15);

    public void AddCard(string type) { // For UI
        CardStruct.CardType ct = CardStruct.determineCard(type);
        //if (Check(ct)) deck.Add(ct);
    }

    public void AddCard(Card card) { // For 3D
        if(Check(card.type)) deck.Add(card.type);
    }

    public List<CardStruct.CardType> GetDeck() {
        return deck;
    }

    bool Check(CardStruct.CardType card) {
        int occurances = 0;
        foreach(CardStruct.CardType c in deck) {
            if (c == card)
                occurances++;
        }
        return occurances < 3 && deck.Count < 15;
    }

    void Update() {
/*
#if UNITY_ANDROID
        RaycastHit hit;
        for (int i = 0; i < Input.touchCount; ++i) {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began)) {
                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                if (Physics.Raycast(ray, out hit)) {
                    hit.transform.gameObject.SendMessage("OnMouseDown");
                }
            }
        }
#endif
*/
//#if UNITY_EDITOR
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
                    print(CardStruct.determineCard(card.type));
                } catch { print("Not a card.."); };
            }
        }
//#endif
    }
}
