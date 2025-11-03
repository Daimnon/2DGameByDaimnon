using UnityEngine;
using System.Collections;

public class EarnCoinsAnimation : MonoBehaviour
{
    [SerializeField] private Transform _coinTr;
    [SerializeField] private GameObject _coinShadow;
    public GameObject CoinShadow => _coinShadow;

    [SerializeField] private float _bounceHeight = 50f;

    [SerializeField] private AnimationCurve _appearCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float _appearDuration = 0.3f;

    [SerializeField] private AnimationCurve _bounceCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0)); // peaks at middle
    [SerializeField] private float _bounceDuration = 0.7f;

    [SerializeField] private AnimationCurve _rotationCurve = AnimationCurve.Linear(0, 0, 1, 360);
    [SerializeField] private float _rotateDuration = 0.5f;

    [SerializeField] private AnimationCurve _dissapearCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField] private float _dissapearDuration = 0.5f;

    private RectTransform _coinRect;
    private Vector2 _originalAnchoredPosition;
    private Vector3 _originalScale;

    private void Awake()
    {
        if (_coinTr == null)
        {
            Debug.LogError("Coin Transform not assigned!");
            return;
        }

        _coinRect = _coinTr as RectTransform;
        if (_coinRect == null)
        {
            Debug.LogError("Coin Transform must be a UI element (RectTransform)!");
            return;
        }

        _originalAnchoredPosition = _coinRect.anchoredPosition;
        _originalScale = _coinTr.localScale;
        _coinTr.localScale = Vector3.zero; // start hidden
    }

    public Coroutine PlayCoinAnimation()
    {
        return StartCoroutine(AnimateCoin());
    }

    private IEnumerator AnimateCoin()
    {
        // --- APPEAR ---
        float timer = 0f;
        while (timer < _appearDuration)
        {
            timer += Time.deltaTime;
            float t = timer / _appearDuration;
            _coinTr.localScale = _originalScale * _appearCurve.Evaluate(t);
            yield return null;
        }

        // --- BOUNCE ---
        timer = 0f;
        while (timer < _bounceDuration)
        {
            timer += Time.deltaTime;
            float t = timer / _bounceDuration;

            float yOffset = _bounceHeight * _bounceCurve.Evaluate(t);
            _coinRect.anchoredPosition = _originalAnchoredPosition + new Vector2(0, yOffset);

            yield return null;
        }

        // --- ROTATE IN PLACE ---
        timer = 0f;
        while (timer < _rotateDuration)
        {
            timer += Time.deltaTime;
            float t = timer / _rotateDuration;

            _coinTr.localEulerAngles = new Vector3(0, 0, _rotationCurve.Evaluate(t));
            yield return null;
        }

        // --- SUCK OUT ---
        timer = 0f;
        Vector2 startPos = _coinRect.anchoredPosition;
        while (timer < _dissapearDuration)
        {
            timer += Time.deltaTime;
            float t = timer / _dissapearDuration;

            _coinTr.localScale = _originalScale * _dissapearCurve.Evaluate(t);
            yield return null;
        }

        // Reset
        _coinTr.localScale = Vector3.zero;
        _coinRect.anchoredPosition = _originalAnchoredPosition;
        _coinTr.localEulerAngles = Vector3.zero;
    }
}
