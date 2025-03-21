using UnityEngine;

public class CashRegister : MonoBehaviour
{
    public float scanRange = 2f; // Maximální vzdálenost skenování

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ScanItem();
        }
    }

    void ScanItem()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, scanRange);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("PickUp") && col.transform.parent != null) // Musí být držen hráčem
            {
                Debug.Log("Položka naskenována!");
                return;
            }
        }

        Debug.Log("Žádné zboží k naskenování!");
    }
}
