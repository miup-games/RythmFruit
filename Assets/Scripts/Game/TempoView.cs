using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class TempoView : MonoBehaviour
{
    [SerializeField]
    private GameObject _beatPrefab;

    [SerializeField]
    private Transform _hitPosition;

    [SerializeField]
    private MultipleAudioSource _hitAudio;

    [SerializeField]
    private MultipleAudioSource _failAudio;

    [SerializeField]
    private MultipleAudioSource _killAudio;

    [SerializeField]
    private MultipleAudioSource _throwAudio;

    [SerializeField]
    private float _throwAudioPbb = 0.1f;

    protected Dictionary<Beat, BeatController> _beats = new Dictionary<Beat, BeatController>();

    public abstract void SetInitialBeatPosition(BeatController beatView);
    public abstract void UpdateBeatPosition(BeatController beatView);

    public virtual void CommitBeat(Beat beat)
    {
        if (!_beats.ContainsKey(beat))
        {
            return;
        }

        beat.Commit();
    }

    public virtual void StartBeat(Beat beat)
    {
        //_hitAudio.Play();

        BeatController beatView = StartBeat();
        beatView.SetBeat(beat, _hitPosition.position);

        _beats.Add(beat, beatView);

        SetInitialBeatPosition(beatView);

        if (UnityEngine.Random.Range(0, 1f) > (1f - _throwAudioPbb))
        {
            _throwAudio.Play();
        }
    }

    public void FinishBeat(Beat beat)
    {
        if (!_beats.ContainsKey(beat))
        {
            return;
        }

        switch(beat.Result)
        {
            case TempoConstants.Result.Success:
                _hitAudio.Play();
                break;
            case TempoConstants.Result.Fail:
            case TempoConstants.Result.Miss:
                _failAudio.Play();
                break;
        }

        BeatController beatView = _beats[beat];
        _beats.Remove(beat);
        beatView.Hit(DestroyBeat);
    }

    public void KillBeats()
    {
        _killAudio.Play();

        foreach(KeyValuePair<Beat, BeatController> item in _beats)
        {
            item.Value.Hit(DestroyBeat);
        }

        _beats.Clear();
    }

    public void UpdateBeat(Beat beat)
    {
        if (!_beats.ContainsKey(beat))
        {
            return;
        }
        BeatController beatView = _beats[beat];

        UpdateBeatPosition(beatView);
    }

    private void GetHitAudio()
    {
        
    }

    private void GetFailAudio()
    {

    }

    private BeatController StartBeat()
    {
        return GameObjectPool.instance.GetObject<BeatController>(_beatPrefab, Vector3.zero, transform);

        /*
        if (_beatPool.Count > 0)
        {
            BeatController beatView = _beatPool[0];
            _beatPool.RemoveAt(0);
            return beatView;
        }

        return Instantiate<BeatController>(_beatPrefab);
        */
    }

    private void DestroyBeat(BeatController beat)
    {
        GameObjectPool.instance.ReturnObject(beat.gameObject);
    }
}
