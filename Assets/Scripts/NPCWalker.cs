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


    public Transform pointA;
    public Transform pointB;
    

    public Transform pointC;
    public Transform pointD;


    [Tooltip("Pravděpodobnost, že NPC na trase A-B se odbočí do obchodu.")]
    public float storeProbability = 0.3f;
    public Transform storeEntrance; 


    public Transform storePointA;   
    public Transform storePointWait; 
    public Transform storePointB;    

    private NavMeshAgent agent;
    private Transform currentTarget;
    private bool isVisitingStore = false;
    private Transform lastRouteTarget; 


    private static bool slotAOccupied = false;
    private static bool slotBOccupied = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (routeOption == RouteOption.RouteAB)
        {
  
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

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (routeOption == RouteOption.RouteAB)
            {
                if (!isVisitingStore)
                {

                    if (Random.value < storeProbability && storeEntrance != null)
                    {
        
                        lastRouteTarget = (currentTarget == pointA) ? pointB : pointA;
         
                        currentTarget = storeEntrance;
                        isVisitingStore = true;
                        agent.SetDestination(currentTarget.position);
                        Debug.Log(name + " jde do obchodu (store entrance).");
                    }
                    else
                    {

                        currentTarget = (currentTarget == pointA) ? pointB : pointA;
                        agent.SetDestination(currentTarget.position);
                    }
                }
                else
                {


                    if (currentTarget == storeEntrance)
                    {

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

                            Debug.Log(name + " čeká u storeEntrance, fronta je plná.");
                            agent.SetDestination(storeEntrance.position);
                        }
                    }
                    else
                    {
   
                        Debug.Log(name + " čeká ve frontě na " + currentTarget.name);
                    }
                }
            }
            else if (routeOption == RouteOption.RouteCD)
            {
 
                currentTarget = (currentTarget == pointC) ? pointD : pointC;
                agent.SetDestination(currentTarget.position);
            }
        }
    }


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

            if (storePointWait != null)
            {
                currentTarget = storePointWait;
                agent.SetDestination(currentTarget.position);
                Debug.Log(name + " jde na storePointWait.");
            }
   
            if (lastRouteTarget != null)
            {

                currentTarget = lastRouteTarget;
                agent.SetDestination(currentTarget.position);
                Debug.Log(name + " se vrací na původní trasu.");
            }
            isVisitingStore = false;
        }
    }
}
