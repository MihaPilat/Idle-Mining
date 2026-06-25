using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using DG.Tweening;
using Project.Configs;

public class MineController : MonoBehaviour
{
    [SerializeField] private string _mineId;

    [SerializeField] private Button _upgradeButton;
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private TMP_Text _buttonText;

    [SerializeField] private MineConfig _config;

    private CurrencyService _currencyService;
    private PassiveIncomeService _passiveIncomeService;
    private SaveLoadService _saveLoadService;

    private int _currentLevel = 0;
    private double _currentCost;

    [Inject]
    private void Construct(CurrencyService currencyService, PassiveIncomeService passiveIncomeService, SaveLoadService saveLoadService)
    {
        _currencyService = currencyService;
        _passiveIncomeService = passiveIncomeService;
        _saveLoadService = saveLoadService;
    }

    private void Start()
    {
        if (_config == null)
        {
            Debug.LogError($"Config is missing on {gameObject.name}!", this);
            return;
        }
        LoadMineLevelFromSave();

        RecalculateCost();

        UpdateGlobalIncome();

        _upgradeButton.onClick.AddListener(TryUpgradeMine);

        UpdateUI();
    }
    private void TryUpgradeMine()
    {
        if (_currencyService.TrySpendGold(_currentCost))
        {
            _currentLevel++;

            SaveMineLevelToData();

            UpdateGlobalIncome();
            RecalculateCost();

            _upgradeButton.transform.DOComplete();
            _upgradeButton.transform.DOPunchScale(new Vector3(0.05f, -0.05f, 0.05f), 0.15f);

            UpdateUI();
        }
        else
        {
            _upgradeButton.transform.DOComplete();
            _upgradeButton.transform.DOShakePosition(0.2f, 5f, 10);
        }
    }
    private void LoadMineLevelFromSave()
    {
        var saveData = _saveLoadService.Data;
        int index = saveData.MineIds.IndexOf(_mineId);

        if (index >= 0)
        {
            _currentLevel = saveData.MineLevels[index];
        }
        else
        {
            _currentLevel = 0;
        }
    }

    private void SaveMineLevelToData()
    {
        var saveData = _saveLoadService.Data;
        int index = saveData.MineIds.IndexOf(_mineId);

        if (index >= 0)
        {
            saveData.MineLevels[index] = _currentLevel;
        }
        else
        {
            saveData.MineIds.Add(_mineId);
            saveData.MineLevels.Add(_currentLevel);
        }
    }

    private void RecalculateCost()
    {
        _currentCost = _config.BaseCost * Mathf.Pow(_config.CostMultiplier, _currentLevel);
    }

    private void UpdateGlobalIncome()
    {
        double currentMineIncome = _currentLevel * _config.BaseIncome;

        _passiveIncomeService.RecalculateTotalIncome(currentMineIncome);
    }
    private void UpdateUI()
    {
        _infoText.text = $"{_config.Name} (Ур. {_currentLevel})\n Доход: +{_currentLevel * _config.BaseIncome}/сек";
        _buttonText.text = $"Улучшить:\n{_currentCost:N0} Gold";
    }
}
