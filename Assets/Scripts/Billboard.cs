using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Player _player;

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
    }

    void Update()
    {
        transform.LookAt(_player.transform);
    }
}
