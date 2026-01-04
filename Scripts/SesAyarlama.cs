using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SesAyarlama : MonoBehaviour
{
    public AudioMixer audioMixer;

    void Start()
    {
        
    }

    public void audioVal(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

}
