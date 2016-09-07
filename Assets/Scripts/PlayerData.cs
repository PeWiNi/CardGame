using UnityEngine;
using System.Collections.Generic;
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
    float experience; // store level and experience individually?

    public int wins;
    int losses;
    public int totalMatches;

    public List<History> history = new List<History>();

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

    public void RewardExperience(float exp) {
        experience += exp;
    }

    public void RewardExperience(ScoreData.EndState state) {
        switch (state) {
            case (ScoreData.EndState.Win):
                experience += ScoreData.winningPoints;
                break;
            case (ScoreData.EndState.Loose):
                experience += ScoreData.loosingPoints;
                break;
            case (ScoreData.EndState.Draw):
                experience += ScoreData.drawPoints;
                break;
        }
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

        int i = 0;
        string[] hist = new string[history.Count];
        foreach (History h in history) { hist[i++] = h.ToString(); }
        data.history = hist;

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

            history.Clear();
            foreach (string s in data.history) {
                history.Add(new History(s));
            }
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

    /// <summary>
    /// timestamp, enemy user name, draw/win/loose
    /// yyyyMMddHHmmssffff,name,int
    /// </summary>
    public string[] history;
}

public struct History {
    public string timestamp; // System.DateTime.Now.ToString("yyyyMMddHHmmssffff")
    public string enemyName;
    public int win; // 0 = draw, 1 = win, 2 = loose

    public History(string time, string name, int winning) {
        timestamp = time;
        enemyName = name;
        win = winning;
    }

    public History(string everything) {
        string[] me = everything.Split(',');
        timestamp = me[0];
        if (me.Length > 3) { // Comma in name countering
            string s = "";
            for (int i = 1; i < me.Length - 1; i++)
                s += me[i];
            enemyName = s;
            win = int.Parse(me[(me.Length - 1)]);
        } else {
            enemyName = me[1];
            win = int.Parse(me[2]);
        }
    }

    public override string ToString() {
        return String.Format("[1],[2],[3]", timestamp, enemyName, win);
    }
}
