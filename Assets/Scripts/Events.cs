using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Events : NetworkBehaviour {
    public delegate void Mulligan(List<CardStruct> cards);
    [SyncEvent]
    public event Mulligan EventMulligan;

    public delegate void UpdateCards(Player p1, Player p2);
    [SyncEvent]
    public event UpdateCards EventUpdateCards;

    public delegate void PhaseChange(GameMaster.Phase phase);
    [SyncEvent]
    public event PhaseChange EventPhaseChange;

    public delegate void EndState(string str);
    [SyncEvent]
    public event EndState EventEnd;

    public void SendMulligan(List<CardStruct> cards) { // Unused, happens through GameMaster and a integer value on Player
        if (EventMulligan != null)
            EventMulligan(cards);
    }

    public void SendUpdateCards(Player p1, Player p2) { // Unused, GameMaster controls when (and what) cards are drawn and the SyncListStruct handles the rest
        if (EventUpdateCards != null)
            EventUpdateCards(p1, p2);
    }

    // Used to change the state for players (namely whether or not they can see the opponent's cards (Mulligan))
    public void SendPhaseChange(GameMaster.Phase phase) {
        if (EventPhaseChange != null)
            EventPhaseChange(phase);
    }

    // Used to change the state for players (namely whether or not they can see the opponent's cards (Mulligan))
    public void SendEndState(string txt) {
        if (EventEnd != null)
            EventEnd(txt);
    }
}
