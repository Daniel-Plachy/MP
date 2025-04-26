// CarCollision.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CarCollision : MonoBehaviour
{
    void Awake()
    {
        // ujistíme se, že máme trigger collider
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // pokud auto srazí hráče, respawnuj ho
        if (other.CompareTag("Player"))
        {
            var resp = other.GetComponent<PlayerRespawner>();
            if (resp != null)
                resp.Respawn();
        }
    }
}
