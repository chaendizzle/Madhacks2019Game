using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [Serializable]
    public struct SoundEffect
    {
        public string name;
        public AudioClip clip;
    }
    public List<SoundEffect> sfx;
    Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();

    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        foreach (SoundEffect se in sfx)
        {
            sfxDict[se.name] = se.clip;
        }
    }

    public static void PlaySound(string name)
    {
        SoundEffects ths = GameObject.FindGameObjectWithTag("SoundEffect").GetComponent<SoundEffects>();
        ths.source.PlayOneShot(ths.sfxDict[name]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
