using System.Collections;
using UnityEngine;

public class SpawnerCube : MonoBehaviour
{
    [SerializeField] private CubePool _cubePool;
    [SerializeField] private float _timeWaitSpawn;
    [SerializeField] private Transform _transformMainPlatform;
    [SerializeField] private Vector2 _rangePositions;

    private WaitForSeconds _waitForSeconds;
    private Coroutine _jobSpawn;

    private void OnEnable()
    {
        if (_jobSpawn != null)
            _jobSpawn = StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopCoroutine(_jobSpawn);
    }

    private void Start()
    {
        _waitForSeconds = new WaitForSeconds(_timeWaitSpawn);
        _jobSpawn = StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return _waitForSeconds;

            if (_cubePool.TryGetCube(out Cube cube) == false)
            {
                Debug.Log("В CubePool закончились Cube!");
                continue;
            }

            cube.transform.position = GetRandomPosition();
        }
    }

    private Vector3 GetRandomPosition()
    {
        float positionX = UnityEngine.Random.Range(_transformMainPlatform.position.x - _rangePositions.x, _transformMainPlatform.position.x + _rangePositions.x);
        float positionZ = UnityEngine.Random.Range(_transformMainPlatform.position.z - _rangePositions.y, _transformMainPlatform.position.z + _rangePositions.y);
        return new Vector3(positionX, transform.position.y, positionZ);
    }
}
