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

    public delegate void EndMulligan();
    [SyncEvent]
    public event EndMulligan EventEndMulligan;

    public void SendMulligan(List<CardStruct> cards) {
        if (EventMulligan != null)
            EventMulligan(cards);
    }

    public void SendEndMulligan() {
        if (EventEndMulligan != null)
            EventEndMulligan();
    }

    public void SendUpdateCards(Player p1, Player p2) {
        if (EventUpdateCards != null)
            EventUpdateCards(p1, p2);
    }
}
