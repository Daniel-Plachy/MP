using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CarHeightMaintainer : MonoBehaviour
{
    [Tooltip("Výška, ve které má auto zůstat")]
    public float desiredY = 1.45f;

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // ZAKÁŽE NavMeshAgentu, aby sám měnil transform.position
        agent.updatePosition = false;
    }

    void LateUpdate()
    {
        // NavMeshAgent počítá nextPosition, ale nekopíruje ho do transformu
        Vector3 next = agent.nextPosition;
        transform.position = new Vector3(next.x, desiredY, next.z);
    }
}
