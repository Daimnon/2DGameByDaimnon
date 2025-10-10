using System.Collections.Generic;
using System.Linq;
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

// automatically updated by SaveDataManager
public class Inventory : MonoBehaviour
{
    public static Inventory _instance = null;
    public static Inventory Instance => _instance;

    private int _currency;
    public int Currency => _currency;

    private HashSet<int> _unlockedCharacters = new(1) { 0 };
    public HashSet<int> UnlockedCharacters => _unlockedCharacters;

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

    public void CreateNewInventory() // reset inventory
    {
        _currency = 0;
        _unlockedCharacters.Clear();
    }
    public void LoadFromData(GameData data)
    {
        _currency = data.Currency;

        // short if = if ? there are UnlockedCharacters in GameData, copy the array to a HashSet (list with no duplicates), else : create new UnlockedCharacters HashSet.
        // when creating new saveData always unlock first character
        _unlockedCharacters = data.UnlockedCharacters != null ? new HashSet<int>(data.UnlockedCharacters) : new HashSet<int> { 0 };
    }
    public void SaveToData(ref GameData data)
    {
        data.Currency = _currency;
        data.UnlockedCharacters = _unlockedCharacters.ToArray();
    }

    public bool IsCharacterUnlocked(int id) => _unlockedCharacters.Contains(id); // checks if character is already unlocked.
    public void UnlockCharacter(int id) // handle characters unlocks
    {
        // immidiatly save after unlocking character. add in hash set returns a success or failed bool
        if (_unlockedCharacters.Add(id)) SaveDataManager.Instance.SaveGame();
    }
}
