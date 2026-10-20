using System.Collections.Generic;
using UnityEngine;

public enum InteractMode { APPEASE, SUBMIT, PICK_UP, WORK, WORK_PICK_UP, DISABLED };

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    [SerializeField] private BoxCollider _interactionTrigger;
    [SerializeField] private EquipArea _equipArea;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Canvas _hud;
    [SerializeField] private AudioSource _heartsAudio;

    [SerializeField] private RedTaskSpawner _redTaskSpawner;
    [SerializeField] private BlueTaskSpawner _blueTaskSpawner;

    private Vector3 _movement;
    private bool _canMove = false;

    private Rigidbody _rb;
    private Camera _camera;
    private AudioSource _audio;

    private InteractMode _mode = InteractMode.DISABLED;

    private List<Coworker> _coworkers;
    private Manager _manager;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
        _audio = GetComponent<AudioSource>();
        _equipArea = GetComponentInChildren<EquipArea>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _coworkers = new();
        _manager = FindAnyObjectByType<Manager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }

        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Interact();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    #region [Movement]
    private void CalculateMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        _movement = (_camera.transform.right * horizontal + _camera.transform.forward * vertical).normalized;
        _movement.y = 0;
    }

    private void Move()
    {
        if (_canMove)
        {
            _rb.linearVelocity = _speed * 100f * Time.fixedDeltaTime * _movement;
        }
    }

    public void LockMovement()
    {
        _rb.linearVelocity = Vector3.zero;
    }
    #endregion

    #region [Interaction]
    public void SetMode(InteractMode mode)
    {
        _mode = mode;

        if (_mode == InteractMode.APPEASE)
        {
            _hud.enabled = true;
        }
        else
        {
            _hud.enabled = false;
        }

        if (mode == InteractMode.DISABLED)
        {
            _canMove = false;
            LockMovement();
        }
        else
        {
            _canMove = true;
        }

        Debug.Log(_mode);
    }

    private void Interact()
    {
        if (!_canMove)
        {
            return;
        }

        switch (_mode)
        {
            case InteractMode.DISABLED:
                return;
            case InteractMode.APPEASE:
                Appease();
                break;
            case InteractMode.WORK:
                Work();
                break;
            case InteractMode.PICK_UP:
                PickUp();
                break;
            case InteractMode.WORK_PICK_UP:
                WorkPickUp();
                break;
            case InteractMode.SUBMIT:
                Submit();
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Coworker coworker))
        {
            _coworkers.Add(coworker);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Coworker coworker))
        {
            _coworkers.Remove(coworker);
        }
    }
    #endregion

    #region [Coworkers]
    private void Appease()
    {
        _heartsAudio.Play();
        _particleSystem.Play();

        foreach (Coworker coworker in _coworkers)
        {
            coworker.Appease();
        }
    }
    #endregion

    #region [Work]
    public bool IsRed()
    {
        return _equipArea.IsRed();
    }

    public bool IsBlue()
    {
        return _equipArea.IsBlue();
    }

    private void Work()
    {
        _blueTaskSpawner.SpawnBlueTasks(_equipArea.GetNumRed());

        if (_equipArea.FlushRedTasks())
        {
            _audio.Play();
        }

        SetMode(InteractMode.WORK_PICK_UP);
    }

    private void PickUp()
    {
        if (_redTaskSpawner.TakeRedTask())
        {
            _equipArea.SpawnRedTask();
            _audio.Play();
        }
    }

    private void WorkPickUp()
    {
        if (_blueTaskSpawner.TakeBlueTask())
        {
            _equipArea.SpawnBlueTask();
            _audio.Play();
        }
    }

    private void Submit()
    {
        _manager.IncreasePerformance(_equipArea.GetNumBlue());

        if (_equipArea.FlushBlueTasks())
        {
            _audio.Play();
        }

        SetMode(InteractMode.PICK_UP);
    }
    #endregion
}
