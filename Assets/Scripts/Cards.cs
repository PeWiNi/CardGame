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
    public bool revealed;
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

    /// <summary>
    /// Converts a Cardfamily to string (same functionality as .ToString())
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
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
    /// Method mapping integers to Families(/Species)
    /// 
    /// Data fetched from design dokument (29/08/2016 12:18)
    /// https://docs.google.com/document/d/11zGSpZZbW72u2k0DGqssqfsTA2DEX0JAsIaGNO1bgRw/edit#
    /// </summary>
    /// <param name="index">Identity of the card</param>
    /// <returns>Family to which the index belong</returns>
    public static CardFamily determineCard(int index) { // To be expanded to determining species
        switch (index) {
            case 25: goto case -1; // Salamander (Uncommon)
            case 26: goto case -1; // House Snake (Common)
            case 27: goto case -1; // Chameleon (Uncommon)
            case 28: goto case -1; // Turtle (Common)
            case 29: goto case -1; // Frog (Common)
            case 30: goto case -1; // Komodo Dragon (Rare,Epic,Legendary)
            case 31: goto case -1; // Cobra (Rare,Epic,Legendary)
            case 32: goto case -1; // Crocodile (Rare,Epic,Legendary)
            case -1:
                return CardFamily.Reptile;
            case 17: goto case -2; // Ladybug (Common)
            case 18: goto case -2; // Butterfly (Common)
            case 19: goto case -2; // Bee (Common)
            case 20: goto case -2; // Stag Beetle (Uncommon)
            case 21: goto case -2; // Ant (Uncommon)
            case 22: goto case -2; // Mantis (Rare,Epic,Legendary)
            case 23: goto case -2; // Dragonfly (Rare,Epic,Legendary)
            case 24: goto case -2; // Firefly (Rare,Epic,Legendary)
            case -2:
                return CardFamily.Insect;
            case 41: goto case -3; // Turkey (Common)
            case 42: goto case -3; // Sparrow (Common)
            case 43: goto case -3; // Crow (Uncommon)
            case 44: goto case -3; // Pigeon (Common)
            case 45: goto case -3; // Seagull (Uncommon)
            case 46: goto case -3; // Penguin
            case 47: goto case -3; // Eagle
            case 48: goto case -3; // Toucan
            case -3:
                return CardFamily.Avian;
            case 49: goto case -4; // Cow (Common)
            case 50: goto case -4; // Dog/Wolf (Uncommon)
            case 51: goto case -4; // Squirrel (Common)
            case 52: goto case -4; // Bear (Uncommon)
            case 53: goto case -4; // Moose (Common)
            case 54: goto case -4; // Platypus (Rare,Epic,Legendary)
            case 55: goto case -4; // Monkey (Rare,Epic,Legendary)
            case 56: goto case -4; // Honey Badger (Rare,Epic,Legendary)
            case -4:
                return CardFamily.Mammal;
            case 33: goto case -5; // Clown Fish (Common)
            case 34: goto case -5; // Puffer Fish (Common)
            case 35: goto case -5; // Starfish (Uncommon)
            case 36: goto case -5; // Seahorse (Uncommon)
            case 37: goto case -5; // Whale (Common)
            case 38: goto case -5; // Shark (Rare,Epic,Legendary)
            case 39: goto case -5; // Narwhal (Rare,Epic,Legendary)
            case 40: goto case -5; // Dolphin (Rare,Epic,Legendary)
            case -5:
                return CardFamily.Aquatic;
            case 09: goto case -6; // Oak Tree (Common)
            case 10: goto case -6; // Sunflower (Common)
            case 11: goto case -6; // Cactus (Uncommon)
            case 12: goto case -6; // Pine Tree (Uncommon)
            case 13: goto case -6; // Fern (Common)
            case 14: goto case -6; // Algae (Rare,Epic,Legendary)
            case 15: goto case -6; // Flytrap (Rare,Epic,Legendary)
            case 16: goto case -6; // Mistletoe (Rare,Epic,Legendary)
            case -6:
                return CardFamily.Plant; 
            case 01: goto case -7; // Mushroom (Common)
            case 02: goto case -7; // Lichen (Uncommon)
            case 03: goto case -7; // Spore (Uncommon)
            case 04: goto case -7; // Truffle (Rare,Epic,Legendary)
            case 05: goto case -7; // Yeast (Common)
            case 06: goto case -7; // Coral (Rare,Epic,Legendary)
            case 07: goto case -7; // Mold (Common)
            case 08: goto case -7; // Puffball (Rare,Epic,Legendary)
            case -7:
                return CardFamily.Fungus;
            default: goto case 0;
            case 0:
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
        revealed = false;
        strengthPrimary = CardFamily.Unspecified;
        strengthSecondary = CardFamily.Unspecified;
        rarity = Rarity.Unspecified;

        dataEntry = determineCard(myFamily);
        // Apply default strengths
        strengthPrimary = defaultStrengthP(myFamily);
        strengthSecondary = defaultStrengthS(myFamily);
    }

    public CardStruct(int index) {
        family = CardFamily.Unspecified;
        dataEntry = index;
        destroyed = false;
        revealed = false;
        strengthPrimary = CardFamily.Unspecified;
        strengthSecondary = CardFamily.Unspecified;
        rarity = Rarity.Unspecified;

        family = determineCard(index);
        // Apply default strengths
        strengthPrimary = defaultStrengthP(family);
        strengthSecondary = defaultStrengthS(family);
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
        revealed = false;
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
        revealed = cs.revealed;
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
    public CardStruct Reveal() {
        revealed = true;
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
