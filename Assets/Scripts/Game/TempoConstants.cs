using UnityEngine;
using System.Collections;

public static class TempoConstants {

    public enum Result
    {
        Fail,
        Success,
        Miss,
        Aborted,
        None
    }

    public static float[] Speeds = { 0.5f, 1f};
    public static float DeltaBeats = 1f;
}
