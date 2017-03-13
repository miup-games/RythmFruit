using UnityEngine;
using System.Collections;

public class AudioSourceLooper : MonoBehaviour {

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private AudioSource _beatSound;

    [SerializeField]
    private float _audioBPM = 170f;

    [SerializeField]
    private float _totalBeats = 212f;

    [SerializeField]
    private float _iterationTime  = 20f;

    [SerializeField]
    private float _audioSpeed  = 1f;

    [SerializeField]
    private bool _playOnStart  = false;

    [SerializeField]
    private bool _playBeatSound  = false;

    private const int _quantity = 2;

    private AudioSource[] _audioSources;
    private int _currentAudioIndex = 0;
    private float _currentBeats = 0;
    private float _currentLoopsDuration = 0;
    private float _loopDuration;
    private float _songTotalBeats;
    private float _prevTime;

    private AudioSource CurrentAudioSource
    {
        get { return _audioSources[_currentAudioIndex]; }
    }

    public float AudioSpeed
    {
        get { return _audioSpeed; }
    }

    public float CurrentAudioTime
    {
        get
        {
            float currentTime = _audioSpeed * (_currentLoopsDuration + ((float)CurrentAudioSource.timeSamples / CurrentAudioSource.clip.frequency));

            if (_prevTime > currentTime)
            {
                UnityEngine.Debug.LogError("WAAAAAAAAA");
                currentTime = (_audioSpeed * (_currentLoopsDuration + _loopDuration));
            }

            _prevTime = currentTime;

            return currentTime;
        }
    }

    private void Awake()
    {
        _loopDuration = _totalBeats * 60f / _audioBPM;

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

    private void Start()
    {
        if (_playOnStart)
        {
            Play(_audioSpeed);
        }
    }

    private void SetInitialValues()
    {
        CurrentAudioSource.Stop();
        CurrentAudioSource.timeSamples = 0;

        _currentBeats = 0;
        _currentLoopsDuration = 0;
        _prevTime = -10f;
    }

    private void SetNextSource()
    {
        _currentAudioIndex = (_currentAudioIndex + 1) % _quantity;
    }

    private IEnumerator StartBeatCounter()
    {
        float iterationWaitTime = _iterationTime / 1000f;
        float sample = TempoConstants.DeltaBeats * 60f / _audioBPM * _audioSpeed;
        float nextLimit = 0;

        while (true)
        {
            float currentTime = CurrentAudioTime;
            if (currentTime >= nextLimit)
            {
                nextLimit += sample;
                if (_beatSound != null && _playBeatSound)
                {
                    _beatSound.Play();
                }
                _currentBeats += TempoConstants.DeltaBeats;
            }

            yield return new WaitForSeconds(iterationWaitTime);
        }
    }

    private IEnumerator PlayLoop()
    {
        while (true)
        {
            CurrentAudioSource.Play();
            yield return StartCoroutine(WaitBeats(_totalBeats));
            _currentLoopsDuration += _loopDuration;
            SetNextSource();
        }
    }

    private IEnumerator PlayAgain()
    {
        yield return new WaitForSeconds(10f);
        Play(_audioSpeed + 0.2f);
    }

    public void Play(float audioSpeed)
    {
        StopAllCoroutines();
        _audioSpeed = audioSpeed;

        for(int i = 0; i < _audioSources.Length; i++)
        {
            _audioSources[i].pitch = _audioSpeed;   
        }

        SetInitialValues();
        StartCoroutine(PlayLoop());
        StartCoroutine(StartBeatCounter());
        //StartCoroutine(PlayAgain());
    }

    public IEnumerator WaitBeats(float beats)
    {
        float iterationWaitTime = _iterationTime / 1000f;
        float finalBeats = beats + _currentBeats;

        while (true)
        {
            if (_currentBeats >= finalBeats)
            {
                break;
            }

            yield return new WaitForSeconds(iterationWaitTime);
        }
    }
}