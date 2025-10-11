using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string _dataDirectoryPath = string.Empty;
    private string _fileName = string.Empty;

    public FileDataHandler(string dataDirectoryPath, string fileName)
    {
        _dataDirectoryPath = dataDirectoryPath;
        _fileName = fileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(_dataDirectoryPath, _fileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = string.Empty;
                using (FileStream stream = new(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (System.Exception e)
            {
                Debugger.LogError($"Could not find or access {_dataDirectoryPath}, {_fileName}, {e}");
            }
        }
        return loadedData;
    }
    public void Save(GameData data)
    {
        string fullPath = Path.Combine(_dataDirectoryPath, _fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);
            using (FileStream stream = new(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (System.Exception e)
        {
            Debugger.LogError($"Could not find or access {_dataDirectoryPath}, {_fileName}, {e}");
        }
    }
    public void DeleteSaveFile()
    {
        string fullPath = Path.Combine(_dataDirectoryPath, _fileName);
        if (File.Exists(fullPath))
        {
            try
            {
                File.Delete(fullPath);
            }
            catch (System.Exception e)
            {
                Debugger.LogError($"Could not find or access {_dataDirectoryPath}, {_fileName}, {e}");
            }
        }
    }
}
