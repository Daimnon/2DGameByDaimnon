//using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndGameScoreDisplay : MonoBehaviour
{
    //private Coroutine _addScoreEffectRoutine;
    //private Action _flipsIconBounceEvent;

    [Header("Systems")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private ScoreManager _scoreManager;

    [Header("Data")]
    [SerializeField] private int _timeToShowNextBtn = 1;
    [SerializeField] private string _crashedTitle = "Crashed";

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI _endPanelTitle;

    [SerializeField] private Image _flipsIconBg;
    [SerializeField] private Image _flipsIcon;
    [SerializeField] private Sprite _flipsCrashedIcon;
    [SerializeField] private TextMeshProUGUI _flipsScore;

    [SerializeField] private Image _timeIconBg;
    [SerializeField] private Image _timeIcon;
    [SerializeField] private Sprite _timeCrashedIcon;
    [SerializeField] private TextMeshProUGUI _timeScore;

    [SerializeField] private Image _levelIconBg;
    [SerializeField] private Image _levelIcon;
    [SerializeField] private Sprite _levelCrashedIcon;
    [SerializeField] private TextMeshProUGUI _levelScore;

    [SerializeField] private Image _totalScoreIconBg;
    [SerializeField] private Image _totalScoreIcon;
    [SerializeField] private Sprite _totalScoreCrashedIcon;
    [SerializeField] private TextMeshProUGUI _totalScore;

    [SerializeField] private GameObject _nextBtnBg;

    /*[Header("Animations")]
    [SerializeField] private UIBounceEffect _bounceEffect;*/ // should be smale dance effect and not bounce.

    private void Start()
    {
        //_flipsIconBounceEvent += ClearFlipBounceCoroutine;
        _scoreManager.OnUpdateFlipScoreEvent += UpdateFlipScore;
        _scoreManager.OnUpdateTimeScoreEvent += UpdateTimeScore;
        _scoreManager.OnUpdateLevelScoreEvent += UpdateLevelScore;
        _scoreManager.OnUpdateTotalScoreEvent += UpdateTotalScore;
        _gameManager.OnLevelEndEvent += RevealScoreWindow;
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        //_flipsIconBounceEvent -= ClearFlipBounceCoroutine;
        _scoreManager.OnUpdateFlipScoreEvent -= UpdateFlipScore;
        _scoreManager.OnUpdateTimeScoreEvent -= UpdateTimeScore;
        _scoreManager.OnUpdateLevelScoreEvent -= UpdateLevelScore;
        _scoreManager.OnUpdateTotalScoreEvent -= UpdateTotalScore;
        _gameManager.OnLevelEndEvent -= RevealScoreWindow;
    }

    private void UpdateFlipScore(int currentFlipScore)
    {
        //_addScoreEffectRoutine ??= StartCoroutine(_bounceEffect.PlayBounceEffectRoutine(_flipsIcon, _flipsIconBounceEvent));
        _flipsScore.text = currentFlipScore.ToString();
    }
    private void UpdateTimeScore(int currentTimeScore)
    {
        //_addScoreEffectRoutine ??= StartCoroutine(_bounceEffect.PlayBounceEffectRoutine(_flipsIcon, _flipsIconBounceEvent));
        _timeScore.text = currentTimeScore.ToString();
    }
    private void UpdateLevelScore(int currentLevelScore)
    {
        //_addScoreEffectRoutine ??= StartCoroutine(_bounceEffect.PlayBounceEffectRoutine(_flipsIcon, _flipsIconBounceEvent));
        _levelScore.text = currentLevelScore.ToString();
    }
    private void UpdateTotalScore(int currentTotalScore)
    {
        //_addScoreEffectRoutine ??= StartCoroutine(_bounceEffect.PlayBounceEffectRoutine(_flipsIcon, _flipsIconBounceEvent));
        _totalScore.text = currentTotalScore.ToString();
    }

    private void RevealScoreWindow(bool hasFinishedSuccessfully)
    {
        if (hasFinishedSuccessfully) RevealVictoryPanel();
        else RevealCrashedPanel();
    }
    private void RevealVictoryPanel()
    {
        gameObject.SetActive(true);
        StartCoroutine(VictoryPanelAnimations());
    }
    private void RevealCrashedPanel()
    {
        _endPanelTitle.color = ColorCoding.Bad;
        _endPanelTitle.text = _crashedTitle;
        _flipsIcon.sprite = _flipsCrashedIcon;
        _timeIcon.sprite = _timeCrashedIcon;
        _levelIcon.sprite = _levelCrashedIcon;
        _totalScoreIcon.sprite = _totalScoreCrashedIcon;
        gameObject.SetActive(true);
        StartCoroutine(CrashedPanelAnimations());
    }
    private IEnumerator VictoryPanelAnimations()
    {
        // quick n dirty animations before dotween
        _flipsIconBg.transform.parent.gameObject.SetActive(true); // really really really bad.
        yield return new WaitForSeconds(0.2f);
        _timeIconBg.transform.parent.gameObject.SetActive(true); // really really really bad.
        yield return new WaitForSeconds(0.2f);
        _levelIconBg.transform.parent.gameObject.SetActive(true); // really really really bad.
        yield return new WaitForSeconds(0.2f);
        _totalScoreIconBg.transform.parent.gameObject.SetActive(true); // really really really bad.
        yield return new WaitForSeconds(_timeToShowNextBtn);

        _nextBtnBg.SetActive(true);
    }
    private IEnumerator CrashedPanelAnimations()
    {
        // quick n dirty animations before dotween
        _flipsIconBg.transform.parent.gameObject.SetActive(true); // really really really bad.
        yield return new WaitForSeconds(0.2f);
        _timeIconBg.transform.parent.gameObject.SetActive(true); // really really really bad.
        yield return new WaitForSeconds(0.2f);
        _levelIconBg.transform.parent.gameObject.SetActive(true); // really really really bad.
        yield return new WaitForSeconds(0.2f);
        _totalScoreIconBg.transform.parent.gameObject.SetActive(true); // really really really bad.
        yield return new WaitForSeconds(_timeToShowNextBtn);

        _nextBtnBg.SetActive(true);
    }

    // scores should appear one after another, after SHORT time to read reveal the next btn

    /*private void ClearFlipBounceCoroutine()
    {
        //_addScoreEffectRoutine = null;
    }*/
}
