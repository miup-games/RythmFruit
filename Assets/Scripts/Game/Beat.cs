using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Beat
{
    private float _beatPerfectOffset;
    private float _beatThreshold;
    private float _limit;
    private float _lastTime;
    private float _initialTime;
    private bool _setResultFromOut = false;

    public float WaitTime { get; private set; }
    public float CurrentTime { get; private set; }
    public TempoConstants.Result Result { get; private set; }
    public System.Action<Beat> OnBeatHit { get; set; }

    public float TimeFactor
    { 
        get
        {
            return (CurrentTime - _initialTime) / WaitTime;
        }
    }

    public Beat(float waitTime, float initialTime, float beatPerfectOffset, float beatThreshold)
    {
        _initialTime = initialTime;
        WaitTime = waitTime;
        _lastTime = initialTime + waitTime;
        CurrentTime = initialTime;

        Result = TempoConstants.Result.None;
        _beatPerfectOffset = beatPerfectOffset;
        _beatThreshold = beatThreshold;
        _limit = _lastTime + _beatPerfectOffset + _beatThreshold;
    }

    public void SetResultFromOut()
    {
        _setResultFromOut = true;
    }

    public void SetCurrentTime(float currentTime)
    {
        CurrentTime = currentTime;

        //if ((CurrentTime >= (_lastTime + _beatPerfectOffset)))
        //{
        //    Result = TempoConstants.Result.Success;
        //}

        if (_setResultFromOut)
        {
            return;
        }

        if (CurrentTime > _limit && Result == TempoConstants.Result.None)
        {
            Result = TempoConstants.Result.Miss;
        }
    }

    public void Commit()
    {
        if (_setResultFromOut)
        {
            return;
        }

        if (Result == TempoConstants.Result.None)
        {
            bool success = (CurrentTime >= (_lastTime + _beatPerfectOffset - _beatThreshold)) && (CurrentTime <= (_lastTime + _beatPerfectOffset + _beatThreshold));
            Result = success ? TempoConstants.Result.Success : TempoConstants.Result.Fail;
        }
    }

    public void SetResult(TempoConstants.Result result)
    {
        Result = result;
    }

    public void Kill()
    {
        Result = TempoConstants.Result.Aborted;
    }

    public void Hit()
    {
        if (OnBeatHit != null)
        {
            OnBeatHit(this);
        }
    }
}
