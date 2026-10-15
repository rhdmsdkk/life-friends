using UnityEngine;

public enum InteractMode { APPEASE, SUBMIT, PICK_UP, WORK };

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    [SerializeField] private BoxCollider _interactionTrigger;
    [SerializeField] private ParticleSystem _particleSystem;

    private Vector3 _movement;
    private Coworker _coworker = null;

    private Rigidbody _rb;
    private Camera _camera;
    private Canvas _hud;

    private InteractMode _mode = InteractMode.APPEASE;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
        _hud = GetComponentInChildren<Canvas>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

    #region [Movement & Interaction]
    private void CalculateMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        _movement = (_camera.transform.right * horizontal + _camera.transform.forward * vertical).normalized;
        _movement.y = 0;
    }

    private void Move()
    {
        _rb.linearVelocity = _speed * 100f * Time.fixedDeltaTime * _movement;
    }

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
    }

    private void Interact()
    {
        switch (_mode)
        {
            case InteractMode.APPEASE:
                Appease();
                break;
            case InteractMode.WORK:
                Work();
                break;
        }
    }
    #endregion

    #region [Coworkers]
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out _coworker)) { }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Coworker>(out var coworker) && coworker == _coworker)
        {
            _coworker = null;
        }
    }

    private void Appease()
    {
        _particleSystem.Play();

        if (_coworker != null)
        {
            _coworker.Appease();
        }
    }
    #endregion

    private void Work()
    {
        // work
    }
}
