using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MoneySystem : MonoBehaviour
{

    public static MoneySystem instance;

    [Header("Aktuální stav peněz")]
    [SerializeField] private int gold = 0;
    public int Gold => gold;

    [Header("Prefaby košů")]
    public GameObject kosHrusekPrefab;
    public GameObject kosBananyPrefab;
    public GameObject kosMrkevPrefab;
    public GameObject kosRajcataPrefab;

    [Header("Pozice pro spawn košů (kos2, kos3, kos4)")]
    public Transform[] kosSpawnPoints;
    private int nextSpawnIndex = 0;

    // Ceník produktů
    private Dictionary<string, int> productPrices = new Dictionary<string, int>()
    {
        { "Hrušky",   100 },
        { "Banány",   100 },
        { "Mrkve",    100 },
        { "Rajčata",  100 }
    };


    public List<string> purchasedItems = new List<string>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{

    if (scene.name == "Mapa" || scene.buildIndex == 1)
    {

        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("KosSpawnPoint");
        if (spawnPointObjects.Length > 0)
        {
            kosSpawnPoints = new Transform[spawnPointObjects.Length];
            for (int i = 0; i < spawnPointObjects.Length; i++)
            {
                kosSpawnPoints[i] = spawnPointObjects[i].transform;
            }
            nextSpawnIndex = 0;
            Debug.Log("Našel jsem " + spawnPointObjects.Length + " spawn pointů.");
        }
        else
        {
            Debug.LogWarning("Spawn pointy nebyly nalezeny! Ujisti se, že objekty mají tag 'KosSpawnPoint'.");
        }
        

        foreach (string product in purchasedItems)
        {
            SpawnKosByName(product);
        }
        purchasedItems.Clear();
    }
}


    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("Přidáno " + amount + " goldů. Aktuální stav: " + gold);
    }

    public void BuyProduct(string productName)
    {
        if (productPrices.ContainsKey(productName))
        {
            int price = productPrices[productName];
            if (gold >= price)
            {
                gold -= price;
                purchasedItems.Add(productName);
                Debug.Log("Koupeno: " + productName + ". Zbývající goldy: " + gold);
            }
            else
            {
                Debug.LogWarning("Nedostatek peněz na " + productName + "! Máš " + gold + ", ale stojí " + price);
            }
        }
        else
        {
            Debug.LogWarning("Produkt " + productName + " není v ceníku!");
        }
    }

    public void SpawnKosByName(string productName)
    {
        switch (productName)
        {
            case "Hrušky":
                SpawnKos(kosHrusekPrefab);
                break;
            case "Banány":
                SpawnKos(kosBananyPrefab);
                break;
            case "Mrkve":
                SpawnKos(kosMrkevPrefab);
                break;
            case "Rajčata":
                SpawnKos(kosRajcataPrefab);
                break;
            default:
                Debug.LogWarning("Nemáme prefab pro produkt: " + productName);
                break;
        }
    }
    private void SpawnKos(GameObject kosPrefab)
    {
        if (kosPrefab == null)
        {
            Debug.LogWarning("Chybí prefab koše!");
            return;
        }

        if (kosSpawnPoints != null && nextSpawnIndex < kosSpawnPoints.Length)
        {
            Transform spawnPoint = kosSpawnPoints[nextSpawnIndex];
            Instantiate(kosPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Spawnul jsem koš " + kosPrefab.name + " na pozici: " + spawnPoint.name);
            nextSpawnIndex++;
        }
        else
        {
            Debug.LogWarning("Žádný volný spawn point pro koš, nebo už jsi vyčerpal kos2, kos3, kos4.");
        }
    }
}
