using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour 
{
    [SerializeField] AudioClip[] songs;

    int currentSongIndex;

    AudioSource audioSourse;
    void Start () 
    {
        DontDestroyOnLoad(this);
        audioSourse = GetComponent<AudioSource>();
        if (FindObjectsOfType<StartMusic>().Length > 1)
        {
            Destroy(gameObject);
        }
        audioSourse.clip = songs[currentSongIndex];
        StartCoroutine(PlaySong());
    }

    IEnumerator PlaySong()
    {
        while (true)
        {
            if (!audioSourse.isPlaying)
            {
                if (++currentSongIndex >= songs.Length) currentSongIndex = 0;
                audioSourse.clip = songs[currentSongIndex];
                audioSourse.Play();
            }
            yield return null;
        }
    }
}
