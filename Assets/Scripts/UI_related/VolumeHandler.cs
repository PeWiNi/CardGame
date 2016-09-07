using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VolumeHandler : MonoBehaviour {

	public float musicVolume;
	public float sfxVolume;
	public Slider mscVol;
	public Slider sfxVol;

	// Use this for initialization
	void Start () {
	//	main = GameObject.Find ("Main Camera");

	}
	
	// Update is called once per frame
	void Update () {
		AudioListener.volume = musicVolume;
		// would need to refer to audio sources/channels individually 
	}

	public void ChangeMusicVolume()
	{
		musicVolume=mscVol.value;
	}

	public void ChangeSFXVolume()
	{
		sfxVolume=sfxVol.value;
	}

}

