using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Cards : SyncListStruct<CardStruct> {
    public int Size;
    
    public void Renew(List<CardStruct> lst) {
        Clear();
        lst.ForEach((item) => {
            Add(new CardStruct(item));
        });
    }

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
            s += "[" + CardStruct.determineCard(c.family) + "] ";
        }
        return s;
    }

    public string ActiveCards() {
        string s = "";
        foreach (CardStruct c in this) {
            s += "[" + (c.destroyed ?  " " : CardStruct.determineCard(c.family)) + "] ";
        }
        return s;
    }
}

public struct CardStruct { 
    public CardFamily family;
    public int type;
    public bool destroyed;
    public CardFamily strengthPrimary;
    public CardFamily strengthSecondary;
    public Rarity rarity;
    // default defenses or special card with other defense in specific family?

    public enum CardFamily {
        Reptile, Insect, Avian, Mammal, Aquatic, Plant, Fungus, Unspecified
    }
    public enum Rarity {
        Common, Uncommon, Rare, Epic, Legendary, Unspecified
    }

    public static CardFamily RandomCardType() {
        int rnd = Random.Range(0, 7);
        switch (rnd) {
            case 0:
                return CardFamily.Reptile;
            case 1:
                return CardFamily.Insect;
            case 2:
                return CardFamily.Avian;
            case 3:
                return CardFamily.Mammal;
            case 4:
                return CardFamily.Aquatic;
            case 5:
                return CardFamily.Plant;
            case 6:
                return CardFamily.Fungus;
            default:
                return CardFamily.Unspecified;
        }
    }

    public static CardFamily determineCard(string s) {
        switch (s) {
            case "Reptile":
                return CardFamily.Reptile;
            case "Insect":
                return CardFamily.Insect;
            case "Avian":
                return CardFamily.Avian;
            case "Mammal":
                return CardFamily.Mammal;
            case "Aquatic":
                return CardFamily.Aquatic;
            case "Plant":
                return CardFamily.Plant;
            case "Fungus":
                return CardFamily.Fungus;
            default:
                return CardFamily.Unspecified;
        }
    }

    public static string determineCard(CardFamily s) {
        switch (s) {
            case CardFamily.Reptile:
                return "Reptile";
            case CardFamily.Insect:
                return "Insect";
            case CardFamily.Avian:
                return "Avian";
            case CardFamily.Mammal:
                return "Mammal";
            case CardFamily.Aquatic:
                return "Aquatic";
            case CardFamily.Plant:
                return "Plant";
            case CardFamily.Fungus:
                return "Fungus";
            default:
                return "Unknown";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="myFamily">Card family type</param>
    public CardStruct(CardFamily myFamily) {
        family = myFamily;
        type = 0;
        destroyed = false;
        strengthPrimary = CardFamily.Unspecified;
        strengthSecondary = CardFamily.Unspecified;
        rarity = Rarity.Unspecified;

        // Apply default strengths
        strengthPrimary = defaultStrengthP(myFamily);
        strengthSecondary = defaultStrengthS(myFamily);
    }

    /// <summary>
    /// Define different strengths from the default
    /// </summary>
    /// <param name="myFamily">Card family type</param>
    /// <param name="strengths">List of strengths (Primary@[0], Secondary@[1])</param>
    public CardStruct(CardFamily myFamily, CardFamily[] strengths) {
        this.family = myFamily;
        type = 0;
        destroyed = false;
        strengthPrimary = strengths[0];
        rarity = Rarity.Unspecified;

        if (strengths.Length > 1)
            strengthSecondary = strengths[0];
        else
            strengthSecondary = CardFamily.Unspecified;
        // Can be expanded for more than 2 strengths..
    }

    public CardStruct(CardStruct cs) {
        family = cs.family;
        type = cs.type;
        destroyed = cs.destroyed;
        strengthPrimary = cs.strengthPrimary;
        strengthSecondary = cs.strengthSecondary;
        rarity = cs.rarity;
    }

    /// <summary>
    /// Card was countered and dies
    /// </summary>
    public CardStruct Kill() {
        destroyed = true;
        return this;
    }

    /// <summary>
    /// Determine the default Primary strength of a card type
    /// Taken from type effectiveness matrix from 26/07/2016
    /// </summary>
    /// <param name="ctype">Card type</param>
    /// <returns>Default Primary strength of ctype</returns>
    CardFamily defaultStrengthP(CardFamily ctype) {
        switch(ctype) {
            case CardFamily.Reptile:
                return CardFamily.Insect;
            case CardFamily.Insect:
                return CardFamily.Mammal;
            case CardFamily.Avian:
                return CardFamily.Reptile;
            case CardFamily.Mammal:
                return CardFamily.Avian;
            case CardFamily.Aquatic:
                return CardFamily.Fungus;
            case CardFamily.Plant:
                return CardFamily.Aquatic;
            case CardFamily.Fungus:
                return CardFamily.Plant;
            default:
                return CardFamily.Unspecified;
        }
    }

    /// <summary>
    /// Determine the default Secondary strength of a card type
    /// Taken from type effectiveness matrix from 26/07/2016
    /// </summary>
    /// <param name="ctype">Card type</param>
    /// <returns>Default Secondary strength of ctype</returns>
    CardFamily defaultStrengthS(CardFamily ctype) {
        switch (ctype) {
            case CardFamily.Reptile:
                return CardFamily.Plant;
            case CardFamily.Insect:
                return CardFamily.Fungus;
            case CardFamily.Avian:
                return CardFamily.Aquatic;
            case CardFamily.Mammal:
                return CardFamily.Reptile;
            case CardFamily.Aquatic:
                return CardFamily.Insect;
            case CardFamily.Plant:
                return CardFamily.Mammal;
            case CardFamily.Fungus:
                return CardFamily.Avian;
            default:
                return CardFamily.Unspecified;
        }
    }

    public override string ToString() {
        return determineCard(family);
    }
}
