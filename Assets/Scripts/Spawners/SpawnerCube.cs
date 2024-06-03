using UnityEngine;

public class SpawnerCube : AutoSpawner<Cube>
{
    [SerializeField] private Transform _transformMainPlatform;
    [SerializeField] private Vector2 _rangePositions;

    protected override Vector3 GetSpawnPosition()
    {
        float positionX = Random.Range(_transformMainPlatform.position.x - _rangePositions.x, _transformMainPlatform.position.x + _rangePositions.x);
        float positionZ = Random.Range(_transformMainPlatform.position.z - _rangePositions.y, _transformMainPlatform.position.z + _rangePositions.y);
        return new Vector3(positionX, transform.position.y, positionZ);
    }

    protected override Cube CreateSpawnObject()
    {
        Cube spawnObject = Instantiate(Prefab, transform);
        spawnObject.gameObject.SetActive(false);
        return spawnObject;
    }

    protected override void GetSpawnObject(Cube spawnObject)
    {
        spawnObject.Died += OnDied;
        spawnObject.gameObject.SetActive(true);
    }

    protected override void RefundSpawnObject(Cube spawnObject)
    {
        spawnObject.gameObject.SetActive(false);
    }

    private void OnDied(ILifetime spawnObject)
    {
        spawnObject.Died -= OnDied;
        Pool.Refund((Cube)spawnObject);
    }
}