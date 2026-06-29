using UnityEngine;
using Project.Configs;
using Zenject;
using System.Collections.Generic;

public class MineService : IInitializable
{
    private readonly SaveLoadService _saveLoadService;
    private readonly PassiveIncomeService _passiveIncomeService;
    private readonly List<MineConfig> _allMineConfigs;

    private readonly Dictionary<string, int> _mineLevels = new Dictionary<string, int>();

    public MineService(SaveLoadService saveLoadService, PassiveIncomeService passiveIncomeService, List<MineConfig> allConfigs)
    {
        _saveLoadService = saveLoadService;
        _passiveIncomeService = passiveIncomeService;
        _allMineConfigs = allConfigs;
    }

    public void Initialize()
    {
        LoadMinesFromSave();
        UpdateGlobalPassiveIncome();
    }

    private void LoadMinesFromSave()
    {
        var saveData = _saveLoadService.Data;

        foreach (var config in _allMineConfigs)
        {
            int index = saveData.MineIds.IndexOf(config.Id);
            _mineLevels[config.Id] = index >= 0 ? saveData.MineLevels[index] : 0;
        }
    }

    public void HandleMineUpgrade(string mineId, int newLevel)
    {
        _mineLevels[mineId] = newLevel;

        var saveData = _saveLoadService.Data;
        int index = saveData.MineIds.IndexOf(mineId);

        if (index >= 0)
        {
            saveData.MineLevels[index] = newLevel;
        }
        else
        {
            saveData.MineIds.Add(mineId);
            saveData.MineLevels.Add(newLevel);
        }

        UpdateGlobalPassiveIncome();
    }

    private void UpdateGlobalPassiveIncome()
    {
        double totalIncome = 0;

        foreach (var config in _allMineConfigs)
        {
            int level = GetMineLevel(config.Id);
            totalIncome += level * config.BaseIncome;
        }

        _passiveIncomeService.RecalculateTotalIncome(totalIncome);
    }

    public int GetMineLevel(string mineId)
        => _mineLevels.TryGetValue(mineId, out int lvl) ? lvl : 0;
}
