using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ShakeController : MonoBehaviour
{
    [SerializeField] private Vector3 _shakeStrenght = new Vector3(0, 0, 80f);
    [SerializeField] private int _shakeVibrato = 150;
    [SerializeField] private float _shakeRandomness = 80f;
    [SerializeField] private float _recoverDuration = 0.5f;

    Vector3 _currentRotation;
    Tweener _shakeTweener;
    Tweener _recoverTweener;
    public void StartShake(float duration, bool useTimeScale = false)
    {
        this.StopShake();
        if (this._recoverTweener != null && !this._recoverTweener.IsComplete())
        {
            this._recoverTweener.Complete();
        }

        int vibrato = useTimeScale ? (int)((float)this._shakeVibrato / Time.timeScale) : this._shakeVibrato;

        this._currentRotation = this.transform.rotation.eulerAngles;
        this._shakeTweener = this.transform.DOShakeRotation(duration, this._shakeStrenght, vibrato, this._shakeRandomness);
    }

    public void StopShake(bool useTimeScale = false)
    {
        if (this._shakeTweener != null && !this._shakeTweener.IsComplete())
        {
            this._shakeTweener.Kill();
            float duration = useTimeScale ? this._recoverDuration * Time.timeScale : this._recoverDuration;
            this._recoverTweener = this.transform.DORotate(this._currentRotation, duration);
        }
    }
}
