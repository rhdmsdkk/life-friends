using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { TITLE, PLAY, FAIL }

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image _title;
    [SerializeField] private Animator _titleAnimator;
    [SerializeField] private Image _killedOverlay;

    private Player _player;
    private Manager _manager;
    private GameState _gameState;
    private Vector3 _titleSourcePos;
    private bool _failed;

    void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _manager = FindAnyObjectByType<Manager>();
        _titleSourcePos = _title.rectTransform.position;
        _failed = false;

        StartTitle();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (_gameState)
            {
                case GameState.TITLE:
                    StartPlay();
                    break;
                case GameState.FAIL:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name); // restart
                    break;
            }
        }
    }

    private void StartTitle()
    {
        _gameState = GameState.TITLE;

        _title.rectTransform.position = _titleSourcePos;

        _player.SetMode(InteractMode.DISABLED);
    }

    private void StartPlay()
    {
        StartCoroutine(IStartPlay());
    }

    private IEnumerator IStartPlay()
    {
        _gameState = GameState.PLAY;

        _titleAnimator.Play("TitleExit");
        _manager.ClearScore();

        yield return new WaitForSeconds(0.2f);

        _player.SetMode(InteractMode.APPEASE);
    }

    public void StartFail()
    {
        _gameState = GameState.FAIL;

        _player.SetMode(InteractMode.DISABLED);

        if (!_failed)
        {
            StartCoroutine(ShowOverlay());
            _failed = true;
        }
    }

    private IEnumerator ShowOverlay()
    {
        float t = 0f;

        Color killedOverlayColor = _killedOverlay.color;

        while (t < 1f)
        {
            t += Time.deltaTime;
            _killedOverlay.color = new Color(killedOverlayColor.r, killedOverlayColor.g, killedOverlayColor.b, t / 4f);
            yield return null;
        }
    }

    public GameState GetGameState()
    {
        return _gameState;
    }
}
