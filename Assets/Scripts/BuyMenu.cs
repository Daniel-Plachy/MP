using UnityEngine;
using UnityEngine.SceneManagement;
public class BuyMenu : MonoBehaviour
{

    void Start()
{
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
}

    public GameObject buyMenu;
    public GameObject productsPanel;
    public GameObject roomsPanel;
    public void Products()
    {
        buyMenu.SetActive(false);
        productsPanel.SetActive(true);
    }
    public void Rooms() {
        buyMenu.SetActive(false);
        roomsPanel.SetActive(true);
    }
    public void BuyHrusky()
{
    MoneySystem.instance.BuyProduct("Hrušky");
}

public void BuyBanany()
{
    MoneySystem.instance.BuyProduct("Banány");
}

public void BuyMrkev()
{
    MoneySystem.instance.BuyProduct("Mrkve");
}
public void BuyRajcata()
{
    MoneySystem.instance.BuyProduct("Rajčata");
}

public void Exit() {
    SceneManager.LoadScene(1);
}

}
