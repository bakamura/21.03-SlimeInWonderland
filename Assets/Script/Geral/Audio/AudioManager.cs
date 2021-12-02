using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance = null;

    public Sound[] sounds;

    private void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        foreach (Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.outputAudioMixerGroup = sound.audioGroup;
        }
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) s.source.PlayOneShot(s.clip);
        else Debug.LogWarning("Invalid sound.name Requested in Play (" + name + ")!");
    }
}
