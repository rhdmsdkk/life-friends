using UnityEngine;

public class Coworker : MonoBehaviour
{
    [SerializeField] private float _loseCharmRate = 1f;

    private SpriteRenderer _spriteRenderer;

    private const float _maxCharm = 100f;
    private const float _charmEffect = 40f;
    private float _charm;

    private bool _didKill = false;

    private Vector3 _originalPos;

    private float _shakeAmount = 0f;

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _charm = _maxCharm;

        _originalPos = transform.position;
    }

    private void Update()
    {
        _charm -= _loseCharmRate * Time.deltaTime;

        if (_charm <= 0 && !_didKill)
        {
            Kill();
        }

        Shake();
    }

    public void Appease()
    {
        _charm = Mathf.Min(_charm + _charmEffect, _maxCharm);
    }

    private void Kill()
    {
        _didKill = true;
        _spriteRenderer.color = Color.red;
        _charm = 0f;
    }

    private void Shake()
    {
        float charmGap = _maxCharm - _charm;

        // wutever
        if (charmGap < 45f)
        {
            _shakeAmount = 2f;
        }
        else if (charmGap < 60f)
        {
            _shakeAmount = 15f;
        }
        else if (charmGap < 80f)
        {
            _shakeAmount = 50f;
        }
        else
        {
            _shakeAmount = 90f;
        }

        Vector3 newPos = Random.insideUnitCircle * (Time.deltaTime * _shakeAmount);

        newPos += _originalPos;
        newPos.z = _originalPos.z;

        transform.position = newPos;
    }
}
