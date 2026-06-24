using System;
using UnityEngine;

public class CurrencyService
{
    public event Action<double> OnGoldChanged;

    public double CurrentGold { get; private set; }

    public CurrencyService()
    {
        CurrentGold = 0;
    }

    public void AddGold(double amount)
    {
        if (amount <= 0) return;

        CurrentGold += amount;

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
            CurrentGold -= amount;
            OnGoldChanged?.Invoke(CurrentGold);
            return true;
        }
        return false;
    }
}
