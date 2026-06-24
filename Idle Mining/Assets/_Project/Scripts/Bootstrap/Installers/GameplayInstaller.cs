using System;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindCurrencyService();
        BindPassiveIncomeService();
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
