using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitchGroupManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<ToggleSwitch> _toggleSwitches = new List<ToggleSwitch>();
    [SerializeField] private ToggleSwitch initialToggleSwitch;
    [SerializeField] private bool allCanBeToggledOff;

    private void Awake()
    {
        foreach (ToggleSwitch toggleSwitch in _toggleSwitches)
        {
            RegisterToggleButtonToGroup(toggleSwitch);
        }
    }

    private void RegisterToggleButtonToGroup(ToggleSwitch toggleSwitch)
    {
        toggleSwitch.SetupForManager(this);
    }

    private void Start()
    {
        bool areAllToggledOff = true;
        foreach (ToggleSwitch button in _toggleSwitches)
        {
            if (!button.CurrentValue) continue;
            areAllToggledOff = false;
            break;
        }

        if (!areAllToggledOff || allCanBeToggledOff)
            return;

        if (initialToggleSwitch != null) initialToggleSwitch.ToggleByGroupManager(true);
        else _toggleSwitches[0].ToggleByGroupManager(true);
    }

    public void ToggleGroup(ToggleSwitch toggleSwitch)
    {
        if (_toggleSwitches.Count <= 1) return;

        if (allCanBeToggledOff && toggleSwitch.CurrentValue)
        {
            foreach (ToggleSwitch button in _toggleSwitches)
            {
                if (button == null) continue;
                button.ToggleByGroupManager(false);
            }
        }
        else
        {
            foreach (ToggleSwitch button in _toggleSwitches)
            {
                if (button == null)
                    continue;

                if (button == toggleSwitch) button.ToggleByGroupManager(true);
                else button.ToggleByGroupManager(false);
            }
        }
    }
}
