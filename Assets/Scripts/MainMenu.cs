using UnityEngine;
using UnityEngine.SceneManagement;
public class DataManager : MonoBehaviour
{
    public static string vybranaPolozka;
}
public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject PlayOptionsPanel;
    public GameObject vyberPanel;
    public string vybranaPolozka;

    public void PlayGame()
    {
        mainMenu.SetActive(false);
        PlayOptionsPanel.SetActive(true);
    }

    public void NewGame()
    {
        PlayOptionsPanel.SetActive(false);
        vyberPanel.SetActive(true);
    }

    public void LoadGame()
    {
        Debug.Log("Načítání hry...");
        // Doplň kód pro načtení uložené hry
    }

    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        PlayOptionsPanel.SetActive(false);
    }

    public void VyberBrambory()
{
    DataManager.vybranaPolozka = "Brambory";
    Debug.Log("Vybrána položka: Brambory");
    Debug.Log("Načítám scénu s mapou");
    SceneManager.LoadScene(1, LoadSceneMode.Single);
    Debug.Log("Scéna s mapou načtena");
}

public void VyberJablka()
{
    DataManager.vybranaPolozka = "Jablka";
    Debug.Log("Vybrána položka: Jablka");
    Debug.Log("Načítám scénu s mapou");
    SceneManager.LoadScene(1, LoadSceneMode.Single);
    Debug.Log("Scéna s mapou načtena");
}

    public void QuitGame()
    {
        Application.Quit();
    }
}
