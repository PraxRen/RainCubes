using System;
using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour, ILifetime
{
    private const float UpForceOffset = 0.7f;

    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _minTimeLifeTime = 2f;
    [SerializeField] private float _maxTimeLifeTime = 5f;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private LayerMask _layerMaskExplodebleObject;

    private Color _defaultColor;

    public event Action<ILifetime> Died;

    public float MinLifetime => _minTimeLifeTime;
    public float MaxLifetime => _maxTimeLifeTime;

    private void Awake()
    {
        _defaultColor = _renderer.material.color;
    }

    private void OnDisable()
    {
        _renderer.material.color = _defaultColor;
    }

    public void Run()
    {
        StartCoroutine(RunTimerLife());
    }

    private IEnumerator RunTimerLife()
    {
        float lifeTime = UnityEngine.Random.Range(_minTimeLifeTime, _maxTimeLifeTime);
        float timer = 0f;

        while (timer < lifeTime)
        {
            timer += Time.deltaTime;
            Color color = _renderer.material.color;
            color.a = Mathf.Lerp(color.a, 0, timer / lifeTime);
            _renderer.material.color = color;
            yield return null;
        }

        Explode();
        Died?.Invoke(this);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius, _layerMaskExplodebleObject, QueryTriggerInteraction.Ignore);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Rigidbody rigidbody) == false)
                continue;

            rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, UpForceOffset);
        }
    }
}