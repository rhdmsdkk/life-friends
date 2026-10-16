using System.Collections.Generic;
using UnityEngine;

public enum InteractMode { APPEASE, SUBMIT, PICK_UP, WORK, DISABLED };

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    [SerializeField] private BoxCollider _interactionTrigger;
    [SerializeField] private Transform _taskEquipTransform;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Canvas _hud;

    private Vector3 _movement;
    private bool _canMove = false;

    private Rigidbody _rb;
    private Camera _camera;

    private InteractMode _mode = InteractMode.DISABLED;

    private List<Coworker> _coworkers;

    // holding
    private int _numRedTasks;
    private int _numBlueTasks;
    private List<RedTask> _redTasks;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _coworkers = new();

        _numRedTasks = 0;
        _numBlueTasks = 0;

        _redTasks = new();
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
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Coworker coworker))
        {
            _coworkers.Add(coworker);
        }
        
        if (other.TryGetComponent(out RedTask redTask))
        {
            _numRedTasks++;
            _redTasks.Add(redTask);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Coworker coworker))
        {
            _coworkers.Remove(coworker);
        }

        if (other.TryGetComponent(out RedTask redTask))
        {
            _numRedTasks--;
            _redTasks.Remove(redTask);
        }
    }
    #endregion

    #region [Coworkers]
    private void Appease()
    {
        _particleSystem.Play();

        foreach (Coworker coworker in _coworkers)
        {
            coworker.Appease();
        }
    }
    #endregion

    #region [Work]
    public bool isRed()
    {
        return _numRedTasks >= 1;
    }

    public bool isBlue()
    {
        return _numBlueTasks >= 1;
    }

    private void Work()
    {
        // work
    }

    private void PickUp()
    {
        Debug.Log("picking up");

        // probably refactor to call a task zone to update displayed tasks
        foreach (RedTask redTask in _redTasks)
        {
            Destroy(redTask.gameObject);
            _numRedTasks++;
        }
    }
    #endregion
}
