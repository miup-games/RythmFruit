using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BarTempoView : TempoView
{
    [SerializeField]
    private Transform _initialPosition;

    [SerializeField]
    private Transform _finalPosition;

    private float _distance;

    private void Awake()
    {
        _distance = _initialPosition.position.x - _finalPosition.position.x;
    }

    public override void SetInitialBeatPosition(BeatController beatView)
    {
        beatView.transform.parent = _initialPosition;
        beatView.transform.localPosition = Vector3.zero;
        beatView.transform.localScale = Vector3.one;
    }

    public override void UpdateBeatPosition(BeatController beatView)
    {
        Vector3 position = beatView.transform.position;
        position.x = _finalPosition.position.x + (_distance * (1f - beatView.Beat.TimeFactor));
        beatView.transform.position = position;
    }
}
