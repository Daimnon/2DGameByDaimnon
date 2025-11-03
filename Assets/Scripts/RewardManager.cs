using DG.Tweening;
using TMPro;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [Header("General Coin Animations Settings")]
    [SerializeField] private RectTransform _coinsAnimationParent;
    [SerializeField] private Transform _inventoryCoinTr;
    [SerializeField] private Transform[] _subCoinsTrs;
    [SerializeField] private TextMeshProUGUI _coinsTMP;

    [Header("Award Coins Animation Settings")]
    [SerializeField, Tooltip("x = endValue, y = duration")] private Vector2 _awardDoScale = new(1.0f, 0.3f);
    [SerializeField] private float _awardDelayIncrements = 0.0f;
    [SerializeField] private float _awardDuration = 1.0f;
    [SerializeField] private Transform _scoreToCoinsTr;

    [Header("Spend Coins Animation Settings")]
    [SerializeField] private Transform _coinsToNothingTr;
    private Vector3[] _initialSpendSubCoinsPos;
    private Quaternion[] _initialSpendSubCoinsRots;

    private void Start()
    {
        for (int i = 0; i < _subCoinsTrs.Length; i++)
        {
            _initialSpendSubCoinsPos[i] = _subCoinsTrs[i].position;
            _initialSpendSubCoinsRots[i] = _subCoinsTrs[i].rotation;
        }
    }
    private void Reset()
    {
        for (int i = 0; i < _subCoinsTrs.Length; i++)
        {
            _subCoinsTrs[i].position = _initialSpendSubCoinsPos[i];
            _subCoinsTrs[i].rotation = _initialSpendSubCoinsRots[i];
        }
    }

    private void AwardCoinsToPlayer()
    {
        for (int i = 0; i < _subCoinsTrs.Length; i++)
        {
            _subCoinsTrs[i].DOScale(_awardDoScale.x, _awardDoScale.y).SetDelay(_awardDelayIncrements).SetEase(Ease.OutBack);
            _coinsAnimationParent.DOAnchorPos(_coinsAnimationParent.anchoredPosition, _awardDuration).SetDelay(_awardDelayIncrements).SetEase(Ease.OutBack);
        }
    }
    private void SpendCoins()
    {

    }
}
