using UnityEngine;

public class CashRegister : MonoBehaviour
{
    public float scanRange = 2f; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ScanItem();
        }
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            AcceptCustomer();
        }
    }

    void ScanItem()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, scanRange);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("PickUp") && col.transform.parent != null) 
            {
                Debug.Log("Položka naskenována!");
                return;
            }
        }
        Debug.Log("Žádné zboží k naskenování!");
    }
    
    void AcceptCustomer()
    {

        Debug.Log("Zákazník přijat, objednávka přijata!");
    }
}
