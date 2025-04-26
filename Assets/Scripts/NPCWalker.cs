using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPCWalker : MonoBehaviour
{
    public enum RouteOption { RouteAB, RouteCD }

     public static void ResetStoreSlots()
    {
        slotAOccupied    = false;
        slotBOccupied    = false;
       slotWaitOccupied = false;
    }
    public RouteOption routeOption = RouteOption.RouteAB;

    [Header("Trasy")]
    public Transform pointA, pointB, pointC, pointD;

    [Header("Obchodní sloty")]
    public Transform storePointA, storePointB, storePointWait;

    [Header("Viewpointy pro natočení")]
    public Transform viewPointA, viewPointB, viewPointWait;

    [Header("Obchodní nastavení")]
    public float storeProbability = 0.3f;
    public float visitDuration = 3f;
    public float transitionCheckInterval = 0.5f;
    [Min(1)] public int maxOrderQuantity = 3;

    private NavMeshAgent agent;
    private Transform currentTarget, lastRouteTarget, currentViewpoint;
    private bool isVisitingStore;
    private TMP_Text orderTextUI;

    private static bool slotAOccupied, slotBOccupied, slotWaitOccupied;
    private bool playerIsNearby, playerIsNearBag;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentTarget = routeOption == RouteOption.RouteAB ? pointA : pointC;
        agent.SetDestination(currentTarget.position);

        orderTextUI = GetComponentInChildren<TMP_Text>();
        if (orderTextUI != null) orderTextUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isVisitingStore && currentViewpoint != null)
        {
            FaceCurrentViewpoint();
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isVisitingStore)
        {
            if (routeOption == RouteOption.RouteAB)
            {
                if (currentTarget == pointA && Random.value < storeProbability)
                {
                    lastRouteTarget = pointB;
                    if (!slotAOccupied || !slotBOccupied)
                    {
                        StartCoroutine(ProcessStoreEntry());
                        return;
                    }
                }
                currentTarget = currentTarget == pointA ? pointB : pointA;
            }
            else
            {
                currentTarget = currentTarget == pointC ? pointD : pointC;
            }
            agent.SetDestination(currentTarget.position);
        }
    }

    IEnumerator ProcessStoreEntry()
    {
        isVisitingStore = true;

        // 1) Obsazení slotu A nebo B
        if (!slotAOccupied)
        {
            slotAOccupied   = true;
            currentTarget   = storePointA;
            currentViewpoint = viewPointA;
        }
        else if (!slotBOccupied)
        {
            slotBOccupied   = true;
            currentTarget   = storePointB;
            currentViewpoint = viewPointB;
            agent.SetDestination(currentTarget.position);
            while (slotAOccupied)
                yield return new WaitForSeconds(transitionCheckInterval);
            if (!slotAOccupied)
            {
                slotBOccupied = false;
                slotAOccupied = true;
                currentTarget = storePointA;
                currentViewpoint = viewPointA;
            }
            else { isVisitingStore = false; yield break; }
        }
        else { isVisitingStore = false; yield break; }

        // Přechod na storePointA
        agent.SetDestination(currentTarget.position);
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.5f);

        // 2) Čekání na Q
        yield return new WaitUntil(() => playerIsNearby && Input.GetKeyDown(KeyCode.Q));

        // 3) Výběr produktu a počtu
        string product = DataManager.unlockedProducts[
            Random.Range(0, DataManager.unlockedProducts.Count)
        ];
        int qty = Random.Range(1, maxOrderQuantity + 1);

        // 4) Zobrazení textu
        if (orderTextUI != null)
        {
            orderTextUI.text = qty + "× " + product;
            orderTextUI.gameObject.SetActive(true);
        }

        // 5) Přechod na wait slot
        yield return new WaitUntil(() => !slotWaitOccupied);
        slotAOccupied     = false;
        slotWaitOccupied  = true;
        currentTarget     = storePointWait;
        currentViewpoint  = viewPointWait;
        agent.SetDestination(currentTarget.position);
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.5f);

        // 6) Pro každý kus: nejdřív sken, pak zabalení
        for (int i = 1; i <= qty; i++)
        {
            // a) čekej na sken požadovaného produktu
            DataManager.scannedProduct = "";
            yield return new WaitUntil(() => DataManager.scannedProduct == product);
            Debug.Log(name + ": naskenováno " + i + "/" + qty);

            // b) čekej na zabalení
            yield return new WaitUntil(() => playerIsNearBag && Input.GetKeyDown(KeyCode.G));
           
            if (DataManager.scannedObject != null) {
                Destroy(DataManager.scannedObject);
                MoneySystem.instance.RecordRemoval(product, 1);
                DataManager.scannedObject = null;
            }
            Debug.Log(name + ": zabalení " + i + "/" + qty);
            
            
        }

                // --- POXYN PRODEJE: Přidání goldů hráči ---
        int unitPrice = MoneySystem.instance.GetSalePrice(product);
        int totalPrice = unitPrice * qty;
        MoneySystem.instance.AddGold(totalPrice);
         SoundManager.instance.PlayCoin();
        Debug.Log(name + ": zaplaceno " + totalPrice + " goldů za " + qty + "×" + product);

        // 7) Dokončení a návrat na trasu
        yield return new WaitForSeconds(visitDuration);
        if (orderTextUI != null)
            orderTextUI.gameObject.SetActive(false);

        slotWaitOccupied = false;
        isVisitingStore  = false;
        currentTarget    = lastRouteTarget;
        agent.SetDestination(currentTarget.position);

    }

    void FaceCurrentViewpoint()
    {
        Vector3 dir = currentViewpoint.position - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerIsNearby = true;
        if (other.CompareTag("Bag"))    playerIsNearBag  = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerIsNearby = false;
        if (other.CompareTag("Bag"))    playerIsNearBag  = false;
    }

    public void SetPlayerNearby(bool state) => playerIsNearby = state;
}
