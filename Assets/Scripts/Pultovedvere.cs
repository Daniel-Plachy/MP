using UnityEngine;

public class Pultovedvere : MonoBehaviour
{
    public float uhelOtevreni = 90f; 
    public float rychlostOtevirani = 2f; 

    private bool playerDetected = false;
    private bool dvereOtevrene = false;
    private float aktualniUhel = 0f;

    void Update()
    {
        if (playerDetected && Input.GetKeyDown(KeyCode.E))
        {
            dvereOtevrene = !dvereOtevrene;
        }

        if (dvereOtevrene)
        {
            aktualniUhel = Mathf.Lerp(aktualniUhel, uhelOtevreni, Time.deltaTime * rychlostOtevirani);
        }
        else
        {
            aktualniUhel = Mathf.Lerp(aktualniUhel, 0f, Time.deltaTime * rychlostOtevirani);
        }

        transform.localRotation = Quaternion.Euler(0f, 0f, aktualniUhel); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }
}