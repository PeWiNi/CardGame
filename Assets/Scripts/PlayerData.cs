using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Contains the available decks (aswell as selected deck), experience/level, profile name, unlocks
/// 
/// Save a history of win/loose (with player names) - 'stina 6th Sep. 2016
/// Save last weeks entries
/// 
/// Purchase history and unlocks would be needed too (for the sake of preventing hackers and compensating peeps where unlocks bugs)
/// </summary>
public class PlayerData : NetworkLobbyPlayer {
    public string Name = "";
    public Cards Deck = new Cards(15);
    float experience;

    public int wins;
    int losses;
    public int totalMatches;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        if(!isLocalPlayer) {
            GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        Data data = new Data();
        data.name = Name;
        data.experience = experience;

        data.winCount = wins;
        data.looseCount = losses;
        data.matchCount = totalMatches;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load() {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            Data data = (Data)bf.Deserialize(file);
            file.Close();

            Name = data.name;
            experience = data.experience;

            wins = data.winCount;
            losses = data.looseCount;
            totalMatches = data.matchCount;
        }
    }

    void OnEnable() {
        Load();
    }

    void OnDisable() {
        Save();
    }
}

[Serializable]
class Data {
    public string name;
    public float experience;

    public int[] CardsUnlocked;

    public int winCount;
    public int looseCount;
    public int matchCount;
}
