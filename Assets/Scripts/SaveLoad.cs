using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public int gold;
    public List<string> unlockedProducts;
    public List<ProductCount> itemsRemoved;
    public string sceneName;
}

[System.Serializable]
public struct ProductCount
{
    public string productName;
    public int removedCount;
}

public static class SaveLoad
{
    private static SaveData _pendingData;
    public static bool SkipLoadOnStart = false;
    private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "savegame.json");

    public static void SaveGame()
    {
        if (MoneySystem.instance == null) return;

        // Převod slovníku na seznam pro serializaci
        var list = MoneySystem.instance.itemsRemoved
            .Select(kv => new ProductCount { productName = kv.Key, removedCount = kv.Value })
            .ToList();

        var data = new SaveData
        {
            gold             = MoneySystem.instance.Gold,
            unlockedProducts = new List<string>(DataManager.unlockedProducts),
            itemsRemoved     = list,
            sceneName        = SceneManager.GetActiveScene().name
            
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[SaveLoad] Game saved: {json}");
        SkipLoadOnStart = false;
    }

    public static bool HasSave() => File.Exists(SavePath);

    public static SaveData GetSaveData()
    {
        if (!HasSave() || SkipLoadOnStart) 
            return null;
        return JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));
    }

    public static void LoadGame()
    {
        var data = GetSaveData();
        if (data == null)
        {
            Debug.LogWarning("[SaveLoad] No save data.");
            return;
        }
        // Načti scénu podle jména; data aplikujeme přímo v MoneySystem.OnSceneLoaded
        SceneManager.LoadScene(data.sceneName);
    }



    private static void ApplyLoadedState(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= ApplyLoadedState;

        if (_pendingData == null || MoneySystem.instance == null)
            return;

        // 1) Upravení gold
        int delta = _pendingData.gold - MoneySystem.instance.Gold;
        if (delta > 0)
            MoneySystem.instance.AddGold(delta);

        // 2) Obnovení odemčených produktů
        DataManager.unlockedProducts = new List<string>(_pendingData.unlockedProducts);

        // 3) Naplnění itemsRemoved do slovníku
        MoneySystem.instance.itemsRemoved.Clear();
        foreach (var pc in _pendingData.itemsRemoved)
        {
            MoneySystem.instance.itemsRemoved[pc.productName] = pc.removedCount;
        }

        _pendingData = null;
    }
}
