using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using DG.Tweening;
using Project.Configs;

public class MineController : MonoBehaviour
{
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private TMP_Text _buttonText;

    [SerializeField] private MineConfig _config;

    private CurrencyService _currencyService;
    private PassiveIncomeService _passiveIncomeService;

    private int _currentLevel = 0;
    private double _currentCost;

    [Inject]
    private void Construct(CurrencyService currencyService, PassiveIncomeService passiveIncomeService)
    {
        _currencyService = currencyService;
        _passiveIncomeService = passiveIncomeService;
    }

    private void Start()
    {
        if (_config == null)
        {
            Debug.LogError($"Config is missing on {gameObject.name}!", this);
            return;
        }
        _currentCost = _config.BaseCost;

        _upgradeButton.onClick.AddListener(TryUpgradeMine);

        UpdateUI();
    }
    private void TryUpgradeMine()
    {
        if (_currencyService.TrySpendGold(_currentCost))
        {
            _currentLevel++;

            _passiveIncomeService.AddIncomeSource(_config.BaseIncome);

            _currentCost = _config.BaseCost * Mathf.Pow(_config.CostMultiplier, _currentLevel);

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

    private void UpdateUI()
    {
        _infoText.text = $"{_config.Name} (Ур. {_currentLevel})\n Доход: +{_currentLevel * _config.BaseIncome}/сек";
        _buttonText.text = $"Улучшить:\n{_currentCost:N0} Gold";
    }
}
