using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectPool<T> where T : Component
{
    private readonly T _prefab;
    private readonly Transform _container;
    private readonly IInstantiator _instantiator;
    private readonly Stack<T> _objects = new Stack<T>();

    public ObjectPool(IInstantiator instantiator, T prefab, int initialSize, Transform container = null)
    {
        _instantiator = instantiator;
        _prefab = prefab;
        _container = container;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = CreateNewObject();
            _objects.Push(obj);
        }
    }

    public T Get(Transform textParent = null)
    {
        T obj = _objects.Count > 0 ? _objects.Pop() : CreateNewObject();

        if (textParent != null)
        {
            obj.transform.SetParent(textParent, false);
        }

        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);

        if (_container != null)
        {
            obj.transform.SetParent(_container, false);
        }

        if (!_objects.Contains(obj))
        {
            _objects.Push(obj);
        }
    }

    private T CreateNewObject()
    {
        T obj = _instantiator.InstantiatePrefabForComponent<T>(_prefab, _container);
        obj.gameObject.SetActive(false);
        return obj;
    }
}
