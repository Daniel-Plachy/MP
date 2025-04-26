using UnityEngine;

public class Pultovedvere : MonoBehaviour
{
    public float uhelOtevreni = 90f, rychlostOtevirani = 2f;
    bool playerDetected, dvereOtevrene;
    float aktualniUhel;

    void Update()
    {
        if (playerDetected && Input.GetKeyDown(KeyCode.E))
            dvereOtevrene = !dvereOtevrene;

        aktualniUhel = Mathf.Lerp(
            aktualniUhel,
            dvereOtevrene ? uhelOtevreni : 0f,
            Time.deltaTime * rychlostOtevirani
        );
        transform.localRotation = Quaternion.Euler(0f, 0f, aktualniUhel);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerDetected = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerDetected = false;
    }
}
