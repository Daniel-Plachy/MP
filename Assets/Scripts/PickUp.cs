using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public Transform holdPoint;
    public float pickUpRange = 2f; 
    public float placeDistance = 2f;
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
            rb.isKinematic = true; 
        }
        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
    }

    void PlaceObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, placeDistance))
        {
            heldObject.transform.position = hit.point; 
        }
        else
        {
            heldObject.transform.position = transform.position + transform.forward * placeDistance; 
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
