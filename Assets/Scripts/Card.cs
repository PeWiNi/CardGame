using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {
    public CardStruct.CardType type;

    public CardStruct ToCardStruct() {
        return new CardStruct(type);
    }
}
