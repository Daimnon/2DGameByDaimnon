using System;
using System.Collections;
using UnityEngine;

public class FloatEffect : BaseEffect
{
    [SerializeField] private float _heightOffset = 0.2f;
    public IEnumerator PlayFloatEffectRoutine(Transform spriteTr, Action clearRoutine)
    {
        float timeElapsed = 0f;
        float startY = transform.position.y;
        float xPos = transform.position.x;
        float zPos = transform.position.z;

        Vector3 startPos = new(xPos, startY, zPos);
        Vector3 highestPos = new(xPos, startY + _heightOffset, zPos);
        Vector3 lowestPos = new(xPos, startY - _heightOffset, zPos);

        while (timeElapsed < _duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / _duration;
            float curveValue = _curve.Evaluate(t);

            spriteTr.position = Vector3.LerpUnclamped(startPos, highestPos, curveValue);
            yield return null;
        }
        timeElapsed = 0f;

        while (timeElapsed < _duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / _duration;
            float curveValue = _curve.Evaluate(t);

            spriteTr.position = Vector3.LerpUnclamped(startPos, lowestPos, curveValue);
            yield return null;
        }
        spriteTr.position = startPos;
        clearRoutine?.Invoke();
    }
}
