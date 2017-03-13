using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private string _dieAnim;

    [SerializeField]
    private float _hp;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private Transform _hpBar;

    private float _currentMaxHp;
    private float _currentHp;

    public bool IsAlive
    {
        get { return _currentHp > 0; }
    }

    private void Awake()
    {
        Reset(1f);
    }

    private void SetHp()
    {
        Vector3 localScale = _hpBar.transform.localScale;
        localScale.x = _currentHp / _currentMaxHp;
        _hpBar.transform.localScale = localScale;

        if (!IsAlive)
        {
            _animator.SetTrigger(_dieAnim);
        }
    }

    public void Reset(float multiplier)
    {
        //_animator.SetTrigger("Reset");
        //Iddle();
        Attack();
        //Walk();
        _currentMaxHp = _hp * multiplier;
        _currentHp = _currentMaxHp;
        SetHp();
    }

    public void ReceiveHit()
    {
        _animator.SetTrigger("Hit");
        _currentHp--;
        SetHp();
    }

    public void UpdateWalkPosition()
    {
        Vector3 position = transform.position;
        position.x += _speed;
        transform.position = position;
    }

    public void Walk()
    {
        _animator.SetBool("Walking", true);
    }

    public void Iddle()
    {
        _animator.SetBool("Walking", false);
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }

    public void Die()
    {
        _animator.SetTrigger("Die");
    }
}
