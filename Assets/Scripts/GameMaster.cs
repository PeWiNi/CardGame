using UnityEngine;
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
}
