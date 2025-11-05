using UnityEngine;
using System.Collections;
using TMPro;

public class EarnCoinsAnimation : MonoBehaviour
{
    [Header("Systems")]
    [SerializeField] private ScoreManager _scoreManager;

    [Header("Finish Level Successfuly Animation")]
    [SerializeField] private Transform _coinTr;
    [SerializeField] private GameObject _coinShadow;
    public GameObject CoinShadow => _coinShadow;

    [SerializeField] private TextMeshProUGUI _earningsText;
    [SerializeField] private RectTransform _textDestination;

    [SerializeField] private float _bounceHeight = 50f;
    [SerializeField] private float _textMoveFactor = 3.0f;

    [SerializeField] private AnimationCurve _appearCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float _appearDuration = 0.3f;

    [SerializeField] private AnimationCurve _bounceCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.4f, 1), new Keyframe(0.57f, 0.84f), new Keyframe(1, 0));
    [SerializeField] private float _bounceDuration = 0.7f;

    [SerializeField] private AnimationCurve _rotationCurve = AnimationCurve.Linear(0, 0, 1, 720);
    [SerializeField] private float _rotateDuration = 0.5f;

    [SerializeField] private AnimationCurve _dissapearCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField] private float _dissapearDuration = 0.5f;

    private RectTransform _coinRect;
    private RectTransform _textRect;
    private Vector2 _originalAnchoredPosition;
    private Vector3 _originalScale;
    private Vector3 _originalTextScale;

    private void Awake()
    {
        _scoreManager.OnUpdateTotalScoreEvent += UpdateEarningsText;
        _coinRect = _coinTr as RectTransform;
        _textRect = _earningsText.rectTransform;
        _originalAnchoredPosition = _coinRect.anchoredPosition;
        _originalScale = _coinTr.localScale;
        _originalTextScale = _textRect.localScale;
        _textRect.localScale = Vector3.zero;
        _coinTr.localScale = Vector3.zero;
        _coinShadow.SetActive(false);
    }
    private void OnDestroy()
    {
        _scoreManager.OnUpdateTotalScoreEvent -= UpdateEarningsText;
    }

    public Coroutine PlayCoinAnimation()
    {
        return StartCoroutine(AnimateCoin());
    }

    private IEnumerator AnimateCoin()
    {
        // --- appear ---
        float timer = 0f;
        while (timer < _appearDuration)
        {
            timer += Time.deltaTime;
            float t = timer / _appearDuration;
            _coinTr.localScale = _originalScale * _appearCurve.Evaluate(t);
            yield return null;
        }

        // --- bounce ---
        timer = 0f;
        while (timer < _bounceDuration)
        {
            timer += Time.deltaTime;
            float t = timer / _bounceDuration;

            float yOffset = _bounceHeight * _bounceCurve.Evaluate(t);
            _coinRect.anchoredPosition = _originalAnchoredPosition + new Vector2(0, yOffset);

            yield return null;
        }

        // --- rotate & move text ---
        timer = 0f;
        Vector2 textStartPos = _originalAnchoredPosition;
        Vector2 textEndPos = _textDestination.anchoredPosition;
        float textScaleInDuration = _rotateDuration * 0.3f;
        float textScaleOutStart = _rotateDuration * 0.7f;

        while (timer < _rotateDuration)
        {
            timer += Time.deltaTime;
            float t = timer / _rotateDuration;

            // rotate coin
            _coinTr.localEulerAngles = new Vector3(0, 0, _rotationCurve.Evaluate(t));
            

            if (_textRect != null)
            {
                // move text from coin to destination
                float moveT = Mathf.Clamp01(t * _textMoveFactor);
                _textRect.anchoredPosition = Vector2.Lerp(textStartPos, textEndPos, moveT);

                // scale in/out
                float scaleValue = 1f;
                if (timer < textScaleInDuration)
                {
                    float sT = timer / textScaleInDuration;
                    scaleValue = Mathf.Lerp(0f, 1f, sT);
                }
                else if (timer > textScaleOutStart)
                {
                    float sT = (timer - textScaleOutStart) / (_rotateDuration - textScaleOutStart);
                    scaleValue = Mathf.Lerp(1f, 0f, sT);
                }

                _textRect.localScale = _originalTextScale * scaleValue;
            }

            yield return null;
        }

        // --- dissapear ---
        timer = 0f;
        Vector2 startPos = _coinRect.anchoredPosition;
        while (timer < _dissapearDuration)
        {
            timer += Time.deltaTime;
            float t = timer / _dissapearDuration;

            _coinTr.localScale = _originalScale * _dissapearCurve.Evaluate(t);
            yield return null;
        }

        // --- reset ---
        _coinTr.localScale = Vector3.zero;
        _coinRect.anchoredPosition = _originalAnchoredPosition;
        _coinTr.localEulerAngles = Vector3.zero;
    }
    private void UpdateEarningsText(int totalScore)
    {
        _earningsText.text = _scoreManager.CalculateCurrencyFromScore(totalScore).ToString();
    }
}
