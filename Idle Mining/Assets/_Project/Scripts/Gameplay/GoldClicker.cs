using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;
using System;

public class GoldClicker : MonoBehaviour
{
    [SerializeField] private Button _button;

    private CurrencyService _currencyService;

    [Inject]
    private void Constuct(CurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    private void Start()
    {
        _button.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        _currencyService.AddGold(1);

        transform.DOComplete();

        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
    }
}
