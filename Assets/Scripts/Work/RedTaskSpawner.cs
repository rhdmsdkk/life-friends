using System.Collections.Generic;
using UnityEngine;

public class RedTaskSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _redTaskPrefab;

    private int _numRedTasks;
    private List<RedTask> _redTasks;

    private void Start()
    {
        _numRedTasks = 0;
        _redTasks = new();
    }

    public void SpawnRedTask()
    {
        _numRedTasks++;

        Vector3 newPos = new(transform.position.x, transform.position.y, transform.position.z);

        if (_numRedTasks > 1)
        {
            newPos.y += (_numRedTasks - 1) * 0.1f;
        }

        RedTask newRedTask = Instantiate(_redTaskPrefab, newPos, Quaternion.identity).GetComponent<RedTask>();
        _redTasks.Add(newRedTask);
    }

    public bool TakeRedTask()
    {
        if (_numRedTasks >= 1)
        {
            RedTask topRedTask = _redTasks[_numRedTasks - 1];

            _redTasks.Remove(topRedTask);

            Destroy(topRedTask.gameObject);

            _numRedTasks--;

            return true;
        }

        return false;
    }

    public int GetNumTasks()
    {
        return _numRedTasks;
    }
}
