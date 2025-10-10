using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    int _totalScore = 0;

    [Header("Data")]
    [SerializeField] private int _flipScore = 100;

    [Header("Components")]
    [SerializeField] private Image _flipsIcon;
    [SerializeField] private TextMeshProUGUI _flipsCounter;

    [Header("Effects")]
    [SerializeField] private AnimationCurve _flipsIconBounce;
    [SerializeField] private float _flipIconBounceTime = 0.2f;
    [SerializeField] private Vector3 _startFlipIconBounceSize = Vector3.one;
    [SerializeField] private Vector3 _endFlipIconBounceSize = new(1.2f, 1.2f, 1.0f);
    private Coroutine _addScoreEffectRoutine;

    public void SetScore(int currentFlipCount)
    {
        _totalScore = currentFlipCount * _flipScore;
        UpdateScore();
    }
    private void UpdateScore()
    {
        _addScoreEffectRoutine ??= StartCoroutine(PlayAddScoreEffectRoutine());
        _flipsCounter.text = _totalScore.ToString();
    }

    private IEnumerator PlayAddScoreEffectRoutine() // a reusable coroutined sequence that we can use a synchronicly, timing action within the game's constraints
    {
        float timeElapsed = 0f;
        while (timeElapsed < _flipIconBounceTime)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / _flipIconBounceTime;
            float curveValue = _flipsIconBounce.Evaluate(t);

            _flipsIcon.rectTransform.localScale = Vector3.LerpUnclamped(_startFlipIconBounceSize, _endFlipIconBounceSize, curveValue);
            yield return null;
        }
        _flipsIcon.rectTransform.localScale = _startFlipIconBounceSize;
        _addScoreEffectRoutine = null;
    }
}
