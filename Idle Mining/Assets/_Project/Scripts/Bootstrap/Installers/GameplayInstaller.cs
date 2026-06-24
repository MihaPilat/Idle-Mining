using System;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindCurrencyService();
    }

    private void BindCurrencyService()
    {
        Container
            .Bind<CurrencyService>()
            .AsSingle()
            .NonLazy();
    }
}
