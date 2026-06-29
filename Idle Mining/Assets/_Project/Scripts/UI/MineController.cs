using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using DG.Tweening;
using Project.Configs;
using System;

public class MineController : MonoBehaviour
{
    public event Action<string, int> OnMineUpgraded;

    [SerializeField] private Button _upgradeButton;
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private TMP_Text _buttonText;

    [SerializeField] private MineConfig _config;

    private CurrencyService _currencyService;

    private string _mineId;
    private int _currentLevel = 0;
    private double _currentCost;

    [Inject]
    private void Construct(CurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    public void Initialize(MineConfig config, int level)
    {
        _config = config;
        _mineId = config.Id;
        _currentLevel = level;

        RecalculateCost();
        UpdateUI();

        _upgradeButton.onClick.RemoveAllListeners();
        _upgradeButton.onClick.AddListener(TryUpgradeMine);
    }

    private void TryUpgradeMine()
    {
        if (_currencyService.TrySpendGold(_currentCost))
        {
            _currentLevel++;

            RecalculateCost();
            UpdateUI();

            _upgradeButton.transform.DOComplete();
            _upgradeButton.transform.DOPunchScale(new Vector3(0.05f, -0.05f, 0.05f), 0.15f);

            OnMineUpgraded?.Invoke(_mineId, _currentLevel);
        }
        else
        {
            _upgradeButton.transform.DOComplete();
            _upgradeButton.transform.DOShakePosition(0.2f, 5f, 10);
        }
    }

    private void RecalculateCost()
    {
        _currentCost = _config.BaseCost * Mathf.Pow(_config.CostMultiplier, _currentLevel);
    }

    private void UpdateUI()
    {
        _infoText.text = $"{_config.Name} (Ур. {_currentLevel})\n Доход: +{_currentLevel * _config.BaseIncome}/сек";
        _buttonText.text = $"Улучшить:\n{_currentCost:N0} Gold";
    }
}
