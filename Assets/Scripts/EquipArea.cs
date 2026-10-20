using System.Collections.Generic;
using UnityEngine;

public class EquipArea : MonoBehaviour
{
    [SerializeField] private GameObject _redTaskPrefab;
    [SerializeField] private GameObject _blueTaskPrefab;

    private int _numRedTasks;
    private List<RedTask> _redTasks;

    private int _numBlueTasks;
    private List<BlueTask> _blueTasks;

    private void Start()
    {
        _numRedTasks = 0;
        _redTasks = new();

        _numBlueTasks = 0;
        _blueTasks = new();
    }

    public bool IsRed()
    {
        return _numRedTasks >= 1;
    }

    public bool IsBlue()
    {
        return _numBlueTasks >= 1;
    }

    public int GetNumRed()
    {
        return _numRedTasks;
    }

    public int GetNumBlue()
    {
        return _numBlueTasks;
    }

    public void SpawnRedTask()
    {
        _numRedTasks++;

        Vector3 newPos = new(0, 0, 0);

        if (_numRedTasks > 1)
        {
            newPos.y += (_numRedTasks - 1) * 0.1f;
        }

        RedTask newRedTask = Instantiate(_redTaskPrefab, transform).GetComponent<RedTask>();

        newRedTask.transform.localPosition = newPos;

        _redTasks.Add(newRedTask);
    }

    public bool FlushRedTasks()
    {
        bool didFlush = false;

        while (_numRedTasks > 0)
        {
            RedTask topRedTask = _redTasks[_numRedTasks - 1];

            _redTasks.Remove(topRedTask);

            Destroy(topRedTask.gameObject);

            _numRedTasks--;

            didFlush = true;
        }

        return didFlush;
    }

    public void SpawnBlueTask()
    {
        _numBlueTasks++;

        Vector3 newPos = new(0, 0, 0);

        if (_numBlueTasks > 1)
        {
            newPos.y += (_numBlueTasks - 1) * 0.1f;
        }

        BlueTask newBlueTask = Instantiate(_blueTaskPrefab, transform).GetComponent<BlueTask>();

        newBlueTask.transform.localPosition = newPos;

        _blueTasks.Add(newBlueTask);
    }

    public bool FlushBlueTasks()
    {
        bool didFlush = false;

        while (_numBlueTasks > 0)
        {
            BlueTask topBlueTask = _blueTasks[_numBlueTasks - 1];

            _blueTasks.Remove(topBlueTask);

            Destroy(topBlueTask.gameObject);

            _numBlueTasks--;

            didFlush = true;
        }

        return didFlush;
    }
}
