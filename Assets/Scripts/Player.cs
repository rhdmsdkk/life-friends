using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    private Vector3 _movement;

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

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        _movement = (_camera.transform.right * horizontal + _camera.transform.forward * vertical).normalized;
        _movement.y = 0;
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _speed * 100f * Time.fixedDeltaTime * _movement;
    }
}
