using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour
{
    [Tooltip("Sada waypointů v pořadí, kudy auto pojede.")]
    public Transform[] waypoints;

    private NavMeshAgent agent;
    private int idx = 0;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogError("CarAI: Musíš nastavit minimálně 2 waypoints!");
            enabled = false;
            return;
        }

        // 1) Najdi nejbližší bod na NavMesh u prvního waypointu
        Vector3 desiredPos = waypoints[0].position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(desiredPos, out hit, 2f, NavMesh.AllAreas))
        {
            // 2) Přesun agenta přímo na NavMesh výšku
            agent.Warp(hit.position);
        }
        else
        {
            // fallback
            agent.Warp(desiredPos);
        }

        // 3) Nastav cíl na druhý waypoint
        idx = 1;
        agent.SetDestination(waypoints[idx].position);
    }

    void Update()
    {
        if (agent.pathPending) return;

        if (agent.remainingDistance < 0.5f)
        {
            idx = (idx + 1) % waypoints.Length;
            agent.SetDestination(waypoints[idx].position);
        }
    }
}
