using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

public class PassiveIncomeService : IDisposable
{
    private readonly CurrencyService _currencyService;
    private readonly CancellationTokenSource _cts;

    public event Action<double> OnIncomeChanged;

    public double GoldPerSecond { get; private set; }

    public PassiveIncomeService(CurrencyService currencyService)
    {
        _currencyService = currencyService;
        _cts = new CancellationTokenSource();

        GoldPerSecond = 0;

        StartIncomeLoop(_cts.Token).Forget();
    }

    private async UniTaskVoid StartIncomeLoop(CancellationToken token)
    {
        while(!token.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);

            if(GoldPerSecond>0)
            {
                _currencyService.AddGold(GoldPerSecond);
            }
        }
    }
    public void RecalculateTotalIncome(double totalIncome)
    {
        GoldPerSecond = totalIncome;
        OnIncomeChanged?.Invoke(GoldPerSecond);
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
