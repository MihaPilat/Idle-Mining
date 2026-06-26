using UnityEngine;
using System;
using Zenject;
using Cysharp.Threading.Tasks;

public class OfflineIncomeService : IInitializable
{
    private readonly CurrencyService _currencyService;
    private readonly PassiveIncomeService _passiveIncomeService;
    private readonly SaveLoadService _saveLoadService;

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
                double earnedGold = secondsOffline * incomePerSecond;

                _currencyService.AddGold(earnedGold);

                Debug.Log($"Отсутствие: {timePassed.Hours}ч {timePassed.Minutes}м {timePassed.Seconds}с. Заработано: {earnedGold:N0} золота");
            }
        }
    }
}
