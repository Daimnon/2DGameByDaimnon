using UnityEngine;

public class Trail : MonoBehaviour
{
    private string _groundTag = "LevelCollider";
    private string _finishLineTag = "FinishLine";
    private bool _hasFinished = false;
    [SerializeField] private ParticleSystem _trailParticles;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(_groundTag) && !_hasFinished)
        {
            _trailParticles.Play();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(_groundTag))
        {
            _trailParticles.Stop();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_finishLineTag))
        {
            _trailParticles.Stop();
            _hasFinished = true;
        }
    }
}
