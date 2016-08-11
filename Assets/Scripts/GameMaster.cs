using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameMaster : NetworkBehaviour {
    BoardScript GameBoard;

    Cards p1 = new Cards(7);
    Cards p2 = new Cards(7);
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

    public void StartGame() {

    }

    public void Matchup(Cards p1Hand, Cards p2Hand) {
        p1 = p1Hand;
        p2 = p2Hand;
        print("Deck matchup: ");
        printState(p1, p2);
        #region Primary Matchup
        int[] p1Primary = strengthArray(p1Hand);
        int[] p2Primary = strengthArray(p2Hand);

        p1.Renew(killStuff(p1Hand, p2Primary));
        p2.Renew(killStuff(p2Hand, p1Primary));
        #endregion
        print("Primary matchup: ");
        printState(p1, p2);

        #region Secondary Matchup
        int[] p1Secondary = strengthArray(p1, false);
        int[] p2Secondary = strengthArray(p2, false);

        p1.Renew(killStuff(p1, p2Secondary));
        p2.Renew(killStuff(p2, p1Secondary));
        #endregion
        print("Secondary matchup: ");
        printState(p1, p2);
    }

    void printState(Cards deck1, Cards deck2) {
        print("Player 1: " + deck1.ActiveCards());
        print("Player 2: " + deck2.ActiveCards());
    }

    int[] strengthArray(Cards hand, bool primary = true) {
        int[] strengths = new int[7];
        foreach (CardStruct card in hand) {
            if(!card.destroyed) {
                switch (primary ? card.strengthPrimary : card.strengthSecondary) {
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
                        print("No type, yo!");
                        continue;
                }
            }
        }
        return strengths;
    }

    #region Simulation functions
    public void DrawCards() {
        if(!GameBoard) {
            GameBoard = FindObjectOfType<BoardScript>();
        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
            go.GetComponent<Events>().SendCleanupDeck();
            go.GetComponent<Events>().SendDrawCards();
        }
        StartCoroutine(UpdateArt());
    }

    public void MatchupCurrent() {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Player");
        Cards[] playerCards = new Cards[go.Length];
        for (int i = 0; i < go.Length; i++)
            playerCards[i] = go[i].GetComponent<Player>().ActiveCards;
        Matchup(playerCards[0], playerCards[1]);
    }
    #endregion

    /// <summary>
    /// Refresh p1 and p2 for the sake of updating the cards on the table
    /// </summary>
    IEnumerator UpdateArt() {
        yield return new WaitForSeconds(1);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        p1 = players[0].GetComponent<Player>().ActiveCards;
        p2 = players[1].GetComponent<Player>().ActiveCards;
        GameBoard.UpdateHandCards(p1, p2);
    }

    List<CardStruct> killStuff(Cards hand, int[] opponentStrengths) {
        List<CardStruct> handiez = new List<CardStruct>(15);
        // In the future we might want to take every card 1 by 1 to make animations 'n' such
        //   or we could manage another array alongside (managing which card if more of same type) the opponentStrengths and then check for type to match against card
        foreach (CardStruct card in hand) {
            switch (card.type) {
                case CardStruct.CardType.Reptile:
                    if (opponentStrengths[0] > 0) {
                        handiez.Add(new CardStruct(card.Kill()));
                        opponentStrengths[0]--;
                    } else { handiez.Add(new CardStruct(card)); }
                    break;
                case CardStruct.CardType.Insect:
                    if (opponentStrengths[1] > 0) {
                        handiez.Add(new CardStruct(card.Kill()));
                        opponentStrengths[1]--;
                    } else { handiez.Add(new CardStruct(card)); }
                    break;
                case CardStruct.CardType.Avian:
                    if (opponentStrengths[2] > 0) {
                        handiez.Add(new CardStruct(card.Kill()));
                        opponentStrengths[2]--;
                    } else { handiez.Add(new CardStruct(card)); }
                    break;
                case CardStruct.CardType.Mammal:
                    if (opponentStrengths[3] > 0) {
                        handiez.Add(new CardStruct(card.Kill()));
                        opponentStrengths[3]--;
                    } else { handiez.Add(new CardStruct(card)); }
                    break;
                case CardStruct.CardType.Aquatic:
                    if (opponentStrengths[4] > 0) {
                        handiez.Add(new CardStruct(card.Kill()));
                        opponentStrengths[4]--;
                    } else { handiez.Add(new CardStruct(card)); }
                    break;
                case CardStruct.CardType.Plant:
                    if (opponentStrengths[5] > 0) {
                        handiez.Add(new CardStruct(card.Kill()));
                        opponentStrengths[5]--;
                    } else { handiez.Add(new CardStruct(card)); }
                    break;
                case CardStruct.CardType.Fungus:
                    if (opponentStrengths[6] > 0) {
                        handiez.Add(new CardStruct(card.Kill()));
                        opponentStrengths[6]--;
                    } else { handiez.Add(new CardStruct(card)); }
                    break;
                default:
                    handiez.Add(new CardStruct(card)); 
                    continue;
            }
        }
        return handiez;
    }
}
