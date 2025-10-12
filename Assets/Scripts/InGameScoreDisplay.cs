using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameScoreDisplay : MonoBehaviour
{
    private Coroutine _addScoreEffectRoutine;
    private Action _flipsIconBounceEvent;

    [Header("Systems")]
    [SerializeField] private ScoreManager _scoreManager;

    [Header("Components")]
    [SerializeField] private Image _flipsIcon;
    [SerializeField] private TextMeshProUGUI _flipsCounter;

    [Header("Animations")]
    [SerializeField] private UIBounceEffect _bounceEffect;

    private void Start()
    {
        _flipsIconBounceEvent += ClearFlipBounceCoroutine;
        _scoreManager.OnUpdateFlipScoreEvent += UpdateFlipScore;
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        _flipsIconBounceEvent -= ClearFlipBounceCoroutine;
    }

    private void UpdateFlipScore(int currentFlipScore)
    {
        _addScoreEffectRoutine ??= StartCoroutine(_bounceEffect.PlayBounceEffectRoutine(_flipsIcon, _flipsIconBounceEvent));
        _flipsCounter.text = currentFlipScore.ToString();
    }
    private void ClearFlipBounceCoroutine()
    {
        _addScoreEffectRoutine = null;
    }
}
