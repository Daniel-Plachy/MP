using UnityEngine;

public class NPCTriggerDetector : MonoBehaviour
{
    NPCWalker npcWalker;
    void Start()
    {
        npcWalker = GetComponentInParent<NPCWalker>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            npcWalker.SetPlayerNearby(true);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            npcWalker.SetPlayerNearby(false);
    }
}
