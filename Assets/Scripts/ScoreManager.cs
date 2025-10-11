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

    private Action<int> _onUpdateFlipScoreEvent;
    public Action<int> OnUpdateFlipScoreEvent { get => _onUpdateFlipScoreEvent; set => _onUpdateFlipScoreEvent = value; }

    private Action<int> _onUpdateTimeScoreEvent;
    public Action<int> OnUpdateTimeScoreEvent { get => _onUpdateTimeScoreEvent; set => _onUpdateTimeScoreEvent = value; }

    private Action<int> _onUpdateLevelScoreEvent;
    public Action<int> OnUpdateLevelScoreEvent { get => _onUpdateLevelScoreEvent; set => _onUpdateLevelScoreEvent = value; }

    private Action<int> _onUpdateTotalScoreEvent;
    public Action<int> OnUpdateTotalScoreEvent { get => _onUpdateTotalScoreEvent; set => _onUpdateTotalScoreEvent = value; }

    [Header("Data")]
    [SerializeField] private int _flipScore = 25;
    [SerializeField] private int _maxTimeScore = 75;
    [SerializeField] private int _levelScore = 100;
    [SerializeField] private int _overTimePenalty = 50;

    [Header("Systems")]
    [SerializeField] private GameManager _gameManager;

    private void Start()
    {
        _gameManager.OnUpdateScoreEvent += OnLevelTimeCalculated;
        _gameManager.OnCheckLevelIndexEvent += SetLevelIndex;
        _gameManager.OnLevelEndEvent += CalculateScore;
    }
    private void OnDestroy()
    {
        _gameManager.OnUpdateScoreEvent -= OnLevelTimeCalculated;
        _gameManager.OnCheckLevelIndexEvent -= SetLevelIndex;
        _gameManager.OnLevelEndEvent -= CalculateScore;
    }

    public void AddFlipScore(int currentFlipCount)
    {
        _totalFlipScore += currentFlipCount * _flipScore;
        _onUpdateFlipScoreEvent?.Invoke(_totalFlipScore);
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
            _onUpdateTimeScoreEvent?.Invoke(_totalTimeScore);
            return;
        }

        // checking how much of the time should I decrease if I should at all
        float halfTime = maxLevelTime * 0.5f;
        if (timeLeft < halfTime)
        {
            float fraction = Mathf.Clamp01(timeLeft / halfTime); // ensures 0 ≤ fraction ≤ 1
            totalTimeScore *= fraction; // linear score drop
        }

        _totalTimeScore = Mathf.RoundToInt(totalTimeScore);
        _onUpdateTimeScoreEvent?.Invoke(_totalTimeScore);
    }

    private void CalculateScore(bool hasFinishedSuccessfully)
    {
        if (hasFinishedSuccessfully) CalculateScoreFinished();
        else CalculateScoreFailed();
    }
    private void CalculateScoreFinished()
    {
        _totalLevelScore = _levelScore * _levelIndex;
        _totalScore = _totalFlipScore + _totalTimeScore + _totalLevelScore;

        _onUpdateFlipScoreEvent?.Invoke(_totalFlipScore);
        _onUpdateTimeScoreEvent?.Invoke(_totalTimeScore);
        _onUpdateLevelScoreEvent?.Invoke(_totalLevelScore);
        _onUpdateTotalScoreEvent?.Invoke(_totalScore);
    }
    private void CalculateScoreFailed()
    {
        _totalFlipScore = 0;
        _totalLevelScore = 0;
        _totalTimeScore = 0;
        _totalScore = 0;
        _onUpdateFlipScoreEvent?.Invoke(_totalFlipScore);
        _onUpdateTimeScoreEvent?.Invoke(_totalTimeScore);
        _onUpdateLevelScoreEvent?.Invoke(_totalLevelScore);
        _onUpdateTotalScoreEvent?.Invoke(_totalScore);
    }
}
