using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float TIME_STOPPED = 0.0f;
    private float _finishLevelTimer = 0;
    private bool _finishedLevel = false;

    private Coroutine _levelTimerRoutine = null;

    private Action<int, float> _updateTimeScoreEvent; // by order int = _maxLevelTime[_levelIndex], float = time left to finish level
    public Action<int, float> UpdateScoreEvent { get => _updateTimeScoreEvent; set => _updateTimeScoreEvent = value; }

    private Action<int> _checkLevelIndexEvent; // int = _levelIndex.
    public Action<int> CheckLevelIndexEvent { get => _checkLevelIndexEvent; set => _checkLevelIndexEvent = value; }

    [Header("Data")]
    [SerializeField] private int _levelIndex;
    [SerializeField] private int[] _maxLevelTime;

    [Header("Systems")]
    [SerializeField] private CrashDetector _crashDetector;
    [SerializeField] private CharacterSelect _characterSelection;

    private void Start()
    {
        Time.timeScale = TIME_STOPPED;
        _checkLevelIndexEvent?.Invoke(_levelIndex);
    }

    public void StartTimer(float duration)
    {
        _levelTimerRoutine ??= StartCoroutine(LevelTimerRoutine(duration));
    }
    private IEnumerator LevelTimerRoutine(float maxDuration)
    {
        float timeElapsed = 0f;
        float timeLeft = maxDuration;
        float overtimeThreshold = -maxDuration * 0.25f; // overtime is a quarter 

        while (timeLeft > overtimeThreshold)
        {
            timeElapsed += Time.deltaTime;
            timeLeft = maxDuration - timeElapsed; // can go negative after 0

            if (_crashDetector.HasCrashed)
            {
                timeLeft = 0.0f;
                _updateTimeScoreEvent?.Invoke(_maxLevelTime[_levelIndex], timeLeft);
                _levelTimerRoutine = null;
                yield break;
            }

            if (_finishedLevel)
            {
                _updateTimeScoreEvent?.Invoke(_maxLevelTime[_levelIndex], timeLeft);
                _levelTimerRoutine = null;
                yield break;
            }

            yield return null;
        }

        _updateTimeScoreEvent?.Invoke(_maxLevelTime[_levelIndex], timeLeft); // if we reached here it's penalty time.
        _levelTimerRoutine = null;
    }
}
