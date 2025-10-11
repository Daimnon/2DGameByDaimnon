using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveDataManager : MonoBehaviour
{
    private static SaveDataManager _instance = null;     /* shortened singleton pattern in unity */
    public static SaveDataManager Instance => _instance; /* shortened singleton pattern in unity */

    private GameData _gameData;
    private List<ISaveable> _saveableGameObjects;
    private bool _wasSaveFileKilled = false;

    [Header("File Storage Confing")]
    [SerializeField] private string _saveFileName = "SaveGame.Json";
    private FileDataHandler _fileDataHandler;

    private void Awake()
    {
        /* singleton pattern in unity, must be in awake */
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
        /* singleton pattern in unity, must be in awake */
    }
    private void Start()
    {
        _fileDataHandler = new(Application.persistentDataPath, _saveFileName); // set the file handler on the correct file address
        _saveableGameObjects = FindAllSaveableGameObjects(); // get all saveables
        LoadGame();
    }
    private void OnApplicationQuit()
    {
        if (!_wasSaveFileKilled) SaveGame(); // make sure will not save if I deleted game file purposly
    }

    public void NewGame() // creates a new save file
    {
        _gameData = new();
        Inventory.Instance.CreateNewInventory();
    }
    public void LoadGame() // get the data from the save file and import it while doing other neccessary operations.
    {
        _gameData = _fileDataHandler.Load(); // update the gameData script with the save file's content

        if (_gameData == null)
        {
            Debugger.Log("Couldn't find Existing data, creating new file.");
            NewGame();
        }

        Inventory.Instance.LoadFromData(_gameData); // update Inventory before everything loads

        foreach (ISaveable saveable in _saveableGameObjects) // calls "LoadGame" method in all ISaveables which should apply the changes and do the "actual load".
        {
            saveable.LoadData(_gameData);
        }

        Debugger.Log("Data Loaded\n" + _gameData.ToString());
    }
    public void SaveGame()
    {
        Inventory.Instance.SaveToData(ref _gameData); // save Inventory before everything saves

        foreach (ISaveable saveable in _saveableGameObjects) // calls "SaveGame" method in all ISaveables which should check for the changes and register them.
        {
            saveable.SaveData(ref _gameData);
        }

        _fileDataHandler.Save(_gameData); // actually saves the data
        Debugger.Log("Data Saved\n" + _gameData.ToString());
    }

    private List<ISaveable> FindAllSaveableGameObjects() // search for all Monobehaviours with the ISaveable Interface
    {
        IEnumerable<ISaveable> saveableGameObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ISaveable>();
        return new List<ISaveable>(saveableGameObjects);
    }

    [ContextMenu("Kill Save File")]
    private void KillSaveFile()
    {
        _fileDataHandler.DeleteSaveFile();
        _wasSaveFileKilled = true;
    }
}
