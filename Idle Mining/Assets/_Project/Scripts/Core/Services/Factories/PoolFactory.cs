using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PoolFactory
{
    private readonly Dictionary<object, object> _pools = new Dictionary<object, object>();
    private readonly Transform _poolRoot;
    private readonly IInstantiator _instantiator;

    private int _initialPoolSize = 50;

    [Inject]
    public PoolFactory(IInstantiator instantiator)
    {
        _instantiator = instantiator;
        _poolRoot = new GameObject("--- GLOBAL_POOL ---").transform;
    }

    public T Get<T>(T prefab, Transform parent = null) where T : Component
    {
        if (!_pools.ContainsKey(prefab))
        {
            _pools[prefab] = new ObjectPool<T>(_instantiator, prefab, _initialPoolSize, _poolRoot);
        }

        var pool = (ObjectPool<T>)_pools[prefab];
        return pool.Get(parent);
    }

    public void Reclaim<T>(T instance, T prefab) where T : Component
    {
        if (_pools.TryGetValue(prefab, out var poolObj))
        {
            ((ObjectPool<T>)poolObj).Return(instance);
        }
    }
}
