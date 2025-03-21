using UnityEngine;

public class Pultovedvere : MonoBehaviour
{
    public float uhelOtevreni = 90f; // Úhel otevření dveří
    public float rychlostOtevirani = 2f; // Rychlost otevírání dveří

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

        transform.localRotation = Quaternion.Euler(0f, 0f, aktualniUhel); // Rotace kolem osy Y
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