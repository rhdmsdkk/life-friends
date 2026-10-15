using UnityEngine;

public class Coworker : MonoBehaviour
{
    [SerializeField] private float _loseCharmRate = 1f;

    private const float _maxCharm = 100f;
    private const float _charmEffect = 40f;
    private float _charm;

    private bool _didKill = false;

    private void Start()
    {
        _charm = _maxCharm;
    }

    private void Update()
    {
        _charm -= _loseCharmRate * Time.deltaTime;

        if (_charm <= 0 && !_didKill)
        {
            Kill();
        }
    }

    public void Appease()
    {
        _charm = Mathf.Min(_charm + _charmEffect, _maxCharm);

        Debug.Log("APPEASE: " + _charm);
    }

    private void Kill()
    {
        _didKill = true;
        Debug.Log("KILL");
    }
}
