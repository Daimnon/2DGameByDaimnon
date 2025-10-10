using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterID
{
    Red = 0,
    Yellow = 1,
    Turquoise = 2,
    Pink = 3,
    Green = 4,
    Blue = 5,
    Gradient1 = 6,
    Gradient2 = 7,
    Gold = 8
}

public class Inventory : MonoBehaviour
{
    public static Inventory _instance = null;
    public static Inventory Instance => _instance;

    private int _currency;
    public int Currency => _currency;

    private List<int> _unlockedCharacters = new(1) { 0 };
    public List<int> UnlockedCharacters => _unlockedCharacters;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    public void AddCurrency(int amount)
    {
        _currency += amount;
    }
    public void AddCharacter(int charID)
    {
        _unlockedCharacters.Add(charID);
    }
}
