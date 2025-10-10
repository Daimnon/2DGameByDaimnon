using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class UnlockableCharacter : MonoBehaviour, ISaveable
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
            UnlockCharacter();
        }
        else
        {
            
        }
    }

    public void Unlock()
    {
        Inventory inventory = Inventory.Instance;
        if (inventory.Currency >= _amountToUnlock)
        {
            inventory.AddCharacter(_id);
            SaveDataManager.Instance.UnlockCharacter(_id); // actually saves the unlocked character

            _closedLock.SetActive(false);
            _characterSelectBtn.enabled = true;
            PlayerPrefs.SetInt("UnlockedCharacters", _id);
        }
        else
        {
            _errorPanelRoutine ??= StartCoroutine(ErrorPanelRoutine());
        }
    }

    private IEnumerator ErrorPanelRoutine() // a reusable coroutined sequence that we can use a synchronicly, timing action within REAL TIME constraints
    {
        _errorPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(ERROR_PANEL_TIME);

        _errorPanel.SetActive(false);
        _errorPanelRoutine = null; 
    }

    private void UnlockCharacter()
    {
        _coinIcon.SetActive(false);
        _closedLock.SetActive(false);
        _openLock.SetActive(false);
        _characterSelectBtn.enabled = true;
    }

    public void LoadData(GameData data)
    {
        if (SaveDataManager.Instance.IsCharacterUnlocked(_id))
        {
            UnlockCharacter();
        }
        else
        {
            _amountText.text = _amountToUnlock.ToString();
        }
    }
    public void SaveData(ref GameData data)
    {
        // nothing to do here
    }
}