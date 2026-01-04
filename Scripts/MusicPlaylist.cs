using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlaylist : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] musicTracks; // Oyun içi müzikler
    private int currentTrackIndex = 0;

    void Start()
    {
        if (musicTracks.Length > 0)
        {
            PlayTrack(currentTrackIndex);
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            NextTrack();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            NextTrack();
        }
    }

    void PlayTrack(int index)
    {
        audioSource.clip = musicTracks[index];
        audioSource.Play();
    }

    void NextTrack()
    {
        currentTrackIndex = Random.Range(0, musicTracks.Length);
        PlayTrack(currentTrackIndex);
    }
}
