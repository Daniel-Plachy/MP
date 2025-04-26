using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    bool playerNearby;

    void OnTriggerEnter(Collider c) { if (c.CompareTag("Player")) playerNearby = true; }
    void OnTriggerExit (Collider c) { if (c.CompareTag("Player")) playerNearby = false; }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.C))
            SceneManager.LoadScene(2);
    }
}
