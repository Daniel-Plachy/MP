using UnityEngine;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    // Seznam produktů, které hráč odemkl v menu
    public static List<string> unlockedProducts = new List<string>();

    // Pro skenování u pokladny
    public static string scannedProduct = "";
    public static GameObject scannedObject = null;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void UnlockProduct(string productName)
    {
        if (!unlockedProducts.Contains(productName))
        {
            unlockedProducts.Add(productName);
            Debug.Log($"[DataManager] unlockedProducts přidáno: {productName}");
        }
    }
}
