using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private float _spawnTaskInterval = 5f;
    [SerializeField] private RedTaskSpawner _redTaskSpawner;

    private Vector3 _originalPos;
    private float t = 0;

    private void Start()
    {
        _originalPos = transform.position;
    }

    private void Update()
    {
        t += Time.deltaTime;

        if (t > _spawnTaskInterval)
        {
            _redTaskSpawner.SpawnRedTask();
            // _spawnTaskInterval -= 0.1f; // replace with performance-based value
            t = 0;
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
}
