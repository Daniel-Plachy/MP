using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveLoadMenu : MonoBehaviour
{
    public TMP_Text infoText;
    public Button loadButton;

    void Start() => Refresh();

    public void Refresh()
    {
        if (SaveLoad.HasSave())
        {
            var d = SaveLoad.GetSaveData();
            infoText.text =
                $"Scéna: {d.sceneName}\n" +
                $"Gold: {d.gold}\n" +
                $"Odemčeno: {string.Join(", ", d.unlockedProducts)}\n" +
                $"Prodáno položek: {d.itemsRemoved.Count}";
            loadButton.interactable = true;
        }
        else
        {
            infoText.text = "Žádné uložené hry nenalezeny.";
            loadButton.interactable = false;
        }
    }

    public void OnLoadButton() => SaveLoad.LoadGame();
}
