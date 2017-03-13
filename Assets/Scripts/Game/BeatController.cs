using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BeatController : MonoBehaviour
{
    [SerializeField]
    protected float _minRotSpeed;

    [SerializeField]
    protected float _maxRotSpeed;

    [SerializeField]
    protected GameObject _beatContainer;

    [SerializeField]
    protected ParticleSystem _hitParticles;

    [SerializeField]
    protected float _hitDuration;

    [SerializeField]
    protected float _flyBackDuration;

    private float _rotSpeed;
    protected float _initialPosition;
    protected float _finalPosition;

    private Vector3 _hitPosition;

    public float InitialSpeed { get; private set; }
    public Beat Beat { get; private set; }

    public TempoConstants.Result TempResult { get; private set; }

    private void Update()
    {
        if (_rotSpeed != 0)
        {
            Vector3 rot = transform.localEulerAngles;
            rot.z += _rotSpeed;
            transform.localEulerAngles = rot;
        }
    }

    public void SetBeat(Beat beat, Vector3 hitPosition)
    {
        _beatContainer.SetActive(true);
        _rotSpeed = UnityEngine.Random.Range(_minRotSpeed, _maxRotSpeed);
        TempResult = TempoConstants.Result.None;
        Beat = beat;
        _hitPosition = hitPosition;
    }

    public void SetInitialSpeed(float initialSpeed)
    {
        InitialSpeed = initialSpeed;
    }

    public void Hit(System.Action<BeatController> doneCb)
    {
        StartCoroutine(HitCoroutine(doneCb));
    }

    public IEnumerator HitCoroutine(System.Action<BeatController> doneCb)
    {
        if (TempResult == TempoConstants.Result.Success)
        {
            yield return transform.DOMove(_hitPosition, _flyBackDuration, false).SetEase(Ease.Linear).WaitForCompletion();
        }

        if (_hitParticles != null)
        {
            _beatContainer.SetActive(false);
            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
            Beat.Hit();

            yield return new WaitForSeconds(_hitDuration);

            _hitParticles.Stop();
        }
        else
        {
            Beat.Hit();
        }

        if (doneCb != null)
        {
            doneCb(this);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<HitArea>() != null)
        {
            TempResult = TempoConstants.Result.Success;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<HitArea>() != null)
        {
            TempResult = TempoConstants.Result.Miss;
        }
    }
}
