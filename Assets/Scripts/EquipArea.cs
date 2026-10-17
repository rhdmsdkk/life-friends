using System.Collections.Generic;
using UnityEngine;

public class EquipArea : MonoBehaviour
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

        Vector3 newPos = new(0, 0, 0);

        if (_numRedTasks > 1)
        {
            newPos.y += (_numRedTasks - 1) * 0.1f;
        }

        RedTask newRedTask = Instantiate(_redTaskPrefab, transform).GetComponent<RedTask>();

        newRedTask.transform.localPosition = newPos;

        _redTasks.Add(newRedTask);
    }
}
