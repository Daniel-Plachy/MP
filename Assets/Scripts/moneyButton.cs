using UnityEngine;

public class MoneyButton : MonoBehaviour
{
    private bool playerNearby = false;

    // Když hráč vstoupí do triggeru
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    // Když hráč opustí trigger
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    void Update()
    {
        // Pokud je hráč v blízkosti a stiskne E, přidá 50 goldů
        if(playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            MoneySystem.instance.AddGold(50);
        }
    }
}
