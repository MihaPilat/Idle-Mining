using UnityEngine;
using Zenject;

public class FloatingTextService
{
    private readonly PoolFactory _poolFactory;
    private readonly FloatingText _prefab;
    private readonly Transform _canvasTransform;

    public FloatingTextService(PoolFactory poolFactory, FloatingText prefab, Transform canvasTransform)
    {
        _poolFactory = poolFactory;
        _prefab = prefab;
        _canvasTransform = canvasTransform;
    }

    public void ShowText(string text, Vector3 screenPosition)
    {
        FloatingText instance = _poolFactory.Get(_prefab, _canvasTransform);

        instance.Spawn(text, screenPosition, _poolFactory, _prefab);
    }
}
