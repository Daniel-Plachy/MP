using UnityEngine;

public class CashRegister : MonoBehaviour
{
    public float scanRange = 2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) ScanItem();
        if (Input.GetKeyDown(KeyCode.G)) AcceptCustomer();
    }

    void ScanItem()
    {
        DataManager.scannedProduct = "";
        DataManager.scannedObject = null;
        foreach(var col in Physics.OverlapSphere(transform.position, scanRange))
        {
            if (col.CompareTag("PickUp") && col.transform.parent != null)
            {
                DataManager.scannedProduct = col.gameObject.name;
                DataManager.scannedObject  = col.gameObject;
                SoundManager.instance.PlayScan();
                break;
            }
        }
    }

    void AcceptCustomer() { }
}
