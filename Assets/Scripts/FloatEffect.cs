using System.Collections;
using TMPro;
using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _spriteTr;
    [SerializeField] private AnimationCurve _curve;

    [Header("Data")]
    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private float _yOffset = 0.2f;
    private Coroutine _floatEffectRoutine;

    private void Start()
    {
        _floatEffectRoutine = StartCoroutine(PlayFloatEffectRoutine());
    }

    private IEnumerator PlayFloatEffectRoutine()
    {
        float timeElapsed = 0f;
        float startY = transform.position.y;
        float xPos = transform.position.x;
        float zPos = transform.position.z;

        Vector3 startPos = new(xPos, startY, zPos);
        Vector3 abovePos = new(xPos, startY + _yOffset, zPos);
        Vector3 belowPos = new(xPos, startY - _yOffset, zPos);

        while (timeElapsed < _duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / _duration;
            float curveValue = _curve.Evaluate(t);

            _spriteTr.position = Vector3.LerpUnclamped(startPos, abovePos, curveValue);
            yield return null;
        }
        timeElapsed = 0f;

        while (timeElapsed < _duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / _duration;
            float curveValue = _curve.Evaluate(t);

            _spriteTr.position = Vector3.LerpUnclamped(startPos, belowPos, curveValue);
            yield return null;
        }
        _spriteTr.position = startPos;
        _floatEffectRoutine = null;

        _floatEffectRoutine = StartCoroutine(PlayFloatEffectRoutine());
    }
}
