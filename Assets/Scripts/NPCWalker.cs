using UnityEngine;
using UnityEngine.AI;

public class NPCWalker : MonoBehaviour
{
    public void SetDestination(Vector3 destination)
{
    agent.SetDestination(destination);
}

    public enum RouteOption { RouteAB, RouteCD }
    public RouteOption routeOption = RouteOption.RouteAB;

    // Trasa A-B (pro první chodník)
    public Transform pointA;
    public Transform pointB;
    
    // Trasa C-D (pro druhý chodník)
    public Transform pointC;
    public Transform pointD;

    // Nastavení pro návštěvu obchodu (pouze pro RouteAB)
    [Tooltip("Pravděpodobnost, že NPC na trase A-B se odbočí do obchodu.")]
    public float storeProbability = 0.3f;
    public Transform storeEntrance; // Bod, kam NPC odbočí, pokud jde do obchodu

    // Body fronty v obchodě
    public Transform storePointA;    // Slot pro prvního zákazníka
    public Transform storePointWait; // Bod, kam se přesune první zákazník po obsluze
    public Transform storePointB;    // Slot pro druhého zákazníka

    private NavMeshAgent agent;
    private Transform currentTarget;
    private bool isVisitingStore = false;
    private Transform lastRouteTarget; // Původní cílový bod před odbočením do obchodu

    // Statické proměnné pro sledování obsazenosti fronty (sdílené všemi NPC)
    private static bool slotAOccupied = false;
    private static bool slotBOccupied = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (routeOption == RouteOption.RouteAB)
        {
            // Začínáme na bodě A – normálně by měl jít do B
            currentTarget = pointA;
        }
        else if (routeOption == RouteOption.RouteCD)
        {
            currentTarget = pointC;
        }
        
        if (currentTarget != null)
            agent.SetDestination(currentTarget.position);
    }

    void Update()
    {
        // Pokud agent dosáhne cíle (zbytková vzdálenost menší než 0.5)
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (routeOption == RouteOption.RouteAB)
            {
                if (!isVisitingStore)
                {
                    // Rozhodneme se, zda odbočit do obchodu
                    if (Random.value < storeProbability && storeEntrance != null)
                    {
                        // Uložíme si původní cíl (alternaci A <-> B)
                        lastRouteTarget = (currentTarget == pointA) ? pointB : pointA;
                        // Nastavíme cíl na vstup do obchodu
                        currentTarget = storeEntrance;
                        isVisitingStore = true;
                        agent.SetDestination(currentTarget.position);
                        Debug.Log(name + " jde do obchodu (store entrance).");
                    }
                    else
                    {
                        // Normální chování – alternace mezi A a B
                        currentTarget = (currentTarget == pointA) ? pointB : pointA;
                        agent.SetDestination(currentTarget.position);
                    }
                }
                else
                {
                    // Již jsme v procesu návštěvy obchodu
                    // Pokud jsme právě dorazili na storeEntrance, přidělíme frontový slot
                    if (currentTarget == storeEntrance)
                    {
                        // Dosaženo storeEntrance – vybereme volný slot
                        if (!slotAOccupied && storePointA != null)
                        {
                            slotAOccupied = true;
                            currentTarget = storePointA;
                            agent.SetDestination(currentTarget.position);
                            Debug.Log(name + " obsadil slot A ve frontě obchodu.");
                        }
                        else if (!slotBOccupied && storePointB != null)
                        {
                            slotBOccupied = true;
                            currentTarget = storePointB;
                            agent.SetDestination(currentTarget.position);
                            Debug.Log(name + " obsadil slot B ve frontě obchodu.");
                        }
                        else
                        {
                            // Oba sloty jsou obsazené – NPC zůstane u vstupu a čeká
                            Debug.Log(name + " čeká u storeEntrance, fronta je plná.");
                            agent.SetDestination(storeEntrance.position);
                        }
                    }
                    else
                    {
                        // Pokud jsme již v jednom z frontových slotů, můžeme čekat (zde nepřidáváme další logiku)
                        Debug.Log(name + " čeká ve frontě na " + currentTarget.name);
                    }
                }
            }
            else if (routeOption == RouteOption.RouteCD)
            {
                // Standardní chování pro trasu C-D – alternace mezi C a D
                currentTarget = (currentTarget == pointC) ? pointD : pointC;
                agent.SetDestination(currentTarget.position);
            }
        }
    }

    /// <summary>
    /// Volá se externě (např. při obsluze zákazníka), aby NPC opustilo frontu.
    /// NPC se pak vrátí k původní trase a uvolní příslušný slot.
    /// </summary>
    public void LeaveStoreQueue()
    {
        if (isVisitingStore)
        {
            if (currentTarget == storePointA)
            {
                slotAOccupied = false;
                Debug.Log(name + " opouští slot A fronty.");
            }
            else if (currentTarget == storePointB)
            {
                slotBOccupied = false;
                Debug.Log(name + " opouští slot B fronty.");
            }
            // Přesuneme NPC do storePointWait (kde je obsloužený)
            if (storePointWait != null)
            {
                currentTarget = storePointWait;
                agent.SetDestination(currentTarget.position);
                Debug.Log(name + " jde na storePointWait.");
            }
            // Po odchodu z fronty se vrátí na původní trasu
            if (lastRouteTarget != null)
            {
                // Můžete případně po nějakém čase znovu spustit navrácení na trasu
                // Např. pomocí coroutine – zde příklad zjednodušeně ihned:
                currentTarget = lastRouteTarget;
                agent.SetDestination(currentTarget.position);
                Debug.Log(name + " se vrací na původní trasu.");
            }
            isVisitingStore = false;
        }
    }
}
