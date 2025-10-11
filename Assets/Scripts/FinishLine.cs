using System;
using System.Collections; // for IEnumerator
using UnityEngine;
using UnityEngine.SceneManagement; // for SceneManager

public class FinishLine : MonoBehaviour
{
    private const string PLAYER_TAG = "Player"; // the tag of the player, const == cannot and should not be changed

    private Action _onLevelFinished;
    public Action OnLevelFinished { get => _onLevelFinished; set => _onLevelFinished = value; }

    [Header("Systems")]
    [SerializeField] private PlayerSlideController _playerController; // find a way to remove and still DisableInputs

    [Header("Data")]
    [SerializeField] private float _victoryDelay = 2.0f;
    
    [Header("Components")]
    [SerializeField] private ParticleSystem _victoryParticles;

    private void OnTriggerEnter2D(Collider2D collision) // trigger detection on this gameObjcet's collider
    {
        if (collision.CompareTag(PLAYER_TAG))
        {
            StartCoroutine(WaitForResetAfterWin()); // quick and dirty use of coroutine, cause I hate Invoke("", time).
        }
    }

    private IEnumerator WaitForResetAfterWin() // a standalone coroutined sequence that we can use a synchronicly, timing action within the game's constraints
    {
        _victoryParticles.Play();
        _playerController.DisableInputs();

        yield return new WaitForSeconds(_victoryDelay); // WaitForSeconds is one of many scripts that wait for stuff
        _onLevelFinished?.Invoke();
        Debugger.Log("Invoked _onLevelFinished");
    }
}
