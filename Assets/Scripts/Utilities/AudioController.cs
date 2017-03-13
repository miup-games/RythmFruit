using System.Collections.Generic;
using MiupGames.Common.Singleton;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource[] _audioSources;

    public AudioSource CurrentMusic
    {
        get
        {
            return this._musicSource;
        }
    }

    public void PlayMusic(AudioClip audioClip)
    {
        _musicSource.Stop();
        _musicSource.clip = audioClip;
        _musicSource.Play();
    }

    public void PlayFx(AudioClip audioClip)
    {
        AudioSource audioSource = this.GetAvailableAudioSource();
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private AudioSource GetAvailableAudioSource()
    {
        for(int i = 0; i < this._audioSources.Length; i++)
        {
            if (!this._audioSources[i].isPlaying)
            {
                return this._audioSources[i];
            }
        }

        return this._audioSources[0];
    }
}