using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour, ILifetime
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color[] _colors;
    [SerializeField] private float _minTimeLifeTime = 2f;
    [SerializeField] private float _maxTimeLifeTime = 5f;

    private Color _defaultColor;
    private bool _isFaced;

    public float MinLifetime => _minTimeLifeTime;
    public float MaxLifetime => _maxTimeLifeTime;

    public event Action<ILifetime> Died;

    private void Awake()
    {
        _defaultColor = _renderer.material.color;
    }

    private void OnDisable()
    {
        _isFaced = false;
        _renderer.material.color = _defaultColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isFaced)
            return;

        if (collision.transform.TryGetComponent(out Platform platform) == false)
            return;

        _isFaced = true;
        StartCoroutine(RunTimerLife());
        SetRandomColor();
    }

    private IEnumerator RunTimerLife()
    {
        float lifeTime = UnityEngine.Random.Range(MinLifetime, MaxLifetime);
        yield return new WaitForSeconds(lifeTime);
        Died?.Invoke(this);
    }

    private void SetRandomColor()
    {
        int randomIndex = UnityEngine.Random.Range(0, _colors.Length);
        Color color = _colors[randomIndex];
        _renderer.material.color = color;
    }
}