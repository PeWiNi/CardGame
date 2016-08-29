using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
/// Class for Cards shown in the game (Actual card functionality in Cards->CardStruct)
/// </summary>
public class Card : NetworkBehaviour {
    #region Should be a unique identifier for the card
    //[SyncVar(hook = "SyncCardStruct")]
    public CardStruct.CardFamily type; // Dummy card type
    [SerializeField][SyncVar]
    public GameObject owner;
    //public Cards list = new Cards(0); // Not syncronized with clients (reference lost over network)
    [SerializeField][SyncVar]
    public int listIndex;
    [SyncVar]
    Quaternion rotation = Quaternion.Euler(0, 180, 0);
    #endregion

    [SyncVar]
    public Interaction interaction = Interaction.Unspecified;
    public int generation = 0;
    public bool dead = false;
    public bool selected = false;

    public enum Interaction {
        Select, Add, Remove, Unspecified
    }

    public CardStruct ToCardStruct() {
        return new CardStruct(type);
    }

    void Start() {
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/" + CardStruct.determineCard(type).ToUpper());
        transform.rotation = rotation;
    }

    void Update() {
        if(owner) {
            Cards list = owner.GetComponent<Player>().ActiveCards;
            if (list.Size > 0) {
                if (list.Count <= listIndex) {
                    gameObject.SetActive(false);
                    return;
                }
                if (type != list.GetItem(listIndex).family) {
                    type = list.GetItem(listIndex).family;
                    gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/" + CardStruct.determineCard(type).ToUpper());
                }
                if (!dead && list.GetItem(listIndex).destroyed) {
                    gameObject.GetComponent<MeshRenderer>().material.color -= new Color(0, 1, 1, .5f);
                    dead = true;
                }
                if (dead && !list.GetItem(listIndex).destroyed) {
                    //gameObject.GetComponent<MeshRenderer>().material.color += new Color(0, 1, 1, .5f);
                    dead = false;
                }
                if(selected) {
                    gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0, 1);
                } if(gameObject.GetComponent<MeshRenderer>().material.color == new Color(0, 1, 0, 1) && !selected) {
                    gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1);
                }
            }
        }
    }

    public void SetReference(GameObject player, int index) {
        owner = player;
        Player ply = owner.GetComponent<Player>();
        listIndex = index;
        type = ply.ActiveCards.GetItem(listIndex).family;
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/" + CardStruct.determineCard(type).ToUpper());
    }

    public void SetRotation(Quaternion rotation) {
        this.rotation = rotation;
    }
}
