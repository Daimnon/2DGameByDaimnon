using UnityEngine;

[System.Serializable]
public class GameData
{
    public int Id;
    public int Currency;
    public int[] UnlockedCharacters;

    public override string ToString()
    {
        string characterIds = string.Empty;
        int unlockedCharLength = UnlockedCharacters != null ? UnlockedCharacters.Length : 0;
        for (int i = 0; i < unlockedCharLength; i++)
        {
            characterIds += i != unlockedCharLength -1 ? UnlockedCharacters[i] + ", " : UnlockedCharacters[i];
        }
        return $"Id: {Id}, Currency: {Currency}, Unlocked Characters {characterIds}";
    }
}
