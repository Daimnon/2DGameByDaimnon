using System.Collections; // for IEnumerator
using UnityEngine;
using UnityEngine.SceneManagement; // for SceneManager

public class CrashDetector : MonoBehaviour
{
    private bool _hasCrashed = false;
    public bool HasCrashed => _hasCrashed;

    [SerializeField] private string _groundTag = "LevelCollider"; // the tag of the player
    [SerializeField] private float _timeToWaitForLevelToReset = 2.0f;
    [SerializeField] private ParticleSystem _crashParticles;
    [SerializeField] private PlayerSlideController _slideController;

    private void OnTriggerEnter2D(Collider2D collision) // trigger detection on this gameObjcet's collider
    {
        if (collision.CompareTag(_groundTag) && !_hasCrashed)
        {
            StartCoroutine(WaitForResetAfterWin()); // quick and dirty use of coroutine, cause I hate Invoke("", time).
        }
    }

    private IEnumerator WaitForResetAfterWin()
    {
        _crashParticles.Play();
        _slideController.DisableInputs();
        _slideController.SE2D.speed = 0.0f;
        _slideController.SE2D.useFriction = true;
        _hasCrashed = true;

        yield return new WaitForSeconds(_timeToWaitForLevelToReset); // WaitForSeconds is one of many scripts that wait for stuff
        SceneManager.LoadScene(0); // reloads the first level
    }
}
