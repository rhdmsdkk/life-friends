using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    private Vector3 _movement;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }

        _movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _speed * 100f * Time.fixedDeltaTime * _movement;
    }
}
