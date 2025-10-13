using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField, Range(0.0f, 1.0f)] private float _musicVolume = 0.75f;
    [SerializeField] private AudioSource _musicSource;

    [Header("Sounds")]
    [SerializeField, Range(0.0f, 1.0f)] private float _soundsVolume = 0.75f;
    [SerializeField] private float _crossfadeDuration = 0.3f; // how fast to crossfade
    [SerializeField] private AudioSource _soundsSource1;
    [SerializeField] private AudioSource _soundsSource2;
    private bool _isSource1Active = true;
    private Coroutine _crossfadeRoutine;

    /// <summary>
    /// If both are silent, play on source 1. If source1 is active, crossfade to source2. If source2 is active, crossfade back to source1
    /// </summary>
    /// <param name="clip">The to be played clip</param>
    public void PlaySound(AudioClip clip)
    {
        if (!_soundsSource1.isPlaying && !_soundsSource2.isPlaying)
        {
            _isSource1Active = true;
            _soundsSource1.clip = clip;
            _soundsSource1.volume = 1f;
            _soundsSource1.Play();
            return;
        }

        if (_isSource1Active)
        {
            if (_crossfadeRoutine != null) StopCoroutine(_crossfadeRoutine);
            _crossfadeRoutine = StartCoroutine(Crossfade(_soundsSource1, _soundsSource2, clip));
            _isSource1Active = false;
        }
        else
        {
            if (_crossfadeRoutine != null) StopCoroutine(_crossfadeRoutine);
            _crossfadeRoutine = StartCoroutine(Crossfade(_soundsSource2, _soundsSource1, clip));
            _isSource1Active = true;
        }
    }
    private IEnumerator Crossfade(AudioSource fromSource, AudioSource toSource, AudioClip newClip)
    {
        toSource.clip = newClip;
        toSource.volume = 0f;
        toSource.Play();

        float time = 0f;

        while (time < _crossfadeDuration)
        {
            time += Time.unscaledDeltaTime; // unscaled so it works even if time is paused
            float t = time / _crossfadeDuration;

            fromSource.volume = Mathf.Lerp(1f, 0f, t);
            toSource.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        fromSource.Stop();
        toSource.volume = 1f;
        fromSource.volume = 0f;
    }

    #region Unity Events
    public void ToggleMusicVolume(bool isOn)
    {
        _musicSource.volume = isOn ? _musicVolume : 0f;
    }
    public void ToggleSoundsVolume(bool isOn)
    {
        _soundsSource1.volume = isOn ? _soundsVolume : 0f;
        _soundsSource2.volume = isOn ? _soundsVolume : 0f;
    }
    #endregion
}
