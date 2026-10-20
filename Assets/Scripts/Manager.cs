using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private float _originalSpawnTaskInterval = 5f;
    private float _spawnTaskInterval;

    [SerializeField] private RedTaskSpawner _redTaskSpawner;
    [SerializeField] private int _maxRedTasks = 10;

    [SerializeField] private TextMeshProUGUI _scoreText;

    private SpriteRenderer _spriteRenderer;
    private AudioSource _audio;

    private GameManager _gameManager;

    private Vector3 _originalPos;
    private float t = 0;
    private int _performance;
    private bool _didKill;

    private void Start()
    {
        _spawnTaskInterval = _originalSpawnTaskInterval;

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _audio = GetComponent<AudioSource>();

        _gameManager = FindAnyObjectByType<GameManager>();

        _originalPos = transform.position;
        _performance = 0;
        _didKill = false;

        _scoreText.text = PlayerPrefs.GetInt("highscore", 0).ToString();
    }

    private void Update()
    {
        t += Time.deltaTime;

        if (t > _spawnTaskInterval)
        {
            _redTaskSpawner.SpawnRedTask();
            t = 0;
        }

        if (_redTaskSpawner.GetNumTasks() >= _maxRedTasks && !_didKill)
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
        _spawnTaskInterval = _originalSpawnTaskInterval - Mathf.Sqrt(_performance) * 0.4f;
        _spawnTaskInterval = Mathf.Max(0.1f, _spawnTaskInterval);
        _scoreText.text = _performance.ToString();

        SetHighScore();
    }

    private void SetHighScore()
    {
        int highscore = PlayerPrefs.GetInt("highscore", 0);

        if (_performance > highscore)
        {
            PlayerPrefs.SetInt("highscore", _performance);
            PlayerPrefs.Save();
        }
    }

    public void ClearScore()
    {
        _scoreText.text = "0";
    }

    private void Kill()
    {
        _audio.Play();

        _gameManager.StartFail();
        _spriteRenderer.color = Color.red;

        // bum rush player

        _didKill = true;
    }
}
