using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
/// Class for Cards shown in the game (Actual card functionality in Cards->CardStruct)
/// </summary>
public class Card : NetworkBehaviour {
    #region Should be a unique identifier for the card
    //[SyncVar(hook = "SyncCardStruct")]
    public CardStruct.CardType type; // Dummy card type
    [SerializeField]
    CardStruct cardStruct;
    #endregion

    public Interaction interaction = Interaction.Unspecified;
    bool dead = false;

    public enum Interaction {
        Select, Add, Remove, Unspecified
    }

    public CardStruct ToCardStruct() {
        return new CardStruct(type);
    }

    void Start() {
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Textures/" + CardStruct.determineCard(type).ToUpper());
    }

    void Update() {
        if (!dead && cardStruct.destroyed) {
            gameObject.GetComponent<MeshRenderer>().material.color -= new Color(0, 0, 0, .5f);
            dead = true;
        }
    }

    public void SetCardStruct(CardStruct ct) {
        cardStruct = ct;
        type = ct.type;
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Textures/" + CardStruct.determineCard(type).ToUpper());
    }
}
