// PlayerRespawner.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerRespawner : MonoBehaviour
{
    Vector3 startPosition;
    Quaternion startRotation;

    void Start()
    {
        // uložíme původní pozici a rotaci hráče
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    /// <summary>
    /// Přesune hráče zpátky na startovní pozici.
    /// </summary>
    public void Respawn()
    {
        var cc = GetComponent<CharacterController>();
        if (cc != null)
        {
            // při teleportu musíme CharacterController dočasně vypnout
            cc.enabled = false;
        }

        transform.position = startPosition;
        transform.rotation = startRotation;

        if (cc != null)
        {
            cc.enabled = true;
        }
    }
}
