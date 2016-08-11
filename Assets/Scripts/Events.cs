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

    public void SendDrawCards() {
        if (EventDrawCards != null)
            EventDrawCards();
    }

    public void SendCleanupDeck() {
        if (EventCleanupDeck != null)
            EventCleanupDeck();
    }
}
