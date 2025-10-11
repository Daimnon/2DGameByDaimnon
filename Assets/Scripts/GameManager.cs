using System;
using System.Collections;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const float TIME_STOPPED = 0.0f;
    private const float TIME_REGULAR = 1.0f;

    private bool _finishedLevel = false;

    private Coroutine _levelTimerRoutine = null;

    private Action<int, float> _onUpdateTimeScoreEvent; // by order int = _maxLevelTime[_levelIndex], float = time left to finish level
    public Action<int, float> OnUpdateScoreEvent { get => _onUpdateTimeScoreEvent; set => _onUpdateTimeScoreEvent = value; }

    private Action<int> _onCheckLevelIndexEvent; // int = _levelIndex.
    public Action<int> OnCheckLevelIndexEvent { get => _onCheckLevelIndexEvent; set => _onCheckLevelIndexEvent = value; }

    private Action _onLevelStartEvent;
    public Action OnLevelStartEvent { get => _onLevelStartEvent; set => _onLevelStartEvent = value; }

    private Action<bool> _onLevelEndEvent; // bool is if finished (true) or ended from another reason (false)
    public Action<bool> OnLevelEndEvent { get => _onLevelEndEvent; set => _onLevelEndEvent = value; }

    [Header("Data")]
    [SerializeField] private int _levelIndex;
    [SerializeField] private int[] _maxLevelTime;

    [Header("Systems")]
    [SerializeField] private CrashDetector _crashDetector;
    [SerializeField] private FinishLine _finishLine;

    private void Start()
    {
        Time.timeScale = TIME_STOPPED; // pause
    }
    private void OnDestroy()
    {
        _finishLine.OnLevelFinished -= OnFinishedGame;
    }

    public void StartTimer(float duration)
    {
        _levelTimerRoutine ??= StartCoroutine(LevelTimerRoutine(duration));
    }
    public void OnLevelStart()
    {
        _onCheckLevelIndexEvent?.Invoke(_levelIndex);
        _finishLine.OnLevelFinished += OnFinishedGame;
        Time.timeScale = TIME_REGULAR; // unpause
    }
    public void SetLevelFinished(bool isTrue)
    {
        _finishedLevel = isTrue;
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
                _onUpdateTimeScoreEvent?.Invoke(_maxLevelTime[_levelIndex], timeLeft);
                yield return null;

                _onLevelEndEvent?.Invoke(false);
                _levelTimerRoutine = null;
                yield break;
            }

            if (_finishedLevel)
            {
                _onUpdateTimeScoreEvent?.Invoke(_maxLevelTime[_levelIndex], timeLeft);
                yield return null;

                _onLevelEndEvent?.Invoke(true);
                _levelTimerRoutine = null;
                yield break;
            }

            yield return null;
        }

        _onUpdateTimeScoreEvent?.Invoke(_maxLevelTime[_levelIndex], timeLeft); // if we reached here it's penalty time.
        _levelTimerRoutine = null;
    }

    private void OnFinishedGame()
    {
        _finishedLevel = true;

        /*int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int maxSceneCount = SceneManager.sceneCountInBuildSettings;
        if (currentSceneIndex < maxSceneCount) SceneManager.LoadScene(currentSceneIndex + 1); // reloads the first level*/
    }
}
