using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CircleTempoView : TempoView
{
    [SerializeField]
    private float _initialRotation;

    [SerializeField]
    private float _finalRotation;

    [SerializeField]
    private Transform _beatContainer;

    private float _distance;

    private void Awake()
    {
        _distance = _initialRotation - _finalRotation;
    }

    public override void SetInitialBeatPosition(BeatController beatView)
    {
        beatView.transform.parent = _beatContainer;
        beatView.transform.localPosition = Vector3.zero;
        beatView.transform.localScale = Vector3.one;
        beatView.transform.localEulerAngles = new Vector3(0f, 0f, _initialRotation);
    }

    public override void UpdateBeatPosition(BeatController beatView)
    {
        Vector3 rotation = beatView.transform.localEulerAngles;
        rotation.z = _initialRotation - (_distance * (beatView.Beat.TimeFactor));
        beatView.transform.localEulerAngles = rotation;
    }
}
