using System;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private FloatingText _floatingTextPrefab;
    [SerializeField] private Transform _dynamicCanvasTransform;

    public override void InstallBindings()
    {
        BindPoolFactory();
        BindSaveLoadService();
        BindCurrencyService();
        BindPassiveIncomeService();
        BindOfflineIncomeService();
        BindFloatingTextService();
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
