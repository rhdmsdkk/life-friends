using System.Collections.Generic;
using UnityEngine;

public enum InteractMode { APPEASE, SUBMIT, PICK_UP, WORK, DISABLED };

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    [SerializeField] private BoxCollider _interactionTrigger;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Canvas _hud;

    private Vector3 _movement;
    private List<Coworker> _coworkers;
    private bool _canMove = false;

    private Rigidbody _rb;
    private Camera _camera;

    private InteractMode _mode = InteractMode.DISABLED;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();

        _coworkers = new();

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
        if (_canMove)
        {
            _rb.linearVelocity = _speed * 100f * Time.fixedDeltaTime * _movement;
        }
    }

    public void LockMovement()
    {
        _rb.linearVelocity = Vector3.zero;
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

        if (mode == InteractMode.DISABLED)
        {
            _canMove = false;
        }
        else
        {
            _canMove = true;
        }
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
        }
    }
    #endregion

    #region [Coworkers]
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

    private void Appease()
    {
        _particleSystem.Play();

        foreach (Coworker coworker in _coworkers)
        {
            coworker.Appease();
        }
    }
    #endregion

    private void Work()
    {
        // work
    }
}
