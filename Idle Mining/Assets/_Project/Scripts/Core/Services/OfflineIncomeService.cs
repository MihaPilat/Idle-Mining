using UnityEngine;
using System;
using Zenject;
using Cysharp.Threading.Tasks;

public class OfflineIncomeService : IInitializable
{
    public event Action<TimeSpan, double> OnOfflineIncomeCalculated;

    private readonly CurrencyService _currencyService;
    private readonly PassiveIncomeService _passiveIncomeService;
    private readonly SaveLoadService _saveLoadService;

    public double PendingOfflineGold { get; private set; }

    public OfflineIncomeService(
            CurrencyService currencyService,
            PassiveIncomeService passiveIncomeService,
            SaveLoadService saveLoadService)
    {
        _currencyService = currencyService;
        _passiveIncomeService = passiveIncomeService;
        _saveLoadService = saveLoadService;
    }

    public async void Initialize()
    {
        await UniTask.Yield();

        CalculateOfflineIncome();
    }

    private void CalculateOfflineIncome()
    {
        string lastTimeStr = _saveLoadService.Data.LastPlayTime;

        if (string.IsNullOrEmpty(lastTimeStr)) return;

        if (DateTime.TryParse(lastTimeStr, out DateTime lastPlayTime))
        {
            DateTime now = DateTime.UtcNow;
            TimeSpan timePassed = now - lastPlayTime;

            double secondsOffline = timePassed.TotalSeconds;
            double incomePerSecond = _passiveIncomeService.GoldPerSecond;

            if (secondsOffline > 10 && incomePerSecond > 0)
            {
                PendingOfflineGold = secondsOffline * incomePerSecond;

                OnOfflineIncomeCalculated?.Invoke(timePassed, PendingOfflineGold);
            }
        }
    }
    public void ClaimGold()
    {
        if (PendingOfflineGold <= 0) return;

        _currencyService.AddGold(PendingOfflineGold);
        PendingOfflineGold = 0;
    }
}
