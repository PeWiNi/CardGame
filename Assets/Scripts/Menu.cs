using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {
    //[SerializeField]
    public List<CardStruct.CardFamily> deck = new List<CardStruct.CardFamily>();
    public GameObject slot;
    //public Cards deck = new Cards(15);
    
    public void Start() {
        // Turn on the cards
        UnityEngine.Networking.NetworkIdentity[] uvs = Resources.FindObjectsOfTypeAll<UnityEngine.Networking.NetworkIdentity>();
        foreach (UnityEngine.Networking.NetworkIdentity ni in uvs) {
            if(ni.GetComponent<Card>())
                ni.gameObject.SetActive(true);
        }
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
        if (Check(card.type)) {
            deck.Add(card.type);
            if (slot) {
                float size = slot.GetComponentsInChildren<Card>().Length;
                GameObject go = Resources.Load<GameObject>("Prefabs/Card");
                go.GetComponent<Card>().type = card.type;
                //go.GetComponent<Card>().SetCardStruct(card.ToCardStruct());
                go.GetComponent<Card>().interaction = Card.Interaction.Remove;
                Instantiate(go, new Vector3(slot.transform.position.x + (0.85f * size), slot.transform.position.y, slot.transform.position.z + (-0.01f * size)), Quaternion.Euler(0, 180, 0), slot.transform);
            }
        }
    }

    public void RemoveCard(Card card) {
        deck.Remove(card.type);
        bool killed = false;
        if (slot) {
            foreach (Card c in slot.GetComponentsInChildren<Card>()) {
                if (c.type == card.type && !killed) {
                    Destroy(c.gameObject);
                    killed = true;
                    continue;
                } if (killed) c.transform.position -= new Vector3(.66f, 0, -0.01f);
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
                    if(card.interaction == Card.Interaction.Add)
                        AddCard(card);
                    if (card.interaction == Card.Interaction.Remove)
                        RemoveCard(card);
                    print(CardStruct.determineCard(card.type));
                } catch { print("Not a card.."); };
            }
        }
        //#endif
    }
}
