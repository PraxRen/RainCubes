using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private const float MinTimeLifeTime = 2f;
    private const float MaxTimeLifeTime = 5f;

    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Color[] _colors;

    private Color _defaultColor;
    private bool _isFaced;

    public event Action<Cube> Died;

    private void Awake()
    {
        _defaultColor = _meshRenderer.material.color;
    }

    private void OnDisable()
    {
        _isFaced = false;
        _meshRenderer.material.color = _defaultColor;
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
        float lifeTime = UnityEngine.Random.Range(MinTimeLifeTime, MaxTimeLifeTime);
        yield return new WaitForSeconds(lifeTime);
        Died?.Invoke(this);
    }

    private void SetRandomColor()
    {
        int randomIndex = UnityEngine.Random.Range(0, _colors.Length);
        Color color = _colors[randomIndex];
        _meshRenderer.material.color = color;
    }
}
