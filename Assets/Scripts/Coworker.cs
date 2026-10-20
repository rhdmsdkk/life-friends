using System.Collections;
using UnityEngine;

public class Coworker : MonoBehaviour
{
    [SerializeField] private float _loseCharmRate = 1f;
    [SerializeField] private float _moveInterval = 3f;
    [SerializeField] private float _moveLength = 0.5f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody _rb;
    private GameManager _gameManager;
    private AudioSource _audio;
    private Player _player;

    private const float _maxCharm = 100f;
    private const float _charmEffect = 40f;
    private float _charm;

    private Vector3 _movement;

    private float _shakeAmount = 0f;
    private bool _didKill;

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rb = GetComponent<Rigidbody>();
        _gameManager = FindAnyObjectByType<GameManager>();
        _audio = GetComponent<AudioSource>();
        _player = FindAnyObjectByType<Player>();

        _charm = _maxCharm;
        _didKill = false;
    }

    private float t = 0f;
    private void Update()
    {
        LoseCharm();

        Shake();

        t += Time.deltaTime;

        if (t > _moveInterval)
        {
            Move();
            t = 0f;
        }
    }

    private void Move()
    {
        CalculateMovement();
        StartCoroutine(IMove());
    }

    private IEnumerator IMove()
    {
        _rb.linearVelocity = 500f * Time.fixedDeltaTime * _movement;

        yield return new WaitForSeconds(_moveLength);

        _rb.linearVelocity = Vector3.zero;
    }

    private void CalculateMovement()
    {
        float horizontal = Random.Range(-1f, 1f);
        float vertical = Random.Range(-1f, 1f);

        Vector3 unitVector = new(1f, 1f, 1f);

        _movement = (unitVector * horizontal + unitVector * vertical).normalized;
        _movement.y = 0;
    }

    public void Appease()
    {
        _charm = Mathf.Min(_charm + _charmEffect, _maxCharm);
    }

    private void Kill()
    {
        _audio.Play();

        _gameManager.StartFail();
        _spriteRenderer.color = Color.red;
        _charm = 0f;

        _didKill = true;
    }

    private void LoseCharm()
    {
        GameState currentGameState = _gameManager.GetGameState();

        if (currentGameState == GameState.PLAY || currentGameState == GameState.FAIL)
        {
            _charm -= _loseCharmRate * Time.deltaTime;
            _charm = Mathf.Max(_charm, 0f);
        }

        if (_charm <= 0 && !_didKill)
        {
            Kill();
        }
    }

    private void Shake()
    {
        float charmGap = _maxCharm - _charm;

        _shakeAmount = Mathf.Max(1f, (charmGap * charmGap) / _maxCharm);

        Vector3 newPos = Random.insideUnitCircle * (Time.deltaTime * _shakeAmount);

        newPos.z = 0f;

        _spriteRenderer.transform.localPosition = newPos;
    }
}
