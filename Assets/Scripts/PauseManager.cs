using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("UI pro pauzu (Canvas)")]
    public GameObject pauseMenuUI;

    bool isPaused = false;
    PlayerMovement playerMovement; 

    void Start()
    {
        // předpokládáme, že tvůj hráč má tag "Player"
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) playerMovement = p.GetComponent<PlayerMovement>();

        // UI na startu schované
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // zpět zamknout kurzor a vypnout viditelnost
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;

        // zapnout ovládání hráče
        if (playerMovement != null) 
            playerMovement.enabled = true;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // odemknout kurzor a zobrazit ho
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;

        // vypnout ovládání hráče
        if (playerMovement != null) 
            playerMovement.enabled = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
