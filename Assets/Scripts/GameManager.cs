using UnityEngine;
using UnityEngine.UI;

public enum GameState { TITLE, PLAY, FAIL }

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image _title;
    [SerializeField] private Animator _titleAnimator;

    private Player _player;
    private GameState _gameState;
    private Vector3 _titleSourcePos;

    void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _titleSourcePos = _title.rectTransform.position;

        StartTitle();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            switch (_gameState)
            {
                case GameState.TITLE:
                    StartPlay();
                    break;
                case GameState.FAIL:
                    StartTitle();
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
        _gameState = GameState.PLAY;

        _titleAnimator.Play("TitleExit");

        _player.SetMode(InteractMode.APPEASE);
    }

    public void StartFail()
    {
        _gameState = GameState.FAIL;

        _player.SetMode(InteractMode.DISABLED);
    }
}
