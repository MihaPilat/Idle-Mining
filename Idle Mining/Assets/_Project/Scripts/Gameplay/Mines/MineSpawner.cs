using Project.Configs;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MineSpawner : MonoBehaviour
{
    [SerializeField] private MineController _minePrefab;
    [SerializeField] private List<MineConfig> _minesToSpawn;

    private PoolFactory _poolFactory;
    private MineService _mineService;

    [Inject]
    private void Construct(PoolFactory poolFactory, MineService mineService)
    {
        _poolFactory = poolFactory;
        _mineService = mineService;
    }

    private void Start()
    {
        foreach (var config in _minesToSpawn)
        {
            MineController mineInstance = _poolFactory.Get(_minePrefab, transform);

            int currentLevel = _mineService.GetMineLevel(config.Id);
            mineInstance.Initialize(config, currentLevel);

            mineInstance.OnMineUpgraded += _mineService.HandleMineUpgrade;
        }
    }
}
