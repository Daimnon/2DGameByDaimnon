using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIBounceEffect : BaseEffect
{
    [Header("Bounce Effect")]
    [SerializeField] private Vector3 _minIconBounceSize = Vector3.one;
    [SerializeField] private Vector3 _maxIconBounceSize = new(1.2f, 1.2f, 1.0f);

    // a reusable coroutined sequence that we can use a synchronicly, timed action within the game's constraints
    public IEnumerator PlayBounceEffectRoutine(Image imageToApplyEffect, Action clearRoutine)
    {
        float timeElapsed = 0f;
        while (timeElapsed < _duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / _duration;
            float curveValue = _curve.Evaluate(t);

            imageToApplyEffect.rectTransform.localScale = Vector3.LerpUnclamped(_minIconBounceSize, _maxIconBounceSize, curveValue);
            yield return null;
        }
        imageToApplyEffect.rectTransform.localScale = _minIconBounceSize;
        clearRoutine?.Invoke();
    }
}
