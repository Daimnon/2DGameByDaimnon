using System.Collections; // for IEnumerator
using UnityEngine;
using UnityEngine.SceneManagement; // for SceneManager

public class FinishLine : MonoBehaviour
{
    private const string PLAYER_TAG = "Player"; // the tag of the player, const == cannot and should not be changed

    [SerializeField] private float _timeToWaitForLevelToReset = 4.0f;
    [SerializeField] private ParticleSystem _victoryParticles;

    private void OnTriggerEnter2D(Collider2D collision) // trigger detection on this gameObjcet's collider
    {
        if (collision.CompareTag(PLAYER_TAG))
        {
            StartCoroutine(WaitForResetAfterWin()); // quick and dirty use of coroutine, cause I hate Invoke("", time).
        }
    }

    private IEnumerator WaitForResetAfterWin()
    {
        _victoryParticles.Play();

        yield return new WaitForSeconds(_timeToWaitForLevelToReset); // WaitForSeconds is one of many scripts that wait for stuff
        //SceneManager.LoadScene(0); // reloads the first level
    }
}
