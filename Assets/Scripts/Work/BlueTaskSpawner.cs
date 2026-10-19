using System.Collections.Generic;
using UnityEngine;

public class BlueTaskSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _blueTaskPrefab;

    private int _numBlueTasks;
    private List<BlueTask> _blueTasks;

    private void Start()
    {
        _numBlueTasks = 0;
        _blueTasks = new();
    }

    public void SpawnBlueTasks(int numBlueTasksToSpawn)
    {
        for (int i = 0; i < numBlueTasksToSpawn; i++)
        {
            SpawnBlueTask();
        }
    }

    public void SpawnBlueTask()
    {
        _numBlueTasks++;

        Vector3 newPos = new(transform.position.x, transform.position.y, transform.position.z);

        if (_numBlueTasks > 1)
        {
            newPos.y += (_numBlueTasks - 1) * 0.1f;
        }

        BlueTask newBlueTask = Instantiate(_blueTaskPrefab, newPos, Quaternion.identity).GetComponent<BlueTask>();
        _blueTasks.Add(newBlueTask);
    }

    public bool TakeBlueTask()
    {
        if (_numBlueTasks >= 1)
        {
            BlueTask topBlueTask = _blueTasks[_numBlueTasks - 1];

            _blueTasks.Remove(topBlueTask);

            Destroy(topBlueTask.gameObject);

            _numBlueTasks--;

            return true;
        }

        return false;
    }
}
