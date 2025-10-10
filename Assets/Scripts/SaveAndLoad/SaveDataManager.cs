using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveDataManager : MonoBehaviour
{
    private static SaveDataManager _instance;
    public static SaveDataManager Instance => _instance;

    private GameData _gameData;
    private List<ISaveable> _saveableGameObjects;

    [Header("File Storage Confing")]
    [SerializeField] private string _saveFileName = "SaveGame.Json";
    private FileDataHandler _fileDataHandler;

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

    private void Start()
    {
        _fileDataHandler = new(Application.persistentDataPath, _saveFileName);
        _saveableGameObjects = FindAllSaveableGameObjects();
        LoadGame();
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void NewGame()
    {
        _gameData = new();
    }

    public void LoadGame()
    {
        _gameData = _fileDataHandler.Load();

        if (_gameData == null)
        {
            Debugger.Log("Couldn't find Existing data, creating new file.");
            NewGame();
        }

        foreach (ISaveable saveable in _saveableGameObjects)
        {
            saveable.LoadData(_gameData);
        }

        Debugger.Log("Data Loaded");
    }

    public void SaveGame()
    {
        foreach (ISaveable saveable in _saveableGameObjects)
        {
            saveable.SaveData(ref _gameData);
        }

        _fileDataHandler.Save(_gameData);
    }

    private List<ISaveable> FindAllSaveableGameObjects()
    {
        IEnumerable<ISaveable> saveableGameObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ISaveable>();
        return new List<ISaveable>(saveableGameObjects);
    }
}
