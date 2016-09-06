using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReadyScript : MonoBehaviour {
    public Player player;
    bool isReady = false;
    private Image img;
    private Text txt;
    public Color readyColor = new Color(0, 1, 0);
    public Color notReadyColor = new Color(1, 0, 0);

    // Use this for initialization
    void Start () {
        img = GetComponent<Image>();
        img.color = notReadyColor;
        txt = GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (player) {
            if (player.ready && isReady != player.ready) {
                img.color = readyColor;
                txt.text = "Ready";
                isReady = true;
            } else if (!player.ready && isReady != player.ready) {
                img.color = notReadyColor;
                txt.text = "Not ready";
                isReady = false;
            }
        }
    }
}
