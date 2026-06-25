using System;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindSaveLoadService();
        BindCurrencyService();
        BindPassiveIncomeService();
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
