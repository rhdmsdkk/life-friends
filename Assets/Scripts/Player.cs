using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    [SerializeField] private BoxCollider _interactionTrigger;

    private Vector3 _movement;
    private Coworker _coworker = null;

    private Rigidbody _rb;
    private Camera _camera;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();

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

        if (_coworker != null && Input.GetKeyDown(KeyCode.LeftShift))
        {
            AppeaseCoworker();
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
        _rb.linearVelocity = _speed * 100f * Time.fixedDeltaTime * _movement;
    }
    #endregion

    #region [Coworkers]
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out _coworker))
        {
            Debug.Log("can APPEASE");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Coworker>(out var coworker) && coworker == _coworker)
        {
            _coworker = null;
            Debug.Log("cannot APPEASE");
        }
    }

    private void AppeaseCoworker()
    {
        _coworker.Appease();
    }
    #endregion
}
