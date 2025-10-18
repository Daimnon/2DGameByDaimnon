using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.SceneManagement;

// Next Button (if not last level - leads to next level, if crashed, restart level)
//             (if last level send to level select, remember to add bool in gameData)
//             (for if completed all levels and if so always apply send to next level)

public class GameManager : MonoBehaviour
{
    private const float TIME_STOPPED = 0.0f;
    private const float TIME_REGULAR = 1.0f;

    private Coroutine _levelTimerRoutine = null;

    private Action<int, float> _onUpdateTimeScoreEvent; // by order int = _maxLevelTime[_levelIndex], float = time left to finish level
    public Action<int, float> OnUpdateScoreEvent { get => _onUpdateTimeScoreEvent; set => _onUpdateTimeScoreEvent = value; }

    private Action<int> _onCheckLevelIndexEvent; // int = _levelIndex.
    public Action<int> OnCheckLevelIndexEvent { get => _onCheckLevelIndexEvent; set => _onCheckLevelIndexEvent = value; }

    private Action _onLevelStartEvent;
    public Action OnLevelStartEvent { get => _onLevelStartEvent; set => _onLevelStartEvent = value; }

    private Action<bool> _onLevelEndEvent; // bool is if finished (true) or ended from another reason (false)
    public Action<bool> OnLevelEndEvent { get => _onLevelEndEvent; set => _onLevelEndEvent = value; }

    [Header("Systems")]
    [SerializeField] private AudioManager _audioManager; public AudioManager AudioManager => _audioManager;
    [SerializeField] private CrashDetector _crashDetector;
    [SerializeField] private FinishLine _finishLine;

    [Header("Settings")]
    [SerializeField] private int _levelIndex;
    [SerializeField] private int[] _maxLevelTime; // lvl 1 = 40 (20 secs to finish level * 2)
    [SerializeField] private AudioClip[] _musicPerLevel;
    [SerializeField] private AudioClip _victoryJingle;
    [SerializeField] private AudioClip _failureJingle;
    private bool _finishedLevel = false;

    private void Start()
    {
        Time.timeScale = TIME_STOPPED; // pause
        _onLevelStartEvent += OnLevelStart;
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
        Debugger.Log("Invoked _onCheckLevelIndexEvent");

        _finishLine.OnLevelFinished += OnFinishedGame;
        Time.timeScale = TIME_REGULAR; // unpause

        StartTimer(_maxLevelTime[_levelIndex -1]);
        _audioManager.PlayMusic(_musicPerLevel[_levelIndex - 1]);
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
                _onUpdateTimeScoreEvent?.Invoke(_maxLevelTime[_levelIndex - 1], timeLeft);
                Debugger.Log("Invoked _onUpdateTimeScoreEvent when Player Crashed");
                _audioManager.StopMusic();
                _audioManager.PlayMusicOnShot(_failureJingle);
                yield return null;

                _onLevelEndEvent?.Invoke(false);
                Debugger.Log("Invoked _onLevelEndEvent after Player Crashed");

                _levelTimerRoutine = null;
                yield break;
            }

            if (_finishedLevel)
            {
                _onUpdateTimeScoreEvent?.Invoke(_maxLevelTime[_levelIndex - 1], timeLeft);
                Debugger.Log("Invoked _onUpdateTimeScoreEvent when Finished Level");
                _audioManager.PlayMusicOnShot(_victoryJingle);
                yield return null;

                _onLevelEndEvent?.Invoke(true);
                Debugger.Log("Invoked _onLevelEndEvent after Finished Level");

                _levelTimerRoutine = null;
                yield break;
            }

            yield return null;
        }

        _onUpdateTimeScoreEvent?.Invoke(_maxLevelTime[_levelIndex - 1], timeLeft); // if we reached here it's penalty time.
        Debugger.Log("Invoked _onUpdateTimeScoreEvent when Player is in Overtime");

        while (!_finishedLevel && !_crashDetector.HasCrashed) yield return null;
        _onLevelEndEvent?.Invoke(_finishedLevel);

        if (!_finishedLevel)
        {
            _audioManager.StopMusic();
            _audioManager.PlayMusicOnShot(_failureJingle);
        }
        _audioManager.PlayMusicOnShot(_victoryJingle);

        _levelTimerRoutine = null;
    }

    public void NextButton()
    {
        // quick and dirty for keeping the game loop
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex > SceneManager.sceneCount || !_finishedLevel) // check if actually fixes the last level issue
        {
            SceneManager.LoadScene(currentSceneIndex);
            return;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    private void OnFinishedGame()
    {
        _finishedLevel = true;

        /*int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int maxSceneCount = SceneManager.sceneCountInBuildSettings;
        if (currentSceneIndex < maxSceneCount) SceneManager.LoadScene(currentSceneIndex + 1); // reloads the first level*/
    }
}
