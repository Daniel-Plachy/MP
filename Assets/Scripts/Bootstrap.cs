using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    void Awake()
    {
        if (MoneySystem.instance == null)
        {
            // Předpokládáme, že váš MoneySystem-prefab jste uložili do Resources/Prefabs/MoneySystem.prefab
            var prefab = Resources.Load<GameObject>("Prefabs/moneySystem");
            Instantiate(prefab);
        }
    }
}
