using System;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private float _spawnTaskInterval = 5f;
    [SerializeField] private RedTaskSpawner _redTaskSpawner;
    [SerializeField] private int _maxRedTasks = 10;

    private SpriteRenderer _spriteRenderer;

    private GameManager _gameManager;

    private Vector3 _originalPos;
    private float t = 0;
    private int _performance;

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _gameManager = FindAnyObjectByType<GameManager>();

        _originalPos = transform.position;
        _performance = 0;
    }

    private void Update()
    {
        t += Time.deltaTime;

        if (t > _spawnTaskInterval)
        {
            _redTaskSpawner.SpawnRedTask();
            t = 0;
        }

        if (_redTaskSpawner.GetNumTasks() >= _maxRedTasks)
        {
            Kill();
        }

        Shake();
    }

    private void Shake()
    {
        float shakeAmount = _redTaskSpawner.GetNumTasks();

        Vector3 newPos = UnityEngine.Random.insideUnitCircle * (Time.deltaTime * shakeAmount);

        newPos += _originalPos;
        newPos.z = _originalPos.z;

        transform.position = newPos;
    }

    public void IncreasePerformance(int performance)
    {
        _performance += performance;
        _spawnTaskInterval -= Mathf.Sqrt(_performance) * 0.2f;
        _spawnTaskInterval = Mathf.Max(0.1f, _spawnTaskInterval);
    }

    private void Kill()
    {
        _gameManager.StartFail();
        _spriteRenderer.color = Color.red;

        // bum rush player
    }
}
