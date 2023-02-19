using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {
    // Start is called before the first frame update
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    public float pitch;

    public string name;

    [HideInInspector]
    public AudioSource src;
}
