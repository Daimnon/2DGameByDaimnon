using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitchGroupManager : MonoBehaviour
{
    private List<ToggleSwitch> _toggleSwitches = new List<ToggleSwitch>();
    
    [Header("Data")]
    [SerializeField] private ToggleSwitch initialToggleSwitch;
    [SerializeField] private bool allCanBeToggledOff;

    private void Awake()
    {
        ToggleSwitch[] toggleSwitches = GetComponentsInChildren<ToggleSwitch>();
        foreach (ToggleSwitch toggleSwitch in toggleSwitches)
        {
            RegisterToggleButtonToGroup(toggleSwitch);
        }
    }

    private void RegisterToggleButtonToGroup(ToggleSwitch toggleSwitch)
    {
        if (_toggleSwitches.Contains(toggleSwitch)) return;

        _toggleSwitches.Add(toggleSwitch);
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
