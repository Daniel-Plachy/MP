using UnityEngine;
using UnityEngine.AI;

public class CarDespawner : MonoBehaviour
{
    [HideInInspector] public System.Action onDespawn;
    public float arriveThreshold = 0.5f;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent.pathPending) return;
        if (agent.remainingDistance <= arriveThreshold)
        {
            onDespawn?.Invoke();
            Destroy(gameObject);
        }
    }
}
