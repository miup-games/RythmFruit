using System.Collections.Generic;
using MiupGames.Common.Singleton;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private Dictionary<string, AudioClip> _audioClipsFromNames = new Dictionary<string, AudioClip>();
    private AudioController _audioController;

    public AudioSource CurrentMusic
    {
        get
        {
            return this._audioController.CurrentMusic;
        }
    }

    private void Awake()
    {
        GameObject prefab = Resources.Load("Audio/AudioController") as GameObject;
        GameObject newObject = Instantiate(prefab, Vector3.zero, Quaternion.identity, this.transform) as GameObject;
        this._audioController = newObject.GetComponent<AudioController>();
    }

    private AudioClip GetAudioClip(string audioClipName)
    {
        AudioClip audioClip;

        if (!this._audioClipsFromNames.TryGetValue(audioClipName, out audioClip))
        {
            audioClip = Resources.Load<AudioClip>("Audio/" + audioClipName);
            this._audioClipsFromNames[audioClipName] = audioClip;
        }

        return audioClip;
    }

    public void PlayMusic(string audioClipName)
    {
        
        this._audioController.PlayMusic(this.GetAudioClip(audioClipName));
    }

    public void PlayFx(string audioClipName)
    {
        Resources.Load<AudioClip>(name);
        this._audioController.PlayFx(this.GetAudioClip(audioClipName));
    }
}