using UnityEngine;

public class MoneyButton : MonoBehaviour
{
    private bool playerNearby = false;


    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    void Update()
    {
 
        if(playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            MoneySystem.instance.AddGold(50);
        }
    }
}
