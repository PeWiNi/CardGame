﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameMaster : NetworkBehaviour {
    /// <summary>
    /// 2 Players
    /// Each with a Deck of 15 Cards
    ///     A Deck can contain a max of 3 copies of a Card
    /// 7 Types of Cards
    /// Each Card beats 2 other types (Primary and Secondary) and can be beaten by 2 other types
    /// </summary>
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MatchupPrimary(Cards p1Hand, Cards p2Hand) {
        int[] p1Primary = strengthArray(p1Hand);
        int[] p2Primary = strengthArray(p2Hand);
        Cards p1 = killStuff(p1Hand, p2Primary);
        Cards p2 = killStuff(p2Hand, p1Primary);
    }

    int[] strengthArray(Cards hand) {
        int[] strengths = new int[7];
        foreach (CardStruct card1 in hand) {
            switch (card1.strengthPrimary) {
                case CardStruct.CardType.Reptile:
                    strengths[0]++;
                    break;
                case CardStruct.CardType.Insect:
                    strengths[1]++;
                    break;
                case CardStruct.CardType.Avian:
                    strengths[2]++;
                    break;
                case CardStruct.CardType.Mammal:
                    strengths[3]++;
                    break;
                case CardStruct.CardType.Aquatic:
                    strengths[4]++;
                    break;
                case CardStruct.CardType.Plant:
                    strengths[5]++;
                    break;
                case CardStruct.CardType.Fungus:
                    strengths[6]++;
                    break;
                default:
                    continue;
            }
        }
        return strengths;
    }

    Cards killStuff(Cards hand, int[] opponentStrengths) {
        foreach (CardStruct card in hand) {
            switch (card.type) {
                case CardStruct.CardType.Reptile:
                    if (opponentStrengths[0] > 0) card.Kill();
                    opponentStrengths[0]--;
                    break;
                case CardStruct.CardType.Insect:
                    if (opponentStrengths[1] > 0) card.Kill();
                    opponentStrengths[1]--;
                    break;
                case CardStruct.CardType.Avian:
                    if (opponentStrengths[2] > 0) card.Kill();
                    opponentStrengths[2]--;
                    break;
                case CardStruct.CardType.Mammal:
                    if (opponentStrengths[3] > 0) card.Kill();
                    opponentStrengths[3]--;
                    break;
                case CardStruct.CardType.Aquatic:
                    if (opponentStrengths[4] > 0) card.Kill();
                    opponentStrengths[4]--;
                    break;
                case CardStruct.CardType.Plant:
                    if (opponentStrengths[5] > 0) card.Kill();
                    opponentStrengths[5]--;
                    break;
                case CardStruct.CardType.Fungus:
                    if (opponentStrengths[6] > 0) card.Kill();
                    opponentStrengths[6]--;
                    break;
                default:
                    continue;
            }
        }
        return hand;
    }
}
