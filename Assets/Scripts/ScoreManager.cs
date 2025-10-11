using System;
using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _levelIndex = 1; // set from GameManager at start

    private int _totalFlipScore = 0;
    private int _totalTimeScore = 0;
    private int _totalLevelScore = 0;
    private int _totalScore = 0;

    private Action<int> _updateFlipScoreEvent;
    public Action<int> UpdateFlipScoreEvent { get => _updateFlipScoreEvent; set => _updateFlipScoreEvent = value; }

    private Action<int> _updateTimeScoreEvent;
    public Action<int> UpdateTimeScoreEvent { get => _updateTimeScoreEvent; set => _updateTimeScoreEvent = value; }

    private Action<int> _updateLevelScoreEvent;
    public Action<int> UpdateLevelScoreEvent { get => _updateLevelScoreEvent; set => _updateLevelScoreEvent = value; }

    [Header("Data")]
    [SerializeField] private int _flipScore = 100;
    [SerializeField] private int _maxTimeScore = 100;
    [SerializeField] private int _levelScore = 100;
    [SerializeField] private int _overTimePenalty = 50;

    [Header("Systems")]
    [SerializeField] private GameManager _gameManager;

    private void Start()
    {
        _gameManager.UpdateScoreEvent += OnLevelTimeCalculated;
        _gameManager.CheckLevelIndexEvent += SetLevelIndex;
    }
    private void OnDestroy()
    {
        _gameManager.UpdateScoreEvent -= OnLevelTimeCalculated;
        _gameManager.CheckLevelIndexEvent -= SetLevelIndex;
    }
    public void AddFlipScore(int currentFlipCount)
    {
        _totalFlipScore += currentFlipCount * _flipScore;
        _updateFlipScoreEvent?.Invoke(_totalFlipScore);
    }

    private void SetLevelIndex(int levelIndex)
    {
        _levelIndex = levelIndex;
    }
    private void OnLevelTimeCalculated(int maxLevelTime, float timeLeft)
    {
        float totalTimeScore = _maxTimeScore * _levelIndex;

        if (timeLeft <= -maxLevelTime * 0.25f) // if player reached overtime
        {
            totalTimeScore = -_overTimePenalty;
            _totalTimeScore = Mathf.RoundToInt(totalTimeScore);
            _updateTimeScoreEvent?.Invoke(_totalTimeScore);
            return;
        }

        // checking how much of the time should I decrease if I should at all
        float halfTime = maxLevelTime * 0.5f;
        if (timeLeft < halfTime)
        {
            float fraction = Mathf.Clamp01(timeLeft / halfTime); // ensures 0 ≤ fraction ≤ 1
            totalTimeScore *= fraction; // linear drop
        }

        _totalTimeScore = Mathf.RoundToInt(totalTimeScore);
        _updateTimeScoreEvent?.Invoke(_totalTimeScore);
    }
}
