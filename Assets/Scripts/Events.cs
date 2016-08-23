using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Events : NetworkBehaviour {
    public delegate void DrawCards();
    [SyncEvent]
    public event DrawCards EventDrawCards;
    public delegate void CleanupDeck();
    [SyncEvent]
    public event CleanupDeck EventCleanupDeck;
    public delegate void UpdateCards(Player p1, Player p2);
    [SyncEvent]
    public event UpdateCards EventUpdateCards;

    public void SendDrawCards() {
        if (EventDrawCards != null)
            EventDrawCards();
    }

    public void SendCleanupDeck() {
        if (EventCleanupDeck != null)
            EventCleanupDeck();
    }

    public void SendUpdateCards(Player p1, Player p2) {
        if (EventUpdateCards != null)
            EventUpdateCards(p1, p2);
    }
}
