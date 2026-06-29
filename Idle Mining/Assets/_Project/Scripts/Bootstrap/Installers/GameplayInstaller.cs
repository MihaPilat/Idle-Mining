using Project.Configs;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private List<MineConfig> _allMines;

    [SerializeField] private FloatingText _floatingTextPrefab;
    [SerializeField] private Transform _dynamicCanvasTransform;

    public override void InstallBindings()
    {
        BindPoolFactory();
        BindFirebaseService();
        BindSaveLoadService();
        BindCurrencyService();
        BindPassiveIncomeService();
        BindOfflineIncomeService();
        BindFloatingTextService();
        BindMineService();

    }

    private void BindFirebaseService()
    {
        Container
           .BindInterfacesAndSelfTo<FirebaseService>()
           .AsSingle()
           .NonLazy();
    }

    private void BindMineService()
    {
        Container
            .BindInterfacesAndSelfTo<MineService>()
            .AsSingle()
            .WithArguments(_allMines)
            .NonLazy();
    }

    private void BindFloatingTextService()
    {
        Container
            .Bind<FloatingTextService>()
            .AsSingle()
            .WithArguments(_floatingTextPrefab, _dynamicCanvasTransform)
            .NonLazy();
    }

    private void BindPoolFactory()
    {
        Container
            .Bind<PoolFactory>()
            .AsSingle()
            .NonLazy();
    }

    private void BindOfflineIncomeService()
    {
        Container
            .BindInterfacesAndSelfTo<OfflineIncomeService>()
            .AsSingle()
            .NonLazy();
    }

    private void BindSaveLoadService()
    {
        Container
            .Bind<SaveLoadService>()
            .AsSingle()
            .NonLazy();
    }

    private void BindPassiveIncomeService()
    {
        Container
            .BindInterfacesAndSelfTo<PassiveIncomeService>()
            .AsSingle()
            .NonLazy();
    }

    private void BindCurrencyService()
    {
        Container
            .Bind<CurrencyService>()
            .AsSingle()
            .NonLazy();
    }
}
