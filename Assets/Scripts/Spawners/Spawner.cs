using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
#if UNITY_EDITOR

    [ReadOnly][SerializeField] private int _countActiveObjects;
    [ReadOnly][SerializeField] private int _countFullObjects;

#endif

    [SerializeField] protected T Prefab;
    [SerializeField] private int _capacity;

    protected ObjectPool<T> Pool { get; private set; }
    public IReadOnlyObjectPool<T> ReadOnlyObjectPool => Pool;

    private void Awake()
    {
        Pool = InitilizePool();
        HandleAwake();
    }

    private void OnEnable()
    {
        HandleEnable();
    }

    private void OnDisable()
    {
        HandleDisable();
    }

    private void Start()
    {
        HandleStart();
    }

#if UNITY_EDITOR

    private void Update()
    {
        _countActiveObjects = Pool.CountActiveObjects;
        _countFullObjects = Pool.CountFullObjects;
    }

#endif

    public void Spawn()
    {
        if (Pool.TryGet(out T spawnObject) == false)
        {
            Debug.LogWarning($"{GetType().Name} не готов предоставить {typeof(T).Name}! Он отключен или в нем закончились {typeof(T).Name}!");

            return;
        }

        spawnObject.transform.position = GetSpawnPosition();
    }

    protected abstract Vector3 GetSpawnPosition();

    protected abstract T CreateSpawnObject();

    protected abstract void GetSpawnObject(T spawnObject);

    protected abstract void RefundSpawnObject(T spawnObject);

    protected virtual void HandleAwake() { }

    protected virtual void HandleEnable() { }
    
    protected virtual void HandleDisable() { }

    protected virtual void HandleStart() { }

    private ObjectPool<T> InitilizePool()
    {
        return new ObjectPool<T>(CreateSpawnObject, GetSpawnObject, RefundSpawnObject, _capacity);
    }
}