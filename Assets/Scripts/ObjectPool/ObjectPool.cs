using System;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ObjectPool<T> : IReadOnlyObjectPool<T> where T : MonoBehaviour
{
    private int _maxSize;
    private T[] _hash;
    private Func<T> _createFunc;
    private Func<T, bool> _funcFindIsNotActive;
    private Func<T, T, bool> _funcEquals;

    public event Action<T> Geted;
    public event Action<T> Refunded;

    public int CountActiveObjects { get; private set; }
    public int CountFullObjects { get; private set; }

    public ObjectPool(Func<T> createFunc, Action<T> actionOnGet, Action<T> actionOnRefund, int maxSize)
    {
        if (createFunc == null)
            throw new ArgumentNullException(nameof(createFunc));

        if (maxSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxSize));

        _createFunc = createFunc;
        Geted += actionOnGet;
        Refunded += actionOnRefund;
        _funcFindIsNotActive = (objectToPool) => !objectToPool.gameObject.activeSelf;
        _funcEquals = (objectToPoolOne, objectToPoolTwo) => objectToPoolOne.Equals(objectToPoolTwo);
        _maxSize = maxSize;
        _hash = new T[maxSize];
        CreateObjects();
    }

    public ObjectPool(Func<T> createFunc, Action<T> actionOnGet, Action<T> actionOnRefund, Func<T, bool> funcFindIsNotActive, Func<T, T, bool> funcEquals, int maxSize) : this(createFunc, actionOnGet, actionOnRefund, maxSize)
    {
        if (funcFindIsNotActive == null)
            throw new ArgumentNullException(nameof(createFunc));

        if (funcEquals == null)
            throw new ArgumentNullException(nameof(funcEquals));

        _funcFindIsNotActive = funcFindIsNotActive;
        _funcEquals = funcEquals;
    }

    private void CreateObjects()
    {
        for (int i = 0; i < _maxSize; i++)
        {
            _hash[i] = _createFunc.Invoke();
        }

        CountFullObjects = _hash.Length;
    }

    public bool TryGet(out T receivedObject)
    {
        receivedObject = _hash.FirstOrDefault(_funcFindIsNotActive);

        if (receivedObject == null)
            return false;

        CountActiveObjects++;
        Geted?.Invoke(receivedObject);
        return true;
    }

    public void Refund(T objectRefund)
    {
        if (objectRefund == null)
            throw new ArgumentNullException(nameof(objectRefund));

        T containsObjectToPool = _hash.FirstOrDefault(objectToPool => _funcEquals.Invoke(objectToPool, objectRefund));

        if (containsObjectToPool == null)
            throw new InvalidOperationException($"Нельзя вернуть {objectRefund.GetType().Name} не принадлежащий {GetType().Name}");

        CountActiveObjects--;
        Refunded?.Invoke(objectRefund);
    }
}