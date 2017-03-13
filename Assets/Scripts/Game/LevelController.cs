using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

    [SerializeField]
    private TempoController _tempoController;

    [SerializeField]
    private Animator _levelAnimator;

    [SerializeField]
    private CharacterController _character;

    [SerializeField]
    private CharacterController _enemy;

    [SerializeField]
    private Transform _enemyHideContainer;

    [SerializeField]
    private Transform _enemyBattleContainer;

    [SerializeField]
    private GameObject _gameOverUi;

    [SerializeField]
    private Wave _wave;

    private bool _inBattle = false;
    private bool _gameOver = false;

	// Use this for initialization
    private void Start ()
    {
        _gameOverUi.SetActive(false);
        _tempoController.OnBeatFinish += OnBeatFinish;
        _tempoController.OnBeatStart += OnBeatStart;
        _tempoController.OnBeatHit += OnBeatHit;
        StartCoroutine(GameRoutine());
	}

    private IEnumerator GameRoutine()
    {
        while(true)
        {
            float speed = _wave.GetNextSpeed();
            _tempoController.Play(speed);

            for (int i = 0; i < _wave.Rounds.Length; i++)
            {
                HideEnemy(speed);
                _inBattle = false;
                _character.Walk();

                yield return new WaitForSeconds(5f);

                ShowEnemy();

                yield return StartCoroutine(WaitForEnemy(_wave.Rounds[i].TimeBeforeHit));

                _tempoController.TimeBeforeHit = _wave.Rounds[i].TimeBeforeHit;
                _tempoController.StartTempo();

                _inBattle = true;
                yield return StartCoroutine(WaitForBattle());

                _tempoController.StopTempo();

                if (_gameOver)
                {
                    StartCoroutine(ShowGameOver());
                    yield break;
                }
                else
                {
                    yield return new WaitForSeconds(3f);
                }

                _levelAnimator.speed = 1;
            }
        }
    }

    private IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(6f);
        _gameOverUi.SetActive(true);
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("BattleScene");
    }

    private IEnumerator WaitForBattle()
    {
        while (true)
        {
            if(!_inBattle)
            {
                break;   
            }

            yield return 0;
        }
    }

    private IEnumerator WaitForEnemy(float currentTimeBeforeHit)
    {
        float speed = (_enemyBattleContainer.position.x - _enemy.transform.position.x) / (currentTimeBeforeHit + 0.5f);
        float initialPosition = _enemy.transform.position.x;
        float currentTime = 0;

        while (true)
        {
            if(_enemy.transform.position.x <= _enemyBattleContainer.position.x)
            {
                _levelAnimator.speed = 0;
                _character.Iddle();
                _enemy.Iddle();
                break;
            }
            currentTime += Time.fixedDeltaTime;

            float positionX = initialPosition + speed * currentTime;
            Vector3 position = _enemy.transform.position;
            position.x = positionX;
            _enemy.transform.position = position;
            yield return 0;
        }
    }

    private void ShowEnemy()
    {
        _enemy.Walk();
        //_enemy.transform.SetParent(_enemyBattleContainer, true);
    }

    private void HideEnemy(float speed)
    {
        _enemy.Reset(speed);
        //_enemy.transform.SetParent(_enemyHideContainer, true);
        _enemy.transform.position = _enemyHideContainer.position;
    }

    private void OnBeatStart(Beat beat)
    {
        _enemy.Attack();
    }

    private void OnBeatFinish(Beat beat)
    {
        if (!_inBattle)
        {
            return;
        }

        switch(beat.Result)
        {
            case TempoConstants.Result.Success:
                _character.Attack();
                break;
            case TempoConstants.Result.Fail:
            case TempoConstants.Result.Miss:
                break;
        }
    }

    private void OnBeatHit(Beat beat)
    {
        switch(beat.Result)
        {
            case TempoConstants.Result.Success:
                _enemy.ReceiveHit();
                break;
            case TempoConstants.Result.Fail:
            case TempoConstants.Result.Miss:
                //_enemy.Attack();
                _character.ReceiveHit();
                break;
                //_character.Attack();
        }

        if(!_enemy.IsAlive)
        {
            _inBattle = false;
        }

        if(!_character.IsAlive)
        {
            _gameOver = true;
            _inBattle = false;
        }
    }
}
