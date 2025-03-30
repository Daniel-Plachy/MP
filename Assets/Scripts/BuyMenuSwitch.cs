using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
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

        if(playerNearby && Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene(2);
        }
    }
}
