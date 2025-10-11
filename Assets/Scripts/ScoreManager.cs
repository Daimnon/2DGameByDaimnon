using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _totalScore = 0;

    [Header("Data")]
    [SerializeField] private int _flipScore = 100;

    private Action<int> _updateScoreEvent;
    public Action<int> UpdateScoreEvent { get => _updateScoreEvent; set => _updateScoreEvent = value; }

    public void AddScore(int currentFlipCount)
    {
        _totalScore += currentFlipCount * _flipScore;
        _updateScoreEvent?.Invoke(_totalScore);
    }
}
