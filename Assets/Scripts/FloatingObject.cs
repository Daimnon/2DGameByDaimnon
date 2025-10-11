using System;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    private Coroutine _floatEffectRoutine;
    private Action _floatEvent;

    private Transform _spriteTr;

    [Header("Effects")]
    [SerializeField] private FloatEffect _floatEffect;

    private void Start()
    {
        _spriteTr = transform;
        _floatEvent += ResetFloatCoroutine;
        _floatEffectRoutine ??= StartCoroutine(_floatEffect.PlayFloatEffectRoutine(_spriteTr, _floatEvent));
    }
    private void OnDestroy()
    {
        _floatEvent -= ResetFloatCoroutine;
    }

    private void ResetFloatCoroutine()
    {
        _floatEffectRoutine = null;
        _floatEffectRoutine ??= StartCoroutine(_floatEffect.PlayFloatEffectRoutine(_spriteTr, _floatEvent));
    }
}
