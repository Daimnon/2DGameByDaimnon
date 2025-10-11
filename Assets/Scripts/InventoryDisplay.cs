using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private Image _coinImage;
    [SerializeField] private TextMeshProUGUI _coinsAmountTMP;

    private void Start()
    {
        Inventory.Instance.OnUpdateCurrencyEvent += UpdateCurrency;
        UpdateCurrency(Inventory.Instance.Currency);
    }
    private void OnEnable()
    {
        UpdateCurrency(Inventory.Instance.Currency);
    }
    private void OnDestroy()
    {
        Inventory.Instance.OnUpdateCurrencyEvent -= UpdateCurrency;
    }

    private void UpdateCurrency(int currentCurrency)
    {
        _coinsAmountTMP.text = currentCurrency.ToString();
    }
}
