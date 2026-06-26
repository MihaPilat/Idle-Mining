using System;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindPoolFactory();
        BindSaveLoadService();
        BindCurrencyService();
        BindPassiveIncomeService();
        BindOfflineIncomeService();
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
