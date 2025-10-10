using System.Collections;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private const string PLAYER_TAG = "Player"; // the tag of the player, const == cannot and should not be changed
    private PlayerSlideController _playerSlideController;
    
    [SerializeField] private PowerupSO _powerup;
    [SerializeField] private SpriteRenderer _sR;
    [SerializeField] private Collider2D _collider2D;

    private void OnTriggerEnter2D(Collider2D collision) // trigger detection on this gameObjcet's collider
    {
        if (collision.CompareTag(PLAYER_TAG))
        {
            _playerSlideController = collision.GetComponent<PlayerSlideController>();
            if (_playerSlideController) StartCoroutine(ApplyPowerupRoutine());
        }
    }

    private IEnumerator ApplyPowerupRoutine()
    {
        _playerSlideController.ActivatePowerup(_powerup);
        _sR.enabled = false;
        _collider2D.enabled = false;

        yield return new WaitForSeconds(_powerup.Duration);
        _playerSlideController.DeactivatePowerup(_powerup);

        yield return null;
        Destroy(gameObject);
    }
}
