using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField, Range(0.0f, 1.0f)] private float _musicVolume = 0.75f;
    [SerializeField] private AudioSource _musicSource;

    [Header("Sounds")]
    [SerializeField, Range(0.0f, 1.0f)] private float _soundsVolume = 0.75f;
    [SerializeField] private float _minPitch = 0.9f;
    [SerializeField] private float _maxPitch = 1.1f;
    [SerializeField] private float _crossfadeDuration = 0.3f; // how fast to crossfade
    [SerializeField] private AudioSource _soundsSource1;
    [SerializeField] private AudioSource _soundsSource2;
    private bool _isSource1Active = true;
    private Coroutine _crossfadeRoutine;
    private Coroutine _repeatedlyPlayedRoutine;
    private Coroutine _sequentialyPlayedRoutine;

    /// <summary>
    /// If both are silent, play on source 1. If source1 is active, crossfade to source2. If source2 is active, crossfade back to source1
    /// </summary>
    /// <param name="clip">The to be played clip</param>
    public AudioSource PlaySound(AudioClip clip)
    {
        if (!_soundsSource1.isPlaying && !_soundsSource2.isPlaying)
        {
            _soundsSource1.pitch = Random.Range(_minPitch, _maxPitch);
            _isSource1Active = true;
            _soundsSource1.clip = clip;
            _soundsSource1.volume = 1f;
            _soundsSource1.Play();
            return _soundsSource1;
        }

        if (_isSource1Active)
        {
            if (_crossfadeRoutine != null) StopCoroutine(_crossfadeRoutine);
            _crossfadeRoutine = StartCoroutine(Crossfade(_soundsSource1, _soundsSource2, clip));
            _isSource1Active = false;
            return _soundsSource2;
        }
        else
        {
            if (_crossfadeRoutine != null) StopCoroutine(_crossfadeRoutine);
            _crossfadeRoutine = StartCoroutine(Crossfade(_soundsSource2, _soundsSource1, clip));
            _isSource1Active = true;
            return _soundsSource1;
        }
    }
    private IEnumerator Crossfade(AudioSource fromSource, AudioSource toSource, AudioClip newClip)
    {
        toSource.pitch = Random.Range(_minPitch, _maxPitch);
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

    /// <summary>
    /// To be used after "PlaySound" and use the source from it. Will repeatedly play same sound
    /// </summary>
    /// <param name="audioSource">"PlaySound" return source</param>
    public void PlayRepeatedly(AudioSource audioSource)
    {
        _repeatedlyPlayedRoutine ??= StartCoroutine(PlayRepeatedlyRoutine(audioSource));
    }
    private IEnumerator PlayRepeatedlyRoutine(AudioSource audioSource)
    {
        AudioClip clip = audioSource.clip;
        while (audioSource.clip == clip)
        {
            audioSource.pitch = Random.Range(_minPitch, _maxPitch);
            audioSource.Play();

            // Wait for clip to finish (accounting for pitch change)
            yield return new WaitForSeconds(audioSource.clip.length / audioSource.pitch);
            //yield return new WaitUntil(() => !audioSource.isPlaying);
        }
        _repeatedlyPlayedRoutine = null;
    }

    /// <summary>
    /// To be used after "PlaySound" and use the source from it. Will play two sequential sfxs
    /// </summary>
    /// <param name="audioSource">"PlaySound" return source</param>
    /// <param name="secondClip">Next clip to play on the same source</param>
    public void PlaySequential(AudioSource audioSource, AudioClip secondClip)
    {
        _sequentialyPlayedRoutine ??= StartCoroutine(PlaySequentialRoutine(audioSource, secondClip));
    }
    private IEnumerator PlaySequentialRoutine(AudioSource audioSource, AudioClip secondClip)
    {
        // Wait until the first clip finishes
        yield return new WaitUntil(() => !audioSource.isPlaying);
        yield return new WaitForSeconds(0.2f);
        //yield return new WaitWhile(() => audioSource.isPlaying);

        audioSource.clip = secondClip;
        audioSource.Play();
        _sequentialyPlayedRoutine = null;
    }

    #region Unity Events
    public void ToggleMusicVolume(bool isOn)
    {
        _musicSource.volume = isOn ? _musicVolume : 0.0f;
    }
    public void ToggleSoundsVolume(bool isOn)
    {
        _soundsSource1.volume = isOn ? _soundsVolume : 0.0f;
        _soundsSource2.volume = isOn ? _soundsVolume : 0.0f;
    }
    #endregion
}
