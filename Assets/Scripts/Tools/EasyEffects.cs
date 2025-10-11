using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EasyEffects : MonoBehaviour
{
    [Header("Bounce Effect")]
    [SerializeField] private AnimationCurve _iconBounce;
    [SerializeField] private float _iconBounceTime = 0.2f;
    [SerializeField] private Vector3 _minIconBounceSize = Vector3.one;
    [SerializeField] private Vector3 _maxIconBounceSize = new(1.2f, 1.2f, 1.0f);

    // a reusable coroutined sequence that we can use a synchronicly, timed action within the game's constraints
    public IEnumerator PlayBounceEffectRoutine(Image imageToApplyEffect, Action clearRoutine)
    {
        float timeElapsed = 0f;
        while (timeElapsed < _iconBounceTime)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / _iconBounceTime;
            float curveValue = _iconBounce.Evaluate(t);

            imageToApplyEffect.rectTransform.localScale = Vector3.LerpUnclamped(_minIconBounceSize, _maxIconBounceSize, curveValue);
            yield return null;
        }
        imageToApplyEffect.rectTransform.localScale = _minIconBounceSize;
        clearRoutine?.Invoke();
    }

    [Header("Float Effect")]
    [SerializeField] private AnimationCurve _floatCurve;

    // a reusable coroutined sequence that we can use a synchronicly, timing action within the game's constraints
    public IEnumerator PlayFloatEffectRoutine(Transform spriteTr, float heightOffset, float duration, Action clearRoutine) 
    {
        float timeElapsed = 0f;
        float startY = transform.position.y;
        float xPos = transform.position.x;
        float zPos = transform.position.z;

        Vector3 startPos = new(xPos, startY, zPos);
        Vector3 highestPos = new(xPos, startY + heightOffset, zPos);
        Vector3 lowestPos = new(xPos, startY - heightOffset, zPos);

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            float curveValue = _floatCurve.Evaluate(t);

            spriteTr.position = Vector3.LerpUnclamped(startPos, highestPos, curveValue);
            yield return null;
        }
        timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            float curveValue = _floatCurve.Evaluate(t);

            spriteTr.position = Vector3.LerpUnclamped(startPos, lowestPos, curveValue);
            yield return null;
        }
        spriteTr.position = startPos;
        clearRoutine?.Invoke();
    }
}
