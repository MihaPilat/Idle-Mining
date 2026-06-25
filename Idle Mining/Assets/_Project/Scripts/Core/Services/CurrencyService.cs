using System;
using UnityEngine;

public class CurrencyService
{
    private readonly SaveLoadService _saveLoadService;

    public event Action<double> OnGoldChanged;

    public double CurrentGold => _saveLoadService.Data.Gold;

    public CurrencyService(SaveLoadService saveLoadService)
    {
        _saveLoadService = saveLoadService;
    }

    public void AddGold(double amount)
    {
        if (amount <= 0) return;

        _saveLoadService.Data.Gold += amount;

        OnGoldChanged?.Invoke(CurrentGold);
    }

    public bool HasEnoughGold(double amount)
    {
        return CurrentGold >= amount;
    }

    public bool TrySpendGold(double amount)
    {
        if (amount <= 0) return false;

        if(HasEnoughGold(amount))
        {
            _saveLoadService.Data.Gold -= amount;
            OnGoldChanged?.Invoke(CurrentGold);
            return true;
        }
        return false;
    }
}
