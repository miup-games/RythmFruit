using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TempoController : MonoBehaviour {
    [SerializeField]
    private TempoView _tempoView;

    [SerializeField]
    private AudioSourceLooper _audioSource;

    [SerializeField]
    private AudioSource _sound;

    [SerializeField]
    private float _speedAdjuster = 0.98f;

	[SerializeField]
	private float _timeBeforeHit = 4f;

	[SerializeField]
    private float _beatPerfectOffset = 0.25f;

    [SerializeField]
    private float _beatThreshold = 0.2f;

    private bool _active = false;
	private List<Beat> _beats = new List<Beat>();

    public float TimeBeforeHit
    {
        get { return _timeBeforeHit; }
        set { _timeBeforeHit = value; }
    }

	public Action<Beat> OnBeatStart { get; set; }
	public Action<Beat> OnBeatUpdate { get; set; }
	public Action<Beat> OnBeatFinish { get; set; }
    public Action<Beat> OnBeatKill { get; set; }
    public Action<Beat> OnBeatHit { get; set; }

    public void Play(float audioSpeed)
	{
        StopTempo();
        StopAllCoroutines();

        //double initTime = AudioSettings.dspTime;
        _audioSource.Play(audioSpeed);//Scheduled(initTime);

        StartCoroutine(TempoCoroutine());
        StartCoroutine(StartBeatCreator());
	}

    public void StartTempo()
    {
        _active = true;
    }

    public void StopTempo()
    {
        if (_active)
        {
            _active = false;
            KillBeats();
        }
    }

    private void KillBeats()
    {
        for(int i = _beats.Count - 1; i >= 0; i--)
        {
            Beat beat = _beats[i];
            beat.Kill();
            _beats.Remove(beat);
        }

        _tempoView.KillBeats();
    }

	private float GetSpeed()
	{
        //return UnityEngine.Random.Range(0f, 1f) > 0.7f ? 0.5f : 1f;
        return TempoConstants.Speeds[UnityEngine.Random.Range(0, TempoConstants.Speeds.Length)];
	}

	private void CreateBeat(float waitTime)
	{   
        Beat beat = new Beat (_timeBeforeHit * _audioSource.AudioSpeed, _audioSource.CurrentAudioTime, _beatPerfectOffset, _beatThreshold);

        beat.OnBeatHit += OnBeatHit;

		if (OnBeatStart != null)
		{
			OnBeatStart(beat);
		}
        _tempoView.StartBeat(beat);

		_beats.Add(beat);
	}

	public void Commit()
	{
		if(_beats.Count > 0)
		{
            _tempoView.CommitBeat(_beats[0]);
		}
	}

    private IEnumerator StartBeatCreator ()
	{
        while(true)
		{
            if (!_active)
            {
                yield return 0;
                continue;
            }
            float beatSpeed = GetSpeed();
            yield return StartCoroutine(_audioSource.WaitBeats(1f / beatSpeed));

            if (_active)
            {
                CreateBeat(_timeBeforeHit);
            }
		}
	}

	private IEnumerator TempoCoroutine ()
	{
        while (true)
		{
            if (!_active)
            {
                yield return 0;
                continue;
            }

            float currentAudioTime = _audioSource.CurrentAudioTime;
			for(int i = _beats.Count - 1; i >= 0; i--)
			{
				Beat beat = _beats[i];
                beat.SetCurrentTime(currentAudioTime);

                if (beat.Result != TempoConstants.Result.None)
				{
					_tempoView.FinishBeat(beat);
					_beats.Remove(beat);
                    if (OnBeatFinish != null)
                    {
                        OnBeatFinish (beat);
                    }
                    continue;
				}

                beat.SetCurrentTime(_audioSource.CurrentAudioTime);

                if (OnBeatUpdate != null)
                {
                    OnBeatUpdate(beat);
                }
                _tempoView.UpdateBeat(beat);
			}
			yield return 0;
		}
	}
}