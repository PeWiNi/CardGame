﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Class for Cards shown in the game (Actual card functionality in Cards->CardStruct)
/// </summary>
public class Card : MonoBehaviour {
    public CardStruct.CardType type;

    public CardStruct ToCardStruct() {
        return new CardStruct(type);
    }

    void Start() {
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Textures/" + CardStruct.determineCard(type).ToUpper());
    }
}
