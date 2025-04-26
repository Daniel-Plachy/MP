using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject PlayOptionsPanel;
    public GameObject vyberPanel;
    public SaveLoadMenu saveLoadMenu;
public GameObject mainMenuUI;
public GameObject loadMenuUI;
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
    mainMenuUI.SetActive(false);
    loadMenuUI.SetActive(true);
    saveLoadMenu.Refresh();
}
    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        PlayOptionsPanel.SetActive(false);
    }
    public void VyberBrambory()
    {
         SaveLoad.SkipLoadOnStart = true;
                 DataManager.unlockedProducts.Clear();
        DataManager.UnlockProduct("Brambory");
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void VyberJablka()
    {
         SaveLoad.SkipLoadOnStart = true;
            DataManager.unlockedProducts.Clear();
        DataManager.UnlockProduct("Jablka");
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
