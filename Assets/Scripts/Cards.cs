using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Cards : SyncListStruct<CardStruct> {
    int Size;

    public Cards(int size) {
        Size = size;
    }

    public void AddCard(CardStruct c) { } 
}

public struct CardStruct { 
    public CardType type;

    public enum CardType {
        Reptile, Insect, Bird, Mammal, Aqatic, Plant, Fungus, Unspecified
    }

    public static CardType determineCard(string s) {
        switch (s) {
            case "Reptile":
                return CardType.Reptile;
            case "Insect":
                return CardType.Insect;
            case "Bird":
                return CardType.Bird;
            case "Mammal":
                return CardType.Mammal;
            case "Aqatic":
                return CardType.Aqatic;
            case "Plant":
                return CardType.Plant;
            case "Fungus":
                return CardType.Fungus;
            default:
                return CardType.Unspecified;
        }
    }

    public static string determineCard(CardType s) {
        switch (s) {
            case CardType.Reptile:
                return "Reptile";
            case CardType.Insect:
                return "Insect";
            case CardType.Bird:
                return "Bird";
            case CardType.Mammal:
                return "Mammal";
            case CardType.Aqatic:
                return "Aqatic";
            case CardType.Plant:
                return "Plant";
            case CardType.Fungus:
                return "Fungus";
            default:
                return "Unknown";
        }
    }

    public CardStruct(CardType type) {
        this.type = type;
    }
}
