using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private float _spawnTaskInterval = 5f;
    [SerializeField] private RedTaskSpawner _redTaskSpawner;

    private Vector3 _originalPos;
    private float t = 0;
    private int _performance;

    private void Start()
    {
        _originalPos = transform.position;
        _performance = 0;
    }

    private void Update()
    {
        t += Time.deltaTime;

        if (t > _spawnTaskInterval)
        {
            _redTaskSpawner.SpawnRedTask();
            _spawnTaskInterval -= _performance * 0.05f; // replace with formula impossible to reach 0
            t = 0;
        }

        Shake();
    }

    private void Shake()
    {
        float shakeAmount = _redTaskSpawner.GetNumTasks();

        Vector3 newPos = Random.insideUnitCircle * (Time.deltaTime * shakeAmount);

        newPos += _originalPos;
        newPos.z = _originalPos.z;

        transform.position = newPos;
    }

    public void IncreasePerformance(int performance)
    {
        _performance += performance;
    }
}
