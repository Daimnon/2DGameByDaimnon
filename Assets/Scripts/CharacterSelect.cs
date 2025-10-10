using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    private const float TIME_REGULAR = 1.0f; // for unpausing the game.

    [Header("Components")]
    [SerializeField] private GameObject _scoreCanvasGO;
    [SerializeField] private PlayerSlideController _playerController;
    [SerializeField] private GameObject _characterSelectionCanvasGo;

    [Header("Data")]
    [SerializeField] private Sprite _characterSprite;

    public void SelectCharacterAndStartGame()
    {
        _playerController.SR.sprite = _characterSprite; // set player's desired sprite
        _scoreCanvasGO.SetActive(true); // turn on the score canvas
        _characterSelectionCanvasGo.SetActive(false); // turn off the selection canvas
        Time.timeScale = TIME_REGULAR; // unpause
    }
}
