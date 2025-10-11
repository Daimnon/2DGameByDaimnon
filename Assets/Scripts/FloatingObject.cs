using System;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    private Coroutine _floatEffectRoutine;
    private Action _onFloatEvent;

    private Transform _spriteTr;

    [Header("Effects")]
    [SerializeField] private FloatEffect _floatEffect;

    private void Start()
    {
        _spriteTr = transform;
        _onFloatEvent += ResetFloatCoroutine;
        _floatEffectRoutine ??= StartCoroutine(_floatEffect.PlayFloatEffectRoutine(_spriteTr, _onFloatEvent));
    }
    private void OnDestroy()
    {
        _onFloatEvent -= ResetFloatCoroutine;
    }

    private void ResetFloatCoroutine()
    {
        _floatEffectRoutine = null;
        _floatEffectRoutine ??= StartCoroutine(_floatEffect.PlayFloatEffectRoutine(_spriteTr, _onFloatEvent));
    }
}
