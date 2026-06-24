using UnityEngine;
using TMPro;
using Zenject;

public class GoldDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _goldText;

    private CurrencyService _currencyService;

    [Inject]
    private void Construct(CurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    private void Start()
    {
        UpdateText(_currencyService.CurrentGold);
    }
    private void OnEnable()
    {
        _currencyService.OnGoldChanged += UpdateText;
    }
    private void OnDisable()
    {
        _currencyService.OnGoldChanged -= UpdateText;
    }

    private void UpdateText(double gold)
    {
        _goldText.text = $"Золото: {gold:N0}";
    }
}
