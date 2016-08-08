using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Cards : SyncListStruct<CardStruct> {
    int Size;

    public Cards(int size) {
        Size = size;
    }

    public void AddCard(CardStruct c) {
        if(Count < Size)
            Add(c);
    }

    public override string ToString() {
        string s = "";
        foreach(CardStruct c in this) {
            s += "[" + CardStruct.determineCard(c.type) + "] ";
        }
        return s;
    }

    public string ActiveCards() {
        string s = "";
        foreach (CardStruct c in this) {
            s += "[" + (c.destroyed ?  " " : CardStruct.determineCard(c.type)) + "] ";
        }
        return s;
    }
}

public struct CardStruct { 
    public CardType type;
    public bool destroyed;
    public CardType strengthPrimary;
    public CardType strengthSecondary;
    // default defenses or special card with other defense in specific family?

    public enum CardType {
        Reptile, Insect, Avian, Mammal, Aquatic, Plant, Fungus, Unspecified
    }

    public static CardType determineCard(string s) {
        switch (s) {
            case "Reptile":
                return CardType.Reptile;
            case "Insect":
                return CardType.Insect;
            case "Avian":
                return CardType.Avian;
            case "Mammal":
                return CardType.Mammal;
            case "Aquatic":
                return CardType.Aquatic;
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
            case CardType.Avian:
                return "Avian";
            case CardType.Mammal:
                return "Mammal";
            case CardType.Aquatic:
                return "Aquatic";
            case CardType.Plant:
                return "Plant";
            case CardType.Fungus:
                return "Fungus";
            default:
                return "Unknown";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="myType">Card family type</param>
    public CardStruct(CardType myType) {
        type = myType;
        destroyed = false;
        strengthPrimary = CardType.Unspecified;
        strengthSecondary = CardType.Unspecified;

        // Apply default strengths
        strengthPrimary = defaultStrengthP(myType);
        strengthSecondary = defaultStrengthS(myType);
    }

    /// <summary>
    /// Define different strengths from the default
    /// </summary>
    /// <param name="type">Card family type</param>
    /// <param name="strengths">List of strengths (Primary@[0], Secondary@[1])</param>
    public CardStruct(CardType type, CardType[] strengths) {
        this.type = type;
        destroyed = false;
        strengthPrimary = strengths[0];
        if (strengths.Length > 1)
            strengthSecondary = strengths[0];
        else
            strengthSecondary = CardType.Unspecified;
        // Can be expanded for more than 2 strengths..
    }

    /// <summary>
    /// Card was countered and dies
    /// </summary>
    public void Kill() {
        destroyed = true;
    }

    /// <summary>
    /// Determine the default Primary strength of a card type
    /// Taken from type effectiveness matrix from 26/07/2016
    /// </summary>
    /// <param name="ctype">Card type</param>
    /// <returns>Default Primary strength of ctype</returns>
    CardType defaultStrengthP(CardType ctype) {
        switch(ctype) {
            case CardType.Reptile:
                return CardType.Insect;
            case CardType.Insect:
                return CardType.Mammal;
            case CardType.Avian:
                return CardType.Reptile;
            case CardType.Mammal:
                return CardType.Avian;
            case CardType.Aquatic:
                return CardType.Fungus;
            case CardType.Plant:
                return CardType.Aquatic;
            case CardType.Fungus:
                return CardType.Plant;
            default:
                return CardType.Unspecified;
        }
    }

    /// <summary>
    /// Determine the default Secondary strength of a card type
    /// Taken from type effectiveness matrix from 26/07/2016
    /// </summary>
    /// <param name="ctype">Card type</param>
    /// <returns>Default Secondary strength of ctype</returns>
    CardType defaultStrengthS(CardType ctype) {
        switch (ctype) {
            case CardType.Reptile:
                return CardType.Plant;
            case CardType.Insect:
                return CardType.Fungus;
            case CardType.Avian:
                return CardType.Aquatic;
            case CardType.Mammal:
                return CardType.Reptile;
            case CardType.Aquatic:
                return CardType.Insect;
            case CardType.Plant:
                return CardType.Mammal;
            case CardType.Fungus:
                return CardType.Avian;
            default:
                return CardType.Unspecified;
        }
    }
}
