using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public Transform holdPoint; // Místo, kam se objekt "přichytí", např. před hráče
    public float pickUpRange = 2f; // Jak daleko můžeš objekt sebrat
    public float placeDistance = 2f; // Maximální vzdálenost položení
    private GameObject heldObject;
    private Collider detectedObject;

    void Update()
    {
        DetectPickUp();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null && detectedObject != null)
            {
                PickUp(detectedObject.gameObject);
            }
            else if (heldObject != null)
            {
                PlaceObject();
            }
        }
    }

    void DetectPickUp()
    {
        detectedObject = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, pickUpRange);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("PickUp"))
            {
                detectedObject = col;
                return;
            }
        }
    }

    void PickUp(GameObject obj)
    {
        heldObject = obj;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true; // Zamezí fyzice
        }
        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
    }

    void PlaceObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, placeDistance))
        {
            heldObject.transform.position = hit.point; // Položení na místo
        }
        else
        {
            heldObject.transform.position = transform.position + transform.forward * placeDistance; // Položí před hráče
        }

        heldObject.transform.SetParent(null);
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = false;
        }

        heldObject = null;
    }
}
