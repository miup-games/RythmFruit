using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ProjectileTempoView : TempoView
{
    [SerializeField]
    private Transform _initialPosition;

    [SerializeField]
    private Transform _finalPosition;

    [SerializeField]
    private float _minAngle;

    [SerializeField]
    private float _maxAngle;

    [SerializeField]
    private float _gravity;

    [SerializeField]
    private float _distanceOffset;

    private float _distance;
    private float _initialVelY;

    private void Awake()
    {
        _distance = _initialPosition.position.x - _finalPosition.position.x;
    }

    private float GetInitialVelY()
    {
        //return _gravity * 0.5f / Mathf.Sin(Mathf.Deg2Rad * UnityEngine.Random.Range(_minAngle, _maxAngle));
        return Mathf.Sqrt((Mathf.Abs(_distance) + _distanceOffset) * _gravity / Mathf.Sin(Mathf.Deg2Rad * 2 * UnityEngine.Random.Range(_minAngle, _maxAngle)));
    }

    public override void StartBeat(Beat beat)
    {
        beat.SetResultFromOut();
        base.StartBeat(beat);
    }

    public override void CommitBeat(Beat beat)
    {
        if (!_beats.ContainsKey(beat))
        {
            return;
        }

        BeatController beatController = _beats[beat];
        if (beatController.TempResult == TempoConstants.Result.Success)
        {
            beatController.Beat.SetResult(TempoConstants.Result.Success);
        }
        else
        {
            beatController.Beat.SetResult(TempoConstants.Result.Fail);
        }
    }

    public override void SetInitialBeatPosition(BeatController beatView)
    {
        beatView.transform.parent = _initialPosition;
        beatView.transform.localPosition = Vector3.zero;
        beatView.transform.localScale = Vector3.one;
        beatView.SetInitialSpeed(GetInitialVelY());
    }

    public override void UpdateBeatPosition(BeatController beatView)
    {
        Vector3 position = beatView.transform.position;

        position.x = _finalPosition.position.x + (_distance * (1f - beatView.Beat.TimeFactor));
        position.y = _initialPosition.position.y
            + beatView.InitialSpeed * beatView.Beat.TimeFactor
            - 0.5f * _gravity * beatView.Beat.TimeFactor * beatView.Beat.TimeFactor;

        beatView.transform.position = position;

        if (beatView.TempResult == TempoConstants.Result.Miss)
        {
            beatView.Beat.SetResult(TempoConstants.Result.Miss);
        }
    }
}
