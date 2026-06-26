using UnityEngine;

public class SaveLoadService
{
    private const string SaveKey = "IdleMining_GameSave";

    public GameSaveData Data { get; private set; }

    public SaveLoadService()
    {
        Load();
    }

    public void Save()
    {
        Data.LastPlayTime = System.DateTime.UtcNow.ToString();

        string json = JsonUtility.ToJson(Data, true);

        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();

        Debug.Log($"Игра сохранена.");
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            try
            {
                Data = JsonUtility.FromJson<GameSaveData>(json);
                Debug.Log($"Данные загружены.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SaveSystem] Ошибка парсинга JSON: {e.Message}. Создаем чистое сохранение.");
                CreateNewSave();
            }
        }
        else
        {
            Debug.Log($"Файл сохранения не найден. Создаем новую игру.");
            CreateNewSave();
        }
    }

    private void CreateNewSave() => Data = new GameSaveData();

    public void ClearSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        CreateNewSave();
        Save();
        Debug.Log($"Сохранение полностью стерто.");
    }
}
