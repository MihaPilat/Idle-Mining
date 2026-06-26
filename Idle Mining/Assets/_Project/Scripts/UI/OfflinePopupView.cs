using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Zenject;

public class OfflinePopupView : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private Transform _windowTransform;
    [SerializeField] private Transform _panelTransform;
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private Button _claimButton;

    private OfflineIncomeService _offlineIncomeService;

    [Inject]
    private void Construct(OfflineIncomeService offlineIncomeService)
    {
        _offlineIncomeService = offlineIncomeService;
    }

    private void Start()
    {
        _claimButton.onClick.AddListener(HandleClaimPressed);

        _offlineIncomeService.OnOfflineIncomeCalculated += Open;
    }
    private void OnDestroy()
    {
        if (_offlineIncomeService != null)
        {
            _offlineIncomeService.OnOfflineIncomeCalculated -= Open;
        }
    }
    private void Open(TimeSpan timePassed, double earnedGold)
    {
        _infoText.text = $"Вы отсутствовали:\n" +
                         $"{timePassed.Hours}ч {timePassed.Minutes}м {timePassed.Seconds}с\n\n" +
                         $"Вы накопили:\n" +
                         $"+{earnedGold:N0} золота";

        _panelTransform.gameObject.SetActive(true);

        _background.color = new Color(0, 0, 0, 0);
        _background.DOFade(0.6f, 0.25f);

        _windowTransform.localScale = Vector3.zero;
        _windowTransform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
    }

    private void HandleClaimPressed()
    {
        _background.DOFade(0f, 0.2f);
        _windowTransform.DOScale(Vector3.zero, 0.2f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                    _offlineIncomeService.ClaimGold();
                _panelTransform.gameObject.SetActive(false);
            });
    }
}
