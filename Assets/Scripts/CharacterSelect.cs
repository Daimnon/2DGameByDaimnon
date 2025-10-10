using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    private const float TIME_REGULAR = 1.0f;

    [Header("Components")]
    [SerializeField] private GameObject _scoreCanvasGO;
    [SerializeField] private PlayerSlideController _slideController;
    [SerializeField] private GameObject _characterSelectionCanvasGo;

    [Header("Data")]
    [SerializeField] private Sprite _characterSprite;

    public void SelectCharacterAndStartGame()
    {
        _slideController.SR.sprite = _characterSprite;
        Time.timeScale = TIME_REGULAR;
        _scoreCanvasGO.SetActive(true);
        _characterSelectionCanvasGo.SetActive(false);
    }
}
