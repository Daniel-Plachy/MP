using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MoneySystem : MonoBehaviour
{
    public static MoneySystem instance;

    [Header("Výchozí počet položek v koši")]
    public int defaultItemCount = 10;

    [Header("Peníze")]
    [SerializeField] private int gold = 0;
    public int Gold => gold;

    [Header("Prefaby košů")]
    public GameObject bramboryPrefab;
    public GameObject jablkaPrefab;
    public GameObject hruskyPrefab;
    public GameObject bananyPrefab;
    public GameObject mrkvePrefab;
    public GameObject rajcataPrefab;

    [Header("Spawn pozice (Mapa)")]
    public Transform initialBramboryPoint;
    public Transform initialJablkaPoint;
    public Transform kos2;
    public Transform kosHrusky;
    public Transform kosBanany;
    public Transform kosMrkve;
    public Transform kosRajcata;

    [Header("Prodleva pro znovuzapnutí fyziky (v sekundách)")]
    public float reactivateDelay = 0.5f;

    // runtime stav: kolik položek hráč odebral z každého koše
    [HideInInspector]
    public Dictionary<string,int> itemsRemoved = new Dictionary<string,int>();

    // aktuálně instancované koše + které už spawnuty
    private Dictionary<string, GameObject> basketInstances = new Dictionary<string, GameObject>();
    private HashSet<string> spawned = new HashSet<string>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Mapa")
            return;

        // 1) Načti uložená data
        var save = SaveLoad.GetSaveData();
        if (save != null && save.sceneName == scene.name)
        {
            int delta = save.gold - gold;
            if (delta > 0) AddGold(delta);

            DataManager.unlockedProducts = new List<string>(save.unlockedProducts);

            itemsRemoved.Clear();
            foreach (var pc in save.itemsRemoved)
                itemsRemoved[pc.productName] = pc.removedCount;
        }

        // 2) Reakvizice spawn-pointů
        initialBramboryPoint = GameObject.Find("posPotato")?.transform;
        initialJablkaPoint  = GameObject.Find("posApples")?.transform;
        kos2                = GameObject.Find("kos2")?.transform;
        kosHrusky           = GameObject.Find("kosHrusky")?.transform;
        kosBanany           = GameObject.Find("kosBanany")?.transform;
        kosMrkve            = GameObject.Find("kosMrkve")?.transform;
        kosRajcata          = GameObject.Find("kosRajcata")?.transform;

        // 3) Znič staré koše
        foreach (var kv in basketInstances)
            if (kv.Value != null)
                Destroy(kv.Value);
        basketInstances.Clear();
        spawned.Clear();

        // 4) Spawn nových – a u všech vypni fyziku na reactivateDelay
        string startProd = DataManager.unlockedProducts.FirstOrDefault();
        foreach (var prod in DataManager.unlockedProducts)
        {
            if (spawned.Contains(prod))
                continue;
            spawned.Add(prod);

            GameObject prefab = GetPrefab(prod);
            Transform point  = GetSpawnPoint(prod, startProd);
            if (prefab == null || point == null)
            {
                Debug.LogWarning($"[MoneySystem] Chybí prefab nebo spawn-point pro '{prod}'");
                continue;
            }

            var basketGO = Instantiate(prefab, point.position, point.rotation);
            DontDestroyOnLoad(basketGO);

            // VYPNI fyziku na chvíli
            StabilizeBasket(basketGO);
            StartCoroutine(RestorePhysicsAfterDelay(basketGO, reactivateDelay));

            basketInstances[prod] = basketGO;
            int removedSoFar = itemsRemoved.TryGetValue(prod, out var cnt) ? cnt : 0;
            basketGO.GetComponent<Basket>().RemoveItems(removedSoFar);
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"[MoneySystem] Gold = {gold}");
    }

    public void BuyProduct(string productName)
    {
        int price = GetUnlockPrice(productName);
        if (gold < price)
        {
            Debug.LogWarning($"[MoneySystem] Nedostatek goldů ({gold} < {price})");
            return;
        }
        gold -= price;
        Debug.Log($"[MoneySystem] Odemknuto {productName} za {price}. Zbývá {gold}");

        DataManager.UnlockProduct(productName);
        itemsRemoved[productName] = 0;

        if (SceneManager.GetActiveScene().name == "Mapa")
            RespawnBasket(productName);
    }

    public void RefillProduct(string productName)
    {
        int price = GetRefillPrice(productName);
        if (gold < price)
        {
            Debug.LogWarning($"[MoneySystem] Nedostatek goldů na refill {productName} ({gold} < {price})");
            return;
        }
        gold -= price;
        Debug.Log($"[MoneySystem] Refill {productName} za {price}. Zbývá {gold}");

        itemsRemoved[productName] = 0;

        if (SceneManager.GetActiveScene().name == "Mapa")
            RespawnBasket(productName);
    }

    private void RespawnBasket(string productName)
    {
        // smaž starou instanci
        if (basketInstances.TryGetValue(productName, out var oldGO) && oldGO != null)
        {
            Destroy(oldGO);
            basketInstances.Remove(productName);
            spawned.Remove(productName);
        }

        // spawn nové
        GameObject prefab = GetPrefab(productName);
        string start = DataManager.unlockedProducts.FirstOrDefault();
        Transform point = GetSpawnPoint(productName, start);
        if (prefab == null || point == null)
        {
            Debug.LogWarning($"[MoneySystem] Nelze respawnout '{productName}' (chybí prefab/point).");
            return;
        }

        var basketGO = Instantiate(prefab, point.position, point.rotation);
        DontDestroyOnLoad(basketGO);

        // VYPNI fyziku i tady
        StabilizeBasket(basketGO);
        StartCoroutine(RestorePhysicsAfterDelay(basketGO, reactivateDelay));

        basketInstances[productName] = basketGO;
        spawned.Add(productName);
        basketGO.GetComponent<Basket>().RemoveItems(0);
    }

    // dočasně vypne fyziku na všech dětech
    private void StabilizeBasket(GameObject basketGO)
    {
        foreach (Transform item in basketGO.transform)
        {
            var rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }
    }

    // po prodlevě znovu povolí fyziku
    private IEnumerator RestorePhysicsAfterDelay(GameObject basketGO, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (basketGO == null) yield break;
        foreach (Transform item in basketGO.transform)
        {
            var rb = item.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = false;
        }
    }

    public void RecordRemoval(string productName, int qty)
    {
        if (!itemsRemoved.ContainsKey(productName))
            itemsRemoved[productName] = 0;
        itemsRemoved[productName] += qty;
        Debug.Log($"[MoneySystem] RecordRemoval → {productName}: total removed = {itemsRemoved[productName]}");
    }

    public int GetUnlockPrice(string productName)
    {
        switch (productName)
        {
            case "Brambory":
            case "Jablka":  return 200;
            case "Hrušky":  return 400;
            case "Banány":  return 500;
            case "Mrkve":   return 700;
            case "Rajčata": return 800;
            default:        return 0;
        }
    }
    public int GetRefillPrice(string productName)
    {
        switch (productName)
        {
            case "Brambory":
            case "Jablka":  return 100;
            case "Hrušky":  return 150;
            case "Banány":  return 200;
            case "Mrkve":   return 250;
            case "Rajčata": return 250;
            default:        return 0;
        }
    }
    public int GetSalePrice(string productName)
    {
        switch (productName)
        {
            case "Brambory":
            case "Jablka":  return 15;
            case "Hrušky":  return 20;
            case "Banány":  return 25;
            case "Mrkve":   return 40;
            case "Rajčata": return 50;
            default:        return 0;
        }
    }

    private GameObject GetPrefab(string productName)
    {
        switch (productName)
        {
            case "Brambory": return bramboryPrefab;
            case "Jablka":   return jablkaPrefab;
            case "Hrušky":   return hruskyPrefab;
            case "Banány":   return bananyPrefab;
            case "Mrkve":    return mrkvePrefab;
            case "Rajčata":  return rajcataPrefab;
            default:         return null;
        }
    }

    private Transform GetSpawnPoint(string productName, string startProd)
    {
        bool isStart = productName == startProd;
        switch (productName)
        {
            case "Brambory": return isStart ? initialBramboryPoint : kos2;
            case "Jablka":   return isStart ? initialJablkaPoint   : kos2;
            case "Hrušky":   return kosHrusky;
            case "Banány":   return kosBanany;
            case "Mrkve":    return kosMrkve;
            case "Rajčata":  return kosRajcata;
            default:         return null;
        }
    }
}
