using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnlockableCharacter : MonoBehaviour
{
    private const float ERROR_PANEL_TIME = 1.0f;
    private Coroutine _errorPanelRoutine;

    [Header("Components")]
    [SerializeField] private Button _characterSelectBtn;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private GameObject _coinIcon;
    [SerializeField] private GameObject _closedLock;
    [SerializeField] private GameObject _openLock;
    [SerializeField] private GameObject _errorPanel;

    [Header("Data")]
    [SerializeField] private int _id;
    [SerializeField] private int _amountToUnlock;

    private void Start()
    {
        if (Inventory.Instance.UnlockedCharacters.Contains(_id))
        {
            _coinIcon.SetActive(false);
            _closedLock.SetActive(false);
            _openLock.SetActive(false);
            _characterSelectBtn.enabled = true;
        }
        else
        {
            _amountText.text = _amountToUnlock.ToString();
        }
    }

    public void Unlock()
    {
        Inventory inventory = Inventory.Instance;
        if (inventory.Currency >= _amountToUnlock)
        {
            inventory.AddCharacter(_id);
            _closedLock.SetActive(false);
            _characterSelectBtn.enabled = true;
            PlayerPrefs.SetInt("UnlockedCharacters", _id);
        }
        else
        {
            _errorPanelRoutine ??= StartCoroutine(ErrorPanelRoutine());
        }
    }

    private IEnumerator ErrorPanelRoutine()
    {
        _errorPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(ERROR_PANEL_TIME);

        _errorPanel.SetActive(false);
        _errorPanelRoutine = null; 
    }
}