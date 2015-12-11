using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {
    // used for playback
    private AudioSource audio_src;
    // back ground music track
    public AudioClip sound_loop;
    public float sound_loop_volumn;

    // Use this for initialization
    void Start () {
        audio_src = gameObject.AddComponent<AudioSource>();
        if (sound_loop)
        {
            audio_src.clip = sound_loop;
            audio_src.loop = true;
            audio_src.volume = sound_loop_volumn;
            audio_src.Play();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
