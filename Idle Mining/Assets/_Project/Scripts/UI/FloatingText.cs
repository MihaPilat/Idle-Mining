using UnityEngine;
using TMPro;
using DG.Tweening;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Spawn(string value, Vector3 spawnPosition, PoolFactory poolFactory, FloatingText prefabKey)
    {
        _text.text = value;
        _rectTransform.position = spawnPosition;

        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1f);
        _rectTransform.localScale = Vector3.one;

        Sequence sequence = DOTween.Sequence();

        float randomX = Random.Range(-40f, 40f);
        float randomY = Random.Range(120f, 160f);

        sequence.Join(_rectTransform.DOBlendableMoveBy(new Vector3(randomX, randomY, 0), 0.7f).SetEase(Ease.OutCubic));
        sequence.Join(_rectTransform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.15f, 1));
        sequence.Insert(0.35f, _text.DOFade(0f, 0.35f));

        sequence.OnComplete(() => poolFactory.Reclaim(this, prefabKey));
    }
}
