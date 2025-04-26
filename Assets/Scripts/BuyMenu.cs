using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BuyMenu : MonoBehaviour
{
    [Header("General")]
    public GameObject productsPanel;

    [Header("Brambory (p)")]
    public GameObject buyp, refillp;
    public TMP_Text cenaunlockp, cenarefillp;
    public Button buybtnp, refillbtnp;

    [Header("Jablka (a)")]
    public GameObject buya, refilla;
    public TMP_Text cenaunlocka, cenarefilla;
    public Button buybtna, refillbtna;

    [Header("Hrušky (h)")]
    public GameObject buyh, refillh;
    public TMP_Text cenaunlockh, cenarefillh;
    public Button buybtnh, refillbtnh;

    [Header("Mrkve (c)")]
    public GameObject buyc, refillc;
    public TMP_Text cenaunlockc, cenarefillc;
    public Button buybtnc, refillbtnc;

    [Header("Banány (b)")]
    public GameObject buyb, refillb;
    public TMP_Text cenaunlockb, cenarefillb;
    public Button buybtnb, refillbtnb;

    [Header("Rajčata (t)")]
    public GameObject buyt, refillit;
    public TMP_Text cenaunlockt, cenarefillt;
    public Button buybtnt, refillbtnt;

    [Header("Výběr startovní položky")]
    public GameObject potato;   // panel „potato“ bez buy, jen refill
    public GameObject apples;    // panel „apples“ bez buy, jen refill

    [Header("Výběr sekundární položky")]
    public GameObject potatond; // panel „potato“ s buy+refill
    public GameObject applesnd;  // panel „apples“ s buy+refill

    void Start()
    {
        NPCWalker.ResetStoreSlots();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        RefreshUI();
    }

    public void RefreshUI()
    {
        string start = DataManager.unlockedProducts.Count > 0
            ? DataManager.unlockedProducts[0]
            : "";

        bool isPotatoStart = start == "Brambory";

        // Primární panely
        potato.SetActive(isPotatoStart);
        apples.SetActive(!isPotatoStart);

        // Sekundární panely
        potatond.SetActive(!isPotatoStart);
        applesnd.SetActive(isPotatoStart);

        int gold = MoneySystem.instance.Gold;

        UpdateProductUI("Brambory", start, buyp, refillp, cenaunlockp, cenarefillp, buybtnp, refillbtnp, gold);
        UpdateProductUI("Jablka",   start, buya, refilla, cenaunlocka, cenarefilla, buybtna, refillbtna, gold);
        UpdateProductUI("Hrušky",   start, buyh, refillh, cenaunlockh, cenarefillh, buybtnh, refillbtnh, gold);
        UpdateProductUI("Mrkve",    start, buyc, refillc, cenaunlockc, cenarefillc, buybtnc, refillbtnc, gold);
        UpdateProductUI("Banány",   start, buyb, refillb, cenaunlockb, cenarefillb, buybtnb, refillbtnb, gold);
        UpdateProductUI("Rajčata",  start, buyt, refillit, cenaunlockt, cenarefillt, buybtnt, refillbtnt, gold);
    }

    void UpdateProductUI(
        string productName,
        string startProduct,
        GameObject buyGO,
        GameObject refillGO,
        TMP_Text priceUnlockText,
        TMP_Text priceRefillText,
        Button buyButton,
        Button refillButton,
        int gold
    )
    {
        bool isStart    = productName == startProduct;
        bool isUnlocked = DataManager.unlockedProducts.Contains(productName);
        int unlockCost  = MoneySystem.instance.GetUnlockPrice(productName);
        int refillCost  = MoneySystem.instance.GetRefillPrice(productName);

        // Pokud je to startovní produkt, skryj oba panely
        if (isStart)
        {
            buyGO.SetActive(false);
            refillGO.SetActive(false);
            return;
        }

        // Správa cenovek
        priceUnlockText.gameObject.SetActive(!isUnlocked);
        priceRefillText.gameObject.SetActive(isUnlocked);

        if (!isUnlocked)
        {
            // ještě nezamčeno → nabídni unlock
            buyGO.SetActive(true);
            refillGO.SetActive(false);
            priceUnlockText.text = unlockCost.ToString();
            buyButton.interactable = gold >= unlockCost;
        }
        else
        {
            // odemčeno → nabídni refill
            buyGO.SetActive(false);
            refillGO.SetActive(true);
            priceRefillText.text = refillCost.ToString();
            refillButton.interactable = gold >= refillCost;
        }
    }

    public void BuyBrambory()   => BuyOrRefill("Brambory", true);
    public void RefillBrambory() => BuyOrRefill("Brambory", false);

    public void BuyJablka()     => BuyOrRefill("Jablka", true);
    public void RefillJablka()   => BuyOrRefill("Jablka", false);

    public void BuyHrusky()     => BuyOrRefill("Hrušky", true);
    public void RefillHrusky()   => BuyOrRefill("Hrušky", false);

    public void BuyMrkve()      => BuyOrRefill("Mrkve", true);
    public void RefillMrkve()    => BuyOrRefill("Mrkve", false);

    public void BuyBanany()     => BuyOrRefill("Banány", true);
    public void RefillBanany()   => BuyOrRefill("Banány", false);

    public void BuyRajcata()    => BuyOrRefill("Rajčata", true);
    public void RefillRajcata()  => BuyOrRefill("Rajčata", false);

    void BuyOrRefill(string productName, bool isUnlock)
    {
        if (isUnlock)
            MoneySystem.instance.BuyProduct(productName);
        else
            MoneySystem.instance.RefillProduct(productName);

        RefreshUI();
    }

    public void Exit()
    {
        SceneManager.LoadScene(1);
    }
}
