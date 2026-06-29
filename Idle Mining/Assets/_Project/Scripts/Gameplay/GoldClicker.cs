using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;
using UnityEngine.InputSystem;

public class GoldClicker : MonoBehaviour
{
    [SerializeField] private Button _button;

    private CurrencyService _currencyService;
    private FloatingTextService _floatingTextService;
    private FirebaseService _firebaseService;

    [Inject]
    private void Construct(CurrencyService currencyService, FloatingTextService floatingTextService, FirebaseService firebaseService)
    {
        _currencyService = currencyService;
        _floatingTextService = floatingTextService;
        _firebaseService = firebaseService;
    }

    private void Start()
    {
        _button.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        double clickPower = 1;
        _currencyService.AddGold(clickPower);

        Vector2 screenPos2D = Vector2.zero;

        if (Mouse.current != null)
        {
            screenPos2D = Mouse.current.position.ReadValue();
        }
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.isInProgress)
        {
            screenPos2D = Touchscreen.current.primaryTouch.position.ReadValue();
        }

        Vector3 clickPosition = screenPos2D;
        _floatingTextService.ShowText($"+{clickPower}", clickPosition);

        transform.DOComplete();

        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f);

        _firebaseService.SaveToCloud();
    }
}
