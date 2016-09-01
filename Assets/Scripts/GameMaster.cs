using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameMaster : NetworkBehaviour {
    BoardScript GameBoard;

    Cards p1 = new Cards(7);
    Cards p2 = new Cards(7);

    public enum Phase {
        Startup, Mulligan, Ready, PrimaryMatchup, SecondaryMatchup, BonusMatchup, Aftermath
    }

    public int mulliganCardCount = 3;
    /// <summary>
    /// 2 Players
    /// Each with a Deck of 15 Cards
    ///     A Deck can contain a max of 3 copies of a Card
    /// 7 Types of Cards
    /// Each Card beats 2 other types (Primary and Secondary) and can be beaten by 2 other types
    /// </summary>
    // Use this for initialization
    void Start () {
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
            p.GetComponent<Events>().SendPhaseChange(Phase.Startup);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame() {

    }

    public void Matchup(Cards p1Hand, Cards p2Hand) {
        print("Deck matchup: ");
        printState(p1Hand, p2Hand);
        #region Primary Matchup
        //foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player")) p.GetComponent<Events>().SendPhaseChange(Phase.PrimaryMatchup);
        int[] p1Primary = strengthArray(p1Hand);
        int[] p2Primary = strengthArray(p2Hand);

        p1Hand.Renew(killStuff(p1Hand, p2Primary));
        p2Hand.Renew(killStuff(p2Hand, p1Primary));
        #endregion
        print("Primary matchup: ");
        printState(p1Hand, p2Hand);

        #region Secondary Matchup
        //foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player")) p.GetComponent<Events>().SendPhaseChange(Phase.SecondaryMatchup);
        int[] p1Secondary = strengthArray(p1Hand, false);
        int[] p2Secondary = strengthArray(p2Hand, false);

        p1Hand.Renew(killStuff(p1, p2Secondary));
        p2Hand.Renew(killStuff(p2, p1Secondary));
        #endregion
        print("Secondary matchup: ");
        printState(p1Hand, p2Hand);

        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
            p.GetComponent<Events>().SendPhaseChange(Phase.Aftermath);
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
                    case CardStruct.CardFamily.Reptile:
                        strengths[0]++;
                        break;
                    case CardStruct.CardFamily.Insect:
                        strengths[1]++;
                        break;
                    case CardStruct.CardFamily.Avian:
                        strengths[2]++;
                        break;
                    case CardStruct.CardFamily.Mammal:
                        strengths[3]++;
                        break;
                    case CardStruct.CardFamily.Aquatic:
                        strengths[4]++;
                        break;
                    case CardStruct.CardFamily.Plant:
                        strengths[5]++;
                        break;
                    case CardStruct.CardFamily.Fungus:
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
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        p1 = players[0].GetComponent<Player>().ActiveCards;
        p2 = players[1].GetComponent<Player>().ActiveCards;
        //foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
            //go.GetComponent<Events>().SendCleanupDeck();
            CleanUp(players[0].GetComponent<Player>());
            CleanUp(players[1].GetComponent<Player>());
            //go.GetComponent<Events>().SendDrawCards();
            DrawCards(players[0].GetComponent<Player>());
            DrawCards(players[1].GetComponent<Player>());

            //GameBoard.GetComponent<Events>().SendUpdateCards(p1, p2); //Recieve event somewhere
            //go.GetComponent<Events>().SendUpdateCards(p1, p2); //Recieve event somewhere
        //}
        // ToDo: mulligan before updating art (at least for both players)
        StartCoroutine(UpdateArt());
        MulliganPhase(true);
    }

    private void DrawCards(Player player) {
        //print("I am gonna go in there");
        while (player.ActiveCards.Count < player.ActiveCards.Size && player.ActiveCards.Count < player.Deck.Count - player.Graveyard.Count) {
            //print("I was here!");
            int rnd = Random.Range(0, player.Deck.Count);
            if (!player.GetUsedCards().Contains(rnd)) {
                player.GetUsedCards().Add(rnd);
                //print("Card #" + rnd + " is being added " + (rng.Count > ActiveCards.Count) + ", total count: " + rng.Count);
                if (player.GetUsedCards().Count > player.ActiveCards.Count) {
                    player.ActiveCards.Add(player.Deck.GetItem(rnd));
                }
            }
        }
    }

    private void CleanUp(Player player) {
        List<CardStruct> temp = new List<CardStruct>(15);
        foreach (CardStruct ct in player.ActiveCards) {
            if (ct.destroyed) {
                player.Deck.Remove(ct);
                player.Graveyard.AddCard(ct);
            } else {
                temp.Add(ct);
            }
        }
        player.ActiveCards.Renew(temp);
    }

    public void MulliganPhase(bool start) {
        foreach (GameObject pl in GameObject.FindGameObjectsWithTag("Player")) {
            pl.GetComponent<Events>().SendPhaseChange(Phase.Mulligan);
            if (start) {
                pl.GetComponent<Player>().SetUpMulligan(mulliganCardCount);
            }
            else {
                pl.GetComponent<Player>().StopMulligan();
                StartCoroutine(UpdateArt());
            }
        }
    }

    /// <summary>
    /// Method for populating the ActiveCards (hand) of a given player
    /// It creates a placeholder list for all the available cards (Deck - ActiveCards - Graveyard)
    /// Checks if more cards are mulligan'ed than available (and then repopulates the placeholder list with mulligan'ed cards)
    /// Fills up ActiveCards (hand) of the player
    /// </summary>
    /// <param name="player">Player who mulligan'ed</param>
    public void Mulligan(Player player) {
        List<CardStruct> handiez = new List<CardStruct>(15);
        foreach (CardStruct c in player.Deck) { // Populate the list with the full deck
            handiez.Add(c);
            //print("Adding : Handiez contains #" + handiez.Count + " cards");
        } foreach (CardStruct c in player.ActiveCards) { // Remove the current hand (so the cards won't get re-added when mulligan'ing)
            handiez.Remove(c);
            //print("Removing (ActiveCards) : Handiez contains #" + handiez.Count + " cards");
        } foreach (CardStruct c in player.Graveyard) { // Remove the dead cards
            CardStruct cs = c;
            cs.destroyed = false;
            handiez.Remove(cs);
            //print("Removing (Graveyard) : Handiez contains #" + handiez.Count + " cards");
        }
        print("There are #" + handiez.Count + " cards available for mulligan, they are " + printList(handiez));
        print("Hand before Mulligan: " + player.ActiveCards.ToString() + ", Cards Mulligan'ed: " + player.Mulligan);
        foreach (CardStruct c in player.Mulligan) { // Remove the mulligan'ed cards from the hand
            player.ActiveCards.Remove(c);
        }
        print("ActiveHands trimmed: " + player.ActiveCards.Count + ", Mulligan'ed cards: " + player.Mulligan.Count + ", the sum of the two should be 7 (or maximum available cards)");
        if (handiez.Count < player.Mulligan.Count) { // Add cards from the list only containing unused and alive cards from the Deck
            foreach (CardStruct c in player.Mulligan) {
                handiez.Add(c);
            }
        } if (handiez.Count > 0) { // Add mulligan'ed cards to the list, since more cards were mulligan'ed than left in the list
            for (int i = 0; i < player.Mulligan.Count; i++) {
                player.ActiveCards.AddCard(handiez[i]);
            }
        }
        print("ActiveHands is full again " + player.ActiveCards.Count);
        print("Hand after Mulligan: " + player.ActiveCards.ToString() + ", Cards Mulligan'ed: " + player.Mulligan);
        player.Mulligan.Clear();

        player.GetComponent<Events>().SendPhaseChange(Phase.Ready);
        StartCoroutine(UpdateArt());
    }

    string printList(List<CardStruct> list) {
        string s = "";
        foreach (CardStruct c in list) {
            s += "[" + c.ToString() + "] ";
        }
        return s;
    }

    /// <summary>
    /// Simulation function
    /// Pits the ActiveCards of players in the scene against one another using Matchup(Cards, Cards)
    /// </summary>
    public void MatchupCurrent() {
        MulliganPhase(false);
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
        yield return new WaitForSeconds(.1f);
        print(".1 sec has passed and UpdateArt can proceed");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        p1 = players[0].GetComponent<Player>().ActiveCards;
        p2 = players[1].GetComponent<Player>().ActiveCards;
        //GameBoard.UpdateHandCards(p1, p2);
        GameBoard.UpdateHandCards(players[0], players[1]);
    }

    /// <summary>
    /// Switch-statement for determining what cards must die in the hand
    /// Works by matching a int-array that can be mapped to every CardFamily
    /// This int-array will have a number that determines how many cards of a family that should be destroyed
    /// </summary>
    /// <param name="hand">The victim who will loose cards determined by opponentsStrengths</param>
    /// <param name="opponentStrengths">The CardFamilies and count for what needs to be killed</param>
    /// <returns></returns>
    List<CardStruct> killStuff(Cards hand, int[] opponentStrengths) {
        List<CardStruct> handiez = new List<CardStruct>(15);
        // In the future we might want to take every card 1 by 1 to make animations 'n' such
        //   or we could manage another array alongside (managing which card if more of same type) the opponentStrengths and then check for type to match against card
        foreach (CardStruct card in hand) {
            switch (card.family) {
                case CardStruct.CardFamily.Reptile:
                    if (opponentStrengths[0] > 0) {
                        handiez.Add(new CardStruct(card.Kill()));
                        opponentStrengths[0]--;
                    } else { handiez.Add(new CardStruct(card)); }
                    break;
                case CardStruct.CardFamily.Insect:
                    if (opponentStrengths[1] > 0) {
                        handiez.Add(new CardStruct(card.Kill()));
                        opponentStrengths[1]--;
                    } else { handiez.Add(new CardStruct(card)); }
                    break;
                case CardStruct.CardFamily.Avian:
                    if (opponentStrengths[2] > 0) {
                        handiez.Add(new CardStruct(card.Kill()));
                        opponentStrengths[2]--;
                    } else { handiez.Add(new CardStruct(card)); }
                    break;
                case CardStruct.CardFamily.Mammal:
                    if (opponentStrengths[3] > 0) {
                        handiez.Add(new CardStruct(card.Kill()));
                        opponentStrengths[3]--;
                    } else { handiez.Add(new CardStruct(card)); }
                    break;
                case CardStruct.CardFamily.Aquatic:
                    if (opponentStrengths[4] > 0) {
                        handiez.Add(new CardStruct(card.Kill()));
                        opponentStrengths[4]--;
                    } else { handiez.Add(new CardStruct(card)); }
                    break;
                case CardStruct.CardFamily.Plant:
                    if (opponentStrengths[5] > 0) {
                        handiez.Add(new CardStruct(card.Kill()));
                        opponentStrengths[5]--;
                    } else { handiez.Add(new CardStruct(card)); }
                    break;
                case CardStruct.CardFamily.Fungus:
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
