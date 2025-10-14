using System;
using System.Collections; // for IEnumerator
using UnityEngine;

public class CrashDetector : MonoBehaviour
{
    private bool _isCrashing = false; public bool IsCrashing => _isCrashing;
    private bool _hasCrashed = false; public bool HasCrashed => _hasCrashed;

    private Action _onCrash;
    public Action OnCrash { get => _onCrash; set => _onCrash = value; }

    [Header("Settings")]
    [SerializeField] private string _groundTag = "LevelCollider"; // the tag of the player
    [SerializeField] private float _afterCrashDelay = 2.0f;

    [Header("SFX")]
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private AudioClip _crashSFX;

    [Header("Animations")]
    [SerializeField] private ParticleSystem _crashParticles;

    private void OnTriggerEnter2D(Collider2D collision) // trigger detection on this gameObjcet's collider
    {
        if (collision.CompareTag(_groundTag) && !_hasCrashed)
        {
            StartCoroutine(CrashRoutine()); // quick and dirty use of coroutine, cause I hate Invoke("", time).
        }
    }

    private IEnumerator CrashRoutine() // a standalone coroutined sequence that we can use a synchronicly, timing action within the game's constraints
    {
        _isCrashing = true;
        _crashParticles.Play();
        _audioManager.PlaySound(_crashSFX);
        _onCrash?.Invoke();
        Debugger.Log("Invoked OnCrash");

        yield return new WaitForSeconds(_afterCrashDelay); // WaitForSeconds is one of many scripts that wait for stuff
        _hasCrashed = true;
    }
}
