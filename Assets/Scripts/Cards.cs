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
            s += "[" + c.ToString() + "] ";
        }
        return s;
    }

    public string ActiveCards() {
        string s = "";
        foreach (CardStruct c in this) {
            s += "[" + (c.destroyed ?  " " : c.ToString()) + "] ";
        }
        return s;
    }
}

public struct CardStruct { 
    public CardFamily family;
    public int dataEntry;
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

    public static int determineCard(CardFamily s) {
        switch (s) {
            case CardFamily.Reptile:
                return 25;
            case CardFamily.Insect:
                return 17;
            case CardFamily.Avian:
                return 41;
            case CardFamily.Mammal:
                return 49;
            case CardFamily.Aquatic:
                return 33;
            case CardFamily.Plant:
                return 9;
            case CardFamily.Fungus:
                return 1;
            default:
                return 0;
        }
    }

    public static string typeToString(CardFamily s) {
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
                return "Backside";
        }
    }

    /// <summary>
    /// Data fetched from design dokument (29/08/2016 12:18)
    /// https://docs.google.com/document/d/11zGSpZZbW72u2k0DGqssqfsTA2DEX0JAsIaGNO1bgRw/edit#
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static CardFamily determineCard(int s) {
        switch (s) {
            case 25: // Salamander (Uncommon)
            case 26: // House Snake (Common)
            case 27: // Chameleon (Uncommon)
            case 28: // Turtle (Common)
            case 29: // Frog (Common)
            case 30: // Komodo Dragon (Rare,Epic,Legendary)
            case 31: // Cobra (Rare,Epic,Legendary)
            case 32: // Crocodile (Rare,Epic,Legendary)
                return CardFamily.Reptile;
            case 17: // Ladybug (Common)
            case 18: // Butterfly (Common)
            case 19: // Bee (Common)
            case 20: // Stag Beetle (Uncommon)
            case 21: // Ant (Uncommon)
            case 22: // Mantis (Rare,Epic,Legendary)
            case 23: // Dragonfly (Rare,Epic,Legendary)
            case 24: // Firefly (Rare,Epic,Legendary)
                return CardFamily.Insect;
            case 41: // Turkey (Common)
            case 42: // Sparrow (Common)
            case 43: // Crow (Uncommon)
            case 44: // Pigeon (Common)
            case 45: // Seagull (Uncommon)
            case 46: // Penguin
            case 47: // Eagle
            case 48: // Toucan
                return CardFamily.Avian;
            case 49: // Cow (Common)
            case 50: // Dog/Wolf (Uncommon)
            case 51: // Squirrel (Common)
            case 52: // Bear (Uncommon)
            case 53: // Moose (Common)
            case 54: // Platypus (Rare,Epic,Legendary)
            case 55: // Monkey (Rare,Epic,Legendary)
            case 56: // Honey Badger (Rare,Epic,Legendary)
                return CardFamily.Mammal;
            case 33: // Clown Fish (Common)
            case 34: // Puffer Fish (Common)
            case 35: // Starfish (Uncommon)
            case 36: // Seahorse (Uncommon)
            case 37: // Whale (Common)
            case 38: // Shark (Rare,Epic,Legendary)
            case 39: // Narwhal (Rare,Epic,Legendary)
            case 40: // Dolphin (Rare,Epic,Legendary)
                return CardFamily.Aquatic;
            case 9: // Oak Tree (Common)
            case 10: // Sunflower (Common)
            case 11: // Cactus (Uncommon)
            case 12: // Pine Tree (Uncommon)
            case 13: // Fern (Common)
            case 14: // Algae (Rare,Epic,Legendary)
            case 15: // Flytrap (Rare,Epic,Legendary)
            case 16: // Mistletoe (Rare,Epic,Legendary)
                return CardFamily.Plant; 
            case 1: // Mushroom (Common)
            case 2: // Lichen (Uncommon)
            case 3: // Spore (Uncommon)
            case 4: // Truffle (Rare,Epic,Legendary)
            case 5: // Yeast (Common)
            case 6: // Coral (Rare,Epic,Legendary)
            case 7: // Mold (Common)
            case 8: // Puffball (Rare,Epic,Legendary)
                return CardFamily.Fungus;
            case 0:
            default:
                return CardFamily.Unspecified;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="myFamily">Card family type</param>
    public CardStruct(CardFamily myFamily) {
        family = myFamily;
        dataEntry = 0;
        destroyed = false;
        strengthPrimary = CardFamily.Unspecified;
        strengthSecondary = CardFamily.Unspecified;
        rarity = Rarity.Unspecified;

        dataEntry = determineCard(myFamily);
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
        family = myFamily;
        dataEntry = 0;
        destroyed = false;
        strengthPrimary = strengths[0];
        rarity = Rarity.Unspecified;

        dataEntry = determineCard(myFamily);
        if (strengths.Length > 1)
            strengthSecondary = strengths[0];
        else
            strengthSecondary = CardFamily.Unspecified;
        // Can be expanded for more than 2 strengths..
    }

    public CardStruct(CardStruct cs) {
        family = cs.family;
        dataEntry = cs.dataEntry;
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
        switch (family) {
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
}
