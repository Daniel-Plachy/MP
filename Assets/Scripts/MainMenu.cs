using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;	
using System.Collections;
using TMPro; // Make sure to include the TextMeshPro namespace
using UnityEngine.EventSystems; // Include this for EventSystem

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject PlayOptionsPanel;
    public GameObject vyberPanel;
    public SaveLoadMenu saveLoadMenu;
public GameObject mainMenuUI;
public GameObject loadMenuUI;
public float messageDuration = 2f;
public TMPro.TextMeshProUGUI messageText; 
    public Button loadButton;

    void Start()
    {
        loadButton.interactable = SaveLoad.HasSave();
        messageText.text = "";
    }
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
    PlayOptionsPanel.SetActive(false);
    saveLoadMenu.Refresh();

}
    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        PlayOptionsPanel.SetActive(false);
    }
    public void BackToPlayOptions()
    {
        PlayOptionsPanel.SetActive(true);
        vyberPanel.SetActive(false);
        loadMenuUI.SetActive(false);
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

    public void OnLoadButton()
    {
        if (SaveLoad.HasSave())
            StartCoroutine(ShowAndLoad("Save nalezen"));
        else
            StartCoroutine(ShowMessage("Žádný save neexistuje"));
    }
    private IEnumerator ShowAndLoad(string msg)
    {
        messageText.text = msg;
        yield return new WaitForSeconds(messageDuration);
        SaveLoad.LoadGame();
    }

    private IEnumerator ShowMessage(string msg)
    {
        messageText.text = msg;
        yield return new WaitForSeconds(messageDuration);
        messageText.text = "";
    }
}
