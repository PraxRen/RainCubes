using System.Collections;
using UnityEngine;

public abstract class AutoSpawner<T> : Spawner<T> where T : MonoBehaviour
{
    [SerializeField] private float _timeWaitSpawn;

    private WaitForSeconds _waitForSeconds;
    private Coroutine _jobRunSpawn;

    protected override void HandleAwake()
    {
        _waitForSeconds = new WaitForSeconds(_timeWaitSpawn);
    }

    protected override void HandleEnable()
    {
        _jobRunSpawn = StartCoroutine(RunSpawn());
    }

    protected override void HandleDisable()
    {
        if (_jobRunSpawn != null)
            StopCoroutine(_jobRunSpawn);
    }

    private IEnumerator RunSpawn()
    {
        while (true)
        {
            Spawn();

            yield return _waitForSeconds;
        }
    }
}