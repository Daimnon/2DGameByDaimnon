using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
{
    private bool _currentValue;
    public bool CurrentValue => _currentValue;

    private bool _previousValue;
    private Coroutine _sliderAnimationRoutine;
    private Action _transitionEffect;
    private ToggleSwitchGroupManager _toggleSwitchGroupManager;

    [Header("Data")]
    [SerializeField, Range(0.0f, 1.0f)] private float _sliderValue;
    [SerializeField, Range(0.0f, 1.0f)] private float _duration = 0.5f;
    [SerializeField] private AnimationCurve _curve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    [SerializeField] private Color _onColor = new(0.4f, 1f, 0.4f, 1f);   // Greenish
    [SerializeField] private Color _offColor = new(0.6f, 0.6f, 0.6f, 1f); // Grayish

    [Header("Components")]
    [SerializeField] private Slider _slider;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent _onToggleTrue;
    [SerializeField] private UnityEvent _onToggleFalse;

    private void Start()
    {
        SetupSliderComponent();
    }
    private void OnValidate()
    {
        SetupToggleComponents();
        _slider.value = _sliderValue;
    }
    private void SetupToggleComponents()
    {
        SetupSliderComponent();
    }
    private void SetupSliderComponent()
    {
        _slider.interactable = false;
        ColorBlock sliderColors = _slider.colors;
        sliderColors.disabledColor = Color.white;
        _slider.colors = sliderColors;
        _slider.transition = Selectable.Transition.None;
    }
    public void SetupForManager(ToggleSwitchGroupManager manager)
    {
        _toggleSwitchGroupManager = manager;
    }
    private void Toggle()
    {
        if (_toggleSwitchGroupManager != null) _toggleSwitchGroupManager.ToggleGroup(this);
        else SetStateAndStartAnimation(!CurrentValue);
    }
    public void ToggleByGroupManager(bool valueToSetTo)
    {
        SetStateAndStartAnimation(valueToSetTo);
    }

    private void SetStateAndStartAnimation(bool state)
    {
        _previousValue = CurrentValue;
        _currentValue = state;

        if (_previousValue != CurrentValue)
        {
            if (CurrentValue) _onToggleTrue?.Invoke();
            else _onToggleFalse?.Invoke();
        }

        if (_sliderAnimationRoutine != null) StopCoroutine(_sliderAnimationRoutine);
        _sliderAnimationRoutine = StartCoroutine(AnimateSlider());
    }

    private IEnumerator AnimateSlider()
    {
        float startValue = _slider.value;
        float endValue = CurrentValue ? 1 : 0;

        float time = 0;
        if (_duration > 0)
        {
            while (time < _duration)
            {
                time += Time.unscaledDeltaTime;

                float lerpFactor = _curve.Evaluate(time / _duration);
                _slider.value = _sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);

                _transitionEffect?.Invoke();

                yield return null;
            }
        }

        _slider.value = endValue;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
        Debugger.Log("Toggled");
    }
}
