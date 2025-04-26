using UnityEngine;

public class MoneyButton : MonoBehaviour
{
    bool playerNearby;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
            MoneySystem.instance.AddGold(50);
    }
}
