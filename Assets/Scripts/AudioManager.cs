/*
@Author - Patrick
@Description - Handles all audio
*/

using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour{
    public Sound[] sounds;
    
    void Awake(){
        foreach(Sound s in sounds){
           s.src = gameObject.AddComponent<AudioSource>();
           s.src.clip = s.clip;
           s.src.volume = s.volume;
           s.src.pitch = s.pitch;
           s.src.GetComponent<AudioSource>().Stop();
        }
    }
    
    public void Play(string name)
    {
        Sound s = sounds[0];
        s.src.Play();
    }
    public void Stop(string name){
        Sound s = sounds[0];
        s.src.Stop();
    }
}