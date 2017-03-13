using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Wave
{
    [SerializeField]
    private Round[] _rounds;

    [SerializeField]
    private float _initialSpeed;

    [SerializeField]
    private float _finalSpeed;

    [SerializeField]
    private float _speedIncreasement;

    private float _currentSpeed = 0;

    public Round[] Rounds
    {
        get { return _rounds; }
    }

    public float GetNextSpeed()
    {
        if (_currentSpeed == 0)
        {
            _currentSpeed = _initialSpeed;
            return _currentSpeed;
        }

        if (_currentSpeed == _finalSpeed)
        {
            return _currentSpeed;
        }

        _currentSpeed += _speedIncreasement;
        if (_currentSpeed > _finalSpeed)
        {
            _currentSpeed = _finalSpeed;
        }

        return _currentSpeed;
    }
}

[Serializable]
public class Round
{
    [SerializeField]
    private float _timeBeforeHit;

    public float TimeBeforeHit
    {
        get { return _timeBeforeHit; }
    }
}