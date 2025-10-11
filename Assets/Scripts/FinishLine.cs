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

    private IEnumerator WaitForResetAfterWin() // a standalone coroutined sequence that we can use a synchronicly, timing action within the game's constraints
    {
        _victoryParticles.Play();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int maxSceneCount = SceneManager.sceneCountInBuildSettings;
        yield return new WaitForSeconds(_timeToWaitForLevelToReset); // WaitForSeconds is one of many scripts that wait for stuff
        if (currentSceneIndex < maxSceneCount) SceneManager.LoadScene(currentSceneIndex+1); // reloads the first level
        // else go to level selection
    }
}
