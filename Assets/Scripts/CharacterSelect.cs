using System;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    private const float TIME_REGULAR = 1.0f; // for unpausing the game.

    [Header("Systems")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private PlayerSlideController _playerController;

    [Header("Data")]
    [SerializeField] private int _id;
    [SerializeField] private Sprite _characterSprite;

    [Header("Components")]
    [SerializeField] private GameObject _scoreCanvasGO;
    [SerializeField] private GameObject _inventoryCanvasGO;
    [SerializeField] private GameObject _characterSelectionCanvasGo;

    

    public void SelectCharacterAndStartGame()
    {
        if (!Inventory.Instance.UnlockedCharacters.Contains(_id)) return;

        _playerController.SR.sprite = _characterSprite; // set player's desired sprite
        _scoreCanvasGO.SetActive(true); // turn on the score canvas
        _inventoryCanvasGO.SetActive(false); // turn off the inventory canvas
        _characterSelectionCanvasGo.SetActive(false); // turn off the selection canvas
        _gameManager.OnLevelStartEvent?.Invoke();
    }
}
