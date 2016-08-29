using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Class for Cards shown in the game (Actual card functionality in Cards->CardStruct)
/// </summary>
public class Card : NetworkBehaviour {
    #region Should be a unique identifier for the card
    //public CardStruct.CardFamily type; // Dummy card type
    public int cardEntry;
    [SerializeField][SyncVar]
    public GameObject owner;
    [SerializeField][SyncVar]
    public int listIndex;
    [SyncVar]
    Quaternion rotation = Quaternion.Euler(0, 180, 0);
    #endregion

    [SyncVar]
    // Interaction available for card
    public Interaction interaction = Interaction.Unspecified;
    // Current iteration of cards since game was started (used by BoardScript for cleanup)
    public int generation = 0;
    // Card is about to go to the graveyard
    public bool dead = false;
    // Card was clicked through Menu.cs
    public bool selected = false;
    // Opponent knows card, no reason to hide it anymore
    public bool revealed = false; // Not yet implemented

    public enum Interaction {
        Select, Add, Remove, Unspecified
    }

    public CardStruct ToCardStruct() {
        return new CardStruct(CardStruct.determineCard(cardEntry));
    }

    void Start() {
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/" + CardStruct.typeToString(CardStruct.determineCard(cardEntry)).ToUpper());
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
                if(owner.GetComponent<Player>().currentPhase == GameMaster.Phase.Mulligan || owner.GetComponent<Player>().currentPhase == GameMaster.Phase.Ready) {
                    bool bothReady = true;
                    foreach (GameObject pl in GameObject.FindGameObjectsWithTag("Player")) {
                        if (!bothReady || pl.GetComponent<Player>().currentPhase != GameMaster.Phase.Ready)
                            bothReady = false;
                    }
                    if (owner.GetComponent<NetworkIdentity>().isLocalPlayer && !bothReady && cardEntry != list.GetItem(listIndex).dataEntry) {
                        cardEntry = list.GetItem(listIndex).dataEntry;
                        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/" + CardStruct.typeToString(CardStruct.determineCard(cardEntry)).ToUpper());
                        return;
                    } else if (!owner.GetComponent<NetworkIdentity>().isLocalPlayer && !bothReady) {
                        cardEntry = 0;
                        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/" + CardStruct.typeToString(CardStruct.determineCard(cardEntry)).ToUpper());
                        return;
                    }
                    if (bothReady && !revealed) {
                        revealed = true;
                    }
                }
                if (cardEntry != list.GetItem(listIndex).dataEntry) {
                    cardEntry = list.GetItem(listIndex).dataEntry;
                    gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/" + CardStruct.typeToString(CardStruct.determineCard(cardEntry)).ToUpper());
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
        //type = ply.ActiveCards.GetItem(listIndex).family;
        cardEntry = ply.ActiveCards.GetItem(listIndex).dataEntry;
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/" + CardStruct.typeToString(CardStruct.determineCard(cardEntry)).ToUpper());
    }

    public void SetRotation(Quaternion rotation) {
        this.rotation = rotation;
    }
    /* // Not allowed to call Load this way ;-;
    // How to use: LookupFamilyForMaterial.First(sw => sw.Key(0)).Value;
    Dictionary<Func<int, bool>, Material> LookupFamilyForMaterial = new Dictionary<Func<int, bool>, Material>
            {
             { x => x ==  0 , Resources.Load<Material>("Materials/Backside") },
             { x => x <=  8 , Resources.Load<Material>("Materials/Fungus")   },  
             { x => x <= 16 , Resources.Load<Material>("Materials/Plant")    },
             { x => x <= 24 , Resources.Load<Material>("Materials/Insect")   },
             { x => x <= 32 , Resources.Load<Material>("Materials/Reptile")  },
             { x => x <= 40 , Resources.Load<Material>("Materials/Aquatic")  },
             { x => x <= 48 , Resources.Load<Material>("Materials/Avian")    },
             { x => x <= 56 , Resources.Load<Material>("Materials/Mammal")   } 
            };
    */
}
