using UnityEngine;

public class MultipleAudioSource : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private int _quantity;

    private AudioSource[] _audioSources;
    private int _currentAudioIndex = 0;

    private void Awake()
    {
        _audioSources = new AudioSource[_quantity];
        _audioSources[0] = _audioSource;

        for (int i = 1; i < _quantity; i++)
        {
            AudioSource newAudio = Instantiate<AudioSource>(_audioSource);
            newAudio.transform.parent = _audioSource.transform.parent;
            newAudio.transform.position = _audioSource.transform.position;
            newAudio.transform.localScale = _audioSource.transform.localScale;
            _audioSources[i] = newAudio;
        }
    }

    public AudioSource GetAudioSource()
    {
        int index = _currentAudioIndex;

        _currentAudioIndex = (_currentAudioIndex + 1) % _quantity;

        return _audioSources[index];
    }

    public void Play()
    {
        GetAudioSource().Play();
    }
}
