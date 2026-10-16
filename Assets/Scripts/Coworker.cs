using UnityEngine;

public class Coworker : MonoBehaviour
{
    [SerializeField] private float _loseCharmRate = 1f;

    private SpriteRenderer _spriteRenderer;
    private GameManager _gameManager;
    private Player _player;

    private const float _maxCharm = 100f;
    private const float _charmEffect = 40f;
    private float _charm;

    private Vector3 _originalPos;

    private float _shakeAmount = 0f;

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _gameManager = FindAnyObjectByType<GameManager>();
        _player = FindAnyObjectByType<Player>();

        _charm = _maxCharm;

        _originalPos = transform.position;
    }

    private void Update()
    {
        LoseCharm();

        Shake();
    }

    public void Appease()
    {
        _charm = Mathf.Min(_charm + _charmEffect, _maxCharm);
    }

    private void Kill()
    {
        _gameManager.StartFail();
        _spriteRenderer.color = Color.red;
        _charm = 0f;
        _player.LockMovement();
    }

    private void LoseCharm()
    {
        if (_gameManager.GetGameState() == GameState.PLAY)
        {
            _charm -= _loseCharmRate * Time.deltaTime;
        }

        if (_charm <= 0)
        {
            Kill();
        }
    }

    private void Shake()
    {
        float charmGap = _maxCharm - _charm;

        _shakeAmount = Mathf.Max(1f, (charmGap * charmGap) / _maxCharm);

        Vector3 newPos = Random.insideUnitCircle * (Time.deltaTime * _shakeAmount);

        newPos += _originalPos;
        newPos.z = _originalPos.z;

        transform.position = newPos;
    }
}
