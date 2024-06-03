using UnityEngine;

public class SpawnerBomb : Spawner<Bomb>
{
    [SerializeField] private SpawnerCube _spawnerCube;

    private Vector3 _lastPositionCube;

    protected override void HandleEnable()
    {
        if (_spawnerCube.ReadOnlyObjectPool != null)
            _spawnerCube.ReadOnlyObjectPool.Geted += OnGetedCube;
    }

    protected override void HandleDisable()
    {
        if (_spawnerCube.ReadOnlyObjectPool != null)
            _spawnerCube.ReadOnlyObjectPool.Geted -= OnGetedCube;
    }

    protected override void HandleStart()
    {
        _spawnerCube.ReadOnlyObjectPool.Geted += OnGetedCube;
    }

    protected override Vector3 GetSpawnPosition() => _lastPositionCube;

    protected override Bomb CreateSpawnObject()
    {
        Bomb spawnObject = Instantiate(Prefab, transform);
        spawnObject.gameObject.SetActive(false);
        return spawnObject;
    }

    protected override void GetSpawnObject(Bomb spawnObject)
    {
        spawnObject.Died += OnDied;
        spawnObject.gameObject.SetActive(true);
        spawnObject.Run();
    }

    protected override void RefundSpawnObject(Bomb spawnObject)
    {
        spawnObject.gameObject.SetActive(false);
    }

    private void OnDied(ILifetime spawnObject)
    {
        spawnObject.Died -= OnDied;
        Pool.Refund((Bomb)spawnObject);
    }

    private void OnGetedCube(Cube cube)
    {
        cube.Died += OnDiedCube;
    }

    private void OnDiedCube(ILifetime cube)
    {
        cube.Died -= OnDiedCube;
        _lastPositionCube = ((Cube)cube).transform.position;
        Spawn();
    }
}